# Task Management Service

Сервис управления задачами пользователя, реализованный на .NET 8 с использованием современных практик и паттернов разработки.

## Архитектура

Проект построен по принципам Clean Architecture и включает следующие слои:

- **TaskManagementService.API** - Web API слой
- **TaskManagementService.Core** - Общие интерфейсы и абстракции
- **TaskManagementService.Infrastructure** - Реализация инфраструктурных сервисов
- **TaskManagementService.Application** - Слой бизнес-логики
- **TaskManagementService.Domain** - Доменная модель
- **TaskManagementService.Shared** - Общие DTO и контракты

## Технологический стек

- .NET 8
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- RabbitMQ
- AutoMapper
- FluentValidation
- MediatR (CQRS)
- OpenTelemetry

## Требования

- .NET 8 SDK
- Docker и Docker Compose
- PostgreSQL
- RabbitMQ

## Запуск проекта

1. Клонируйте репозиторий
2. Перейдите в директорию проекта
3. Выполните команду:
```bash
docker-compose up -d
```

## API Endpoints

### Tasks

- `GET /api/tasks` - Получение списка задач
- `GET /api/tasks/{id}` - Получение задачи по ID
- `POST /api/tasks` - Создание новой задачи
- `PUT /api/tasks/{id}` - Обновление задачи
- `DELETE /api/tasks/{id}` - Удаление задачи

## Структура проекта

```
TaskManagementService/
├── TaskManagementService.API/          # Web API
├── TaskManagementService.Core/         # Общие интерфейсы
├── TaskManagementService.Infrastructure/ # Инфраструктура
├── TaskManagementService.Application/  # Бизнес-логика
├── TaskManagementService.Domain/       # Доменная модель
└── TaskManagementService.Shared/       # Общие DTO
```

## Особенности реализации

- Clean Architecture
- CQRS паттерн
- Event-driven архитектура
- Потокобезопасность
- Валидация через FluentValidation
- Маппинг через AutoMapper
- Конфигурация через IEntityTypeConfiguration
- DI через IServiceCollection extensions
- Логирование через Serilog
- Трассировка через OpenTelemetry 