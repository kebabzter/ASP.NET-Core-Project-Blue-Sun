namespace BlueSun.Services.Users
{
    public interface IUserService
    {
        public decimal GetBalanceByUserId(string userId);
    }
}
