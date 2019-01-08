using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApiService.IServices;
using WebApiService.Models;

namespace WebApiService.Controllers
{
    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public class CustomersController : Controller
    {
        private readonly ICustomersService customersService;

        //public CustomersController(ICustomersService customersService)
        //{
        //    this.customersService = customersService;
        //}

        [HttpGet]
        public ActionResult Index()
        {


            return View(new Customer());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(Customer customer)
        {
           //  customer.FirstName = HttpUtility.HtmlEncode(customer.FirstName);

            ViewBag.Customer = "</script><script>alert('hello');</script><script>"; ;

            return View(customer);
        }
    }
}