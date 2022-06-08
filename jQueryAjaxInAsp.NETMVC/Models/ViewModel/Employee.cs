using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace jQueryAjaxInAsp.NETMVC.Models.ViewModel
{
    public partial class EmployeeModel
    {
        public int EmployeeID { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Position { get; set; }
        public string Office { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public Nullable<int> Age { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public Nullable<int> Salary { get; set; }
        [DisplayName("Image")]
        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("DOJ")]
        public DateTime DateOfJoining { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Address 1")]
        public string Address1 { get; set; }
        [DisplayName("Address 2")]
        public string Address2 { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string State { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string City { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Pincode { get; set; }
        
        public string Password { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [DisplayName("Phone")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone must be numeric")]
        [StringLength(10,ErrorMessage ="Number cannot be more than 10 digits")]
        public string Contact { get; set; }

        public EmployeeModel()
        {
            ImagePath = "~/AppFiles/Images/default.png";
        }
    }
}