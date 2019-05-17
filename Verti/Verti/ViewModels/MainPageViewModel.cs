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
using Verti.Models;
using Verti.Views;
using Xamarin.Forms;

namespace Verti.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private IPageService _pageService;
        private IBookStore _bookStore;
        private readonly char[] charSeparators = new char[] { ' ' };

        private bool _isBookSelected = false;
        private bool _visibility = true;
        private string _btnText = "Start";
        private string _mainText = "Select pdf from library.";
        private int _sliderValue = 60;
        private Book _selectedBook;
        private string _backColor = "#ffffff";
        private string _frontColor = "#000000";

        public string BackColor
        {
            get { return _backColor; }
            set { SetValue(ref _backColor, value); }
        }
        public string FrontColor
        {
            get { return _frontColor; }
            set { SetValue(ref _frontColor, value); }
        }
        public bool IsBookSelected
        {
            get { return _isBookSelected; }
            set { SetValue(ref _isBookSelected, value); }
        }
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set { _selectedBook = value; }
        }
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
            set { SetValue(ref _mainText, value); }
        }
        public int SliderValue
        {
            get { return _sliderValue; }
            set { SetValue(ref _sliderValue, value); }
        }

        //public ICommand PickPdfCommand { get; private set; }
        public ICommand LibraryListPageCommand { get; private set; }
        public ICommand StartClickedCommand { get; private set; }

        public MainPageViewModel(IPageService pageService, IBookStore bookStore)
        {
            _pageService = pageService;
            _bookStore = bookStore;

            //PickPdfCommand = new Command(async () => await PickPdf());
            StartClickedCommand = new Command(async () => await StartClicked());
            LibraryListPageCommand = new Command(async () => await LibraryPage());
        }
        
        public void ChangeTheme(bool status)
        {
            if (status)
            {
                BackColor = "#000000";
                FrontColor = "#ffffff";
            }

            else
            {
                BackColor = "#ffffff";
                FrontColor = "#000000";
            }
                
        }
        public async Task LibraryPage()
        {
            var viewModel = new LibraryListPageViewModel(_pageService, _bookStore, this);
            await _pageService.PushModalAsync(new LibraryListPage(viewModel));
        }

        private async Task ReadingText(List<string> content, int time)
        {
            content.RemoveAll(string.IsNullOrWhiteSpace);
            while (SelectedBook.First < content.Count)
            {
                MainText = content[SelectedBook.First];
                await Task.Delay(time);
                SelectedBook.First++;

                if (ButtonText == "Start")
                    break;
            }
            
        }

        private async Task StartClicked()
        {
            Visibility = !Visibility;

            if (ButtonText == "Start")
            {
                ButtonText = "Stop";
                await ReadingText(await LoadFromTxt(SelectedBook.Path), (int)(60000 / SliderValue));
                ButtonText = "Start";
            }
            else
            {
                await _bookStore.UpdateBook(SelectedBook);
                ButtonText = "Start";
            }
        }

        public async Task<List<string>> LoadFromTxt(string path)
        {
            var content = File.ReadAllLines(path);
            List<string> allLinesText = new List<string>(content);           
            return allLinesText;
        }
    }
}
