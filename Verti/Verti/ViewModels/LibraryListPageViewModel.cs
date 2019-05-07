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

namespace Verti.ViewModels
{
    public class LibraryListPageViewModel : BaseViewModel
    {
        private Book _selectedBook;
        private readonly IPageService _pageService;
        private readonly IBookStore _bookStore;
        private bool _isDataLoaded;

        public ObservableCollection<Book> Books { get; private set; } = new ObservableCollection<Book>();
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set { SetValue(ref _selectedBook, value); }

        }

        public ICommand LoadDataCommand { get; private set; }
        public ICommand DeleteBookCommand { get; private set; }
        public ICommand AddBookCommand { get; private set; }
        public ICommand SelectBookCommand { get; private set; }

        public LibraryListPageViewModel(IPageService pageService, IBookStore bookStore)
        {
            _pageService = pageService;
            _bookStore = bookStore;

            AddBookCommand = new Command(async () => await AddBook());
            SelectBookCommand = new Command<Book>(async vm => await SelectBook(vm));
            LoadDataCommand = new Command(async () => await LoadData());
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
            var viewModel = new BookDetailPageViewModel(new Book(), _pageService, _bookStore);

            viewModel.BookAdded += (source, book) =>
            {
                Books.Add(book);
            };

            await _pageService.PushModalAsync(new BookDetailPage(viewModel));

        }

        private async Task SelectBook(Book book)
        {
            if (book == null)
                return;

            SelectedBook = null;

            var viewModel = new BookDetailPageViewModel(book, _pageService, _bookStore);

            viewModel.BookUpdated += (source, updatedBook) =>
            {
                book.Id = updatedBook.Id;
                book.Name = updatedBook.Name;
                book.Status = updatedBook.Status;
            };

            await _pageService.PushModalAsync(new BookDetailPage(viewModel));
        }

        private async Task DeleteBook(Book book)
        {
            var a = "";
            if (await _pageService.DisplayAlert("Warning", $"Are you sure you want to delete {book.Name}?", "YES", "NO"))
            {
                Books.Remove(book);

                var b = await _bookStore.GetBook(book.Id);
                await _bookStore.DeleteBook(b);
            }
        }
    }
}
