using MyWebFormApp.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleMVC.Services
{
    public class CategoryServices : ICategoryServices
    {
        private const string BaseUrl = "http://localhost:5272/api/v1/Categories";
        private readonly HttpClient _client;

        public CategoryServices(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            var httpResponse = await _client.GetAsync(BaseUrl);

            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception("Cannot retrieve categories");
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var categories = JsonSerializer.Deserialize<IEnumerable<CategoryDTO>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return categories;
        }

        public async Task<CategoryDTO> GetById(int id)
        {
            var response = await _client.GetAsync($"{BaseUrl}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Cannot retrieve category with ID: {id}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var category = JsonSerializer.Deserialize<CategoryDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return category;
        }

        public async Task<CategoryDTO> Insert(CategoryCreateDTO category)
        {
            var json = JsonSerializer.Serialize(category);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(BaseUrl, data);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to insert category. StatusCode: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var insertedCategory = JsonSerializer.Deserialize<CategoryDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return insertedCategory;
        }

        public async Task<CategoryDTO> Update(int id, CategoryUpdateDTO category)
        {
            var json = JsonSerializer.Serialize(category);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{BaseUrl}/{id}", data);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to update category with ID: {id}. StatusCode: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var updatedCategory = JsonSerializer.Deserialize<CategoryDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return updatedCategory;
        }

        public async Task Delete(int id)
        {
            var response = await _client.DeleteAsync($"{BaseUrl}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to delete category with ID: {id}. StatusCode: {response.StatusCode}");
            }
        }

        public async Task<IEnumerable<CategoryDTO>> GetWithPaging(int pageNumber = 1, int pageSize = 10, string? name = null)
        {
            try
            {
                var queryString = $"?pageNumber={Uri.EscapeDataString(pageNumber.ToString())}&pageSize={Uri.EscapeDataString(pageSize.ToString())}";

                if (!string.IsNullOrWhiteSpace(name))
                {
                    queryString += $"&name={Uri.EscapeDataString(name)}";
                }

                var httpResponse = await _client.GetAsync($"{BaseUrl}/paged{queryString}");

                if (!httpResponse.IsSuccessStatusCode)
                {
                    if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        // Handle 404 Not Found
                        throw new HttpRequestException("API endpoint not found.");
                    }
                    else
                    {
                        // Handle other errors
                        var errorContent = await httpResponse.Content.ReadAsStringAsync();
                        throw new HttpRequestException($"Failed to retrieve categories. StatusCode: {httpResponse.StatusCode}. Error: {errorContent}");
                    }
                }

                var content = await httpResponse.Content.ReadAsStringAsync();
                var categories = JsonSerializer.Deserialize<IEnumerable<CategoryDTO>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return categories;
            }
            catch (HttpRequestException ex)
            {
                // Log the exception and handle appropriately
                Console.WriteLine($"HTTP request failed: {ex.Message}");
                throw; // Re-throw the exception to propagate it up the call stack
            }
            catch (JsonException ex)
            {
                // Log the exception and handle appropriately
                Console.WriteLine($"JSON deserialization failed: {ex.Message}");
                throw; // Re-throw the exception to propagate it up the call stack
            }
        }
    }
}
