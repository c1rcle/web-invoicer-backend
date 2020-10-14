using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace WebInvoicer.Core.Token
{
    public class TokenUtility
    {
        public static bool Validate(string userEmail, IHeaderDictionary httpHeaders)
        {
            var token = ParseFromHeaders(httpHeaders);
            if (token == null)
            {
                return false;
            }

            var securityToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
            var tokenEmail = securityToken.Payload["email"] as string;

            var isUserAuthorized = tokenEmail == userEmail;
            var tokenExpired = securityToken.ValidTo < DateTime.UtcNow;

            return isUserAuthorized && !tokenExpired;
        }

        private static string ParseFromHeaders(IHeaderDictionary httpHeaders)
        {
            if (httpHeaders.TryGetValue("Authorization", out var authorizationString))
            {
                return null;
            }

            var token = authorizationString.ToString().Split(' ')[1];
            return token;
        }
    }
}
