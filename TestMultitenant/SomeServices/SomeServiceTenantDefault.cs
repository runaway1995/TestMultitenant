using TestMultitenant.SomeServices.Interfaces;
using TestMultitenant.SomeServices.Models;

namespace TestMultitenant.SomeServices;

public class SomeServiceTenantDefault: ISomeService
{
	public SomeData SomeFunc()
	{
		return new SomeData(0);
	}
}