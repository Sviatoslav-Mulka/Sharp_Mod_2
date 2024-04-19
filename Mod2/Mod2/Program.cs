//2.2

using System;

namespace Mod2
{
    using System;

    public delegate void LibraryItemAction();

    public interface ILibraryItem
    {
        void CheckOut();
        void Return();
        bool IsAvailable { get; }
        event EventHandler<string> StatusChanged;
    }

    public class Book : ILibraryItem
    {
        public string Title { get;  set; }
        public string Author { get;  set; }
        public int Year { get;  set; }
        private bool isBorrowed;

        public event EventHandler<string> StatusChanged;

        public Book(string title, string author, int year)
        {
            Title = title;
            Author = author;
            Year = year;
            isBorrowed = false;
        }

        public void CheckOut()
        {
            if (!isBorrowed)
            {
                isBorrowed = true;
                OnStatusChanged($"{Title} has been checked out.");
            }
            else
            {
                throw new BookBorrowException("This book is already borrowed.");
            }
        }

        public void Return()
        {
            if (isBorrowed)
            {
                isBorrowed = false;
                OnStatusChanged($"{Title} has been returned.");
            }
            else
            {
                throw new BookReturnException("This book is not borrowed.");
            }
        }

        public bool IsAvailable
        {
            get { return !isBorrowed; }
        }

        protected virtual void OnStatusChanged(string message)
        {
            StatusChanged?.Invoke(this, message);
        }
    }

    public class BookBorrowException : Exception
    {
        public BookBorrowException(string message) : base(message)
        {
        }
    }

    public class BookReturnException : Exception
    {
        public BookReturnException(string message) : base(message)
        {
        }
    }


    public class LibraryUser
    {
        public string Name { get; set; }
        public string UserType { get; set; }
        public void TakeBook(Book book, LibraryUser a)
        {
            Console.WriteLine($"{a.Name} Took Book");
            LibraryItemAction checkOutAction = new LibraryItemAction(book.CheckOut);

            try
            {
                checkOutAction();
            }
            catch (BookBorrowException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GaveBook(Book book, LibraryUser a)
        {
            Console.WriteLine($"{a.Name} Gave Book");
            LibraryItemAction returnAction = new LibraryItemAction(book.Return);
            try
            {
                returnAction();
            }
            catch (BookReturnException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    
        public LibraryUser(string name, string userType)
            {
                Name = name;
                UserType = userType;
            }
    }

        public class Student : LibraryUser
        {
            public Student(string name) : base(name, "Student")
            {
            }
        }

        public class Teacher : LibraryUser
        {
            public Teacher(string name) : base(name, "Teacher")
            {
            }
        }




    class Program
    {
        static void Main(string[] args)
        {
            Book book1 = new Book("Kobzar", "T.Schevchenko", 1888);
            book1.StatusChanged += Book_StatusChanged;

            LibraryUser boy = new LibraryUser("Jacob", "Student");
            boy.TakeBook(book1, boy);
            boy.GaveBook(book1, boy);

           
        }

        private static void Book_StatusChanged(object sender, string message)
        {
            Console.WriteLine($"Status changed: {message}");
        }
    }
}