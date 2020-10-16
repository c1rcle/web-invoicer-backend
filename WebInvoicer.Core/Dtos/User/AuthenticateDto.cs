namespace WebInvoicer.Core.Dtos.User
{
    public class AuthenticateDto
    {
        public string Token { get; set; }

        public UserDataDto UserData { get; set; }
    }
}
