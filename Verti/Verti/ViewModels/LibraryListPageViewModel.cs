using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Verti.Models;

namespace Verti.ViewModels
{
    public class LibraryListPageViewModel
    {
        public List<Book> books;
        private SQLiteAsyncConnection _connection;

        public async void PopulateList(SQLiteAsyncConnection connection)
        {
            await connection.CreateTableAsync<Book>();
            books = await connection.Table<Book>().ToListAsync();
            _connection = connection;
        }

        public void AddingBook()
        {
            var book = new Book { Name = "1984 " + DateTime.Now.Ticks };
            _connection.InsertAsync(book);
            books.Add(book);
        }
    }
}
