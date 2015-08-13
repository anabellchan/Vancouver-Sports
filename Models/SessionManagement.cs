using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VancouverSports.Models
{
    public class SessionManagement
    {
        public ShoppingCartEntities context = new ShoppingCartEntities();
        public void RemoveCartItems(string sessionID)
        {
            IEnumerable<ProductVisit> items = (from pv in context.ProductVisits
                                               where pv.sessionID == sessionID
                                               select pv);
            foreach (var i in items)
            {
                context.ProductVisits.Remove(i);
            }
            context.SaveChanges();
        }
        public void RemoveAbandonedItems(Visit s, IEnumerable<ProductVisit> items)
        {
            TimeSpan timeout = new TimeSpan(1, 0, 0);
            Console.WriteLine("Cart is not empty.");
            // delete those that are abandoned
            bool deleted = false;
            foreach (ProductVisit item in items)
            {
                Console.WriteLine(item.productID);
                TimeSpan diff = new TimeSpan();

                if ((item.updated != null) && (s.started != null))
                {
                    DateTime timeUpdated = item.updated.Value;
                    DateTime timeStarted = s.started.Value;
                    diff = timeUpdated - timeStarted;
                    Console.WriteLine("Time difference: " + diff);

                }
                if (diff > timeout)
                {
                    Console.WriteLine("Diff is: " + diff);
                    Console.WriteLine("Deleting...");
                    context.ProductVisits.Remove(item);
                    deleted = true;
                }
            }
            // remove the empty cart
            if (deleted)
            {
                Console.WriteLine("Deleting empty cart...");
                context.Visits.Remove(s);
            }
        }
        public void RemoveAbandonedCart(Visit s)
        {
            TimeSpan timeout = new TimeSpan(1, 0, 0);

            DateTime timeStarted = s.started.Value;
            DateTime now = DateTime.Now;
            TimeSpan diff = new TimeSpan();

            diff = now - timeStarted;
            // delete the session instead, if it's abandoned.
            if (diff > timeout)
                context.Visits.Remove(s);

        }
        public void RemoveExpiredSessions()
        {
            // list all sessions
            IEnumerable<Visit> sessions = (from v in context.Visits
                                           select v).ToList();
            foreach (var s in sessions)
            {
                // get all items in the cart
                IEnumerable<ProductVisit> items = (from pv in context.ProductVisits
                                                   where pv.sessionID == s.sessionID
                                                   select pv);
                // if cart has items,
                if (items.Count() > 0)
                    RemoveAbandonedItems(s, items);
                else  // if cart is  empty
                    RemoveAbandonedCart(s);
            }
            context.SaveChanges();
        }

        public void RemoveAllSessions()
        {
            TimeSpan timeout = new TimeSpan(1, 0, 0);

            // First, remove items in the cart
            var query = context.Visits.Select(v => new { SessionID = v.sessionID });
            foreach (var s in query)
            {
                // get all items in the cart
                IEnumerable<ProductVisit> items = (from pv in context.ProductVisits
                                                   where pv.sessionID == s.SessionID
                                                   select pv);
                // remove them from the cart
                foreach (var item in items)
                {
                    context.ProductVisits.Remove(item);
                }

            }
            // now that shopping cart is empty
            // delete the shopping cart
            foreach (var s in query)
            {
                Visit c = (from v in context.Visits
                           where v.sessionID == s.SessionID
                           select v).FirstOrDefault();
                context.Visits.Remove(c);
            }

            //Save all changes made to DB
            context.SaveChanges();
        }

    }
        
}