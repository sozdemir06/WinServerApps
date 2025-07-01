using Users.AppRoles.Models;

namespace Users.RoleGroups.models
{
    public class RoleGroupItem : Entity<Guid>
    {
        public Guid RoleGroupId { get; private set; }
        public RoleGroup? RoleGroup { get; private set; }
        public Guid AppRoleId { get; private set; }
        public AppRole? AppRole { get; private set; }

        private RoleGroupItem() { }

        public static async Task<RoleGroupItem> Create(Guid roleGroupId, Guid appRoleId)
        {
            if (roleGroupId == Guid.Empty)
                throw new ArgumentException("RoleGroupId cannot be empty.", nameof(roleGroupId));
            if (appRoleId == Guid.Empty)
                throw new ArgumentException("AppRoleId cannot be empty.", nameof(appRoleId));

            var roleGroupItem = new RoleGroupItem
            {
                Id = Guid.CreateVersion7(),
                RoleGroupId = roleGroupId,
                AppRoleId = appRoleId
            };
            return await Task.FromResult(roleGroupItem);
        }

        public void Update(Guid appRoleId, Guid roleGroupId)
        {
            if (roleGroupId == Guid.Empty)
                throw new ArgumentException("RoleGroupId cannot be empty.", nameof(roleGroupId));
            if (appRoleId == Guid.Empty)
                throw new ArgumentException("AppRoleId cannot be empty.", nameof(appRoleId));
            AppRoleId = appRoleId;
            RoleGroupId = roleGroupId;
        }
    }
}