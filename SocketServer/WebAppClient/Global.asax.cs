using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebSocket4Net;

namespace WebAppClient
{
    public class MvcApplication : System.Web.HttpApplication
    {
        WebSocket websocket;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //打开socket 

            string url = "ws://127.0.0.1:2017";
           
            websocket = new WebSocket(url);
            websocket.Opened += websocket_Opened;
            websocket.Closed += websocket_Closed;
            websocket.MessageReceived += websocket_MessageReceived;
            websocket.Open();
            SocketClient.websocket = websocket;
        }

        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {

        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            SocketClient.websocket.Open();
        }

        private void websocket_Opened(object sender, EventArgs e)
        {
            SocketClient.websocket.Send("asp.net online");
        }
    }
}
