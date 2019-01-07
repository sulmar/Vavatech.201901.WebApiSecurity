using BasicAuthentication.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace BasicAuthentication.Filters
{
    public class BasicAuthenticationFilter : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => false;

        //public bool AllowMultiple
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}


        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = context.Request.Headers.Authorization;

            if (authorization==null)
            {
                return;
            }

            if (authorization.Scheme != "Basic")
            {
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            // ..

            var result = ExtractLoginAndPassword(authorization.Parameter);

            if (!OnAuthorizeUser(result.login, result.password))
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid username or password", request);
                return;
            }
            else
            {
                IIdentity identity = new BasicAuthenticationIdentity(result.login, result.password);
                IPrincipal principal = new GenericPrincipal(identity, null);

                SetPrincipal(principal);
               
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

        protected virtual bool OnAuthorizeUser(string username, string password)
        {
            return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
        }

        // PM> Install-Package System.ValueTuple
        private (string login, string password) ExtractLoginAndPassword(string authHeader)
        {
            authHeader = Encoding.Default.GetString(Convert.FromBase64String(authHeader));

            var tokens = authHeader.Split(':');

            if (tokens.Length < 2)
            {
                return (null, null);
            }

            return (tokens[0], tokens[1]);
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var result = await context.Result.ExecuteAsync(cancellationToken);
            
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                result.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Basic"));
            }

            context.Result = new ResponseMessageResult(result);
           
        }
    }

    public class MyBasicAuthenticationFilter : BasicAuthenticationFilter
    {
        protected override bool OnAuthorizeUser(string username, string password)
        {
            return username == "user" && password == "P@ssw0rd";
        }
    }
}