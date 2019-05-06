using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Verti.Models
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}
