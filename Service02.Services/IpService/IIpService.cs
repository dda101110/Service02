namespace Service02.Services.IpService
{
    public interface IIpService
    {
        Task<IEnumerable<string>> GetIpAddressesAsync(long userId);
    }
}
