

namespace Users.Managers.Dtos
{
    public record ManagerDetailDto(
         Guid Id, 
        string FirstName, 
        string LastName, 
        string Email, 
        string PhoneNumber, 
        string UserName, 
        bool IsAdmin, 
        bool IsManager, 
        bool IsActive, 
        string TenantName,
        string BranchName);
}