using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.MVC.BLL.Interfaces;
using Company.MVC.DAL.Data.Contexts;
using Company.MVC.DAL.Models;

namespace Company.MVC.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department> , IDepartmentRepository
    {
        public DepartmentRepository(CompanyDbContext context) : base(context) // ASK CLR Create Object From CompanyDbContext
        {

        }
        //private CompanyDbContext _context; // Null

        //// ASK CLR Create Object From CompanyDbContext

        //public DepartmentRepository(CompanyDbContext context)
        //{
        //    _context = context;
        //}

        //public IEnumerable<Department> GetAll()
        //{
        //    return _context.Departments.ToList();
        //}
        //public Department? Get(int id)
        //{
        //    return _context.Departments.Find(id);
        //}
        //public int Add(Department model)
        //{
        //    _context.Departments.Add(model);
        //    return _context.SaveChanges();
        //}
        //public int Update(Department model)
        //{
        //    _context.Departments.Update(model);
        //    return _context.SaveChanges();
        //}
        //public int Delete(Department model)
        //{
        //    _context.Departments.Remove(model);
        //    return _context.SaveChanges();
        //}
    }
}
