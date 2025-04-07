using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using WebApplication1.Model;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public List<FilmGetDTO> Films { get; set; } = new List<FilmGetDTO>();

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {
            var client = _httpClientFactory.CreateClient("ApiFilms");
            //var response = await client.GetAsync("api/Films/hi");
            var response = await client.GetAsync("api/Films");
            //var response = await client.GetFromJsonAsAsyncEnumerable<List<Film>>("Films",);
            if (response == null || !response.IsSuccessStatusCode)
            {
                _logger.LogError("Error de carrega de dades de la llista Films");
            }
            else
            {
                //_logger.LogError(await response.Content.ReadAsStringAsync());
                var json = await response.Content.ReadAsStringAsync();
                Films = JsonSerializer.Deserialize<List<FilmGetDTO>>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            }
        }
    }
}
