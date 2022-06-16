using TestMultitenant.SomeServices.Interfaces;
using TestMultitenant.SomeServices.Models;

namespace TestMultitenant.SomeServices;

public class SomeServiceTenant1: ISomeService
{
	public SomeData SomeFunc()
	{
		return new SomeData(1);
	}
}