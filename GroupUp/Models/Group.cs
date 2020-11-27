﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroupUp.Models
{
    public class Group : BaseModel
    {
        public string Name { get; set; }
        public List<User> Users { get; } = new List<User>();
    }
}
