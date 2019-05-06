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

namespace Verti
{
	public partial class MainPage : ContentPage
	{
        Page page;
        HttpClient client;
        string file_path;
		int last_iter = 0;
        char[] charSeparators = new char[] {' '};

        public MainPage()
		{
			InitializeComponent();

            page = new LibraryListPage();
		}

		private async Task ReadingText(string randomText, int time, int last)
		{
            System.Diagnostics.Debug.WriteLine(randomText);

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

		private async Task<string> GetTextFromPDFAsync(string path)
		{
            client = new HttpClient();
            client.BaseAddress = new Uri("http://verting.herokuapp.com/PdfToText/");

            var form = new MultipartFormDataContent();
            var stream = File.OpenRead(path);
            var streamContent = new StreamContent(stream);

            form.Add(streamContent, "file.pdf", stream.Name);

            var response = await client.PostAsync("", form);
            var responseString = await response.Content.ReadAsStringAsync();

			return responseString;
        }

        private async void Start_Clicked(object sender, EventArgs e)
		{
			wpm_text.IsVisible =! wpm_text.IsVisible;
			slider.IsVisible =! slider.IsVisible;

			string randomText = await GetTextFromPDFAsync(file_path);
			if (Start.Text == "Start")
			{
				Start.Text = "Stop";
				await ReadingText(await GetTextFromPDFAsync(file_path), (int)(60000 / slider.Value), last_iter);
			}
			else
			{
				Start.Text = "Start";
			}
		}

		private async void Select_File(object sender, EventArgs e)
		{
			var file1 = await CrossFilePicker.Current.PickFile();
			file_path = file1.FilePath;
		}

        private void Library_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(page);
        }
    }
}