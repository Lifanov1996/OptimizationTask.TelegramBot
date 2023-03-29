using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Interface;
using TelegramBot.Models;

namespace TelegramBot.Repository.Response
{
    internal class ApplicationGet
    {
        private const string responseUrl = "https://localhost:7297/home/";
        private IResponseService _responseSer;

        public ApplicationGet(IResponseService responseService)
        {
            _responseSer = responseService;
        }

        public async Task<ApplicationsGet> GetApplicationAsync(string numberApp)
        {
            try
            {
                return await _responseSer.GetRespons<ApplicationsGet>($"{responseUrl}"+ numberApp);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
