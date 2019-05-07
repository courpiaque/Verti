using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Verti.Views;
using Verti.Models;
using Xamarin.Forms;

namespace Verti.ViewModels
{
    public class LibraryListPageViewModel : BaseViewModel
    { 
        public ObservableCollection<Book> Books { get; private set; } = new ObservableCollection<Book>();
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set { SetValue(ref _selectedBook, value); }

        }

        private Book _selectedBook;

        private readonly IPageService _pageService;
        public LibraryListPageViewModel(IPageService pageService)
        {
            _pageService = pageService;
        }

        public void AddBook()
        {
            var book = new Book { Name = "1984 " + DateTime.Now.Ticks };
            Books.Add(book);
        }

        public async Task SelectBook(Book book)
        {
            if (book == null)
                return;

            SelectedBook = null;

            await _pageService.PushAsync(new BookDetailPage(book));
        }
    }
}
