namespace Service02.Services.UserService
{
    public interface IUserService
    {
        Task<IEnumerable<long>> GetUsersByIpAddressAsync(string ipAddress);
    }
}
