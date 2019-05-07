using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Verti.Models;
using Verti.ViewModels;

namespace Verti.Persistance
{
    public class SQLiteBookStore : IBookStore
    {
        private SQLiteAsyncConnection _connection;
        public SQLiteBookStore(ISQLiteDb db)
        {
            _connection = db.GetConnection();
            _connection.CreateTableAsync<Book>();
        }

        public async Task AddBook(Book book)
        {
            await _connection.InsertAsync(book);
        }

        public async Task DeleteBook(Book book)
        {
            await _connection.DeleteAsync(book);
        }

        public async Task<Book> GetBook(int id)
        {
            return await _connection.FindAsync<Book>(id);
        }

        public async Task<IEnumerable<Book>> GetBookAsync()
        {
            return await _connection.Table<Book>().ToListAsync();
        }

        public async Task UpdateBook(Book book)
        {
            await _connection.UpdateAsync(book); 
        }
    }
}
