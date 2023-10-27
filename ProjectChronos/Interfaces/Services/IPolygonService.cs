namespace ProjectChronos.Interfaces.Services
{
    public interface IPolygonService
    {
        // Generate a message to be signed by the user's wallet
        string GenerateAuthMessage(string address);

        Task<bool> ValidateSignedMessageAsync(string address, string message, string signature);
    }
}
