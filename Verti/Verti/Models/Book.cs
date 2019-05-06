using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Verti.Models
{
    public class Book
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }
}
