using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models
{
    public class Tiding
    {
        public DateTime DateTimePublication { get; set; }
        public string Header { get; set; }
        public string File { get; set; }
        public string Description { get; set; }
    }
}
