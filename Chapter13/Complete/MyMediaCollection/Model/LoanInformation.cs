using System;

namespace MyMediaCollection.Model
{
    public class LoanInformation
    {
        public int Id { get; set; }
        public DateTime LoanDate { get; set; }
        public string BorrowerName { get; set; }
        public string BorrowerEmail { get; set; }
    }
}
