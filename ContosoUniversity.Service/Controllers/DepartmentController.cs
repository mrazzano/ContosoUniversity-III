using System.Web.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using ContosoUniversity.Core.Dto;
using ContosoUniversity.Core.Models;
using ContosoUniversity.Core.Persistence;
using AutoMapper;

namespace ContosoUniversity.Service.Controllers
{
    public class DepartmentController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // GET api/student 
        public async Task<IEnumerable<DepartmentDto>> Get()
        {
            return _mapper.Map<List<DepartmentDto>>(await _unitOfWork.Department.GetAsync());
        }

        // GET api/student/5 
        public async Task<DepartmentDto> Get(int id)
        {
            return _mapper.Map<DepartmentDto>(await _unitOfWork.Department.GetAsync(id));
        }

        // POST api/student 
        public async Task Post(DepartmentDto value)
        {
            _unitOfWork.Department.Add(_mapper.Map<Department>(value));
            await _unitOfWork.Complete();
        }

        // PUT api/student/5 
        public async Task Put(int id, DepartmentDto value)
        {
            _unitOfWork.Department.Update(_mapper.Map<Department>(value));
            await _unitOfWork.Complete();
        }

        // DELETE api/student/5 
        public async Task Delete(int id)
        {
            _unitOfWork.Department.Delete(id);
            await _unitOfWork.Complete();
        }
    }
}