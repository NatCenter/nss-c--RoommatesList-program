﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace roomMates.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxOccupancy { get; set; }
    }
}
