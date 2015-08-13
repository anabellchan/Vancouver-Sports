using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using VancouverSports.Models;
using VancouverSports.ViewModel;
using VancouverSports.Business_Logic;

namespace VancouverSports
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Session_Start()
        {
            // Remove abandoned sessions
            SessionManagement s = new SessionManagement();
            s.RemoveExpiredSessions();

            SessionHelper session = new SessionHelper();

            if (Request.Cookies["ASP.NET_SessionId"] == null)
                session.Initialize();
            else
                session.UpdateSession();


            ShoppingCartRepo cartRepo = new ShoppingCartRepo();
            cartRepo.SaveVisit(session.SessionID, session.Start);
            
           
        }
        protected void Session_End() {
            //// remove current session
            //if (HttpContext.Current.Session != null)
            //{
            //    HttpContext.Current.Session.Clear();    // remove collection items
            //    HttpContext.Current.Session.Abandon();  // cancel current session
            //}

            // remove abandoned sessions
            SessionManagement s = new SessionManagement();
            s.RemoveExpiredSessions();
            
        }
        protected void Appliation_End()
        {
            int x = 3;
        }
    }
}
