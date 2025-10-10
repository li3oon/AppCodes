use [variant-4]
go

/*покупатель*/
create table Customer(
ID int identity(1,1) primary key not null,
LastName nvarchar(60) not null,
FirstName nvarchar(60) not null,
Patronymic nvarchar(60) null,
Passport nvarchar(10) unique not null check(Passport not like '%[a-zA-Z]%'), 
Email nvarchar(255) unique not null check(Email like '%@%'),
Phone nvarchar(20) unique not null check(len(Phone) >= 9 and Phone not like '%[a-zA-Z]%')
);

/*аутинфикация*/
create table [User](
ID int identity(1,1) primary key not null,
Login nvarchar(60) not null,
Password nvarchar(60) not null,
Role nvarchar(60) not null check(Role in ('Кассир', 'Покупатель')),
CustomerID int references Customer(ID) null
);

/*маршрут*/
create table Direction(
ID int identity(1,1) primary key not null,
DeparturePoint nvarchar(60) not null check(len(DeparturePoint) >= 3),
ArrivalPoint nvarchar(60) not null check(len(ArrivalPoint) >= 3)
);

/*рейс*/
create table Trip(
ID int identity(1,1) primary key not null,
DirectionID int references Direction(ID) not null,
DepartureDate datetime2(0) not null check(DepartureDate <= getdate()),
ArrivalDate datetime2(0) not null,
Airplane nvarchar(30) not null,

constraint TimeLimit check(ArrivalDate > DepartureDate)
);

/*билет*/
create table Ticket(
ID int identity(1,1) primary key not null,
TicketCode nvarchar(12) unique not null,
TripID int references Trip(ID) not null,
Class nvarchar(1) not null check(Class in ('E', 'B', 'F')),
CustomerID int references Customer(ID) not null,
Cost money not null check(Cost > 0)
);

/*заполнение*/
insert into Customer(LastName, FirstName, Patronymic, Passport, Email, Phone) values
('Мельникова', 'Елизавета', 'Николаевна', '123467890', 'mail1@mail.ru', '89659238002'),
('Гузаирова', 'Аделина', 'Романовна', '123467891', 'mail2@mail.ru', '89659238003'),
('Нагимова', 'Динара', 'Азатовна', '123467892', 'mail3@mail.ru', '89659238004')

insert into [User](Login, Password, Role, CustomerID) values
('Лиза','password1','Покупатель', '1'),
('Николай','password2','Кассир', null)

insert into Direction(DeparturePoint, ArrivalPoint) values
('Уфа', 'Екатеринбург'),
('Москва', 'Казань'),
('Санкт-Петербург', 'Москва')

insert into Trip(DirectionID, DepartureDate, ArrivalDate, Airplane) values
('1', '2025-01-01 01:10:00', '2025-01-03 11:10:00', 'Ту-154'),
('2', '2025-04-30 11:00:00', '2025-05-02 10:00:00', 'Ту-134'),
('2', '2025-03-21 10:00:00', '2025-03-23 10:00:00', 'Ан-24')

insert into Ticket(TicketCode, TripID, Class, CustomerID, Cost) values
('123456789КУ', '1', 'B', '1', '10300'),
('223456789КУ', '2', 'B', '1', '10300'),
('123456789КГ', '3', 'F', '3', '9300')

/*вывод*/
select *from Customer;
select *from [User];
select *from Direction;
select *from Trip;
select *from Ticket;