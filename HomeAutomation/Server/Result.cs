using System;

namespace HomeAutomation.Server
{
    public class Result<T>
    {
        private Result(T value = default, string errorMessage = null)
        {
            this.Value = value;
            this.ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value: value);
        }

        public static Result<T> Failure(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentNullException(nameof(errorMessage));
            }

            return new Result<T>(errorMessage: errorMessage);
        }

        public T Value { get; }

        public string ErrorMessage { get; }

        public bool IsSuccessful => this.ErrorMessage == null;
    }
}
