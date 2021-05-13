namespace Succubus.Core.Common
{
    public sealed class None<T> : Optional<T>
    {
    }

    public sealed class None
    {
        public static None Value { get; } = new None();
    }
}