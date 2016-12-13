using System.Web.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using ContosoUniversity.Core.Dto;
using ContosoUniversity.Core.Models;
using ContosoUniversity.Core.Persistence;
using AutoMapper;

namespace ContosoUniversity.Service.Controllers
{
    public class StudentController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public StudentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // GET api/student 
        public async Task<IEnumerable<StudentDto>> Get()
        {
            return _mapper.Map<List<StudentDto>>(await _unitOfWork.Student.GetAsync());
        }

        // GET api/student/5 
        public async Task<StudentDto> Get(int id)
        {
            return _mapper.Map<StudentDto>(await _unitOfWork.Student.GetAsync(id));
        }

        // POST api/student 
        public async Task Post(StudentDto value)
        {
             _unitOfWork.Student.Add(_mapper.Map<Student>(value));
             await _unitOfWork.Complete();
        }

        // PUT api/student/5 
        public async Task Put(int id, StudentDto value)
        {
             _unitOfWork.Student.Update(_mapper.Map<Student>(value));
             await _unitOfWork.Complete();
        }

        // DELETE api/student/5 
        public async Task Delete(int id)
        {
            _unitOfWork.Student.Delete(id);
            await _unitOfWork.Complete();
        }
    }
}