using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Verti.ViewModels;

namespace Verti.Models
{
    public class Book : BaseViewModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }
}
