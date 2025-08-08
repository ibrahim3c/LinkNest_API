﻿using System.Text.Json.Serialization;

namespace LinkNest.Domain.Abstraction
{
    public class Result<T>
    {
        public T? Value { get; }
        public bool IsSuccess { get; }
        public List<string>? Errors { get; }

        private Result(T? value, bool isSuccess, List<string>? errors = null)
        {
            Value = value;
            IsSuccess = isSuccess;
            Errors = errors ?? new List<string>();
        }

        // Factory Method for Success
        public static Result<T> Success(T value) => new Result<T>(value, true);

        // Factory Method for Failure (No need to pass a value)
        public static Result<T> Failure(List<string> errors) => new Result<T>(default, false, errors);
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public List<string>? Errors { get; }

        private Result(bool isSuccess, List<string>? errors = null)
        {
            IsSuccess = isSuccess;
            this.Errors = errors ?? new List<string>();
        }

        // Factory Method for Success
        public static Result Success()
            => new Result(true);

        // Factory Method for Failure
        public static Result Failure(List<string> errors)
            => new Result(false, errors);

    }
    public class AuthResult
    {
        public List<string>? Messages { get; set; }
        public string? Token { get; set; }
        public bool Success { get; set; }

        // he will not sedn the refresh token in response but we will send it in cookie to frontend
        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiresOn { get; set; }


    }
}
