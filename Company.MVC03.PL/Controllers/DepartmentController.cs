using Company.MVC.BLL.Interfaces;
using Company.MVC.BLL.Repositories;
using Company.MVC.DAL.Models;
using Company.MVC.PL.DTOS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace Company.MVC.PL.Controllers
{
    // MVC Controller
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        // ASK CLR Create Object From DepartmentRepository

        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet] // GET : /Department/Index
        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();

            return View(departments); 
        }

        [HttpGet] 
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDTO model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt,
                };

               var Count = _departmentRepository.Add(department);
                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }


            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id is null)  
                return BadRequest("Invalid Id");

            var department =_departmentRepository.Get(id.Value);

            if(department is null)
                return NotFound(new {StatusCode = 404 , message = $"Department With Id {id} Is Not Found :("}); 
              
            return View(department);
        }

    }
}
