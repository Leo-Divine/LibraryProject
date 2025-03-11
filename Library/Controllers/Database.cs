using Library.Models;
using Microsoft.Data.Sqlite;

namespace Library.Controllers
{
    public class Database
    {
        private SqliteConnection Connection { get; set; }
        public static async Task<Database> Create()
        {
            SqliteConnection connection = new("Data Source=Data.db");
            await connection.OpenAsync();

            Database db = new()
            {
                Connection = connection
            };

            await db.CreateDatabase();
            return db;
        }
        private async Task CreateDatabase()
        {
            using var command = this.Connection.CreateCommand();
            command.CommandText = @"CREATE TABLE IF NOT EXISTS books (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                TITLE TEXT NOT NULL,
                AUTHOR TEXT,
                GENRE TEXT,
                IBSN TEXT UNIQUE,
                AVAILABILITY INTEGER NOT NULL
                )";
            await command.ExecuteNonQueryAsync();
        }
        public async Task<List<BookModel>> GetAllBooks()
        {
            List<BookModel> books = [];
            using var command = this.Connection.CreateCommand();

            command.CommandText = "SELECT * FROM books";
            var data = await command.ExecuteReaderAsync();
            books = await CreateBooksFromData(data);
            await command.DisposeAsync();

            return books;
        }

        public async Task<List<BookModel>> SearchByAll(string searchQuery)
        {
            List<BookModel> books = [];

            using var command = this.Connection.CreateCommand();
            command.CommandText = $"SELECT * FROM books WHERE TITLE LIKE '%{searchQuery}%' OR AUTHOR LIKE '%{searchQuery}%' OR GENRE LIKE '%{searchQuery}%'";
            var data = await command.ExecuteReaderAsync();
            books.AddRange(await CreateBooksFromData(data));
            await command.DisposeAsync();

            return books;
        }

        private async Task<List<BookModel>> CreateBooksFromData(SqliteDataReader data)
        {
            List<BookModel> books = [];
            while (await data.ReadAsync())
            {
                BookModel book = new(data.GetInt32(0), data.GetString(1), data.GetBoolean(5))
                {
                    Author = data.GetString(2),
                    Genre = data.GetString(3),
                    ISBN = data.GetString(4),
                };
                books.Add(book);
            }

            return books;
        }

        public async Task AddBook(BookModel book)
        {
            using var command = this.Connection.CreateCommand();
            command.CommandText = "INSERT or IGNORE INTO books (TITLE, AUTHOR, GENRE, IBSN, AVAILABILITY) VALUES (?1, ?2, ?3, ?4, ?5);";

            command.Parameters.AddWithValue("?1", book.Title);
            command.Parameters.AddWithValue("?2", book.Author);
            command.Parameters.AddWithValue("?3", book.Genre);
            command.Parameters.AddWithValue("?4", book.ISBN);
            command.Parameters.AddWithValue("?5", book.AvailabilityStatus);

            await command.ExecuteNonQueryAsync();
            await command.DisposeAsync();
        }

        public async Task UpdateBook(BookModel book)
        {
            using var command = this.Connection.CreateCommand();
            command.CommandText = "UPDATE books SET TITLE = ?2, AUTHOR = ?3, GENRE = ?4, IBSN = ?5, AVAILABILITY = ?6 WHERE ID = ?1";

            command.Parameters.AddWithValue("?1", book.Id);
            command.Parameters.AddWithValue("?2", book.Title);
            command.Parameters.AddWithValue("?3", book.Author);
            command.Parameters.AddWithValue("?4", book.Genre);
            command.Parameters.AddWithValue("?5", book.ISBN);
            command.Parameters.AddWithValue("?6", book.AvailabilityStatus);

            await command.ExecuteNonQueryAsync();
            await command.DisposeAsync();
        }
    }
}
