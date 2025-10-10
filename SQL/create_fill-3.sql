use [variant-3]
go

/*водительские права*/
create table DriverLicense(
ID int identity(1,1) primary key not null,
Category nvarchar(3) not null,
DateOfIssue date not null,
DateOfExp date not null,

constraint TimeLimitOne check(
DateOfIssue > '2000-01-01' and (DateOfIssue < DateOfExp and DateOfIssue <= getdate()))
);

/*водитель*/
create table Driver(
ID int identity(1,1) primary key not null,
LastName nvarchar(60) not null,
FirstName nvarchar(60) not null,
Patronymic nvarchar(60) null,
Email nvarchar(255) unique not null check(Email like '%@%'),
Phone nvarchar(20) unique not null check(len(Phone) >= 9 and Phone not like '%[a-zA-Z]%'),
DriverLicenseID int references DriverLicense(ID) not null
);

/*аутентификация*/
create table [User](
ID int identity(1,1) primary key not null,
Login nvarchar(60) unique not null,
Password nvarchar(60) not null,
Role nvarchar(60) not null check(Role in ('Диспетчер','Водитель')),
DriverID int references Driver(ID) null
);

/*маршрут*/
create table [Route](
ID int identity(1,1) primary key not null,
PointOfDeparture nvarchar(40) not null check(len(PointOfDeparture) >= 3),
Destination nvarchar(40) not null check(len(Destination) >= 3)
);

/*рейс*/
create table Voyage(
ID int identity(1,1) primary key not null,
NameCompany nvarchar(100) not null,
RouteID int references [Route](ID),
DepartureDate datetime2(0) not null,
ArrivalDate datetime2(0) not null,

constraint TimeLimitTwo check(ArrivalDate > DepartureDate));

/*связь: один водитель несколько рейсов*/
create table DriverVoyage(
ID int identity(1,1) primary key not null,
DriverID int references Driver(ID) not null,
VoyageID int references Voyage(ID) not null
);

insert into DriverLicense(Category, DateOfIssue, DateOfExp)
values
('D','2014-12-12','2024-12-12'),
('DE','2018-06-09','2028-06-09'),
('D','2020-03-03','2030-03-03')

insert into Driver(LastName, FirstName, Patronymic, Email, Phone, DriverLicenseID)
values
('Мельников','Николай','Александрович','mail1@mail.ru','89659238012','2'),
('Гапашов','Айрат','Анатольевич','mail2@mail.ru','89659238013','1'),
('Усманов','Кирилл','Антонович','mail3@mail.ru','89659238023','3')

insert into [User](Login, Password, Role, DriverID)
values
('Лиза','password1','Диспетчер', null),
('Николай','password2','Водитель','1')

insert into [Route](PointOfDeparture, Destination)
values
('Уфа','Москва'),
('Самара','Уфа'),
('Уфа','Екатеринбург')

insert into Voyage(NameCompany, RouteID, DepartureDate, ArrivalDate)
values
('Компания1','2', '2025-01-01 01:10:00', '2025-01-03 11:10:00'),
('Компания2','3', '2025-03-21 10:00:00', '2025-03-23 10:00:00'),
('Компания3','1', '2025-12-30 11:00:00', '2026-01-01 10:00:00')

insert into DriverVoyage(DriverID, VoyageID)
values
('1','2'),
('1','1'),
('3','3')

select *from DriverLicense;
select *from Driver;
select *from [User];
select *from [Route];
select *from Voyage;
select *from DriverVoyage;