using AvaloniaTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaTest
{
    public static class CalculationService
    {
        public static int CalculateProductQuantity(int productTypeId, int materialTypeId,
            int materialQuantity, double param1, double param2)
        {
            try
            {
                using var db = new AppDbContext();

                var productType = db.ProductTypes.FirstOrDefault(pt => pt.Id == productTypeId);
                var materialType = db.MaterialTypes.FirstOrDefault(mt => mt.Id == materialTypeId);

                if (productType == null || materialType == null)
                    return -1;

                if (param1 <= 0 || param2 <= 0 || materialQuantity <= 0)
                    return -1;

                double materialAfterLoss = materialQuantity * (1 - (double)materialType.LossPercentage / 100);
                double productPerUnit = param1 * param2 * (double)productType.Coefficient;

                if (productPerUnit == 0) return -1;

                int productQuantity = (int)(materialAfterLoss / productPerUnit);

                return productQuantity > 0 ? productQuantity : -1;
            }
            catch
            {
                return -1;
            }
        }
    }
}