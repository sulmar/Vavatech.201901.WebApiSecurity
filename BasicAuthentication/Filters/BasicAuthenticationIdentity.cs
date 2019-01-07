using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace BasicAuthentication.Filters
{
    public class BasicAuthenticationIdentity : GenericIdentity
    {
        private string password;

        public BasicAuthenticationIdentity(string name, string password)
            : base(name, "Basic")
        {
            this.password = password;
        }
    }
}