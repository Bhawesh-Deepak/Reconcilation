using Microsoft.Extensions.Configuration;
using Reconcilation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reconcilation.Services
{
    public interface IReconciliationService
    {
        List<PaymentModel> ReadDataFromLocation(string location, Dictionary<string, string> propertyMapping);

        List<(string, string)> ReconcileDataInformation(List<PaymentModel> productModels, List<PaymentModel> productModel1, IEnumerable<string> mappingData);

    }
}
