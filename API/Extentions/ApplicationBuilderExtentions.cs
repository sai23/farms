using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extentions
{
    public static class ApplicationBuilderExtentions
    {
        public static IApplicationBuilder AddApplicationBuilders(this IApplicationBuilder app) 
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            //This is right place to write logic for cors
            app.UseCors(x=> x.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            return app;

        }
    }
}