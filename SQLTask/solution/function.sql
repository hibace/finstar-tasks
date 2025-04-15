CREATE OR REPLACE FUNCTION GetDailyPayments(
    p_ClientId BIGINT,
    p_StartDate DATE,
    p_EndDate DATE
)
RETURNS TABLE (
    PaymentDate DATE,
    Amount MONEY
) AS $$
BEGIN
    RETURN QUERY
    WITH RECURSIVE DateRange AS (
        SELECT p_StartDate AS Date
        UNION ALL
        SELECT Date + 1
        FROM DateRange
        WHERE Date < p_EndDate
    )
    SELECT 
        dr.Date AS PaymentDate,
        COALESCE(SUM(cp.Amount), 0::MONEY) AS Amount
    FROM DateRange dr
    LEFT JOIN ClientPayments cp ON 
        cp.ClientId = p_ClientId AND
        DATE(cp.Dt) = dr.Date
    GROUP BY dr.Date
    ORDER BY dr.Date;
END;
$$ LANGUAGE plpgsql; 