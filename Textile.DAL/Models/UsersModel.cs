using System.Collections.Generic;
namespace Textile.DAL.Models
{
    public class UsersModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public bool IsActive { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        // public List<RolesModel> Roles { get; set; }
        public List<RolesModel> AvailableRoles { get; set; }
        //this list has our default values 
        public List<RolesModel> SelectedRoles { get; set; }
        //this will retrieve the ids of movies selected in list when submitted
        public List<string> SubmittedRoles { get; set; }

    }
}
