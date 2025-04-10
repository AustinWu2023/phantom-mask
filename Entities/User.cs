using System.ComponentModel.DataAnnotations;

namespace phantom_mask.Entities {
    public class User : BaseEntity{

        public string Name { get; private set; } = string.Empty;
        
        public decimal CashBalance { get; private set; }

        private List<PurchaseHistory> _purchaseHistories = [];
        public IEnumerable<PurchaseHistory> PurchaseHistories => _purchaseHistories.AsReadOnly();

        public User() {

        }

        public User(string name, decimal cashbalance) {
            Name = name;
            CashBalance = cashbalance;
        }

        public static User Create(string name, decimal cashbalance) {
            return new User(name, cashbalance);
        }

        public void AddPurchaseHistory(List<PurchaseHistory> purchasehistories) {
            _purchaseHistories.AddRange(purchasehistories);
        }

        public void DeductCashBalance(decimal amount) {
            CashBalance -= amount;
        }

    }
}
