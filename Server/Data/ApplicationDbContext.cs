using Blazor_App_With_Auth.Api.Models;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Blazor_App_With_Auth.Api.Data
{
    public class ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions

    ) : ApiAuthorizationDbContext<ApplicationUser>(options, operationalStoreOptions)
    {
    }
}
