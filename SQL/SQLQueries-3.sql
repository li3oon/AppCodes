/*вывод рейсов водителя*/
SELECT
    Voyage.ID AS [Номер рейса],
    Voyage.DepartureDate AS [Дата отправки],
    Route.PointOfDeparture AS [Пункт отправки],
    Route.Destination AS [Пункт прибытия]
FROM
    Driver
    INNER JOIN DriverVoyage ON Driver.ID = DriverVoyage.DriverID
    INNER JOIN Voyage ON DriverVoyage.VoyageID = Voyage.ID
    INNER JOIN Route ON Voyage.RouteID = Route.ID
WHERE
    DriverVoyage.DriverID = 1;

/*вывод рейсов за указанный период времени*/
SELECT 
    Voyage.ID AS [Номер рейса], 
    Voyage.DepartureDate AS [Дата отправки], 
    Voyage.ArrivalDate AS [Дата прибытия], 
    Route.PointOfDeparture AS [Пункт отправки], 
    Route.Destination AS [Пункт прибытия], 
    Voyage.NameCompany AS [Компания]
FROM 
    Voyage
    INNER JOIN Route ON Voyage.RouteID = Route.ID
WHERE
    Voyage.DepartureDate = '2025-03-21 10:00:00';

/*вывод ресов водителя по датам*/
SELECT 
    Voyage.ID AS [Номер рейса], 
    Voyage.DepartureDate AS [Дата отправки], 
    Route.PointOfDeparture AS [Пункт отправки], 
    Route.Destination AS [Пункт прибытия]
FROM 
    DriverVoyage
    INNER JOIN Voyage ON DriverVoyage.VoyageID = Voyage.ID
    INNER JOIN Route ON Voyage.RouteID = Route.ID
WHERE 
    DriverVoyage.DriverID = 1
    /*AND Voyage.DepartureDate >= '2024-12-30 01:10:00' and Voyage.DepartureDate <= '2025-03-21 10:00:00';*/
	and Voyage.DepartureDate between '2024-12-30 01:10:00' and '2025-03-21 10:00:00';