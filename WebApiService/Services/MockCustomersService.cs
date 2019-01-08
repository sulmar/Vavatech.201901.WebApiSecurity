using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiService.IServices;
using WebApiService.Models;

namespace WebApiService.Services
{
    public class MockCustomersService : ICustomersService
    {
        private static readonly IList<Customer> customers = new List<Customer>();

        public void Add(Customer customer)
        {
            customers.Add(customer);
        }

        public Customer Get(int id)
        {
            return customers.SingleOrDefault(c => c.Id == id);
        }
    }
}