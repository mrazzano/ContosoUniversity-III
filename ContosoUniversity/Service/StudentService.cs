using AutoMapper;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using ContosoUniversity.Core.Dto;
using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Service
{
    public class StudentService
    {
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private const string StudentUri = "api/student/";

        public StudentService(HttpClient httpClient, IMapper mapper)
        {
            _mapper = mapper;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Student>> Get()
        {
            var responseMessage = await _httpClient.GetAsync(StudentUri);
            return responseMessage.IsSuccessStatusCode
                ? _mapper.Map<List<Student>>(responseMessage.Content.ReadAsAsync<List<StudentDto>>().Result)
                : new List<Student>();
        }

        public async Task<Student> Get(int id)
        {
            var responseMessage = await _httpClient.GetAsync(StudentUri + id);
            return responseMessage.IsSuccessStatusCode
                ? _mapper.Map<Student>(responseMessage.Content.ReadAsAsync<StudentDto>().Result)
                : null;
        }

        public async Task<bool> Add(Student student)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync(
                StudentUri, _mapper.Map<StudentDto>(student));
            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> Update(int id, Student student)
        {
            var responseMessage = await _httpClient.PutAsJsonAsync(
                StudentUri + id, _mapper.Map<StudentDto>(student));
            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int id)
        {
            var responseMessage = await _httpClient.DeleteAsync(StudentUri + id);
            return responseMessage.IsSuccessStatusCode;
        }
    }
}