namespace freelanceNew.Models
{
    public class Payment
    {
        public Guid PaymentId { get; set; } // Primary Key
        public Guid ContractId { get; set; } // Foreign Key to Contract
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }

        // Navigation Properties
        public Contract Contract { get; set; }
    }


}
