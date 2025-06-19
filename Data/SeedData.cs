using DNUResourceBooker.Models;
using System.Linq;

namespace DNUResourceBooker.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { RoleName = "Admin" },
                    new Role { RoleName = "User" }
                );
                context.SaveChanges();
            }
            if (!context.ResourceCategories.Any())
            {
                context.ResourceCategories.AddRange(
                    new ResourceCategory { Name = "Phòng học" },
                    new ResourceCategory { Name = "Phòng họp" },
                    new ResourceCategory { Name = "Máy chiếu" }
                );
                context.SaveChanges();
            }
        }
    }
}