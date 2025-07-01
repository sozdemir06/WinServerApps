using Shared.Dtos;

namespace Shared.Services.Http
{
    public interface IGetManagerRoleService
    {
        Task<IEnumerable<ManagerRoleDto>> GetManagerRolesAsync(Guid managerId, CancellationToken cancellationToken = default);
    }
}