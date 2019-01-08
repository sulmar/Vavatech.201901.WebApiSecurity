using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OAuthAuthentication.Providers
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {

        private static ConcurrentDictionary<string, AuthenticationTicket> refreshTickets;


        public RefreshTokenProvider()
        {
            refreshTickets = new ConcurrentDictionary<string, AuthenticationTicket>();
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
            {
                IssuedUtc = context.Ticket.Properties.IssuedUtc,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(10)
            };

            var refreshToken = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);

            var guid = Guid.NewGuid().ToString();

            refreshTickets.TryAdd(guid, refreshToken);

            context.SetToken(guid);

        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            if (refreshTickets.TryRemove(context.Token, out AuthenticationTicket ticket))
            {
                context.SetTicket(ticket);
            }
        }
    }
}