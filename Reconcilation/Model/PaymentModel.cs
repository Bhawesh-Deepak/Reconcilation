using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
namespace Reconcilation.Model
{
    public class PaymentModel
    {


        public string TransactionCreationDate { get; set; }
        public string Type { get; set; }

        public string OrderNumber { get; set; }
        public string LegacyOrderId { get; set; }
        public string ByuerUserName { get; set; }
        public string ByuerName { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToProvienceState { get; set; }
        public string ShipToZip { get; set; }
        public string ShipToCountry { get; set; }
        public string NetAmount { get; set; }
        public string PayoutCurrency { get; set; }
        public string PayoutDate     { get; set; }
        public string PayoutId { get; set; }
        public string PayoutMethod { get; set; }
        public string PayoutStatus { get; set; }

        public string ReasonForHold { get; set; }
        public string ItemId { get; set; }

        public string TransactionId { get; set; }
        public string ItemTitle { get; set; }
        public string CustomLabel { get; set; }
        public string Quantity { get; set; }
        public string ItemSubTotal { get; set; }
        public string ShippingAndHandling { get; set; }
        public string SellerCollectedTax { get; set; }
        public string EBayCollectedTax { get; set; }
        public string FixedFinalValue { get; set; }
        public string VarriableFinalValue { get; set; }
        public string VeryHighItemNotDescribedAsFee { get; set; }
        public string BelowStandardPerformanceFee { get; set; }
        public string TransactionCurency { get; set; }
        public string ExchangeRate { get; set; }

        public string ReferenceId { get; set; }
        public string Description { get; set; }

        public string MonthText { get; set; }
        public string InternationalFee { get; set; }
        public string DepositProcessingFee { get; set; }
        public string GrossTransactionAmount { get; set; }


    }
}
