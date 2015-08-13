using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VancouverSports.ViewModel;
using VancouverSports.Models;

namespace VancouverSports.Business_Logic
{
    public class SessionHelper
    {
        public const string SESSION_START = "Session_Start";
        public const string SESSION_END = "Session_End";
        public const int DEFAULT_QTY = 1;

        // Get data stored under the current session.
        // This data is stored on the server in a collection.
        public DateTime Start
        {
            get { return (DateTime)HttpContext.Current.Session[SESSION_START]; }
        }
        public DateTime End
        {
            get { return (DateTime)HttpContext.Current.Session[SESSION_END]; }
        }

        // Return value from session cookie manually if the session does not exist.
        public string SessionID
        {
            get
            {
                if (HttpContext.Current.Session.SessionID != null)
                    return HttpContext.Current.Session.SessionID;
                return null;
            }
        }
        public void Initialize()
        {
            HttpContext.Current.Session[SESSION_START] = DateTime.Now;
            HttpContext.Current.Session[SESSION_END] = DateTime.Now.AddMinutes(60);
        }
        public void UpdateSession()
        {
            if (SessionID == null)
                Initialize();
            HttpContext.Current.Session[SESSION_START] = DateTime.Now;
            HttpContext.Current.Session[SESSION_END] = DateTime.Now.AddMinutes(60);
        }
        public void Clear()
        {
            // remove cart items
            SessionManagement s = new SessionManagement();
            s.RemoveCartItems(SessionID);

            // then delete this session
            if (SessionID != null)
            {
                HttpContext.Current.Session.Clear(); // remove stored Prods
                HttpContext.Current.Session.Abandon();
            }
        }
        public void SetProductQty(int productID, int qty)
        {
            HttpContext.Current.Session[Convert.ToString(productID)] = qty;
        }
        public int GetProductQty(int productID)
        {
            if (HttpContext.Current.Session[Convert.ToString(productID)] != null)
                return (int)HttpContext.Current.Session[Convert.ToString(productID)];
            else
                return DEFAULT_QTY;
        }
    }
}