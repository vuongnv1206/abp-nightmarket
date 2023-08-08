using NightMarket.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace NightMarket.Admin.Permissions;

public class NightMarketPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(NightMarketPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(NightMarketPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<NightMarketResource>(name);
    }
}
