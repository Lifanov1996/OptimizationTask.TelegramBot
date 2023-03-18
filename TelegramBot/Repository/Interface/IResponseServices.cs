using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Repository.Interface
{
    public interface IResponseServices
    {
        Task<T> GetRespons<T>(string requestUrl);
    }
}
