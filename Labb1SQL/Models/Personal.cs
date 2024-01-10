using System;
using System.Collections.Generic;

namespace Labb1SQL.Models
{
    public partial class Personal
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Category { get; set; }
    }
}
