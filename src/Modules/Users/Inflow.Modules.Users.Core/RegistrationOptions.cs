namespace Inflow.Modules.Users.Core;

internal class RegistrationOptions
{
    public bool Enabled { get; set; }
    public IEnumerable<string> InvalidEmailProviders { get; set; }
}