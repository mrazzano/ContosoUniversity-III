using System.Web.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using ContosoUniversity.Core.Dto;
using ContosoUniversity.Core.Models;
using ContosoUniversity.Core.Persistence;
using AutoMapper;

namespace ContosoUniversity.Service.Controllers
{
    public class CourseController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CourseController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // GET api/course 
        public async Task<IEnumerable<CourseDto>> Get()
        {
            return _mapper.Map<List<CourseDto>>(await _unitOfWork.Course.GetAsync());
        }

        // GET api/course/5 
        public async Task<CourseDto> Get(int id)
        {
            return _mapper.Map<CourseDto>(await _unitOfWork.Course.GetAsync(id));
        }

        // POST api/course 
        public async Task Post(CourseDto value)
        {
            _unitOfWork.Course.Add(_mapper.Map<Course>(value));
            await _unitOfWork.Complete();
        }

        // PUT api/course/5 
        public async Task Put(int id, CourseDto value)
        {
            _unitOfWork.Course.Update(_mapper.Map<Course>(value));
            await _unitOfWork.Complete();
        }

        // DELETE api/course/5 
        public async Task Delete(int id)
        {
            _unitOfWork.Course.Delete(id);
            await _unitOfWork.Complete();
        }
    }
}