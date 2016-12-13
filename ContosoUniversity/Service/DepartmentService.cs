using AutoMapper;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using ContosoUniversity.Core.Dto;
using ContosoUniversity.Core.Models;

namespace ContosoUniversity.Service
{
    public class DepartmentService
    {
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private const string DepartmentUri = "api/department/";

        public DepartmentService(HttpClient httpClient, IMapper mapper)
        {
            _mapper = mapper;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Department>> Get()
        {
            var responseMessage = await _httpClient.GetAsync(DepartmentUri);
            return responseMessage.IsSuccessStatusCode
                ? _mapper.Map<List<Department>>(responseMessage.Content.ReadAsAsync<List<DepartmentDto>>().Result)
                : new List<Department>();
        }

        public async Task<Department> Get(int id)
        {
            var responseMessage = await _httpClient.GetAsync(DepartmentUri + id);
            return responseMessage.IsSuccessStatusCode
                ? _mapper.Map<Department>(responseMessage.Content.ReadAsAsync<DepartmentDto>().Result)
                : null;
        }

        public async Task<bool> Add(Department department)
        {
            var responseMessage = await _httpClient.PostAsJsonAsync(
                DepartmentUri, _mapper.Map<DepartmentDto>(department));
            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> Update(int id, Department department)
        {
            var responseMessage = await _httpClient.PutAsJsonAsync(
                DepartmentUri + id, _mapper.Map<DepartmentDto>(department));
            return responseMessage.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int id)
        {
            var responseMessage = await _httpClient.DeleteAsync(DepartmentUri + id);
            return responseMessage.IsSuccessStatusCode;
        }
    }
}