using phantom_mask.Share.JsonDto;
using phantom_mask.Utilities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace phantom_mask.Entities {
    public class Pharmacy : BaseEntity {        

        public string PharmacyName { get; private set; } = string.Empty;
        
        public decimal CashBalance { get; private set; }

        private List<Mask> _masks = []; 
        public IEnumerable<Mask> Masks => _masks.AsReadOnly();

        private List<OpeningHour> _openingHours = [];
        public IEnumerable<OpeningHour> OpeningHours => _openingHours.AsReadOnly();


        private Pharmacy() {

        }

        private Pharmacy(string pharmacyname, decimal cashbalance) {
            PharmacyName = pharmacyname;
            CashBalance = cashbalance;
        }


        public static Pharmacy Create(string pharmacyname, decimal cashbalance) {
            return new Pharmacy(pharmacyname, cashbalance);
        }

        public void AddOpeningHours(List<OpeningHour> hours) {
            _openingHours.AddRange(hours);
        }

        public void AddMaks(List<Mask> masks) {
            _masks.AddRange(masks);
        }

        public void AddCashBalance(decimal amount) {
            CashBalance += amount;
        }
    }
}
