﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Models;
using TelegramBot.Interface;

namespace TelegramBot.Repository.Response
{
    public class ResponseServices : IResponseService
    {
        private HttpClient _client;
        public ResponseServices()
        {
            _client = new();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public async Task<T> GetRespons<T>(string requestUrl)
        {
            var response = await _client.GetAsync(requestUrl);


            if (response.IsSuccessStatusCode)
            {
                return await _client.GetFromJsonAsync<T>(requestUrl);
            }

            throw new ArgumentNullException(response.ToString());
        }
    }
}
