using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Interface
{
    public interface IResponseService
    {
        Task<T> GetRespons<T>(string requestUrl);
    }
}
