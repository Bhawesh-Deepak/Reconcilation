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

            ///Inject the required service to the main method.
            var collection = new ServiceCollection();
            collection.AddTransient<IReconcilationService, ReconcilationService>();

            IServiceProvider serviceProvider = collection.BuildServiceProvider();
            var reconcilationService = serviceProvider.GetService<IReconcilationService>();

            ///Map the Excel column to Class property name.
            var propertyMapping = new Dictionary<string, string>()
            {
                { "Id", nameof(ProductModel.Id) },
                { "ProductCode", nameof(ProductModel.ProductCode) },
                { "ProductName", nameof(ProductModel.ProductName) },
                { "Price", nameof(ProductModel.Price) },
                { "Specification", nameof(ProductModel.Specification) }
            };

            //Fetch the data Information from Excel
            var productModel1 =new List<ProductModel>();

            //Fetch the data Information from Excel
            var productModel2 = new List<ProductModel>();


            // Perform the Task parlelly
            Parallel.Invoke(
                () => productModel1 = reconcilationService.ReadDataFromLocation(@"E:\ReconcilationSolution\ProductDetail1.xlsx", propertyMapping),
                () => productModel2 = reconcilationService.ReadDataFromLocation(@"E:\ReconcilationSolution\ProductDetail2.xlsx", propertyMapping)
                );

            //Perform the final reconcilation steps;
            var response = reconcilationService.ReconcileDataInformation(productModel1, productModel2);

            watch.Stop();

            //Display the required information : Or create the Excel and save the reconcile data information
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds/1000} seconds");
        }


    }
}
