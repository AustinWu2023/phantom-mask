namespace phantom_mask.Entities {
    public class PurchaseHistory : BaseEntity {
        public User User { get; private set; }

        public Pharmacy Pharmacy { get; private set; }

        public Mask Mask { get; private set; }

        public decimal TransactionAmount { get; private set; }

        public DateTime TransactionDate { get; private set; }


        private PurchaseHistory() {

        }

        private PurchaseHistory(User user, Pharmacy pharmacy, Mask mask, decimal transactionamount, DateTime transactiondate) {
            User = user;
            Pharmacy = pharmacy;
            Mask = mask;
            TransactionAmount = transactionamount;
            TransactionDate = transactiondate;
        }


        public static PurchaseHistory Create(User user, Pharmacy pharmacy, Mask mask, decimal transactionamount, DateTime transactiondate) {
            return new PurchaseHistory(user, pharmacy, mask, transactionamount, transactiondate);
        }

    }
}
