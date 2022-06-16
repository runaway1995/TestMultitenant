using Autofac;
using Autofac.Multitenant;
using TestMultitenant.Enums;

namespace TestMultitenant;

public static class AutofacExtension
{
	public static void ForEachTenants(this MultitenantContainer scope,
		Action<ILifetimeScope, TenantId> action)
	{
		foreach (var countryCode in Enum.GetValues<TenantId>())
		{
			using var newScope = scope.GetTenantScope(countryCode).BeginLifetimeScope(countryCode);
			action(newScope, countryCode);
		}
	}

	public static async Task ForEachTenantsAsync(this MultitenantContainer? scope,
		Func<ILifetimeScope, TenantId, Task> action)
	{
		foreach (var countryCode in Enum.GetValues<TenantId>())
		{
			await using var newScope =
				scope!.GetTenantScope(countryCode).BeginLifetimeScope(countryCode);

			await action(newScope, countryCode);
		}
	}

	public static async Task ParallelForEachTenantsAsync(this MultitenantContainer scope,
		Func<ILifetimeScope, TenantId, ValueTask> action)
	{
		var parallelOptions = new ParallelOptions
		{
			MaxDegreeOfParallelism = 10
		};

		await Parallel.ForEachAsync(Enum.GetValues<TenantId>(),
			parallelOptions, async (countryCode, _) =>
			{
				await using var newScope = scope.GetTenantScope(countryCode)
												.BeginLifetimeScope(countryCode);

				await action(newScope, countryCode);
			});
	}

	public static TenantId GetTenantId(this ILifetimeScope scope)
	{
		var strategy = scope.Resolve<ITenantIdentificationStrategy>();

		if (strategy.TryIdentifyTenant(out var countryCode))
			return (TenantId) countryCode;

		return (TenantId) scope.Tag;
	}
}