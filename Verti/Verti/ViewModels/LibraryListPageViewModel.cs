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

namespace Verti.ViewModels
{
    public class LibraryListPageViewModel : BaseViewModel
    {
        private Book _selectedBook;
        private readonly IPageService _pageService;
        private readonly IBookStore _bookStore;
        private bool _isDataLoaded;
        private Book _newBook;

        public ObservableCollection<Book> Books { get; private set; } = new ObservableCollection<Book>();
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set { SetValue(ref _selectedBook, value); }

        }

        public ICommand LoadDataCommand { get; private set; }
        public ICommand DeleteBookCommand { get; set; }
        public ICommand AddBookCommand { get; private set; }
        public ICommand SelectBookCommand { get; private set; }

        public LibraryListPageViewModel(IPageService pageService, IBookStore bookStore)
        {
            _pageService = pageService;
            _bookStore = bookStore;

            AddBookCommand = new Command(async () => await AddBook());
            //SelectBookCommand = new Command<Book>(async vm => await SelectBook(vm));
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
            var file1 = await CrossFilePicker.Current.PickFile();
            _newBook = new Book()
            {
                Name = file1.FileName,
                Status = file1.FilePath
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
    }
}