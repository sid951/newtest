//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace jQueryAjaxInAsp.NETMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Emp_Attendance
    {
        public int AttendanceId { get; set; }
        public int Employeeid { get; set; }
        public System.DateTime AttendanceInTime { get; set; }
        public System.DateTime AttendanceOutTime { get; set; }
    
        public virtual Employee Employee { get; set; }
    }
}
