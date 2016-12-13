using AutoMapper;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using ContosoUniversity.Core.Dto;
using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Service
{
    public class CourseService
    {
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private const string CourseUri = "api/course/";

        public CourseService(HttpClient httpClient, IMapper mapper)
        {
            _mapper = mapper;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Course>> Get()
        {
            var responseMessage = await _httpClient.GetAsync(CourseUri);
            return responseMessage.IsSuccessStatusCode
                ? _mapper.Map<List<Course>>(responseMessage.Content.ReadAsAsync<List<CourseDto>>().Result)
                : new List<Course>();
        }

        public async Task<Course> Get(int id)
        {
            var responseMessage = await _httpClient.GetAsync(CourseUri + id);
            return responseMessage.IsSuccessStatusCode
                ? _mapper.Map<Course>(responseMessage.Content.ReadAsAsync<CourseDto>().Result)
                : null;
        }

        public async Task<bool> Add(Course course)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync(
                CourseUri, _mapper.Map<CourseDto>(course));
            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> Update(int id, Course course)
        {
            var responseMessage = await _httpClient.PutAsJsonAsync(
                CourseUri + id, _mapper.Map<CourseDto>(course));
            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int id)
        {
            var responseMessage = await _httpClient.DeleteAsync(CourseUri + id);
            return responseMessage.IsSuccessStatusCode;
        }
    }
}