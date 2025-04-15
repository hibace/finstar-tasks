# Task Management Service

## Описание
Task Management Service - это микросервис для управления задачами, построенный с использованием .NET 8 и следующих технологий:
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- RabbitMQ
- DotNetCAP
- MediatR
- AutoMapper
- FluentValidation
- OpenTelemetry
- xUnit

## Архитектура
Сервис построен по принципам чистой архитектуры (Clean Architecture) и разделен на следующие слои:

- **TaskManagementService.API**: Слой представления, содержащий контроллеры и конфигурацию приложения
- **TaskManagementService.Application**: Слой приложения, содержащий CQRS команды, запросы и их обработчики
- **TaskManagementService.Domain**: Слой домена, содержащий бизнес-сущности и интерфейсы
- **TaskManagementService.Infrastructure**: Слой инфраструктуры, содержащий реализацию репозиториев и внешних сервисов
- **TaskManagementService.Core**: Общий слой, содержащий общие компоненты и утилиты
- **TaskManagementService.Shared**: Слой с общими DTO и моделями
- **TaskManagementService.Tests**: Слой с модульными тестами

## Функциональность
- Создание, обновление и удаление задач
- Получение списка задач с фильтрацией и сортировкой
- Получение задачи по идентификатору
- Асинхронная обработка команд через MediatR
- Валидация входных данных через FluentValidation
- Маппинг объектов через AutoMapper
- Интеграция с RabbitMQ через DotNetCAP для публикации событий
- Логирование и мониторинг через OpenTelemetry

## Требования
- .NET 8 SDK
- PostgreSQL 15+
- RabbitMQ 3.12+
- Docker (опционально)

## Запуск проекта
1. Клонировать репозиторий
2. Установить зависимости:
   ```bash
   dotnet restore
   ```
3. Настроить подключение к базе данных в `appsettings.json`
4. Настроить подключение к RabbitMQ в `appsettings.json`
5. Запустить миграции:
   ```bash
   dotnet ef database update --project TaskManagementService/TaskManagementService.Infrastructure --startup-project TaskManagementService/TaskManagementService.API
   ```
6. Запустить приложение:
   ```bash
   dotnet run --project TaskManagementService/TaskManagementService.API
   ```

## API Endpoints
- `GET /api/tasks` - Получение списка задач
- `GET /api/tasks/{id}` - Получение задачи по ID
- `POST /api/tasks` - Создание новой задачи
- `PUT /api/tasks/{id}` - Обновление задачи
- `DELETE /api/tasks/{id}` - Удаление задачи

## Тестирование
Для запуска тестов выполните:
```bash
dotnet test
```

## Мониторинг
Сервис интегрирован с OpenTelemetry для сбора метрик и трейсов. Метрики доступны через Prometheus, а трейсы можно просматривать в Jaeger.

## Лицензия
MIT 