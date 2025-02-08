using DataService.Entity;
using ProjectLayer.Models.Enums;
using ProjectLayer.Models.Stucture;

namespace ProjectLayer.Models.Mod
{
    /// <summary>
    ///  مالی
    /// </summary>
    public class MTransaction
    {
        private Repository.RAccount rAccounts = new Repository.RAccount();
        private Repository.RTransaction rTransaction = new Repository.RTransaction();


        public object list(string token)
        {
            var account = (Account?)rAccounts.GetByToken(token);
            int accountId = MAccount.GetAccountIdByRole(token);

            var list = (List<Transaction>?)rTransaction.ListAndParent(accountId);
            return list.Select(i => new
            {
                i.Id,
                i.Account?.Username,
                Create = Structure.Publics.DateFormatShortTiming(i.Date),
                i.Creditor,
                i.Debtor,
                ProductName = i.Product == null ? "---" : i.Product?.Name,
                Type = i.TransactionType.Name,
                i.Description,
            });
        }

        public MResponce Charge(string token, string username, decimal amount, int typeId, string description)
        {
            var account = (Account?)rAccounts.GetByToken(token);
            int accountId = MAccount.GetAccountIdByRole(token);

            var accountTo = (Account?)rAccounts.Get(accountId, username);

            if (accountId != 0)
                if (!new MAccount().financeAllow(accountId, amount))
                    return new MResponce { Success = false, Message = "Limited. Low balance" };

            #region Create Finance Charge
            // Charge:
            if (accountId <= 2)
            {
                Transaction newTransaction = new Transaction()
                {
                    AccountId = 2, // hesabdari hashemi
                    Date = DateTime.Now,
                    TransactionTypeId = (int)ETransaction.Type.Charge,
                    Description = "Charge To: " + accountTo.Username + description == "" ? "" : (" / " + description),
                    CurrencyId = (int)ECurrency.TOMAN,
                    RawAmount = amount,
                    Changerate = 50000,
                    Creditor = 0,
                    Debtor = amount,
                };
                new Repository.RTransaction().Insert(newTransaction);
            }
            #endregion


            #region Create Finance Deposit
            // Deposit:
            Transaction newTransactionTo = new Transaction()
            {
                AccountId = accountTo.Id,
                Date = DateTime.Now,
                TransactionTypeId = (int)ETransaction.Type.Deposit,
                Description = "Charge From: " + account?.Username + description == "" ? "" : (" / " + description),
                CurrencyId = (int)ECurrency.TOMAN,
                RawAmount = amount,
                Changerate = 50000,
                Creditor = amount,
                Debtor = 0,
            };
            new Repository.RTransaction().Insert(newTransactionTo);
            #endregion


            return new MResponce { Success = true };
        }


        public void FinanceAction(Account? GetIssuedAccount, SProductManager? productManager,string Description = "Active VPN.")
        {
           
            #region Create Finance
            Transaction newTransaction = new Transaction()
            {
                AccountId = GetIssuedAccount.Id,
                Date = DateTime.Now,
                TransactionTypeId = (int)ETransaction.Type.Buy,
                Description = Description,
                CurrencyId = (int)ECurrency.TOMAN,
                RawAmount = productManager.price,
                Changerate = 50000,
                Creditor = 0,
                Debtor = productManager.price,
                ProductId = productManager.id,
                //ProductUserId = productManager.
            };
            new Repository.RTransaction().Insert(newTransaction);
            #endregion

            #region Create Finance Parent
            if (GetIssuedAccount.ParentId > 2)
            {
                //Transaction newTransactionParent = new Transaction()
                //{
                //    AccountId = (int)GetIssuedAccount.ParentId,
                //    ParentId = newTransaction.Id,
                //    Date = DateTime.Now,
                //    TransactionTypeId = (int)ETransaction.Type.Buy,
                //    Description = Description + " From Ref: " + GetIssuedAccount.Username,
                //    CurrencyId = (int)ECurrency.TOMAN,
                //    RawAmount = productManager.price,
                //    Changerate = 50000,
                //    Creditor = 0,
                //    Debtor = productManager.price,
                //    ProductId = productManager.id,
                //};
                //new Repository.RTransaction().Insert(newTransactionParent);

                Transaction newTransactionParentProfit = new Transaction()
                {
                    AccountId = (int)GetIssuedAccount.ParentId,
                    ParentId = newTransaction.Id,
                    Date = DateTime.Now,
                    TransactionTypeId = (int)ETransaction.Type.Profit,
                    Description = Description + " From Ref: " + GetIssuedAccount.Username,
                    CurrencyId = (int)ECurrency.TOMAN,
                    RawAmount = productManager.parentProfit,
                    Changerate = 50000,
                    Creditor = (decimal)productManager.parentProfit,
                    Debtor = 0,
                    ProductId = productManager.id,
                };
                new Repository.RTransaction().Insert(newTransactionParentProfit);
            }
            #endregion

        }




        //public object? Report(string token)
        //{
        //    var accountId = MAccount.GetAccountIdByRole(token);
        //    var account = new Repository.RAccount().GetByToken(token);


        //    var list = (List<User>?)rUser.List(accountId);

        //    list.AddRange((List<User>?)rUser.ListByParent(accountId));

        //    List<int> chartData = new List<int>();
        //    List<int> chartDataRenew = new List<int>();

        //    List<decimal> data = new List<decimal>();


        //    for (int i = 10; i >= 0; i--)
        //    {
        //        int start = -i;
        //        int end = -i + 1;
        //        var dateStart = (Convert.ToDateTime(DateTime.Now.ToShortDateString())).AddDays(start);
        //        var dateEnd = Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddDays(end);


        //        chartDataRenew.Add(
        //                    list.Count(i =>
        //                    i.RenewDate != null &&
        //                    i.RenewDate.Value >= dateStart &&
        //                    i.RenewDate.Value < dateEnd)
        //                );

        //        chartData.Add(
        //                list.Where(i =>
        //                i.CreateDate > dateStart &&
        //                i.CreateDate < dateEnd).Count()
        //            );


        //    }

        //    return new
        //    {
        //        countRenew = list.Count(i => i?.RenewDate != null),
        //        chartDataRenew = chartDataRenew.ToList(),
        //        todayRenew = chartDataRenew[10],
        //        percentRenew = chartDataRenew[9] != 0 ? (((chartDataRenew[10] - chartDataRenew[9]) / chartDataRenew[9]) * 100) : 0,

        //        count = list.Count,
        //        chartData = chartData.ToList(),
        //        today = chartData[10],
        //        Percent = chartData[9] != 0 ? (((chartData[10] - chartData[9]) / chartData[9]) * 100) : 0,
        //    };
        //}

    }
}