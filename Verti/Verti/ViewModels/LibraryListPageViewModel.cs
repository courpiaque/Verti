using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Verti.Views;
using Verti.Models;
using Xamarin.Forms;
using System.Windows.Input;
using Plugin.FilePicker;
using System.Net.Http;
using System.IO;
using System.Linq;

namespace Verti.ViewModels
{
    public class LibraryListPageViewModel : BaseViewModel
    {
        private Book _selectedBook;
        private readonly IPageService _pageService;
        private readonly IBookStore _bookStore;
        private bool _isDataLoaded;
        private Book _newBook;
        private MainPageViewModel _viewModel;
        private char[] charSeparators = new char[] { ' ' };
        private string _backColor;
        private string _frontColor;

        public ObservableCollection<Book> Books { get; private set; } = new ObservableCollection<Book>();
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set { SetValue(ref _selectedBook, value); }

        }
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

        public ICommand LoadDataCommand { get; private set; }
        public ICommand DeleteBookCommand { get; set; }
        public ICommand AddBookCommand { get; private set; }
        public ICommand SelectBookCommand { get; private set; }

        public LibraryListPageViewModel(IPageService pageService, IBookStore bookStore, MainPageViewModel viewModel)
        {
            FrontColor = viewModel.FrontColor;
            BackColor = viewModel.BackColor;
            _viewModel = viewModel;
            _pageService = pageService;
            _bookStore = bookStore;

            AddBookCommand = new Command(async () => await AddBook());
            SelectBookCommand = new Command<Book>(async vm => await SelectBook(vm));
            LoadDataCommand = new Command(async () => await LoadData());
            DeleteBookCommand = new Command<Book>(async vm => await DeleteBook(vm));
        }

        private async Task LoadData()
        {
            if (_isDataLoaded)
                return;

            _isDataLoaded = true;

            var books = await _bookStore.GetBookAsync();

            foreach (var b in books)
                Books.Add(b);
        }

        private async Task AddBook()
        {
            var file = await CrossFilePicker.Current.PickFile();

            if (file == null)
                return;
            if (file.FileName.Substring(Math.Max(0, file.FileName.Length - 4)) != ".pdf")
            {
                await _pageService.DisplayAlert("Wrong extension", "Select .pdf file", null, "OK");
                return;
            }
                
            var content = await GetTextFromPDFAsync(file.FilePath);
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), file.FileName);
            List<string> listContent = content.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries).ToList();

            File.WriteAllLines(path, listContent);

            _newBook = new Book()
            {
                Name = file.FileName.Remove(file.FileName.Length - 4, 4),
                Path = path,
                First = 0,
                Last = listContent.Count
            };

            Books.Add(_newBook);
            await _bookStore.AddBook(_newBook);
        }

        private async Task DeleteBook(Book book)
        {
                Books.Remove(book);

                var b = await _bookStore.GetBook(book.Id);
                await _bookStore.DeleteBook(b);
            
        }

        private async Task SelectBook(Book book)
        {
            _viewModel.SelectedBook = book;
            _viewModel.IsBookSelected = true;
            _viewModel.MainText = "Selected book: " + book.Name;
            await App.Current.MainPage.Navigation.PopModalAsync();
        }

        private async Task<string> GetTextFromPDFAsync(string path)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://verting.herokuapp.com/PdfToText/");

                var form = new MultipartFormDataContent();
                var stream = File.OpenRead(path);
                var streamContent = new StreamContent(stream);

                form.Add(streamContent, "file.pdf", stream.Name);

                var response = await client.PostAsync("", form);
                var responseString = await response.Content.ReadAsStringAsync();

                return responseString;
            }
        }
    }
}