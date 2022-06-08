using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jQueryAjaxInAsp.NETMVC.Models.ViewModel
{
    public class AttendanceModel
    {
        public int AttendanceId { get; set; }
        public int Employeeid { get; set; }
        [DisplayName("In Time")]
        [Required(ErrorMessage ="In time is required")]
        public DateTime AttendanceInTime { get; set; }
        [DisplayName("Out Time")]
        [Required(ErrorMessage = "Out time is required")]
        public DateTime AttendanceOutTime { get; set; }

        [DisplayName("Total Time in minutes")]
        public string TotalTime { get; set; }
        public string EmployeeName { get; set; }
    }
}