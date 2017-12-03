/*
 源码己托管:http://git.oschina.net/kuiyu/dotnetcodes
 */

using SuperSocket.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SocketServer
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        WebSocketServer appServer ;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            appServer = new WebSocketServer();
            var SocketPort = System.Web.Configuration.WebConfigurationManager.AppSettings["SocketPort"].ToString();
            int port = Convert.ToInt32(SocketPort);
            if (!appServer.Setup(2000))
            {
                return;
            }

            //从配置中获取端口号
            if (!appServer.Start())
            {
                return;
            }

            //客户端连接
            appServer.NewSessionConnected += appServer_NewSessionConnected;
            //客户端接收消息
            appServer.NewMessageReceived += appServer_NewMessageReceived;
            //客户端关闭
            appServer.SessionClosed += appServer_SessionClosed;
            
        }

        void appServer_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            session.Close();
        }

        void appServer_NewMessageReceived(WebSocketSession session, string value)
        {
            //单条推送
            // session.Send(DateTime.Now.ToLocalTime()+" 客户端" +session.SessionID +":"+ value);

            //根据条件推送
            //var sessions = appServer.GetSessions(s => s.CompanyId == companyId);
            //foreach (var s in sessions)
            //{
            //    s.Send(data, 0, data.Length);
            //}
            
            //推送消息给所有客户端
            foreach (var s in appServer.GetAllSessions())
            {
                s.Send(value);
            }
        }

        void appServer_NewSessionConnected(WebSocketSession session)
        {
           
            session.Send("客户端：" + session.SessionID + "己上线");
        }

       
    }
}