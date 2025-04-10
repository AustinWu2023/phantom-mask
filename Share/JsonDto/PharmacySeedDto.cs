namespace phantom_mask.Share.JsonDto {
    public class PharmacySeedDto {

        public string name { get; set; } = string.Empty;
        public decimal cashBalance { get; set; }
        public string openingHours { get; set; } = string.Empty;
        public List<MaskDto> masks { get; set; }

    }

    public class MaskDto {
        public string name { get; set; } = string.Empty;
        public decimal price { get; set; }
    }

}
