using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace NightMarket.Seedings
{
    //ITransientDependency tự động inject vào DJ với mode Transient
    public class IdentityDataSeeder : ITransientDependency, IIdentityDataSeeder
    {   
        protected IGuidGenerator GuidGenerator;
        protected IIdentityRoleRepository IdentityRoleRepository;
        protected IIdentityUserRepository IdentityUserRepository;
        protected ILookupNormalizer LookupNormalizer;
        protected IdentityUserManager IdentityUserManager;
        protected IdentityRoleManager IdentityRoleManager;
        protected ICurrentTenant CurrentTenant;
        protected IOptions<IdentityOptions> IdentityOptions;

        public IdentityDataSeeder(
            IGuidGenerator guidGenerator,
            IIdentityRoleRepository identityRoleRepository,
            IIdentityUserRepository identityUserRepository,
            ILookupNormalizer lookupNormalizer,
            IdentityUserManager identityUserManager,
            IdentityRoleManager identityRoleManager,
            ICurrentTenant currentTenant,
            IOptions<IdentityOptions> identityOptions
            )
        {
            GuidGenerator = guidGenerator;
            IdentityRoleRepository = identityRoleRepository;
            IdentityUserRepository = identityUserRepository;
            LookupNormalizer = lookupNormalizer;
            IdentityUserManager = identityUserManager;
            IdentityRoleManager = identityRoleManager;
            CurrentTenant = currentTenant;
            IdentityOptions = identityOptions;


        }
        [UnitOfWork]
        public virtual async Task<IdentityDataSeedResult> SeedAsync(string adminEmail, string adminPassword, Guid? tenantId = null)
        {
            using(CurrentTenant.Change(tenantId))
            {
                await IdentityOptions.SetAsync();

                var result = new IdentityDataSeedResult();
                //admin user
                //const string adminUsername = adminEmail;
                var adminUser = await IdentityUserRepository.FindByNormalizedUserNameAsync(
                    LookupNormalizer.NormalizeName(adminEmail)
                    );

                if (adminUser != null)
                {
                    return result;
                }
                adminUser = new IdentityUser(
                    GuidGenerator.Create(),
                    adminEmail,
                    adminEmail,
                    tenantId
                    )
                {
                    Name = "Admin"
                };

                (await IdentityUserManager.CreateAsync(adminUser, adminPassword, validatePassword: false)).CheckErrors();
                result.CreatedAdminUser = true;
                //admin role
                const string adminRoleName = "Admin";
                var adminRole = await IdentityRoleRepository.FindByNormalizedNameAsync(
                    LookupNormalizer.NormalizeName(adminRoleName));
                if (adminRole == null)
                {
                    adminRole = new IdentityRole(
                        GuidGenerator.Create(),
                        adminRoleName,
                        tenantId
                        )
                    {
                        IsStatic = true,
                        IsPublic = true
                    };
                    (await IdentityRoleManager.CreateAsync( adminRole )).CheckErrors();
                    result.CreatedAdminRole = true;
                }

                (await IdentityUserManager.AddToRoleAsync(adminUser,adminRoleName)).CheckErrors();
                return result;
            }
        }
    }
}
