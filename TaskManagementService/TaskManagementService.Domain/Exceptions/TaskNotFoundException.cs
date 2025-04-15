using System;

namespace TaskManagementService.Domain.Exceptions
{
    /// <summary>
    /// Exception thrown when a task is not found in the system
    /// </summary>
    public class TaskNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskNotFoundException"/> class
        /// </summary>
        public TaskNotFoundException()
            : base("Task not found")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskNotFoundException"/> class with a specific message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public TaskNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskNotFoundException"/> class with a specific message and inner exception
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public TaskNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
} 