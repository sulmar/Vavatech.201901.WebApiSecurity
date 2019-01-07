using JWTAuthentication.ActionResults;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace JWTAuthentication.Filters
{
    public class JWTAuthenticationFilter : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = context.Request.Headers.Authorization;

            if (authorization == null)
            {
                return;
            }

            if (authorization.Scheme != "Bearer")
            {
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            var requestToken = authorization.Parameter;

            string secretKey = WebConfigurationManager.AppSettings["SecretKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = securityKey,
                ValidAudiences = new string[] { "http://www.example.com" },
                ValidIssuers = new string[] { "self" }
            };

            try
            {
                IPrincipal principal = tokenHandler
                    .ValidateToken(requestToken, 
                                    tokenValidationParameters, 
                                out SecurityToken validatedToken);

                SetPrincipal(principal);


            }
            catch(Exception e)
            {

            }
        }

        private void SetPrincipal(IPrincipal principal)
        {
            // dla standardowych aplikacji .NET
            Thread.CurrentPrincipal = principal;

            // dla aplikacji ASP.NET
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var result = await context.Result.ExecuteAsync(cancellationToken);

            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                result.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Bearer", "error=\"invalid_token\""));
            }

            context.Result = new ResponseMessageResult(result);
        }
    }
}