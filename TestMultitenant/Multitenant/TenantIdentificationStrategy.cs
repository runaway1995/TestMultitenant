using Autofac.Multitenant;
using TestMultitenant.Enums;

namespace TestMultitenant.Multitenant;

public class TenantIdentificationStrategy : ITenantIdentificationStrategy
{
	private readonly IHttpContextAccessor _contextAccessor;

	public TenantIdentificationStrategy(IHttpContextAccessor contextAccessor)
	{
		_contextAccessor = contextAccessor;
	}

	public bool TryIdentifyTenant(out object tenantId)
	{
		var context = _contextAccessor.HttpContext;

		if (context == null)
		{
			tenantId = null!;

			return false;
		}

		try
		{
			context.Request.Headers.TryGetValue("TenantId", out var ids);
			if (Enum.TryParse<TenantId>(ids.First(), out var id))
			{
				tenantId = id;
				return true;
			}

			tenantId = null!;
			return false;
		}
		catch (Exception)
		{
			tenantId = null!;

			return false;
		}
	}
}