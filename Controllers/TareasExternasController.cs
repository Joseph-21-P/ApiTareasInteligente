using ApiTareasInteligente.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ApiTareasInteligente.Controllers
{
    [Route("api/tareas-externas")]
    [ApiController]
    public class TareasExternasController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TareasExternasController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: api/tareas-externas
        [HttpGet]
        public async Task<IActionResult> GetTareasExternas()
        {
            var client = _httpClientFactory.CreateClient();
            
            try
            {
                var response = await client.GetAsync("https://jsonplaceholder.typicode.com/todos");
                

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Error al comunicar con la API externa de JSONPlaceholder.");
                }

                var content = await response.Content.ReadAsStringAsync();
                

                var todosExternos = JsonSerializer.Deserialize<List<TodoOriginalResponse>>(content);


                var dtos = todosExternos?.Select(t => new TareaExternaDto
                {
                    ExternalId = t.Id,
                    Titulo = t.Title,
                    Completado = t.Completed
                }).ToList() ?? new List<TareaExternaDto>();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno al intentar consumir la API externa: {ex.Message}");
            }
        }

        // GET: api/tareas-externas/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTareaExterna(int id)
        {
            var client = _httpClientFactory.CreateClient();
            
            try
            {
                var response = await client.GetAsync($"https://jsonplaceholder.typicode.com/todos/{id}");
                

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound($"No se encontró ninguna tarea externa con el ID {id}.");
                }

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Error al comunicar con la API externa.");
                }

                var content = await response.Content.ReadAsStringAsync();
                var todoExterno = JsonSerializer.Deserialize<TodoOriginalResponse>(content);

                if (todoExterno == null) return NotFound();


                var dto = new TareaExternaDto
                {
                    ExternalId = todoExterno.Id,
                    Titulo = todoExterno.Title,
                    Completado = todoExterno.Completed
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno al intentar consumir la API externa: {ex.Message}");
            }
        }

        private class TodoOriginalResponse
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }
            
            [JsonPropertyName("title")]
            public string Title { get; set; } = string.Empty;
            
            [JsonPropertyName("completed")]
            public bool Completed { get; set; }
        }
    }
}