using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models
{
    public class ApplicationsGet
    {
        public int Id { get; set; }
        public Guid NumberApp { get; set; }
        public DateTime DateTimeCreatApp { get; set; }
        public string NameClient { get; set; }
        public string DescriptionApp { get; set; }
        public string StatusApp { get; set; }
        public string EmailClient { get; set; }
    }
}
