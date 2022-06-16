using Autofac;
using Autofac.Multitenant;
using TestMultitenant.Enums;

namespace IntegrationTests.WebApplicationFixture;

public static class AppServices
{
	private static MultitenantContainer? _scope;

	public static void SetContainer(MultitenantContainer? scope)
	{
		_scope = scope;
	}

	public static T Get<T>(TenantId tenantId = TenantId.First) where T : notnull
	{
		return _scope!.GetTenantScope(tenantId).Resolve<T>();
	}
}