using Reconcilation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reconcilation.Services
{
    public interface IReconcilationService
    {
        Task<List<ProductModel>> ReadDataFromLocation(string location, Dictionary<string, string> propertyMapping);

        Task<List<(string, string)>> ReconcileDataInformation(List<ProductModel> productModels, List<ProductModel> productModel1);

    }
}
