namespace phantom_mask.Share.JsonDto {
    public class UserSeedDto {
        public string name { get; set; } = string.Empty;
        public decimal cashBalance { get; set; }
        public List<PurchaseHistoryDto> purchaseHistories { get; set; }
    }


    public class PurchaseHistoryDto {
        public string pharmacyName { set; get; } = string.Empty;
        public string maskName { set; get; } = string.Empty;
        public decimal transactionAmount { set; get; }
        public string transactionDate { set; get; } 
    }
}
