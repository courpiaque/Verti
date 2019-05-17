using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.FilePicker;
using System.Text;
using System.Net;
using System.Net.Http;
using System.IO;
using Verti.Views;
using Verti.ViewModels;
using Verti.Models;
using Verti.Persistance;

namespace Verti
{
	public partial class MainPage : ContentPage
	{
        public MainPage()
		{
			InitializeComponent();

            BindingContext = new MainPageViewModel(new PageService(), new SQLiteBookStore(DependencyService.Get<ISQLiteDb>()));
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private MainPageViewModel _viewModel
        {
            get { return BindingContext as MainPageViewModel; }
            set { _viewModel = value; }
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            System.Diagnostics.Debug.Write(e.Value);
            _viewModel.ChangeTheme(e.Value);
        }
    }
}