namespace Inflow.Modules.Users.Core.Entities;

internal class Role
{
    public const string User = "user";
    public const string Admin = "admin";
    
    public string Name { get; set; }
    public IEnumerable<string> Permissions { get; set; }
    public IEnumerable<User> Users { get; set; }

    public static string Default => User;
}