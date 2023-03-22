using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Models;
using TelegramBot.Interface;

namespace TelegramBot.Repository.Response
{
    public class Contacts
    {
        private const string responseUrl = "https://localhost:7297/contact";
        private IResponseService _responseSer;

        public Contacts(IResponseService responseService)
        {
            _responseSer = responseService;
        }

        public async Task<Contact> GetContactAsync()
        {
            try
            {
                return await _responseSer.GetRespons<Contact>(responseUrl);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
