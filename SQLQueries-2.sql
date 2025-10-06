SELECT        BookGenre.BookID AS ISBN, Book.NameBook AS [Название книги], Genre.NameGenre AS Жанр
FROM            Book INNER JOIN
                         BookGenre ON Book.ISBN = BookGenre.BookID INNER JOIN
                         Genre ON BookGenre.GenreID = Genre.ID INNER JOIN
                         Reader ON BookGenre.ID = Reader.ID
WHERE        (Genre.NameGenre = N'Проза для взрослых')

SELECT        BookGenre.BookID AS ISBN, Book.NameBook AS [Название книги], Genre.NameGenre AS Жанр
FROM            Book INNER JOIN
                         BookGenre ON Book.ISBN = BookGenre.BookID INNER JOIN
                         Genre ON BookGenre.GenreID = Genre.ID INNER JOIN
                         Reader ON BookGenre.ID = Reader.ID
WHERE        (Book.NameBook = 'Колыбель кошки')

SELECT        Book.NameBook AS [Название книги], Book.YearOfPublication AS Годвыпуска, Genre.NameGenre AS Жанр
FROM            Book INNER JOIN
                         BookGenre ON Book.ISBN = BookGenre.BookID INNER JOIN
                         Genre ON BookGenre.GenreID = Genre.ID
