using ASC.Web.Services;

namespace ASC.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMyDependencyGroup(this IServiceCollection services)
        {
            // Add memory cache
            services.AddMemoryCache();

            // Add session
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Add navigation cache operations
            services.AddSingleton<INavigationCacheOperations, NavigationCacheOperations>();

            return services;
        }
    }
}
