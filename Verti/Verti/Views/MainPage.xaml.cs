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

namespace Verti
{
	public partial class MainPage : ContentPage
	{
        public MainPage()
		{
			InitializeComponent();

            BindingContext = new MainPageViewModel(new PageService());
		} 
    }
}