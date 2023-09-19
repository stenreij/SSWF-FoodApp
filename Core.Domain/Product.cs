﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool ContainsAlcohol { get; set; }
        public string PhotoUrl { get; set; } = null!;
        public ICollection<MealPackage> Meals { get; set; } = null!; 
    }
}
