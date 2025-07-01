namespace Users.Managers.Constants;

public static class ManagerConstants
{
  public static class Validation
  {
    public const int MinUserNameLength = 3;
    public const int MaxUserNameLength = 50;
    public const int MinPasswordLength = 8;
    public const int MaxPasswordLength = 100;
    public const int MinFirstNameLength = 2;
    public const int MaxFirstNameLength = 50;
    public const int MinLastNameLength = 2;
    public const int MaxLastNameLength = 50;
    public const int MaxPhoneNumberLength = 20;
    public const int MaxPhotoUrlLength = 500;
    public const int MaxEmailLength = 256;
    public const int MaxFailedLoginAttempts = 5;
    public const int AccountLockoutMinutes = 30;
    public const int PasswordExpiryDays = 90;
  }


  public static class Roles
  {
    public const string Admin = "Admin";
    public const string Manager = "Manager";
  }

  public static class Claims
  {
    public const string TenantId = "tenant_id";
    public const string IsAdmin = "is_admin";
    public const string IsManager = "is_manager";
    public const string FirstName = "first_name";
    public const string LastName = "last_name";
    public const string PhotoUrl = "photo_url";
  }
}