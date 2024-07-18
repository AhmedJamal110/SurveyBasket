namespace SurveyBasket.ApI.Abstractions
{
    public class TResult<TValue> : Result
    {
        private readonly TValue? _value;
        public TResult(bool isSuccess, Error error , TValue value) : base(isSuccess, error)
        {
            _value = value;
        }

        public TValue Value 
            => IsSuccess ? _value! : throw new InvalidOperationException("Faliur Result can thave any value");

    }
}
