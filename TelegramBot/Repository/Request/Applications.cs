using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TelegramBot.Models;

namespace TelegramBot.Repository.Request
{
    internal class Applications
    {
        private const string requestUrl = "https://localhost:7297/home";
        private HttpClient _client;

        public Applications()
        {
            _client = new();
        }

        public async Task<bool> PostAppAsync(Application app)
        {
            JsonContent content = JsonContent.Create(app);
            using var request = await _client.PostAsync(requestUrl, content);

            // получение ответа
            if (request.IsSuccessStatusCode)
            {
                var response = await request.Content.ReadAsStringAsync();
                if(response == "true")
                {
                    return true;
                }
                return false;
            }
            return false;
        }

    }
}
