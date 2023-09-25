using Microsoft.Extensions.DependencyInjection;
using Reconcilation.Model;
using Reconcilation.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Diagnostics;

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

            // Map the Excel column to Class property name.
            var propertyMapping = new Dictionary<string, string>()
                {
                    { "Id", nameof(ProductModel.Id) },
                    { "ProductCode", nameof(ProductModel.ProductCode) },
                    { "ProductName", nameof(ProductModel.ProductName) },
                    { "Price", nameof(ProductModel.Price) },
                    { "Specification", nameof(ProductModel.Specification) }
                };

            // Fetch the data information from Excel.
            var productModel1 = new List<ProductModel>();
            var productModel2 = new List<ProductModel>();

            // Perform the tasks in parallel.
            Parallel.Invoke(
                () => productModel1 = reconciliationService.ReadDataFromLocation(@"E:\ReconciliationSolution\ProductDetail1.xlsx", propertyMapping),
                () => productModel2 = reconciliationService.ReadDataFromLocation(@"E:\ReconciliationSolution\ProductDetail2.xlsx", propertyMapping)
            );

            // Perform the final reconciliation steps.
            var response = reconciliationService.ReconcileDataInformation(productModel1, productModel2);

            watch.Stop();

            // Display the required information or create the Excel and save the reconciled data information.
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds / 1000} seconds");

        }


    }
}
