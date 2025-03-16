using Company.MVC.BLL.Interfaces;
using Company.MVC.DAL.Models;
using Company.MVC.PL.DTOS;
using Microsoft.AspNetCore.Mvc;

namespace Company.MVC.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        // ASK CLR Create Object From IEmployeeRepository

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet] // GET : /Department/Index
        public IActionResult Index()
        {
            var employees = _employeeRepository.GetAll();

            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDTO model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var employee = new Employee()
                {
                    Name = model.Name,
                    Addrees = model.Addrees,
                    Age = model.Age,
                    CreateAt = model.CreateAt,
                    HiringData = model.HiringData,
                    Email = model.Email,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    Phone = model.Phone,
                    Salary = model.Salary,
                };

                var Count = _employeeRepository.Add(employee);
                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                


            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest("Invalid Id");

            var employee = _employeeRepository.Get(id.Value);

            if (employee is null)
                return NotFound(new { StatusCode = 404, message = $"Employee With Id {id} Is Not Found :(" });

            return View(viewName, employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee model)
        {
            if (ModelState.IsValid);
            {
                if (id != model.Id) return BadRequest();
                var Count = _employeeRepository.Update(model);

                    if (Count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                
            }
            return View(model);
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Employee model)
        {
            if (id != model.Id) return BadRequest();
            {
                var Count = _employeeRepository.Delete(model);

                if (ModelState.IsValid)
                {
                    if (Count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View(model);
        }

    }
}
