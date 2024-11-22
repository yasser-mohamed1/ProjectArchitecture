using Project.ResponseHandler.Models;
using Project.Services.DataTransferObject.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<APIOperationResponse<CustomerSearchModel>> UpdateCustomerAsync(CustomerCreateModel model);
        Task<APIOperationResponse<CustomerSearchModel>> CreateCustomerAsync(CustomerCreateModel model);

        Task<APIOperationResponse<List<CustomerGetAllModel>>> GetAllCustomerAsync(string name, string phone, string email);
            Task<APIOperationResponse<bool>> DeleteCustomerAsync(int id);
    }
}
