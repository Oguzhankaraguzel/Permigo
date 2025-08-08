namespace Domain.Entities.User;

public static class Roles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string Manager = "Manager";
    public const string Employee = "Employee";
    public static readonly string[] AllRoles = { SuperAdmin, Manager, Employee };
    public static bool IsValidRole(string role)
    {
        return AllRoles.Contains(role);
    }
}
