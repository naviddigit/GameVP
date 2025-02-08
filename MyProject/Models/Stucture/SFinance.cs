namespace ProjectLayer.Models.Stucture
{
    public class SFinance
    {
        public decimal? Balance { get; set; }
        public decimal? TotalSales { get; set; }
        public object? TotalSalesChart { get; set; }
        public object? Percent { get; set; }
        public object? Today { get; set; }



        private object _BalanceRemaining { get; set; }
        public object BalanceRemaining { get { return Math.Round((decimal)_BalanceRemaining); } set { _BalanceRemaining = value; } }

        public decimal? Deposit { get; set; }
        public decimal? CreditorLimit { get; set; }

        private object _CreditorRemaining { get; set; }
        public object? CreditorRemaining { get { return Math.Round(Convert.ToDecimal(_CreditorRemaining)); } set { _CreditorRemaining = value; } }
    }
}