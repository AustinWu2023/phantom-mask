using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace phantom_mask.Entities {
    public class Mask : BaseEntity {

        public string ProductName { get; set; } = string.Empty;
       
        public decimal Price { get; set; }

        public List<Pharmacy> _pharmacies = [];

        public IEnumerable<Pharmacy> Pharmacies => _pharmacies.AsReadOnly();

        private Mask() {

        }

        private Mask(string productname, decimal price) {
            ProductName = productname;
            Price = price;
        }

        public static Mask Create(string productname, decimal price) {
            return new Mask(productname, price);
        }
    }
}
