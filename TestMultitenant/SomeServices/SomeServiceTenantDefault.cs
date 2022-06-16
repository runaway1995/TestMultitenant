using TestMultitenant.SomeServices.Interfaces;

namespace TestMultitenant.SomeServices;

public class SomeServiceTenantDefault: ISomeService
{
	public int SomeFunc()
	{
		return 0;
	}
}