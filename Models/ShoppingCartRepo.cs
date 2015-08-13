using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VancouverSports.ViewModel;
using VancouverSports.Business_Logic;

namespace VancouverSports.Models
{
    public class ShoppingCartRepo
    {
        public List<Product> ViewProducts()
        {
            // reference db
            ShoppingCartEntities context = new ShoppingCartEntities();

            // query and return all products
            return context.Products.ToList();

        }

        public int GetProductQty(string sessionID, int productID)
        {
            ShoppingCartEntities context = new ShoppingCartEntities();
            ProductVisit p = context.ProductVisits.Find(sessionID, productID);
            if (p == null)
                return 1;
            else
                return (int)p.qtyOrdered;
        }
        public Product GetProduct(int productID)
        {
            // reference db
            ShoppingCartEntities context = new ShoppingCartEntities();

            // query info on ProductID
            var product = context.Products.Find(productID);
            return product;
        }


        public void SaveVisit(string sessionID, DateTime start)
        {
            // reference the db
            ShoppingCartEntities context = new ShoppingCartEntities();
            
            // create and initialize the visit object
            Visit v = new Visit();
            v.sessionID = sessionID;
            v.started = start;

            
            // add and save object in Product table
            if (GetVisit(sessionID)==null) {
                context.Visits.Add(v);
                context.SaveChanges();
            }
        }

        public Visit GetVisit(string sessionID)
        {
            // reference db
            ShoppingCartEntities context = new ShoppingCartEntities();

            // query and return info on sessionID
            var session = context.Visits.Find(sessionID);
            return session;
        }
        public void UpdateShoppingCart(string sessionID, IEnumerable<ProductPicked> items)
        {
            if (items == null) { return; }

            // reference db
            ShoppingCartEntities context = new ShoppingCartEntities();

            foreach (ProductPicked item in items)
            {
                // If item is already in the cart, update qty
                // othherwise, just toss that item into the cart
                ProductVisit row = context.ProductVisits.Find(sessionID, item.ProductID);
                if (row == null)
                {
                    // add new item to cart
                    ProductVisit pv = new ProductVisit();
                    pv.sessionID = sessionID;
                    pv.productID = item.ProductID;
                    pv.qtyOrdered = item.QtyOrdered;
                    pv.updated = DateTime.Now;

                    context.ProductVisits.Add(pv);
                }
                // update existing item in cart
                else 
                {
                    row.qtyOrdered = item.QtyOrdered;
                    row.updated = DateTime.Now;
                }
            }
            // commit
            context.SaveChanges();
        }
        public void SaveProductVisit(string sessionID, int productID, string productName, decimal price, int qty)
        {
            // reference db
            ShoppingCartEntities context = new ShoppingCartEntities();

            // If item is already in the cart, update qty
            // othherwise, just toss that item into the cart
            ProductVisit row = context.ProductVisits.Find(sessionID, productID);
            if (row == null)
            {
                // create and initialize the ProductVisit object
                ProductVisit pv = new ProductVisit();
                pv.sessionID = sessionID;
                pv.productID = productID;
                pv.qtyOrdered = qty;
                pv.updated = DateTime.Now;

                context.ProductVisits.Add(pv);
            }
            else {
                row.qtyOrdered = qty;
                row.updated = DateTime.Now;
            }
            // commit
            context.SaveChanges();
            
        }

        public  IEnumerable<ProductPicked> FillShoppingCart(string sessionID)
        {
            // reference db
            ShoppingCartEntities context = new ShoppingCartEntities();

            // get all items with specified sessionID (cart)
            var items = context.ProductVisits.Where(
                s => s.sessionID == sessionID).Select(p => new
                {
                    ProductID = p.productID,
                    QtyOrdered = p.qtyOrdered,
                    Updated = p.updated
                }).ToList();

            //// create a shopping cart 
            //ShoppingCart cart = new ShoppingCart();

            //// associate shopping cart to sessionID
            //cart.SessionID = sessionID;
            //SessionHelper session = new SessionHelper();
            //cart.Started = session.Start;

            //TODO: create a separate method for this
            // accumulate a list of products picked
            List<ProductPicked> listOfProductsPicked = new List<ProductPicked>();
            foreach (var row in items)
            {
                // get product details
                var itemDetail = context.Products.Find(row.ProductID);  // get item details

                
                ProductPicked picked = new ProductPicked();
                picked.ProductID = (int)itemDetail.productID;
                picked.ProductName = (string)itemDetail.productName;
                picked.Price = (decimal)itemDetail.price;
                picked.QtyOrdered = (int)row.QtyOrdered;
                picked.Updated = Convert.ToDateTime(row.Updated);

                listOfProductsPicked.Add(picked);
                
                //// add to cart TODO: separate from foreach
                //cart.Products = listOfProductsPicked;
            }

            return listOfProductsPicked;


        }
        
    }
}