using Microsoft.Extensions.DependencyInjection;
using Reconcilation.Model;
using Reconcilation.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Reconcilation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            // Inject the required service to the main method.
            var collection = new ServiceCollection();
            collection.AddTransient<IReconciliationService, ReconciliationService>();

            IServiceProvider serviceProvider = collection.BuildServiceProvider();
            var reconciliationService = serviceProvider.GetService<IReconciliationService>();


            //var builder = new ConfigurationBuilder()
            //   .SetBasePath(Directory.GetCurrentDirectory())
            //   .AddJsonFile("MappingFile.json", optional: false);

            //IConfiguration config = builder.Build();

            List<string> mappingData = new List<string>()
            {
                  "GrossTransactionAmount",
                  "InternationalFee",
                  "NetAmount"
            };


            // Map the Excel column to Class property name.
            var propertyMapping = new Dictionary<string, string>()
                {
                    { "Transaction creation date", nameof(PaymentModel.TransactionCreationDate) },
                    { "Type", nameof(PaymentModel.Type) },
                    { "Order number", nameof(PaymentModel.OrderNumber) },
                    { "Legacy order ID", nameof(PaymentModel.LegacyOrderId) },
                    { "Buyer username", nameof(PaymentModel.ByuerUserName) },
                    { "Buyer name", nameof(PaymentModel.ByuerName) },
                    { "Ship to city", nameof(PaymentModel.ShipToCity) },
                    { "Ship to province/region/state", nameof(PaymentModel.ShipToProvienceState) },
                    { "Ship to zip", nameof(PaymentModel.ShipToZip) },
                    { "Ship to country", nameof(PaymentModel.ShipToCountry) },

                    { "Net amount", nameof(PaymentModel.NetAmount) },
                    { "Payout currency", nameof(PaymentModel.PayoutCurrency) },
                    { "Payout date", nameof(PaymentModel.PayoutDate) },
                    { "Payout ID", nameof(PaymentModel.PayoutId) },
                    { "Payout method", nameof(PaymentModel.PayoutMethod) },
                    { "Payout status", nameof(PaymentModel.PayoutStatus) },
                    { "Reason for hold", nameof(PaymentModel.ReasonForHold) },
                    { "Item ID", nameof(PaymentModel.ItemId) },
                    { "Transaction ID", nameof(PaymentModel.TransactionId) },
                    { "Item title", nameof(PaymentModel.ItemTitle) },

                    { "Custom label", nameof(PaymentModel.CustomLabel) },
                    { "Quantity", nameof(PaymentModel.Quantity) },
                    { "Item subtotal", nameof(PaymentModel.ItemSubTotal) },
                    { "Shipping and handling", nameof(PaymentModel.ShippingAndHandling) },
                    { "Seller collected tax", nameof(PaymentModel.SellerCollectedTax) },
                    { "eBay collected tax", nameof(PaymentModel.EBayCollectedTax) },
                    { "Final Value Fee - fixed", nameof(PaymentModel.FixedFinalValue) },
                    { "Final Value Fee - variable", nameof(PaymentModel.VarriableFinalValue) },
                    { "Very high \"item not as described\" fee", nameof(PaymentModel.VeryHighItemNotDescribedAsFee) },
                    { "Below standard performance fee", nameof(PaymentModel.BelowStandardPerformanceFee) },


                    { "International fee", nameof(PaymentModel.InternationalFee) },
                    { "Deposit processing fee", nameof(PaymentModel.DepositProcessingFee) },
                    { "Gross transaction amount", nameof(PaymentModel.GrossTransactionAmount) },


                    { "Transaction currency", nameof(PaymentModel.TransactionCurency) },
                    { "Exchange rate", nameof(PaymentModel.ExchangeRate) },
                    { "Reference ID", nameof(PaymentModel.ReferenceId) },
                    { "Description", nameof(PaymentModel.Description) },
                    { "Month text", nameof(PaymentModel.MonthText) },

                };

            // Fetch the data information from Excel.
            var PaymentModel1 = new List<PaymentModel>();
            var PaymentModel2 = new List<PaymentModel>();

            // Perform the tasks in parallel.
            Parallel.Invoke(
                () => PaymentModel1 = reconciliationService.ReadDataFromLocation(@"E:\ReconcilationSolution\ReconcilationFolder\June 2023 Raw Billing Report.xlsx", propertyMapping),
                () => PaymentModel2 = reconciliationService.ReadDataFromLocation(@"E:\ReconcilationSolution\ReconcilationFolder\June 2023 Raw PaymentReport.xlsx", propertyMapping)
            );

            // Perform the final reconciliation steps.
            var response = reconciliationService.ReconcileDataInformation(PaymentModel1, PaymentModel2, mappingData);

            watch.Stop();

            // Display the required information or create the Excel and save the reconciled data information.
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds / 1000} seconds");

        }


    }
}
