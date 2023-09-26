using Reconcilation.Helpers;
using Reconcilation.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reconcilation.Services
{
    public class ReconciliationService : IReconciliationService
    {
        /// <summary>
        /// Read data from Excel File and convert the data from Excel to Class List details
        /// </summary>
        /// <param name="location"></param>
        /// <param name="propertyMapping"></param>
        /// <returns></returns>
        public List<PaymentModel> ReadDataFromLocation(string location, Dictionary<string, string> propertyMapping)
        {
            var dataTable = ReadExcelDataHelper.ConvertExcelToDataTable(location);

            ReadExcelDataHelper.ConvertColumnToProperty(dataTable, propertyMapping);

            var response = ReadExcelDataHelper.ConvertDataTable<PaymentModel>(dataTable);
            return response.ToList();
        }

        /// <summary>
        /// Perform the reconcilation details for the class and find the non matching data from Excel details
        /// </summary>
        /// <param name="productModels"></param>
        /// <param name="productModel1"></param>
        /// <returns></returns>
        public List<(string, string)> ReconcileDataInformation(List<PaymentModel> productModels, List<PaymentModel> productModel1)
        {
            var response = productModels.Count() > productModel1.Count() ? ReconcileDetail(productModels, productModel1) : ReconcileDetail(productModel1, productModels);

            return response;
        }

        /// <summary>
        /// Helper method to perform the Reconcilation steps
        /// </summary>
        /// <param name="parentModels"></param>
        /// <param name="childModels"></param>
        /// <returns></returns>
        private List<(string, string)> ReconcileDetail(List<PaymentModel> parentModels, List<PaymentModel> childModels)
        {
            List<(string, string)> response = new List<(string, string)>();

            parentModels.ForEach(parentItem =>
            {
                var filterChildModel = childModels.FirstOrDefault(x => x.OrderNumber == parentItem.OrderNumber);
                if (parentItem.NetAmount != filterChildModel.NetAmount)
                {
                    response.Add(($"Order Number :{parentItem.OrderNumber} has net Amount : {parentItem.NetAmount} and child model {filterChildModel.NetAmount}", $"{parentItem.OrderNumber}"));
                }

                if (parentItem.GrossTransactionAmount != filterChildModel.GrossTransactionAmount)
                {
                    response.Add(($"Order Number :{parentItem.OrderNumber} has net Amount : {parentItem.GrossTransactionAmount} and child model {filterChildModel.GrossTransactionAmount}", $"{parentItem.OrderNumber}"));
                }


            });

            return response;
        }
    }
}
