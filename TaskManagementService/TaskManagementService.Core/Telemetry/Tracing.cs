using System.Diagnostics;

namespace TaskManagementService.Core.Telemetry
{
    /// <summary>
    /// Класс для работы с трассировкой
    /// </summary>
    public static class Tracing
    {
        /// <summary>
        /// Имя источника трассировки
        /// </summary>
        public const string SourceName = "TaskManagementService";

        /// <summary>
        /// Создает и возвращает источник трассировки
        /// </summary>
        public static ActivitySource Source = new ActivitySource(SourceName);

        /// <summary>
        /// Создает новую активность для трассировки
        /// </summary>
        /// <param name="name">Имя активности</param>
        /// <param name="kind">Тип активности</param>
        /// <returns>Новая активность</returns>
        public static Activity? StartActivity(string name, ActivityKind kind = ActivityKind.Internal)
        {
            return Source.StartActivity(name, kind);
        }

        /// <summary>
        /// Добавляет тег к активности
        /// </summary>
        /// <param name="activity">Активность</param>
        /// <param name="key">Ключ тега</param>
        /// <param name="value">Значение тега</param>
        public static void AddTag(Activity activity, string key, object value)
        {
            activity?.SetTag(key, value);
        }

        /// <summary>
        /// Добавляет событие к активности
        /// </summary>
        /// <param name="activity">Активность</param>
        /// <param name="name">Имя события</param>
        /// <param name="tags">Теги события</param>
        public static void AddEvent(Activity activity, string name, params (string Key, object Value)[] tags)
        {
            var eventTags = new ActivityTagsCollection();
            foreach (var (key, value) in tags)
            {
                eventTags.Add(key, value);
            }
            activity?.AddEvent(new ActivityEvent(name, tags: eventTags));
        }
    }
} 