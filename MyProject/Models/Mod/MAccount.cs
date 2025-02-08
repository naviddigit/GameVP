using DataService.Entity;
using ProjectLayer.Models.Enums;

namespace ProjectLayer.Models.Mod
{
    /// <summary>
    /// احراز هویت
    /// </summary>
    public class MAccount
    {
        private Repository.RAccount rAccounts = new Repository.RAccount();
        private Repository.RTransaction rTransaction = new Repository.RTransaction();

        public Stucture.SFinance finance(string token)
        {
            var user = (Account?)rAccounts.GetByToken(token);
            var transactionList = (List<Transaction>?)rTransaction.List(GetAccountIdByRole(token), true);

            return finance(transactionList, (int)user.CreditorLimit);
        }

        public Stucture.SFinance finance(string token, DateTime DateLast)
        {
            var user = (Account?)rAccounts.GetByToken(token);
            var transactionList = (List<Transaction>?)rTransaction.ListByDateLast(GetAccountIdByRole(token), DateLast, true);

            return finance(transactionList, (int)user.CreditorLimit);
        }

        public Stucture.SFinance finance(int accountId)
        {
            var transactionList = (List<Transaction>?)rTransaction.List((accountId));
            var user = (Account?)rAccounts.Get(accountId);
            return finance(transactionList, (long)user.CreditorLimit);
        }

        public bool financeAllow(int accountId, decimal amount)
        {
            var transactionList = (List<Transaction>?)rTransaction.List(accountId);
            var user = (Account?)rAccounts.Get(accountId);
            var F = finance(transactionList, (long)user.CreditorLimit);

            //if ((F.Balance - amount) + F.CreditorLimit <= 0)
            if ((((long)user.CreditorLimit + F.Balance) - amount) < 0)
                return false;
            return true;
        }

        private Stucture.SFinance finance(List<Transaction>? transactionList, long? creditorLimit)
        {
            var Creditor = transactionList?.Sum(i => i.Creditor);
            var Debtor = transactionList?.Sum(i => i.Debtor);
            var Charge = transactionList?.Where(i => i.TransactionTypeId == (int)ETransaction.Type.Charge).Sum(i => i.Debtor);
            var Deposit = transactionList?.Where(i => i.TransactionTypeId == (int)ETransaction.Type.Deposit)?.Sum(i => i.Creditor);

            var Balance = Creditor - Debtor;

            List<decimal> chartData = new List<decimal>();

            for (int i = 10; i >= 0; i--)
            {
                int start = -i;
                int end = -i + 1;
                var dateStart = (Convert.ToDateTime(DateTime.Now.ToShortDateString())).AddDays(start);
                var dateEnd = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddDays(end);

                var item = transactionList.Where(i =>
                        i.Date > dateStart &&
                        i.Date < dateEnd);

                chartData.Add(
                        item.Where(i => i.TransactionTypeId == (int)ETransaction.Type.Buy && i.ParentId == null).Sum(i => i.Debtor)
                        - item.Where(i => i.TransactionTypeId == (int)ETransaction.Type.Profit).Sum(i => i.Creditor)
                    );
            }


            return new Stucture.SFinance()
            {
                Balance = Balance,
                BalanceRemaining = Balance <= 0 ? 0 : (Balance / Deposit) * 100,

                Deposit = Deposit,

                CreditorLimit = Balance < 0 ? (creditorLimit + Balance) : creditorLimit,
                CreditorRemaining = Balance > 0 ? creditorLimit == 0 ? 0 : 100 : creditorLimit == 0 ? 0 : ((creditorLimit + Balance) / creditorLimit) * 100,

                TotalSales = Debtor - Charge,
                TotalSalesChart = chartData,

                Today = chartData[10],
                Percent = chartData[9] != 0 ? (((chartData[10] - chartData[9]) / chartData[9]) * 100) : 0,
            };
        }


        public object list(string token)
        {
            var account = (Account?)rAccounts.GetByToken(token);
            int accountId = GetAccountIdByRole(token);

            var listAccount = (List<Account?>)rAccounts.List(0);

            var list = (List<Account>)rAccounts.List(accountId == 0 ? 0 : accountId);
            return list.Select(i => new
            {
                i.Id,
                i.Username,

                UsernameTrack = i.Username + i.ParentId == null ? "" : (i.Username + " (" + rAccounts.GetUsernameById((int?)i?.ParentId) + ")"),
                
                RoleId = i.RoleId,
                i.Mobile,
                name = i.FirstName + " " + i.LastName,
                avatarUrl = "https://minimal-assets-api.vercel.app/assets/images/avatars/avatar_" + new Random().Next(1, 9) + ".jpg",
                //avatarUrl = "https://api-prod-minimal-v4.vercel.app/assets/images/avatars/avatar_" + new Random().Next(1, 24) + ".jpg",
                role = i.Role?.Name,
                isVerified = i.Active,
                status = i.Active ? "active" : "banned",
                finance = finance(i.Id),
                countUser = new Repository.RUser().Count(i.Id),
                i.CreditorLimit,
                ProductPrice = Structure.Publics.CurrencyFormat2(new Repository.RProductUser().GetPrice(i.Id), " Toman", 0)
            }).OrderByDescending(i => i.countUser).ToList();
        }

        public static int GetAccountIdByRole(string token)
        {
            var account = (Account?)new Repository.RAccount().GetByToken(token);

            return account.RoleId > 2 ? account.Id : 0;
        }
        public static int GetAccountIdByRole(int accountId)
        {
            var account = (Account?)new Repository.RAccount().GetById(accountId);

            return account.RoleId > 2 ? account.Id : 0;
        }

        public object? Get(string token, string username)
        {
            var accountId = MAccount.GetAccountIdByRole(token);

            var getAccount = (Account?)rAccounts.Get(accountId, username);
            return new
            {
                id = getAccount?.Id,
                username = getAccount?.Username,
                password = getAccount?.Password,
                Mobile = getAccount?.Mobile,
                Role = getAccount?.Role,
                Email = getAccount?.Email,
                CreditorLimit = getAccount?.CreditorLimit,
                status = (bool)getAccount.Active == true ? "active" : "banned",

            };

        }
    }
}