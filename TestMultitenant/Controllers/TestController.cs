using Autofac;
using Microsoft.AspNetCore.Mvc;
using TestMultitenant.SomeServices.Interfaces;

namespace TestMultitenant.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class TestController : ControllerBase
{
	private readonly ILifetimeScope _scope;

	public TestController(ILifetimeScope scope)
	{
		_scope = scope;
	}

	[HttpGet]
	public async Task<int> Test(int crmId, CancellationToken cancellationToken)
	{
		var someService = _scope.Resolve<ISomeService>();

		return someService.SomeFunc();
	}
}