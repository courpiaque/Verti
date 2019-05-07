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
using Verti.Persistance;

namespace Verti.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LibraryListPage : ContentPage
    {
        public LibraryListPage()
        {
            var bookStore = new SQLiteBookStore(DependencyService.Get<ISQLiteDb>());
            var pageService = new PageService();
            ViewModel = new LibraryListPageViewModel(pageService, bookStore);

            InitializeComponent();
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ViewModel.SelectBookCommand.Execute(e.SelectedItem);
        }

        protected override void OnAppearing()
        {
            ViewModel.LoadDataCommand.Execute(null);

            base.OnAppearing();
        }

        private LibraryListPageViewModel ViewModel
        {
            get { return BindingContext as LibraryListPageViewModel; }
            set { BindingContext = value; }
        }
    }
}