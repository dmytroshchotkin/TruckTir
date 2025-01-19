using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    public class ReturnPriceHandler
    {
        public OperationDetails OperationDetails { get; }

        public ReturnPriceHandler(OperationDetails od)
        {
            OperationDetails = od;
        }

        public void SetMeanPrice(List<OperationDetails> operations)
        {
            float meanPrice = (float)Math.Round(operations.Average(od => od.Price));
            OperationDetails.Price = meanPrice;
        }
    }
}
