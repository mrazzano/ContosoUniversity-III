using System;
using System.Data.Entity;
using Microsoft.Practices.Unity;
using ContosoUniversity.Core.Dto;
using ContosoUniversity.Core.Models;
using ContosoUniversity.Infrastructure.Logging;
using ContosoUniversity.Core.Persistence;
using ContosoUniversity.Infrastructure.Persistence;
using AutoMapper;
using ContosoUniversity.Infrastructure.Database;

namespace ContosoUniversity.Service
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }
        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ILogger, Logger>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<DbContext, SchoolContext>();

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
        }
    }
}