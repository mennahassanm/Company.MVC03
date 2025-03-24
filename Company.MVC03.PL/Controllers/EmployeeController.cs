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

            var dto = _mapper.Map<CreateEmployeeDTO>(employee);

            return View(dto);
        }

        [HttpGet]
        public IActionResult Edit(int? id, string viewName = "Edit")
        {
          
            if (id is null)
                return BadRequest("Invalid Id");
            var employee = _employeeRepository.Get(id.Value);
            var departments = _departmentRepository.GetAll();
            ViewData["departments"] = departments;
            if (employee is null)
                return NotFound(new { StatusCode = 404, message = $"Department With Id {id} Is Not Found :(" });

            var dto = _mapper.Map<CreateEmployeeDTO>(employee);

            return View(viewName,dto);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDTO model, string viewName = "Edit")
        {
            if (ModelState.IsValid)
            {
                //if (id != model.Id) return BadRequest();

                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                var count = _employeeRepository.Update(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(viewName, model);
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            return Edit(id, "Delete");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, CreateEmployeeDTO model)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                var Count = _employeeRepository.Delete(employee);
                
                    if (Count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                
            }

            return View(model);
        }

    }
}
