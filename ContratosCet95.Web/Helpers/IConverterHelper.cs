using ContratosCet95.Web.Data.Entities;
using ContratosCet95.Web.Models;

namespace ContratosCet95.Web.Helpers;

public interface IConverterHelper
{
    UserViewModel ToUserViewModel(User user, string role);

    Contrato ToContrato(CreateContractViewModel model);
}
