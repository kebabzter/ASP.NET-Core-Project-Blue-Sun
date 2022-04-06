namespace BlueSun.Services.Users
{
    using BlueSun.Data;
    using BlueSun.Data.Models;

    public class UserService : IUserService
    {
        private readonly BlueSunDbContext data;

        public UserService(BlueSunDbContext data) => this.data = data;

        public decimal GetBalanceByUserId(string userId)
        {
            var wallet = this.data
                       .Wallets
                       .FirstOrDefault(w => w.UserId == userId);

            if (wallet == null)
            {
                return -1;
            }

            return wallet.Balance;
        }
    }
}
