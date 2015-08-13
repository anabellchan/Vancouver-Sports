using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VancouverSports.Business_Logic;
using VancouverSports.Models;
using VancouverSports.ViewModel;

namespace VancouverSports.Controllers
{
    public class HomeController : Controller
    {
        ShoppingCartRepo cartRepo = new ShoppingCartRepo();
        // GET: Home
        public ActionResult Index()
        {
            return View(cartRepo.ViewProducts());
        }

        [HttpGet]
        public ActionResult AddProduct(int productID)
        {
           SessionHelper session = new SessionHelper();
           ViewBag.qty = session.GetProductQty(productID);
           ViewBag.qty = cartRepo.GetProductQty(session.SessionID, productID);
           return View(cartRepo.GetProduct(productID));

        }

        [HttpPost]
        public ActionResult AddProduct(int productID, string productName, decimal price, int qty)
        {
           
            ShoppingCartRepo cartRepo = new ShoppingCartRepo();
            SessionHelper session = new SessionHelper();
            if (ModelState.IsValid)
            {
                session.SetProductQty(productID, qty);
                cartRepo.SaveProductVisit(session.SessionID, productID, productName, price, qty);
                return RedirectToAction("ViewCart", new { sessionID = session.SessionID });
            }
            else
            {
                ViewBag.qty = session.GetProductQty(productID);
                return View(cartRepo.GetProduct(productID));
            }
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ViewCart(string sessionID)
        {
            if (sessionID == null)
            {
                SessionHelper session = new SessionHelper();
                sessionID = session.SessionID;
            }
            IEnumerable<ProductPicked> items = new List<ProductPicked>();
            items = cartRepo.FillShoppingCart(sessionID);
            return View(items);
        }
        [HttpPost]
        public ActionResult ViewCart(IEnumerable<ViewModel.ProductPicked> item)
        {
            if (ModelState.IsValid)
            {
                SessionHelper session = new SessionHelper();
                cartRepo.UpdateShoppingCart(session.SessionID, item);
                return RedirectToAction("SaveOrder", "Home", new {sessionID = session.SessionID});
            }
            else
            {
                return RedirectToAction("ViewCart", "Home", Session.SessionID);
            }
        }


        public ActionResult SaveOrder(string sessionID)
        {
            IEnumerable<ProductPicked> items = new List<ProductPicked>();
            items = cartRepo.FillShoppingCart(sessionID);
            return View(items);

        }

        public ActionResult ThankYou(int id)
        {
            if (id==1)
            {
                SessionHelper session = new SessionHelper();
                session.Clear();
            }
            return View();
        }
    }
}