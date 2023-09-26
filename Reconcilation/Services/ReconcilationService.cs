using Microsoft.Extensions.Configuration;
using Reconcilation.Helpers;
using Reconcilation.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
        public List<(string, string)> ReconcileDataInformation(List<PaymentModel> productModels, List<PaymentModel> productModel1, IEnumerable<string> mappingData)
        {
            var response = productModels.Count() > productModel1.Count() ? ReconcileDetail(productModels, productModel1, mappingData) : ReconcileDetail(productModel1, productModels, mappingData);

            return response;
        }

        /// <summary>
        /// Helper method to perform the Reconcilation steps
        /// </summary>
        /// <param name="parentModels"></param>
        /// <param name="childModels"></param>
        /// <returns></returns>
        private List<(string, string)> ReconcileDetail(List<PaymentModel> parentModels, List<PaymentModel> childModels, IEnumerable<string> mappingData)
        {
            List<(string, string)> response = new List<(string, string)>();

            parentModels.ForEach(parentItem =>
            {
                var filterChildModel = childModels.FirstOrDefault(x => x.OrderNumber == parentItem.OrderNumber);

                CompareAndAddMismatchedResponse<PaymentModel>(parentItem, filterChildModel, response,
                    mappingData.ToArray());
            });

            return response;
        }



        public static void CompareAndAddMismatchedResponse<T>(T parentItem, T filterChildModel, List<(string, string)> response, params string[] properties)
        {
            foreach (var propertyName in properties)
            {
                var parentPropertyValue = typeof(T).GetProperty(propertyName)?.GetValue(parentItem, null);
                var childPropertyValue = typeof(T).GetProperty(propertyName)?.GetValue(filterChildModel, null);

                if (!Equals(parentPropertyValue, childPropertyValue))
                {
                    response.Add(($"Order Number: {parentItem.GetType().GetProperty("OrderNumber")?.GetValue(parentItem, null)} has {propertyName}: {parentPropertyValue} and child model {childPropertyValue}", $"{parentItem.GetType().GetProperty("OrderNumber")?.GetValue(parentItem, null)}"));
                }
            }
        }
    }
}
