using Microsoft.EntityFrameworkCore;
using OpenIddict;

namespace repro_openiddict_passwordgrant.Models
{
    public class ApplicationDbContext : OpenIddictDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }

    public class ApplicationUser : OpenIddictUser
    {
    }
}