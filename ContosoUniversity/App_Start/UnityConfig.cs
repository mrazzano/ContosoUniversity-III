using System;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using Microsoft.Practices.Unity;
using ContosoUniversity.Core.Dto;
using ContosoUniversity.Core.Models;
using AutoMapper;

namespace ContosoUniversity
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<HttpClient>(
                  new InjectionFactory(x =>
                      new HttpClient { BaseAddress = new Uri(ConfigurationManager.AppSettings["ApiUrl"]) }));

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Student, StudentDto>().ReverseMap();
                cfg.CreateMap<Enrollment, EnrollmentDto>().ReverseMap();

                cfg.CreateMap<Course, CourseDto>();
                cfg.CreateMap<CourseDto, Course>()
                    .ForMember(dest => dest.Department, opt => opt.MapFrom(
                        src => new Department()
                        {
                            DepartmentId = src.DepartmentId,
                            Name = src.DepartmentName
                        }));

                cfg.CreateMap<Department, DepartmentDto>().ReverseMap();
                cfg.CreateMap<DepartmentDto, Department>()
                    .ForMember(dest => dest.Administrator, opt => opt.MapFrom(
                        src => src.InstructorId.HasValue ? new Instructor
                        {
                            Id = src.InstructorId.Value,
                            FirstMidName = src.AdministratorFirstMidName,
                            LastName = src.AdministratorLastName,

                        } : null));

                cfg.CreateMap<Instructor, InstructorDto>();
                cfg.CreateMap<InstructorDto, Instructor>()
                    .ForMember(dest => dest.OfficeAssignment, opt => opt.MapFrom(
                        src => src.OfficeAssignmentLocation != null ? new OfficeAssignment()
                        {
                            InstructorId = src.Id,
                            Location = src.OfficeAssignmentLocation
                        } : null));
            });
            container.RegisterInstance(config.CreateMapper());

            DependencyResolver.SetResolver(new Unity.Mvc5.UnityDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
        }
    }
}