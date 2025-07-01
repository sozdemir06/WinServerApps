using Microsoft.AspNetCore.Authorization;
using Shared.Extensions;
using Shared.Languages;
using Shared.Messages.Extensions;
using Shared.Services.Claims;


namespace Api.Extensions
{
    public static class CommonExtensions
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection services, IConfiguration configuration)
        {
            var usersAssembly = typeof(UsersModule).Assembly;
            var accountingAssembly = typeof(AccountingModule).Assembly;
            var catalogAssembly = typeof(CatalogModule).Assembly;
            var customersAssembly = typeof(CustomersModule).Assembly;
            var notificationsAssembly = typeof(NotificationsModule).Assembly;
            var ordersAssembly = typeof(OrdersModule).Assembly;
            var shippingAssembly = typeof(ShippingModule).Assembly;

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
            services.AddFastEndpointsWithAssemblies(usersAssembly, accountingAssembly, catalogAssembly, customersAssembly, notificationsAssembly, ordersAssembly, shippingAssembly);
            services.AddMediatRWithAssemblies(usersAssembly, accountingAssembly, catalogAssembly, customersAssembly, notificationsAssembly, ordersAssembly, shippingAssembly);
            services.AddRedisExtensions(configuration);
            services.AddMassTransitWithAssemblies(configuration, usersAssembly, accountingAssembly, catalogAssembly, customersAssembly, notificationsAssembly, ordersAssembly, shippingAssembly);
            services.AddJwtAuthentication(configuration);
            // IHttpContextAccessor gerekiyor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ILocalizationService, JsonLocalizationService>();
            services.AddSingleton<IClaimsPrincipalService, ClaimsPrincipalService>();

            // Add HTTP services
            services.AddHttpServices();

            return services;
        }
    }
}