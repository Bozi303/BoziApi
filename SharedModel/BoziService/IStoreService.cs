using SharedModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModel.BoziService
{
    public interface IStoreService
    {
        List<TitleId> GetStoreCategories();
        string GetStoreRegistrationNumber();
        void RegistrationStore(CreateStore request);
        bool CheckCustomerIsStoreOwner(string customerId, string storeId);
        List<TitleId> GetStoreStatuses();
        void ChangeStoreStatus(ChangeStoreStatus changeStoreStatus, string adminId);
    }
}
