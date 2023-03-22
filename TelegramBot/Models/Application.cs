using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models
{
    public class Application
    {
        [StringLength(50, MinimumLength = 3)]
        public string? NameClient { get; set; }

        [StringLength(500, MinimumLength = 20)]
        public string? DescriptionApp { get; set; }

        [EmailAddress]
        public string? EmailClient { get; set; }
    }
}
