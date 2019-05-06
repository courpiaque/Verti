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
        private SQLiteAsyncConnection _connection;
        private ObservableCollection<Book> _books;

        public LibraryListPage()
        {
            InitializeComponent();

            BindingContext = new LibraryListPageViewModel();
            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected override void OnAppearing()
        {
            var context = (BindingContext as LibraryListPageViewModel);
            context.PopulateList(_connection);
            _books = new ObservableCollection<Book>(context.books);
            listView.ItemsSource = _books;
            base.OnAppearing();
        }

        private void Btn_Clicked(object sender, EventArgs e)
        {
            (BindingContext as LibraryListPageViewModel).AddingBook();
        }
    }
}