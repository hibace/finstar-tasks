CREATE FUNCTION GetDailyPayments
(
    @ClientId bigint,
    @StartDate date,
    @EndDate date
)
RETURNS TABLE
AS
RETURN
(
    WITH Numbers AS (
        SELECT TOP (DATEDIFF(DAY, @StartDate, @EndDate) + 1)
            ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1 AS n
        FROM sys.objects a
        CROSS JOIN sys.objects b
    ),
    DateRange AS (
        SELECT DATEADD(DAY, n, @StartDate) AS Dt
        FROM Numbers
    )
    SELECT 
        d.Dt,
        ISNULL(SUM(cp.Amount), 0) AS Amount
    FROM DateRange d
    LEFT JOIN ClientPayments cp ON 
        cp.ClientId = @ClientId AND 
        CAST(cp.Dt AS date) = d.Dt
    GROUP BY d.Dt
    ORDER BY d.Dt
) 