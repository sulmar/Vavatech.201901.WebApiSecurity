using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiService.Models;

namespace WebApiService.IServices
{
    public interface ICustomersService
    {
        Customer Get(int id);
        void Add(Customer customer);
    }
}
