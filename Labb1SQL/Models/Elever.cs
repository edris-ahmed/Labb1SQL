using System;
using System.Collections.Generic;

namespace Labb1SQL.Models
{
    public partial class Elever
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? ClassId { get; set; }
        public int? GradeId { get; set; }
    }
}
