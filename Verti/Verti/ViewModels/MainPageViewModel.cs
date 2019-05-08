using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Verti.Views;
using Xamarin.Forms;

namespace Verti.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private HttpClient client;
        private string file_path;
        private int last_iter = 0;
        private char[] charSeparators = new char[] { ' ' };
        private IPageService _pageService;

        private bool _visibility = true;
        private string _btnText = "Start";
        private string _mainText = "Welcome";
        private int _sliderValue = 60;

        public bool Visibility
        {
            get { return _visibility; }
            private set { SetValue(ref _visibility, value); }
        }
        public string ButtonText
        {
            get { return _btnText; }
            private set { SetValue(ref _btnText, value); }
        } 
        public string MainText
        {
            get { return _mainText; }
            private set { SetValue(ref _mainText, value); }
        }
        public int SliderValue
        {
            get { return _sliderValue; }
            set { SetValue(ref _sliderValue, value); }
        }

        //public ICommand PickPdfCommand { get; private set; }
        public ICommand LibraryListPageCommand { get; private set; }
        public ICommand StartClickedCommand { get; private set; }

        public MainPageViewModel(IPageService pageService)
        {
            _pageService = pageService;

            //PickPdfCommand = new Command(async () => await PickPdf());
            StartClickedCommand = new Command(async () => await StartClicked());
            LibraryListPageCommand = new Command(async () => await LibraryPage());
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

        public async Task LibraryPage()
        {
            await _pageService.PushModalAsync(new LibraryListPage());
        }

        private async Task ReadingText(string randomText, int time, int last)
        {            
            List<string> a = randomText.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries).ToList();
            for (int i = last; i < a.Count; i++)
            {
                MainText = a[i];
                System.Diagnostics.Debug.Write(MainText);
                await Task.Delay(time);
                if (ButtonText == "Start")
                {
                    last_iter = i;
                    break;
                }
            }
        }

        private async Task StartClicked()
        {
            Visibility = !Visibility;

            System.Diagnostics.Debug.Write(Visibility);

            string randomText = await GetTextFromPDFAsync(file_path);
            if (ButtonText == "Start")
            {
                ButtonText = "Stop";
                await ReadingText(await GetTextFromPDFAsync(file_path), (int)(60000 / SliderValue), last_iter);
            }
            else
            {
                ButtonText = "Start";
            }
        }
    }
}
