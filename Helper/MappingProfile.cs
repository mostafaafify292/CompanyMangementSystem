using AutoMapper;
using Company_DAL.Models;
using CompanyMangementSystem_PL.ViewModels;

namespace CompanyMangementSystem_PL.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Department , DepartmentViewModel>().ReverseMap();
            CreateMap<Employee , EmployeeViewModel>().ReverseMap();
            
        }
    }
}
