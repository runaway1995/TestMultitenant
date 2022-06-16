using System.Net.Http.Json;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Multitenant;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using TestMultitenant.Enums;
using Xunit;

namespace IntegrationTests.WebApplicationFixture;

public class ApplicationFixture : IAsyncLifetime
{
	private readonly HttpClient _client;
	private readonly MultitenantContainer? _scope;

	public ApplicationFixture()
	{
		var application = new MyWebApplication();

		_client = application.CreateClient(new WebApplicationFactoryClientOptions
		{
			AllowAutoRedirect = false
		});

		_scope = application.Services.GetAutofacRoot().Resolve<MultitenantContainer>();
	}

	public async Task InitializeAsync()
	{
		AppServices.SetContainer(_scope);

		Thread.Sleep(4000);
	}

	public async Task DisposeAsync()
	{
		await _scope!.DisposeAsync();
		_client.Dispose();
	}

	public async Task<Response<TResult>> GetAsync<TResult>(string url, TenantId? tenantId)
		where TResult : class
	{
		using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
		if (tenantId != null)
			requestMessage.Headers.Add("CountryCode", tenantId.ToString()!.ToLower());

		var response = await _client.SendAsync(requestMessage);

		var json = await response.Content.ReadAsStringAsync();

		return new Response<TResult>
		{
			Content = (json.IsNullOrEmpty() ? null! : await response.Content.ReadFromJsonAsync<TResult>())!,
			StatusCode = response.StatusCode
		};
	}
}