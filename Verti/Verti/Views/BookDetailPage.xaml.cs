using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verti.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Verti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BookDetailPage : ContentPage
    {
        private Book _book;
        public BookDetailPage(Book book)
        {
            _book = book;

            InitializeComponent();
        }
    }
}