using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Verti.Models;
using Xamarin.Forms;

namespace Verti.ViewModels
{
    public class BookDetailPageViewModel
    {
        private readonly IPageService _pageService;
        private readonly IBookStore _bookStore;

        public event EventHandler<Book> BookAdded;
        public event EventHandler<Book> BookUpdated;
        public Book _book { get; private set; }
        public ICommand SaveCommand { get; private set; }
        
        public BookDetailPageViewModel(Book book, IPageService pageService, IBookStore bookStore)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            _bookStore = bookStore;
            _pageService = pageService;

            SaveCommand = new Command(async () => await Save());

            _book = new Book
            {
                Id = book.Id, Name = book.Name, Status = book.Status
            };
        }

        private async Task Save()
        {
            if (_book.Id == 0)
            {
                await _bookStore.AddBook(_book);
                BookAdded?.Invoke(this, _book);
            }
            else
            {
                await _bookStore.UpdateBook(_book);
                BookUpdated?.Invoke(this, _book);
            } 
        }
    }
}
