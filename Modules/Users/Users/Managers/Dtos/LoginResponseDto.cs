

namespace Users.Managers.Dtos
{
    public record LoginResponseDto(
    string Token, string UserName,
    string Email,IEnumerable<string>? UserRoles = null
    );  

}