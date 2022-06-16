using System.Net;

namespace IntegrationTests.WebApplicationFixture;

public record Response<TResult> where TResult : class?
{
	public HttpStatusCode StatusCode { get; set; }
	public TResult Content { get; set; } = null!;
}