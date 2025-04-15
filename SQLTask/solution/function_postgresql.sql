CREATE OR REPLACE FUNCTION get_daily_payments(
    p_client_id bigint,
    p_start_date date,
    p_end_date date
)
RETURNS TABLE (
    dt date,
    amount numeric
) AS $$
BEGIN
    RETURN QUERY
    WITH RECURSIVE date_range AS (
        SELECT p_start_date AS dt
        UNION ALL
        SELECT dt + 1
        FROM date_range
        WHERE dt < p_end_date
    )
    SELECT 
        d.dt,
        COALESCE(SUM(cp.amount), 0)::numeric AS amount
    FROM date_range d
    LEFT JOIN client_payments cp ON 
        cp.client_id = p_client_id AND 
        cp.dt::date = d.dt
    GROUP BY d.dt
    ORDER BY d.dt;
END;
$$ LANGUAGE plpgsql; 