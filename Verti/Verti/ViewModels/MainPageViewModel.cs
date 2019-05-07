using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Verti.Views;

namespace Verti.ViewModels
{
    public class MainPageViewModel
    {
        HttpClient client;
        string file_path;
        int last_iter = 0;
        char[] charSeparators = new char[] { ' ' };

        private IPageService _pageService;
        public MainPageViewModel(IPageService pageService)
        {
            _pageService = pageService;
        }

        public async Task<string> GetTextFromPDFAsync(string path)
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

        public async Task<String> SelectingFile()
        {
            var file1 = await CrossFilePicker.Current.PickFile();
            return file1.FilePath;
        }

        public async Task LibraryPage()
        {
            await _pageService.PushModalAsync(new LibraryListPage());
        }
    }
}
