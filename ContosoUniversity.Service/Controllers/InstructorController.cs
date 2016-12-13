using System.Web.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using ContosoUniversity.Core.Dto;
using ContosoUniversity.Core.Models;
using ContosoUniversity.Core.Persistence;
using AutoMapper;

namespace ContosoUniversity.Service.Controllers
{
    public class InstructorController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public InstructorController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // GET api/student 
        public async Task<IEnumerable<InstructorDto>> Get()
        {
            return _mapper.Map<List<InstructorDto>>(await _unitOfWork.Instructor.GetAsync());
        }

        // GET api/student/5 
        public async Task<InstructorDto> Get(int id)
        {
            return _mapper.Map<InstructorDto>(await _unitOfWork.Instructor.GetAsync(id));
        }

        // POST api/student 
        public async Task Post(InstructorDto value)
        {
            _unitOfWork.Instructor.Add(_mapper.Map<Instructor>(value));
            await _unitOfWork.Complete();
        }

        // PUT api/student/5 
        public async Task Put(int id, InstructorDto value)
        {
            _unitOfWork.Instructor.Update(_mapper.Map<Instructor>(value));
            await _unitOfWork.Complete();
        }

        // DELETE api/student/5 
        public async Task Delete(int id)
        {
            _unitOfWork.Instructor.Delete(id);
            await _unitOfWork.Complete();
        }
    }
}