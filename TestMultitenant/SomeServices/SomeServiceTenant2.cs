using TestMultitenant.SomeServices.Interfaces;

namespace TestMultitenant.SomeServices;

public class SomeServiceTenant2: ISomeService
{
	public int SomeFunc()
	{
		return 2;
	}
}