namespace Succubus.Core.Common
{
    public sealed class Any<T> : Optional<T>
    {
        public T Value { get; }

        public Any(T value)
        {
            Value = value;
        }

        public static implicit operator T(Any<T> any) => any.Value;
    }
}