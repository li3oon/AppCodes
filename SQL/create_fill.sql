use variant;
go

/*клиенты*/
create table Client(
ID int identity(1,1) primary key not null,
LastName nvarchar(100) not null,
FirstName nvarchar(100) not null,
Patronymic nvarchar(100) null,
Email nvarchar(255) not null unique check(Email like '%@%'),
Phone nvarchar(20) unique not null check(len(Phone) >= 9 and Phone not like '%[a-zA-Z]%')
);

/*аутентификация*/
create table [User](
ID int identity(1,1) primary key not null,
Login nvarchar(60) not null unique,
Password nvarchar(60) not null,
Role nvarchar(60) not null check(Role in ('Техник', 'Клиент')),
IdClient int null references Client(ID)
);

/*статус услуги*/
create table [Status](
ID int identity(1,1) primary key not null,
Progress nvarchar(20) not null unique check (Progress in ('Завершено', 'В процессе', 'Ожидание', 'Отменено'))
);

/*услуга*/
create table [Service](
ID int identity(1,1) primary key not null,
NameService nvarchar(100) not null,
DescriptionService nvarchar(500) NULL,
Cost money not null check(Cost > 0),
IsDeleted bit not null default 0
);

/*заказ*/
create table [Order](
ID int identity(1,1) primary key not null,
IdClient int references Client(ID) not null,
IdService int references [Service](ID) not null,
IdStatus int references [Status](ID) not null,
CreatedDate date not null default getdate(),
CompletedDate date null,
IsDeleted bit not null default 0,

constraint CheckDates check(
CreatedDate <= getdate() and CreatedDate > '2023-12-31' and
(CompletedDate is null or CreatedDate < CompletedDate))
);

insert into Client (LastName, FirstName, Patronymic, Email, Phone)
values
('Мельникова', 'Елизавета', 'Николаевна', 'mail@mail.ru', '89659238002'),
('Гузаирова', 'Аделина', 'Романовна', 'mail@yandex.ru', '89659238012'),
('Нагимова', 'Динара', 'Азатовна', 'google@gmail.com', '89659238102')

insert into [User] (Login, Password, Role, IdClient)
values
('Николай', 'password1', 'Техник', null),
('Лиза', 'password2', 'Клиент', 1)

insert into [Status](Progress)
values
('Завершено'),
('В процессе'),
('Ожидание'),
('Отменено')

insert into [Service] (NameService, Cost)
values
('Осмотр легкового автомобиля', '1300'),
('Замена лампочек в фарах', '2500'),
('Химчистка салона', '1000')

insert into [Order](IdClient, IdService, IdStatus, CreatedDate, CompletedDate)
values
('1', '3', '2', '2025-06-06', '2025-10-10'),
('3', '2', '1', '2025-10-01', null),
('2', '1', '4', '2025-06-06', '2025-06-07')

select *from Client;
select *from [Status];
select *from [Service];
select *from [Order];
select *from [User];