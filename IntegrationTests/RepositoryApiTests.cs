using System.Net;
using System.Web;
using FluentAssertions;
using IntegrationTests.WebApplicationFixture;
using Microsoft.AspNetCore.Mvc;
using TestMultitenant.Enums;
using TestMultitenant.SomeServices.Models;
using Xunit;

namespace IntegrationTests;

[Collection("Integration tests")]
public class RepositoryApiTests : IClassFixture<ApplicationFixture>
{
	private readonly ApplicationFixture _applicationFixture;

	public RepositoryApiTests(ApplicationFixture applicationFixture)
	{
		_applicationFixture = applicationFixture;
	}

	[Theory]
	[InlineData(TenantId.First, 1)]
	public async Task TestFirst(TenantId tenantId, int testResponseData)
	{
		await Test(tenantId, testResponseData);
	}
	
	[Theory]
	[InlineData(TenantId.Second, 2)]
	public async Task TestSecond(TenantId tenantId, int testResponseData)
	{
		await Test(tenantId, testResponseData);
	}
	
	[Theory]
	[InlineData(null, 0)]
	public async Task TestDefault(TenantId? tenantId, int testResponseData)
	{
		await Test(tenantId, testResponseData);
	}

	private async Task Test(TenantId? tenantId, int testResponseData)
	{
		var expected = new SomeData(testResponseData);

		var response = await _applicationFixture.GetAsync<SomeData>(
			$"/Test/Test", tenantId);

		response.Content.Should().BeEquivalentTo(expected);
	}
}