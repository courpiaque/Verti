﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Verti.Models;
using Foundation;
using UIKit;
using SQLite;

namespace Verti.iOS.Persistance
{
    class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "MySQLite.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}