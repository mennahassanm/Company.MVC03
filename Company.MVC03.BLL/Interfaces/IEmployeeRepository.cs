using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.MVC.DAL.Models;

namespace Company.MVC.BLL.Interfaces
{
    public interface IEmployeeRepository :IGenericRepository<Employee>
    {
        //Employee? GetEmployee(string Name);
        //IEnumerable<Employee> GetAll();
        //Employee? Get(int id);
        //int Add(Employee model);
        //int Update(Employee model);
        //int Delete(Employee model);
    }
}
