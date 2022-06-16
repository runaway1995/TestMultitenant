using Autofac;
using Autofac.Multitenant;
using TestMultitenant.Enums;
using TestMultitenant.Multitenant;
using TestMultitenant.SomeServices;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureContainer<ContainerBuilder>(b =>
{
	b.RegisterType<SomeServiceTenantDefault>().AsImplementedInterfaces();
	b.RegisterType<TenantIdentificationStrategy>().AsImplementedInterfaces().SingleInstance();
});

builder.Host.UseServiceProviderFactory(new AutofacMultitenantServiceProviderFactory(container =>
{
	var strategy = container.Resolve<ITenantIdentificationStrategy>();
	var mtc = new MultitenantContainer(strategy, container);

	mtc.ConfigureTenant(TenantId.First, b =>
	{
		b.RegisterType<SomeServiceTenant1>().AsImplementedInterfaces();
	});
	
	mtc.ConfigureTenant(TenantId.Second, b =>
	{
		b.RegisterType<SomeServiceTenant2>().AsImplementedInterfaces();
	});

	return mtc;
}));

builder.Services.AddControllers();

builder.Services.AddAutofacMultitenantRequestServices();

var app = builder.Build();


app.UseRouting();
app.UseStaticFiles();
app.UseExceptionHandler("/error");
app.MapControllers();

await app.RunAsync();