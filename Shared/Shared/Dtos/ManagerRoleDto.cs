
namespace Shared.Dtos
{
    public record ManagerRoleDto(
            Guid Id,
            string Name,
            string NormalizedName,
            string? Description);
}