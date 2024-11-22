using Moujam.Casiher.Comman.Models;
using Refit;
using Project.ResponseHandler.Models;

namespace Mujam.Intergration.Service.Mangment
{
   
    public interface IServieMangamentApI
    {
        [Get("/api/integration/customer")]
        Task<ApiResponse<APIOperationResponse<List<CustomerSearchModel>>>> GET(string name,string phone, string email, [Header("X-Api-Key")] string authorization);

    }
}
