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
        public string Path { get; set; }

        private int _first;
        public int First
        {
            get { return _first; }
            set { _first = value; }
        }

        private int _last;
        public int Last
        {
            get { return _last; }
            set { _last = value; }
        }

        private string _progress;
        public string Progress
        {
            get { return "Progress: " + Math.Round(((double)_first / (double)_last)*100, 1) + "%"; }
            set { _progress = value; }
        }
        
    }
}
