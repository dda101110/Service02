using NpgsqlTypes;

namespace Service02.Models
{
    public class ComparableNpgsqlInet : IComparable<ComparableNpgsqlInet>, IComparable
    {
        public NpgsqlInet Value { get; }

        public ComparableNpgsqlInet(NpgsqlInet value)
        {
            Value = value;
        }

        public int CompareTo(ComparableNpgsqlInet other)
        {
            return string.Compare(Value.ToString(), other?.Value.ToString(),
                                StringComparison.Ordinal);
        }

        public int CompareTo(object obj)
        {
            if (obj is ComparableNpgsqlInet other)
                return CompareTo(other);
            return 1;
        }

        public static implicit operator ComparableNpgsqlInet(NpgsqlInet value)
            => new ComparableNpgsqlInet(value);

        public static implicit operator NpgsqlInet(ComparableNpgsqlInet wrapper)
            => wrapper.Value;

        public override string ToString() => Value.ToString();
    }
}
