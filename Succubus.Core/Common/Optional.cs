namespace Succubus.Core.Common
{
    public abstract class Optional<T>
    {
        public static implicit operator Optional<T>(T value)
        {
            return new Any<T>(value);
        }

        public static implicit operator Optional<T>(None none)
        {
            return new None<T>();
        }
    }
}