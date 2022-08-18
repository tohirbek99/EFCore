using EFCore.Data;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Models
{
    public class SeedData
    {
        public static void  Initializee(IServiceProvider serviceProvider)
        {
            using (var context = new DataContext
                (serviceProvider.GetRequiredService<DbContextOptions<DataContext>>()))
            {
                if (context.Pages.Any())
                {
                    return;
                }
                context.Pages.AddRange(
                    new Page
                    {
                        Title = "Home",
                        Slug="home",
                        Content="Home page",
                        Sorting=0
                    },
                    new Page
                    {
                        Title = "Contect",
                        Slug="contect",
                        Content="contect page",
                        Sorting=0
                    },

                    new Page
                    {
                        Title = "Server",
                        Slug="server",
                        Content="server page",
                        Sorting=0
                    }

                    );
                context.SaveChanges();
            }
        }
    }
}
