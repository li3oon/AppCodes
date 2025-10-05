/*1*/
SELECT        [Order].ID AS [Номер заказа], Client.LastName AS [Фамилия клиента], Service.Cost AS [Цена за услугу]
FROM            Client INNER JOIN
                         [Order] ON Client.ID = [Order].IdClient INNER JOIN
                         Service ON [Order].IdService = Service.ID INNER JOIN
                         Status ON [Order].IdStatus = Status.ID

/*2*/
SELECT        Client.LastName AS [Фамилия клиента], Client.Phone AS [Телефон клиента], [Order].ID AS [Номер завершенного заказа]
FROM            Client INNER JOIN
                         [Order] ON Client.ID = [Order].IdClient INNER JOIN
                         Service ON [Order].IdService = Service.ID INNER JOIN
                         Status ON [Order].IdStatus = Status.ID
WHERE        (Status.Progress = 'Завершено')

/*3*/
SELECT        [Order].ID AS [Заказы начатые в выбранный день], Service.ID AS [Номер услуги в заказе]
FROM            Client INNER JOIN
                         [Order] ON Client.ID = [Order].IdClient INNER JOIN
                         Service ON [Order].IdService = Service.ID INNER JOIN
                         Status ON [Order].IdStatus = Status.ID
WHERE        ([Order].CreatedDate = CONVERT(DATETIME, '2025-06-06 00:00:00', 102))