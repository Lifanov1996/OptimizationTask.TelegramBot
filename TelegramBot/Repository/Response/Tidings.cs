using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Interface;
using TelegramBot.Models;

namespace TelegramBot.Repository.Response
{
    public class Tidings
    {
        private const string responseUrl = "https://localhost:7297/tiding";
        private IResponseService _responseSer;

        public Tidings(IResponseService responseService)
        {
            _responseSer = responseService;
        }

        public async Task<List<Tiding>> GetOfficeAsync()
        {
            try
            {
                return await _responseSer.GetRespons<List<Tiding>>(responseUrl);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
