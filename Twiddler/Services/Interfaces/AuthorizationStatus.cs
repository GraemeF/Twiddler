namespace Twiddler.Services.Interfaces
{
    public enum AuthorizationStatus
    {
        Unknown,
        Authorized,
        NotAuthorized,
        InvalidApplication,
        Verifying
    }
}