using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NpgsqlTypes;

namespace Service02.Models
{
    public class ComparableNpgsqlInetConverter : ValueConverter<ComparableNpgsqlInet, string>
    {
        public ComparableNpgsqlInetConverter()
            : base(
                v => v.ToString(),
                v => new ComparableNpgsqlInet(new NpgsqlInet(v)))
        {
        }
    }
}
