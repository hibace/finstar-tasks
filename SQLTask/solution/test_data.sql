-- Создание таблицы платежей
CREATE TABLE IF NOT EXISTS ClientPayments (
    Id BIGINT PRIMARY KEY,
    ClientId BIGINT NOT NULL,
    Dt TIMESTAMP(0) NOT NULL,
    Amount MONEY NOT NULL
);

-- Очистка существующих данных
TRUNCATE TABLE ClientPayments;

-- Вставка тестовых данных
INSERT INTO ClientPayments (Id, ClientId, Dt, Amount) VALUES
(1, 1, '2022-01-03 17:24:00', 100),
(2, 1, '2022-01-05 17:24:14', 200),
(3, 1, '2022-01-05 18:23:34', 250),
(4, 1, '2022-01-07 10:12:38', 50),
(5, 2, '2022-01-05 17:24:14', 278),
(6, 2, '2022-01-10 12:39:29', 300);

-- Тестовые запросы
-- Пример 1: ClientId = 1, период 2022-01-02 - 2022-01-07
SELECT * FROM GetDailyPayments(1, '2022-01-02', '2022-01-07');

-- Пример 2: ClientId = 2, период 2022-01-04 - 2022-01-11
SELECT * FROM GetDailyPayments(2, '2022-01-04', '2022-01-11'); 