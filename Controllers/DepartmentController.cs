using AutoMapper;
using Company_BLL.Interfaces;
using Company_DAL.Models;
using CompanyMangementSystem_PL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CompanyMangementSystem_PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public DepartmentController(IUnitOfWork unitOfWork , IMapper mapper , IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _env = env;
        }
        public async Task<IActionResult> Index(string searchInput)
        {
            if (!string.IsNullOrEmpty(searchInput))
            {
                var dept = _unitOfWork.Repository<Department>().SearchByNameAsync(searchInput);
                var mappeddept = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(dept);
                if (mappeddept.Count() == 0)
                {
                    ViewData["message"] = "Match This Name";
                }
                return View(mappeddept);

            }
            var departments = await _unitOfWork.Repository<Department>().GetAllAsync();
            var mappedDepartment = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel> >(departments);
            return View(mappedDepartment);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int id ,DepartmentViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                var mappedDepartment = _mapper.Map<DepartmentViewModel, Department>(viewModel);
                _unitOfWork.Repository<Department>().Add(mappedDepartment);
                var count = _unitOfWork.Complete();
                if (count>0)
                {
                    TempData["Message"] = "Department Created Successfuly";
                    
                }
                else
                {
                    TempData["Message"] = "An Error Occured While Creating Department";
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
            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(id.Value);
            var mappedDept = _mapper.Map<Department, DepartmentViewModel>(department);
            if (department == null)
                return NotFound();
            else
                return View(mappedDept);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            { return BadRequest(); }
            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(id.Value);
            var mappedDept = _mapper.Map<Department , DepartmentViewModel>(department);
            if (department == null)
                return NotFound();
            return View (mappedDept);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id,DepartmentViewModel department)
        {
            if (id != department.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                var mappedDept = _mapper.Map<DepartmentViewModel, Department>(department);
                _unitOfWork.Repository<Department>().Update(mappedDept);
                var count = _unitOfWork.Complete();
                if (count > 0)
                    return RedirectToAction("Index");
                else
                    return View(department);
            }
            return View(department);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest(); //400
            }
            var department =await _unitOfWork.Repository<Department>().GetByIdAsync(id.Value);
            var mappeddept = _mapper.Map<Department, DepartmentViewModel>(department);
            if (department == null)
                return NotFound();
            else
                return View(mappeddept);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute]int id ,DepartmentViewModel departmentVm)
        {
            if (id != departmentVm.Id)
            {
                return BadRequest();
            }
            try
            {
                var mappeddept = _mapper.Map<DepartmentViewModel, Department>(departmentVm);
                _unitOfWork.Repository<Department>().Delete(mappeddept);
                _unitOfWork.Complete();
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
                    ModelState.AddModelError(string.Empty, "An Error Occured During Deleting Department");
                }

            }
            return View(departmentVm);
        }

    }
}
