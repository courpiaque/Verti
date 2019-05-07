using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Verti.Models;

namespace Verti.ViewModels
{
    public interface IBookStore
    {
        Task<IEnumerable<Book>> GetBookAsync();
        Task<Book> GetBook(int id);
        Task AddBook(Book book);
        Task UpdateBook(Book book);
        Task DeleteBook(Book book);
    }
}
