using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using pocParallelTask.model;
using System.Text;

namespace pocParallelTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<MyController> _logger;

        public MyController(ILogger<MyController> logger)
        {
            _logger = logger;
        }

      
        [HttpGet(Name = "CallApi")]
        public async Task<IEnumerable<input>> CallApi()
        {
            List<input> rep = new();
            await Task.WhenAll(CallWebAPIAsync(1),
          CallWebAPIAsync(2),
          CallWebAPIAsync(3),
          CallWebAPIAsync(4));
            rep.Add(CallWebAPIAsync(1).Result);
            rep.Add(CallWebAPIAsync(2).Result);
            rep.Add(CallWebAPIAsync(3).Result);
            rep.Add(CallWebAPIAsync(4).Result);

            return rep;
        }

        private async Task<input> CallWebAPIAsync(int i)
        {
            input responsedata = new();
            try
            {
                input data = new();
                data.id = i;
                data.role = "role " + DateTime.Now;
                data.name = "name";
                var jsondata = JsonConvert.SerializeObject(data);
                HttpClient client = new HttpClient();
                Uri uri = new Uri("https://localhost:7017/api/Values");
                var response = await client.PostAsync(uri, new StringContent(jsondata, Encoding.UTF8, "application/json"));
                string content = await response.Content.ReadAsStringAsync();
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    responsedata = JsonConvert.DeserializeObject<input>(content);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return responsedata;
        }
    }
}