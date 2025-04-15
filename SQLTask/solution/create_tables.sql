-- SQL Server
CREATE TABLE ClientPayments
(
    Id bigint IDENTITY(1,1) PRIMARY KEY,
    ClientId bigint NOT NULL,
    Dt datetime2(0) NOT NULL,
    Amount money NOT NULL
);

-- PostgreSQL
CREATE TABLE client_payments
(
    id bigserial PRIMARY KEY,
    client_id bigint NOT NULL,
    dt timestamp NOT NULL,
    amount numeric NOT NULL
); 