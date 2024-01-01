using Infrastructure.Storage;
using PartsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PartsApp.SupportClasses
{
    internal class EmployeeOperationsCache
    {
        private readonly Dictionary<int, List<IOperation>> _cache = new Dictionary<int, List<IOperation>>();

        public List<IOperation> GetOperations(Employee employee, DateTime? startDate, DateTime? endDate, bool addOperations = false)
        {
            if (_cache.TryGetValue(employee.EmployeeId, out List<IOperation> operations) && operations.Any())
            {
                // если есть запрос на новую выгрузку, ищем операции за более ранний или поздний период?
                // иначе не делаем никаких доп. действий
                if (addOperations)                
                {
                    DateTime firstCachedOperationDate = operations[0].OperationDate;
                    DateTime lastCachedOperationDate = operations[operations.Count - 1].OperationDate;
                    var newOperations = new List<IOperation>();

                    if (firstCachedOperationDate > startDate)
                    {
                        newOperations.AddRange(GetOperationsFromDB(employee, startDate, firstCachedOperationDate.AddDays(-1)));                   
                    }

                    if (lastCachedOperationDate < endDate)
                    {
                        newOperations.AddRange(GetOperationsFromDB(employee, lastCachedOperationDate.AddDays(1), endDate));                        
                    }

                    if (newOperations.Any())
                    {
                        AddToCacheAndSort(employee.EmployeeId, newOperations);
                    }
                }                
            }
            else
            {
                var newOperations = GetOperationsFromDB(employee, startDate, endDate);
                AddToCacheAndSort(employee.EmployeeId, newOperations);
            }
            
            return _cache[employee.EmployeeId].FindAll(o => o.OperationDate >= startDate && o.OperationDate <= endDate);
        }

        private List<IOperation> GetOperationsFromDB(Employee employee, DateTime? startDate, DateTime? endDate)
        {
            List<IOperation> operations = new List<IOperation>();

            PurchaseRepository.FindPurchases(employee, startDate, endDate).ForEach(p => operations.Add(p));
            SaleRepository.FindSales(employee, startDate, endDate).ForEach(s => operations.Add(s));

            return operations;
        }        

        private void AddToCacheAndSort(int employeeId, List<IOperation> newOperations)
        {
            EnsureEmployeeInCache(employeeId);
            if (newOperations.Any()) 
            {
                foreach (var o in newOperations)
                {
                    if (!_cache[employeeId].Exists(op => op.OperationId == o.OperationId))
                    {
                        _cache[employeeId].Add(o);
                    }
                }                
                _ = _cache[employeeId].OrderBy(o => o.OperationDate);
            }
        }

        private void EnsureEmployeeInCache(int id)
        {
            if (!_cache.ContainsKey(id))
            {
                _cache.Add(id, new List<IOperation>());
            }
        }

        public int GetEmployeeOperationsCount(int id)
        {
            return _cache.ContainsKey(id) ? _cache[id].Count : 0;
        }
    }
}
