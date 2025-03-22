using AutoMapper;
using Company.MVC.BLL.Interfaces;
using Company.MVC.DAL.Models;
using Company.MVC.PL.DTOS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Company.MVC.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        // ASK CLR Create Object From IEmployeeRepository

        public EmployeeController(
            IEmployeeRepository employeeRepository , 
            IDepartmentRepository departmentRepository,
            IMapper mapper
            )
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        [HttpGet] // GET : /Department/Index
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = _employeeRepository.GetAll();
            }
            else
            {
                employees = _employeeRepository.GetByName(SearchInput);
            }

            // Dictionary   : 3 Property
            // 1. ViewData  : Transfer Extra Information From Controller (Action) To View

            //ViewData["Message"] = "Hello From ViewDat,a";

            // 2. ViewBag   : Transfer Extra Information From Controller (Action) To View

            //ViewBag.Massage = "Hello From ViewBag";

            //ViewBag.Message = new { Message = "Hello From ViewBag" };

            // 3. TempData  :


            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var departments = _departmentRepository.GetAll();
            ViewData["departments"] = departments ;
            return View();  
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDTO model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                try
                {
                    // Manual Mapping
                    //var employee = new Employee()
                    //{
                    //    Name = model.Name,
                    //    Addrees = model.Addrees,
                    //    Age = model.Age,
                    //    CreateAt = model.CreateAt,
                    //    HiringData = model.HiringData,
                    //    Email = model.Email,
                    //    IsActive = model.IsActive,
                    //    IsDeleted = model.IsDeleted,
                    //    Phone = model.Phone,
                    //    Salary = model.Salary,
                    //    DepartmentId = model.DepartmentId,
                    //};
                    var employee = _mapper.Map<Employee>(model);

                    var Count = _employeeRepository.Add(employee);
                    if (Count > 0)
                    {
                        TempData["Message"] = "Employee Is Created !! ";
                        return RedirectToAction(nameof(Index));
                    }

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError ("", ex.Message);
                }



            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null)
                return BadRequest("Invalid Id");

            var employee = _employeeRepository.Get(id.Value);

            if (employee is null)
                return NotFound(new { StatusCode = 404, message = $"Employee With Id {id} Is Not Found :(" });

            //var dto = _mapper.Map<CreateEmployeeDTO>(employee);

            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
          var departments = _departmentRepository.GetAll();
            ViewData["departments"] = departments;
            if (id is null)
                return BadRequest("Invalid Id");
            var employee = _employeeRepository.Get(id.Value);

            if (employee is null)
                return NotFound(new { StatusCode = 404, message = $"Department With Id {id} Is Not Found :(" });

            var dto = _mapper.Map<CreateEmployeeDTO>(employee);

            return View(dto);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee model)
        {
            if (ModelState.IsValid)
            {
                //if (id != model.Id) return BadRequest();
              if(ModelState.IsValid)
              {
                    if (id != model.Id) return BadRequest();
                    var count = _employeeRepository.Update(model);
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
              }              
            }
            return View(model);
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            return Details(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Employee model)
        {
            if (!ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest();

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
