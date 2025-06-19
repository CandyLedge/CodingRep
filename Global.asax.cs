using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using CodingRep.service.stores;

namespace CodingRep
{
    public class Global : System.Web.HttpApplication
    {
        
        protected void Application_Start(object sender, EventArgs e)
        { 
            // TODO 这里仅供测试，到时候来个统一的store
            // EnvironmentStore envStore = new EnvironmentStore();
            // var emailAccount = Environment.GetEnvironmentVariable("NETEASE_EMAIL_ACCOUNT");
            // var emailAuthCode = Environment.GetEnvironmentVariable("NETEASE_EMAIL_AUTH_CODE");
            // Console.WriteLine($"Email Account: {emailAccount}, Auth Code: {emailAuthCode}");
        }
    }
}