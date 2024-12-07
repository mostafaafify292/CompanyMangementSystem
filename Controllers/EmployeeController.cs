using System.Drawing;
using AutoMapper;
using Company_BLL.Interfaces;
using Company_DAL.Models;
using CompanyMangementSystem_PL.Helper;
using CompanyMangementSystem_PL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CompanyMangementSystem_PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _env = env;
        }
        public async Task<IActionResult> Index(string searchInput)
        {
            if (!string.IsNullOrEmpty(searchInput))
            {
                var emp = _unitOfWork.Repository<Employee>().SearchByNameAsync(searchInput);
                var mappedemp = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(emp);
                if (mappedemp.Count()==0)
                {
                    ViewData["message"] = "Match This Name";
                }
                return View(mappedemp);

            }
            var Employees = await _unitOfWork.Repository<Employee>().GetAllAsync();
            var mappedEmployee = _mapper.Map<IEnumerable<Employee>,IEnumerable<EmployeeViewModel>>(Employees);
            return View(mappedEmployee);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {   
            ViewData["Departments"] = await _unitOfWork.Repository<Department>().GetAllAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int id, EmployeeViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                viewModel.ImageName = DocumentSetting.UploadFile(viewModel.Image, "Images");
                var mappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(viewModel);
                _unitOfWork.Repository<Employee>().Add(mappedEmployee);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    TempData["Message"] = "Employee Created Succesfully";
   
                }
                else
                {
                    TempData["Message"] = "An Error Occured";
                }
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest();
            }
            var Employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(id.Value);
            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(Employee);
            if (Employee == null)
                return NotFound();
            else
                return View(mappedEmp);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            { return BadRequest(); }
            var Employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(id.Value);
            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(Employee);
            if (Employee == null)
                return NotFound();
            ViewData["Departments"] = await _unitOfWork.Repository<Department>().GetAllAsync();
            return View(mappedEmp);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, EmployeeViewModel Employee)
        {
            if (id != Employee.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                Employee.ImageName = DocumentSetting.UploadFile(Employee.Image, "Images");
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(Employee);
                _unitOfWork.Repository<Employee>().Update(mappedEmp);
                var count = _unitOfWork.Complete();
                if (count > 0)
                    return RedirectToAction("Index");
                else
                    return View(Employee);
            }
            return View(Employee);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest(); //400
            }
            var Employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(id.Value);
            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(Employee);
            if (Employee == null)
                return NotFound();
            else
                return View(mappedEmp);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, EmployeeViewModel EmployeeVm)
        {
            if (id != EmployeeVm.Id)
            {
                return BadRequest();
            }
            try
            {
                var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(EmployeeVm);
                _unitOfWork.Repository<Employee>().Delete(mappedEmp);
                _unitOfWork.Complete();
                DocumentSetting.DeleteFile(EmployeeVm.ImageName, "Images");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An Error Occured During Deleting Employee");
                }

            }
            return View(EmployeeVm);
        }
    }
}
