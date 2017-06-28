﻿using System;
using System.Collections.Generic;

namespace LibraryApp
{
    public class Library
    {
        private List<Book> books;
        private List<Subscriber> subscribers;

        public Library()
        {
            books = new List<Book>();
            subscribers = new List<Subscriber>();
        }

        public Book this[string author, string name]
        {
            get {
                List<Book> find = Find(author, false);
                foreach (Book book in Find(name, true))
                    if (find.Contains(book))
                        return book;
                return null;
            }
        }

        // Добавление книги
        public void AddBook(Book book)
        {
            books.Add(book);
        }

        // Список книг библиотеки
        public List<Book> ListBooks()
        {
            return books;
        }

        // Список книг абонента
        public List<Book> ListBooks(Subscriber sub)
        {
            return sub.ListBooks();
        }

        // Список книг абонентов и библиотеки
        public List<Book> FullList()
        {
            List<Book> full = new List<Book>();
            full.AddRange(books);
            foreach (Subscriber sub in subscribers)
            {
                full.AddRange(sub.ListBooks());
            }
            return full;
        }

        // Поиск по названию или по автору
        public List<Book> Find(string name, bool isName)
        {
            List<Book> find = new List<Book>();

            foreach (Book book in FullList())
                if (isName)
                {
                    if (book.Name == name) find.Add(book);
                }
                else 
                    if (book.Author == name)
                        find.Add(book);

            return find;
        }

        // Выдача книги
        public void GiveBook(Subscriber sub, Book book)
        {
            int countsub = 0;
            foreach (Subscriber subscriber in subscribers)
                if (subscriber.Name == sub.Name && subscriber.Phone == sub.Phone)
                {
                    countsub++;
                }
            if (countsub == 0)
            {
                subscribers.Add(sub);
            }
            
            if (sub.OverdueBooks().Count == 0 && sub.CountBook < 5 && (sub.HasRarityBook == false || (sub.HasRarityBook == true && book.IsRarity == false)))
            {
                bool hasBook = false;
                for (int i = 0; i < books.Count; i++)
                {
                    if (books[i] == book)
                    {
                        books.Remove(book);
                        hasBook = true;
                        break;
                    }
                }
                if (!hasBook)
                {
                    Console.WriteLine("Пользователь {0}, книги {1} в библиотеке нет", sub.Name, book.Author + " " + book.Name);
                    return;
                }
                book.Sub = sub;
                book.Begin = DateTime.Now;
                book.End = DateTime.Now;
                sub.Books.Add(book);
                if (book.IsRarity)
                    sub.HasRarityBook = true ;
                sub.CountBook++;
            }
            else
            {
                Console.WriteLine("Пользователь {0}, сдайте {1} книги", sub.Name, sub.OverdueBooks().Count > 0 ? "просроченные" : "лишние");
            }

        }


        public static void Main()
        {
            Library library = new Library();

            Subscriber sub1 = new Subscriber("First", "8999999999");
            Subscriber sub2 = new Subscriber("Second", "8999954945");

            Book book1 = new Book("1", "11", true);
            Book book2 = new Book("2", "22", false);
            Book book3 = new Book("3", "33", false);
            Book book4 = new Book("4", "44", false);
            Book book5 = new Book("5", "55", false);
            Book book6 = new Book("6", "66", false);
            Book book7 = new Book("7", "77", false);
            Book book8 = new Book("8", "88", true);
            Book book9 = new Book("9", "99", false);

            library.AddBook(book1);
            library.AddBook(book2);
            library.AddBook(book3);
            library.AddBook(book4);
            library.AddBook(book5);
            library.AddBook(book6);
            library.AddBook(book7);
            library.AddBook(book8);
            library.AddBook(book9);

            sub1.TakeBook(library, book1);
            sub1.TakeBook(library, book2);
            sub1.TakeBook(library, book3);
            sub1.TakeBook(library, book4);
            sub1.TakeBook(library, book5);

            sub1.TakeBook(library, book6);
            sub2.TakeBook(library, book1);

            sub2.TakeBook(library, book6);
            sub2.TakeBook(library, book7);
            sub2.TakeBook(library, book8);
            sub2.TakeBook(library, book9);

            Console.WriteLine(sub1.HasRarityBook);
            sub1.ReturnBook(library, book1);
            Console.WriteLine(sub1.HasRarityBook);
            Console.WriteLine(sub1.ListBooks().Count);
            Console.WriteLine(library.ListBooks().Count);
            book2.WhereAreBook();
        }

        
    }

}
