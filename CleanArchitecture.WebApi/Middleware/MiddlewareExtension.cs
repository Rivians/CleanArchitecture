namespace CleanArchitecture.WebApi.Middleware
{
    public static class MiddlewareExtension
    {
        // parametrede ki this IApplicationBuilder classına bir ekleme yaptıgımızı belirtir.
        public static IApplicationBuilder UseMiddlewareExtension(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            return app;
        }
    }
}
