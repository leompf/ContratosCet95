using ContratosCet95.Web.Data.Entities;
using ContratosCet95.Web.Models;

namespace ContratosCet95.Web.Helpers;

public class ConverterHelper : IConverterHelper
{
    public Contrato ToContrato(CreateContractViewModel model)
    {
        return new Contrato
        {
            Name = model.Name,
            JogadorId = model.JogadorId,
            EquipaId = model.EquipaId,
            TipoContratoId = model.TipoContratoId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Conditions = model.Conditions
        };
    }

    public UserViewModel ToUserViewModel(User user, string role)
    {
        return new UserViewModel
        {
            Id = user.Id,
            Name = $"{user.FirstName} {user.LastName}",
            Birthdate = user.Birthdate,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = role
        };
    }
}
