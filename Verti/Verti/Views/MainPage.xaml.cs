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
        Page page;
        string file_path;
		int last_iter = 0;
        char[] charSeparators = new char[] {' '};

        public MainPage()
		{
			InitializeComponent();

            BindingContext = new MainPageViewModel(new PageService());

            page = new LibraryListPage();
		}

		private async Task ReadingText(string randomText, int time, int last)
		{
            List<string> a = randomText.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries).ToList();
            for (int i = last; i < a.Count; i++)
			{
				main_text.Text = a[i];
				await Task.Delay(time);
				if (Start.Text == "Start")
				{
					last_iter = i;
					break;
				}
			}
		}

        private async void Start_Clicked(object sender, EventArgs e)
		{
            var context = (BindingContext as MainPageViewModel);

			wpm_text.IsVisible =! wpm_text.IsVisible;
			slider.IsVisible =! slider.IsVisible;

			string randomText = await context.GetTextFromPDFAsync(file_path);
			if (Start.Text == "Start")
			{
				Start.Text = "Stop";
				await ReadingText(await context.GetTextFromPDFAsync(file_path), (int)(60000 / slider.Value), last_iter);
			}
			else
			{
				Start.Text = "Start";
			}
		}

		private async void Select_File(object sender, EventArgs e)
		{
            file_path = await (BindingContext as MainPageViewModel).SelectingFile();
		}

        private async void Library_Clicked(object sender, EventArgs e)
        {
           await (BindingContext as MainPageViewModel).LibraryPage();
        }
    }
}