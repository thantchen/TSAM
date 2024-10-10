using System;
using System.Collections.Generic;
using System.Text;

namespace TamsEmail.Views.Templates
{

    public class StatusChangeDisputeLogViewModel : StatusChangeViewModel
    {
        public string InvoiceNumber { get; set; }
        public string AgreedCost { get; set; }
        public string Currency { get; set; }
    }
}
