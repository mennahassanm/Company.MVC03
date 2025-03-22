using AutoMapper;
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
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;


        // ASK CLR Create Object From DepartmentRepository

        public DepartmentController(
            IEmployeeRepository employeeRepository,
            IDepartmentRepository departmentRepository,
            IMapper mapper
            )
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
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
        public IActionResult Details(int? id , string viewName = "Details")
        {
            if (id is null)  
                return BadRequest("Invalid Id");

            var department =_departmentRepository.Get(id.Value);

            if(department is null)
                return NotFound(new {StatusCode = 404 , message = $"Department With Id {id} Is Not Found :("}); 
              
            return View(viewName , department);
        }

        [HttpGet]
        public IActionResult Edit (int? id)
        {
            if (id is null)
                return BadRequest("Invalid Id");

            var department = _departmentRepository.Get(id.Value);

            if (department is null)
                return NotFound(new { StatusCode = 404, message = $"Department With Id {id} Is Not Found :(" });
            var departmentDTO = new CreateDepartmentDTO()
            {
                Code = department.Code,
                Name = department.Name,
                CreateAt = department.CreateAt,
            };
            return View(departmentDTO);
        }

        //[HttpPost]
        //public IActionResult Edit([FromRoute] int id ,Department department)
        //{
        //    if (id == department.Id)
        //    {
        //        var Count = _departmentRepository.Update(department);

        //        if (ModelState.IsValid)
        //        {
        //            if (Count > 0)
        //            {
        //                return RedirectToAction(nameof(Index));
        //            }
        //        }
        //    }
        //    return View(department);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateDepartmentDTO model)
        {
            if (ModelState.IsValid) 
            {
                //if (id != model.Id) return BadRequest();
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt,
                };

                var Count = _departmentRepository.Update(department);

                if (Count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            return View(model);
        }

        //[HttpPost]
        ////[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, UpdateDepartmentDTO model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var department = new Department()
        //        {
        //            Id = id,
        //            Name = model.Name,
        //            Code = model.Code,
        //            CreateAt = model.CreateAt,
        //        };
        //        var Count = _departmentRepository.Update(department);

        //        if (Count > 0)
        //        {
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }

        //    return View(model);
        //}

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete( int? id)
        {
            //if (id is null)
            //    return BadRequest("Invalid Id");

            //var department = _departmentRepository.Get(id.Value);

            //if (department is null)
            //    return NotFound(new { StatusCode = 404, message = $"Department With Id {id} Is Not Found :(" });

            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Department department)
        {
            if (id != department.Id) return BadRequest();
            {
                var Count = _departmentRepository.Delete(department);

                if (ModelState.IsValid)
                {
                    if (Count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View(department);
        }


    }
}
