using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Models;
using TelegramBot.Interface;

namespace TelegramBot.Repository.Response
{
    internal class Offices
    {
        private const string responseUrl = "https://localhost:7297/office";
        private IResponseService _responseSer;

        public Offices(IResponseService responseService)
        {
            _responseSer = responseService;
        }

        public async Task<List<Office>> GetOfficeAsync()
        {
            try
            {
                return await _responseSer.GetRespons<List<Office>>(responseUrl);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
