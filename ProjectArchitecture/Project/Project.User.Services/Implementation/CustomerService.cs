using AutoMapper;
using Azure;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Moujam.Casiher.Comman.Models;
using Mujam.Intergration.Service.Mangment;
using Project.Data.Entities;
using Project.Data.IGenericRepository_IUOW;
using Project.Repository.Repository;
using Project.ResponseHandler.Consts;
using Project.ResponseHandler.Models;
using Project.Services.DataTransferObject.Customer;
using Project.Services.Interfaces;
using System.Collections.Generic;
using CustomerSearchModel = Moujam.Casiher.Comman.Models.CustomerSearchModel;
using CustomerSearchModelNew = Project.Services.DataTransferObject.Customer.CustomerSearchModel;

namespace Project.Services.Implementation
{
    public class CustomerService : ICustomerService
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IServieMangamentApI _serviceManagementApi;
        private readonly FileSettings _fileSettings;
        #endregion

        #region Constructor
        public CustomerService(UnitOfWork unitOfWork, IMapper mapper,
            IServieMangamentApI serviceManagementApi,
            IOptions<FileSettings> fileSettings)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper;
            _serviceManagementApi = serviceManagementApi ?? throw new ArgumentNullException(nameof(serviceManagementApi));
            _fileSettings = fileSettings?.Value ?? throw new ArgumentNullException(nameof(fileSettings));
        }
        #endregion

        private async Task<APIOperationResponse<List<CustomerSearchModel>>> GetCustomerFromMo3amAsync(string name ,string phone,string email)
        {
            var response = await _serviceManagementApi.GET(name,phone,email, _fileSettings.ServiceMangmentAPIKey);
            if(response.IsSuccessStatusCode)
            return APIOperationResponse<List<CustomerSearchModel>>.Success(response.Content.Data);
            else
                return APIOperationResponse<List<CustomerSearchModel>>.Fail(ResponseType.InternalServerError,CommonErrorCodes.INVALID_INPUT, response.Error.ToString());
        }
        public async Task<APIOperationResponse<List<CustomerGetAllModel>>> GetAllCustomerAsync(string name, string phone, string email)
        {
            var customersEntity = await _unitOfWork.Customers.GetAllAsync();
            if (!string.IsNullOrEmpty(name))
            customersEntity = customersEntity.Where(x => x.Name.Contains(name));
            if (!string.IsNullOrEmpty(phone))
                customersEntity = customersEntity.Where(x => x.Name.Contains(phone));
            if (!string.IsNullOrEmpty(email))
                customersEntity = customersEntity.Where(x => x.Name.Contains(email));
            var customersModel = _mapper.Map<List<CustomerGetAllModel>>(customersEntity);
            var customer = await GetCustomerFromMo3amAsync(name, phone, email);
            if (customer.Data!=null)
            {
                string jsonString = customer.Data.ToString();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

              var  customerFromMo3jam = customer.Data.Select(x => new CustomerGetAllModel
                {

                    Name = x.Name,
                    Phone = x.Phone,
                    Address = x.Address,
                    ID = x.ID,
                    FormMo3gam = true,
                }).ToList();
                if (customersModel.Count > 0)
                {
                    customersModel.Concat(customerFromMo3jam);
                }
                else
                {
                    return APIOperationResponse<List<CustomerGetAllModel>>.Success(customerFromMo3jam);
                }
            }
            return APIOperationResponse<List<CustomerGetAllModel>>.Success(customersModel);
        }

        public async Task<APIOperationResponse<CustomerSearchModelNew>> CreateCustomerAsync(CustomerCreateModel model)
        {
            var customerEntity = _mapper.Map<Customer>(model);
            await _unitOfWork.Customers.AddAsync(customerEntity);
            await _unitOfWork.SaveAsync();
            var customerDto = _mapper.Map<CustomerSearchModelNew>(customerEntity);
            return APIOperationResponse<CustomerSearchModelNew>.Success(customerDto);
        }

        public async Task<APIOperationResponse<CustomerSearchModelNew>> UpdateCustomerAsync( CustomerCreateModel model)
        {
            var customerEntity = await _unitOfWork.Customers.GetByIdAsync(model.Id);
            if (customerEntity == null)
            {
                return APIOperationResponse<CustomerSearchModelNew>.NotFound("Customer not found");
            }

            _mapper.Map(model, customerEntity);
            await _unitOfWork.Customers.UpdateAsync(customerEntity);
            await _unitOfWork.SaveAsync();
            var updatedDto = _mapper.Map<CustomerSearchModelNew>(customerEntity);
            return APIOperationResponse<CustomerSearchModelNew>.Success(updatedDto);
        }

        public async Task<APIOperationResponse<bool>> DeleteCustomerAsync(int id)
        {
            var customerEntity = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customerEntity == null)
            {
                return APIOperationResponse<bool>.NotFound("Customer not found");
            }

            await _unitOfWork.Customers.DeleteAsync(customerEntity);
            await _unitOfWork.SaveAsync();
            return APIOperationResponse<bool>.Success(true);
        }
    }

}
