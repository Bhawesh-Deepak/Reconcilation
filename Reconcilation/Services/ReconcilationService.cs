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
        public List<ProductModel> ReadDataFromLocation(string location, Dictionary<string, string> propertyMapping)
        {
            var dataTable = ReadExcelDataHelper.ConvertExcelToDataTable(location);

            ReadExcelDataHelper.ConvertColumnToProperty(dataTable, propertyMapping);

            var response = ReadExcelDataHelper.ConvertDataTable<ProductModel>(dataTable);
            return response.ToList();
        }

        /// <summary>
        /// Perform the reconcilation details for the class and find the non matching data from Excel details
        /// </summary>
        /// <param name="productModels"></param>
        /// <param name="productModel1"></param>
        /// <returns></returns>
        public  List<(string, string)> ReconcileDataInformation(List<ProductModel> productModels, List<ProductModel> productModel1)
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
        private List<(string, string)> ReconcileDetail(List<ProductModel> parentModels, List<ProductModel> childModels)
        {
            List<(string, string)> response = new List<(string, string)>();
            for (int i = 0; i < parentModels.Count(); i++)
            {
                if (childModels.Count > i)
                {
                    if (parentModels[i].Id != childModels[i].Id)
                    {
                        response.Add((i.ToString(), "Product Id not matched"));
                    }

                    if (parentModels[i].ProductName != childModels[i].ProductName)
                    {
                        response.Add((i.ToString(), "Product Name not matched"));
                    }

                    if (parentModels[i].ProductCode != childModels[i].ProductCode)
                    {
                        response.Add((i.ToString(), "Product Code not matched"));
                    }

                    if (parentModels[i].Price != childModels[i].Price)
                    {
                        response.Add((i.ToString(), "Product Price not matched"));
                    }
                }
               
            }
            return response;
        }
    }
}
