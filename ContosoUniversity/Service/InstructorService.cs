using AutoMapper;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using ContosoUniversity.Core.Dto;
using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Service
{
    public class InstructorService
    {
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private const string InstructorUri = "api/instructor/";

        public InstructorService(HttpClient httpClient, IMapper mapper)
        {
            _mapper = mapper;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Instructor>> Get()
        {
            var responseMessage = await _httpClient.GetAsync(InstructorUri);
            return responseMessage.IsSuccessStatusCode
                ? _mapper.Map<List<Instructor>>(responseMessage.Content.ReadAsAsync<List<InstructorDto>>().Result)
                : new List<Instructor>();
        }

        public async Task<Instructor> Get(int id)
        {
            var responseMessage = await _httpClient.GetAsync(InstructorUri + id);
            return responseMessage.IsSuccessStatusCode
                ? _mapper.Map<Instructor>(responseMessage.Content.ReadAsAsync<InstructorDto>().Result)
                : null;
        }

        public async Task<bool> Add(Instructor instructor)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync(
                InstructorUri, _mapper.Map<InstructorDto>(instructor));
            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> Update(int id, Instructor instructor)
        {
            var responseMessage = await _httpClient.PutAsJsonAsync(
                InstructorUri + id, _mapper.Map<InstructorDto>(instructor));
            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int id)
        {
            var responseMessage = await _httpClient.DeleteAsync(InstructorUri + id);
            return responseMessage.IsSuccessStatusCode;
        }
    }
}