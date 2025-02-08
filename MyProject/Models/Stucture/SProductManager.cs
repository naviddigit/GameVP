namespace ProjectLayer.Models.Stucture
{
    public class SProductManager
    {
        public int AccountId { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string Group_IBSng { get; set; }
        
        public decimal price { get; set; }
        public decimal? parentProfit { get; set; }
        public decimal? parentPrice { get; set; }
        public int? parentAccountId { get; set; }
    }
}