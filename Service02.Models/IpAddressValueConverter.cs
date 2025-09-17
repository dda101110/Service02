using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Net;

namespace Service02.Models
{
    public class IpAddressValueConverter : ValueConverter<IPAddress, string>
    {
        public IpAddressValueConverter()
            : base(
                  v => v.ToString(),
                  s => string.IsNullOrEmpty(s) ? null : IPAddress.Parse(s)
              )
        { }
    }
}
