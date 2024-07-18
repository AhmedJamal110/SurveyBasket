namespace SurveyBasket.ApI.Abstractions
{
    public class Result
    {
        public Result(bool isSuccess, Error error)
        {
            if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; set; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; set; } = default!;


        public static Result Success() 
            => new(true, Error.None);

        public static Result Failure(Error error)
            =>  new(false, error);
        

        public static TResult<TValue> Success<TValue>(TValue value)
             => new(true, Error.None, value);
        

        public static TResult<TValue> Faliure<TValue>(Error error)
            => new(false, error, default!);
        

    }



}
