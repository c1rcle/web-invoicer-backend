namespace WebInvoicer.Core.Utility
{
    public class TokenData
    {
        public string JwtSecret { get; }

        public int TokenExpiryTime { get; }

        public TokenData(string jwtSecret, int tokenExpiryTime)
        {
            JwtSecret = jwtSecret;
            TokenExpiryTime = tokenExpiryTime;
        }
    }
}
