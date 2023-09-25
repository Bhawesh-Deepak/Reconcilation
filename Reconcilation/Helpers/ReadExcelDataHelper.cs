using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reconcilation.Helpers
{
    public static class ReadExcelDataHelper
    {
        /// <summary>
        /// Code to convert the Excel Column Name to Class Property Name so that the generic conversion code 
        /// Works as expected when it will find the column based on property name.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dictionary"></param>
        public static void ConvertColumnToProperty(DataTable dt, Dictionary<string, string> dictionary)
        {
            foreach (DataColumn column in dt.Columns)
            {
                string columnName = dt.Rows[0][column.ColumnName].ToString().Trim();

                if (!dt.Columns.Contains(columnName) && !string.IsNullOrEmpty(columnName))
                {
                    if (dictionary.ContainsKey(columnName))
                    {
                        column.ColumnName = dictionary[columnName];
                    }
                }
            }
            dt.Rows[0].Delete();
            dt.AcceptChanges();
        }

        /// <summary>
        /// Code to convert the Excel to data table dynamically.
        /// </summary>
        /// <returns></returns>
        public static DataTable ConvertExcelToDataTable(string filePath)
        {
            // string filePath = @"E:\ResearchAndDevelopment\LeadAllocation.xlsx";
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            var reader = ExcelReaderFactory.CreateReader(stream);
            var result = reader.AsDataSet();
            return result.Tables[0];
        }

        /// <summary>
        /// Generic code to convert the Data Table to Generic List 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        /// <summary>
        /// Helper method to work inside the ConvertDataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        if (pro.PropertyType.Name == "Int32")
                        {
                            pro.SetValue(obj, dr[column.ColumnName].ToString(), null);
                        }
                        pro.SetValue(obj, dr[column.ColumnName].ToString(), null);
                    }

                    else
                        continue;
                }
            }
            return obj;
        }
    }
}