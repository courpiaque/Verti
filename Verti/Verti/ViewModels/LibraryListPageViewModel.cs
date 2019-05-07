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
        private IBookStore _bookStore;
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

            AddBookCommand = new Command(AddBook);
            SelectBookCommand = new Command<Book>(async vm => await SelectBook(vm));
        }

        private void AddBook()
        {
            var book = new Book { Name = "1984 " + DateTime.Now.Ticks };
            Books.Add(book);
        }

        private async Task SelectBook(Book book)
        {
            if (book == null)
                return;

            SelectedBook = null;

            await _pageService.PushModalAsync(new BookDetailPage(book));
        }
    }
}
