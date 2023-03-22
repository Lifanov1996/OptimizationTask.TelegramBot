using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Interface;
using TelegramBot.Models;

namespace TelegramBot.Repository.Response
{
    internal class Projects
    {
        private const string responseUrl = "https://localhost:7297/project";
        private IResponseService _responseSer;

        public Projects(IResponseService responseService)
        {
            _responseSer = responseService;
        }

        public async Task<List<Project>> GetOfficeAsync()
        {
            try
            {
                return await _responseSer.GetRespons<List<Project>>(responseUrl);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
