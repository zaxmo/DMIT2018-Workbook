
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#region Additional Namespaces
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ChinookSystem.DAL;
using ChinookSystem.BLL;
#endregion

namespace ChinookSystem
{
    public static class StartupExtensions
    {
        public static void AddBackendDependencies(this IServiceCollection services,
                Action<DbContextOptionsBuilder> options)
        {
            //add the context class of your application library to the service collection
            //pass in the connection string options.
            services.AddDbContext<ChinookContext>(options);

            //add any business logic layer class to the service collection so our
            //  web app has access to the methods within the BLL class.
            services.AddTransient<AboutServices>((serviceProvider) =>
            {
                //get the dbcontext class
                var context = serviceProvider.GetRequiredService<ChinookContext>();
                return new AboutServices(context);
            });

            services.AddTransient<AlbumServices>((serviceProvider) =>
            {
                //get the dbcontext class
                var context = serviceProvider.GetRequiredService<ChinookContext>();
                return new AlbumServices(context);
            });

            services.AddTransient<GenreServices>((serviceProvider) =>
            {
                //get the dbcontext class
                var context = serviceProvider.GetRequiredService<ChinookContext>();
                return new GenreServices(context);
            });
        }
    }
}
