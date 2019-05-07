using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verti.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Verti.ViewModels;
using SQLite;
using System.Collections.ObjectModel;

namespace Verti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LibraryListPage : ContentPage
    {
        public LibraryListPage()
        {
            BindingContext = new LibraryListPageViewModel(new PageService());

            InitializeComponent();
        }

        private void Btn_Clicked(object sender, EventArgs e)
        {
            (BindingContext as LibraryListPageViewModel).AddBook();
        }

        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            await (BindingContext as LibraryListPageViewModel).SelectBook(e.SelectedItem as Book);
        }
    }
}