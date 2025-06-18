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
        // 实例化 EnvironmentStore 以加载环境变量
        var envStore = new EnvironmentStore();
        }
    }
}