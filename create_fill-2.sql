use [variant-2];
go

/*читатель*/
create table Reader(
ID int identity(1,1) primary key not null,
LastName nvarchar(60) not null,
FirstName nvarchar(60) not null,
Patronymic nvarchar(60) null,
Email nvarchar(255) not null unique check(Email like '%@%'),
Phone nvarchar(20) unique not null check(len(Phone) >= 9 and Phone not like '%[a-zA-Z]%')
);

/*аутентификация*/
create table [User](
ID int identity(1,1) primary key not null,
Login nvarchar(60) not null unique,
Password nvarchar(60) not null,
Role nvarchar(60) not null check(Role in ('Библиотекарь', 'Читатель')),
ReaderID int references Reader(ID) null
);

/*жанры*/
create table Genre(
ID int identity(1,1) primary key not null,
NameGenre nvarchar(60) not null
);

/*книга*/
create table Book(
ISBN varchar(17) primary key not null,
NameBook nvarchar(100) not null,
AuthorLastName nvarchar(60) not null,
AuthorFirstName nvarchar(60) not null,
AuthorPatronymic nvarchar(60) null,
YearOfPublication date not null check(YearOfPublication > '1900' and YearOfPublication <= getdate()),
Publisher nvarchar(60) not null,
);

/*связь: одна книга имеет много жанров*/
create table BookGenre(
ID int identity(1,1) primary key not null,
BookID varchar(17) references Book(ISBN) not null,
GenreID int references Genre(ID) not null
);

/*связь: одна книга имеет много жанров*/
create table ReaderBook(
ID int identity(1,1) primary key not null,
ReaderID int references Reader(ID) not null,
BookID varchar(17) references Book(ISBN) not null
);

/*заполнение*/
insert into Reader (LastName, FirstName, Patronymic, Email, Phone)
values
('Мельникова','Елизавета','НИколаевна','mail1@mail.ru','89659238001'),
('Нагимова','Динара','Азатовна','mail2@mail.ru','89659238003'),
('Гузаирова','Аделина','Азатовна','mail3@mail.ru','89659238005')

insert into [User] (Login, Password, Role, ReaderID)
values
('Николай', 'password1', 'Библиотекарь', null),
('Лиза', 'password2', 'Читатель', 1)

insert into Genre (NameGenre)
values
('Фантастика'),
('Проза для взрослых'),
('Комедия')

insert into Book (ISBN, NameBook, AuthorLastName, AuthorFirstName, AuthorPatronymic, YearOfPublication, Publisher)
values
('978-5-389-19492-2','О всех созданиях - больших и малых','Херриот','Джеймс', null,'2025','Азбука'),
('978-5-389-19492-3','В окопах Сталинграда','Виктор','Некрасов','Платонович','2025','Азбука'),
('978-5-389-19492-4','Колыбель кошки','Воннегут','Курт', null,'2023','Издательство АСТ')

insert into BookGenre (BookID, GenreID)
values
('978-5-389-19492-4','1'),
('978-5-389-19492-4','3'),
('978-5-389-19492-3','2')

insert into ReaderBook (ReaderID, BookID)
values
('1', '978-5-389-19492-4'),
('2', '978-5-389-19492-4'),
('2', '978-5-389-19492-3')

select *from Reader;
select *from [User];
select *from Book;
select *from Genre;
select *from BookGenre;
select *from ReaderBook;





