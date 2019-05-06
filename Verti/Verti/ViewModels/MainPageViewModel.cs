using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Verti.ViewModels
{
    public class MainPageViewModel
    {
        HttpClient client;

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
    }
}
