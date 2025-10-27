using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContratosCet95.Web.Models
{
    public class UserListViewModel
    {
        public string? NameFilter { get; set; }

        public string? EmailFilter { get; set; }

        public IEnumerable<UserViewModel> Users { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
