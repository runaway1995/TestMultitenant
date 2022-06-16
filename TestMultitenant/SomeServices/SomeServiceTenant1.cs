using TestMultitenant.SomeServices.Interfaces;

namespace TestMultitenant.SomeServices;

public class SomeServiceTenant1: ISomeService
{
	public int SomeFunc()
	{
		return 1;
	}
}