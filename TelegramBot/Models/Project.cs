﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string? NameImage { get; set; }
        public string? UrlImage { get; set; }
        public string Description { get; set; }
    }
}
