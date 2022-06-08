using jQueryAjaxInAsp.NETMVC.Models.ViewModel;
using jQueryAjaxInAsp.NETMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using ClosedXML.Excel;

namespace jQueryAjaxInAsp.NETMVC.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAllEmployee());
        }

        IEnumerable<Employee> GetAllEmployee()
        {
            using (Entities db = new Entities())
            {

                return db.Employees.ToList<Employee>();
            }

        }

        public ActionResult AddOrEdit(int id = 0)
        {
            Employee emp = new Employee();
            EmployeeModel obj = new EmployeeModel();
            if (id != 0)
            {
                using (Entities db = new Entities())
                {
                    emp = db.Employees.Where(x => x.EmployeeID == id).FirstOrDefault<Employee>();
                    obj.EmployeeID = emp.EmployeeID;
                    obj.Name = emp.Name;
                    obj.Position = emp.Position;
                    obj.Office = emp.Office;
                    obj.Age = emp.Age;
                    obj.Salary = emp.Salary;
                    obj.ImagePath = emp.ImagePath;
                    obj.DateOfJoining =emp.DOJ==null?DateTime.Now:Convert.ToDateTime(emp.DOJ);
                    obj.Address2 = emp.Address2;
                    obj.Address1 = emp.Address1;
                    obj.City = emp.City;
                    obj.State = emp.State;
                    obj.Pincode = emp.Pincode;
                    obj.Contact =Convert.ToString(emp.ContactNumber);
                }
            }
            else
            {
                obj.DateOfJoining = DateTime.Now;
            }
            return View(obj);
        }

        [HttpPost]
        public ActionResult AddOrEdit(EmployeeModel emp)
        {
            try
            {
                if (emp.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(emp.ImageUpload.FileName);
                    string extension = Path.GetExtension(emp.ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    emp.ImagePath = "~/AppFiles/Images/" + fileName;
                    emp.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFiles/Images/"), fileName));
                }
                using (Entities db = new Entities())
                {
                    if (emp.EmployeeID == 0)
                    {
                        Employee obj = new Employee();
                        obj.EmployeeID = emp.EmployeeID;
                        obj.Name = emp.Name;
                        obj.Position = emp.Position;
                        obj.Office = emp.Office;
                        obj.Age = emp.Age;
                        obj.Salary = emp.Salary;
                        obj.ImagePath = emp.ImagePath;
                        obj.DOJ = emp.DateOfJoining;
                        obj.Address2 = emp.Address2;
                        obj.Address1 = emp.Address1;
                        obj.City = emp.City;
                        obj.State = emp.State;
                        obj.Pincode = emp.Pincode;
                        obj.ContactNumber =Convert.ToInt32(emp.Contact);
                        db.Employees.Add(obj);
                        db.SaveChanges();
                        User usr = new User();
                        usr.RoleId = db.Roles.FirstOrDefault(x => x.RoleName == "employee").Id;
                        usr.UserName = emp.Name;
                        usr.Password = emp.Password;
                        int lastinsertedemployeeid = obj.EmployeeID;
                        usr.EmployeeId = lastinsertedemployeeid;
                        db.Users.Add(usr);
                        db.SaveChanges();
                    }
                    else
                    {

                        Employee obj = new Employee();

                        var getEmployeeDetails = db.Employees.FirstOrDefault(x => x.EmployeeID == emp.EmployeeID);
                        getEmployeeDetails.EmployeeID = getEmployeeDetails.EmployeeID;
                        getEmployeeDetails.Name = getEmployeeDetails.Name;
                        getEmployeeDetails.Position = emp.Position;
                        getEmployeeDetails.Office = emp.Office;
                        getEmployeeDetails.Age = emp.Age;
                        getEmployeeDetails.Salary = emp.Salary;
                        getEmployeeDetails.ImagePath = emp.ImagePath;
                        getEmployeeDetails.DOJ = emp.DateOfJoining;
                        getEmployeeDetails.Address2 = emp.Address2;
                        getEmployeeDetails.Address1 = emp.Address1;
                        getEmployeeDetails.City = emp.City;
                        getEmployeeDetails.State = emp.State;
                        getEmployeeDetails.Pincode = emp.Pincode;
                        getEmployeeDetails.ContactNumber =Convert.ToInt32(emp.Contact);

                        // obj.Users = db.Employees.FirstOrDefault(x => x.EmployeeID == emp.EmployeeID).Users;
                        db.Entry(getEmployeeDetails).State = EntityState.Modified;
                        //User usr = new User();
                        //usr.RoleId = db.Roles.FirstOrDefault(x => x.RoleName == "employee").Id;
                        //usr.UserName = emp.Name;
                        //usr.Password = db.Users.FirstOrDefault(x => x.UserID == emp.EmployeeID).Password;
                        //usr.UserID = db.Users.FirstOrDefault(x => x.EmployeeId == emp.EmployeeID).UserID;
                        //db.Entry(usr).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployee()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                using (Entities db = new Entities())
                {
                    User user = db.Users.Where(x => x.EmployeeId == id).FirstOrDefault<User>();
                    Employee emp = db.Employees.Where(x => x.EmployeeID == id).FirstOrDefault<Employee>();
                    db.Users.Remove(user);
                    db.Employees.Remove(emp);
                    db.SaveChanges();
                }
                return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployee()), message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult Dashboard(int id)
        {
            return View();
        }

        public ActionResult EmployeeAttendance(int id)
        {
            using (Entities db = new Entities())
            {
                var data = db.Emp_Attendance.Where(x => x.Employeeid == id).OrderByDescending(x => x.AttendanceInTime).OrderByDescending(x => x.AttendanceOutTime).ToList();
                string employeename = db.Employees.FirstOrDefault(x => x.EmployeeID == id).Name;
                List<AttendanceModel> listobj = new List<AttendanceModel>();
                foreach (var item in data)
                {
                   
                    AttendanceModel obj = new AttendanceModel();
                    obj.EmployeeName = employeename;
                    obj.AttendanceInTime = item.AttendanceInTime;
                    obj.AttendanceOutTime = item.AttendanceOutTime;
                    obj.Employeeid = item.Employeeid;
                    obj.TotalTime =Convert.ToString(item.AttendanceOutTime.Subtract(item.AttendanceInTime).TotalMinutes);
                    listobj.Add(obj);
                }
                ViewBag.currentEmployeeid = id;
                ViewBag.currentEmployeeName = employeename;
                return View(listobj);
            }

        }

        public ActionResult CreateAttendance(int id = 0)
        {
            using (Entities db = new Entities())
            {
                AttendanceModel obj = new AttendanceModel();
                obj.Employeeid = id;
                obj.AttendanceInTime = DateTime.Now.AddHours(-.5);
                obj.AttendanceOutTime = DateTime.Now;
                obj.EmployeeName = db.Employees.FirstOrDefault(x => x.EmployeeID == id).Name;
                return View(obj);
            }

        }

        [HttpPost]
        public ActionResult CreateAttendance(AttendanceModel obj)
        {
            try
            {
                using (Entities db = new Entities())
                {
                    Emp_Attendance att = new Emp_Attendance();
                    att.AttendanceInTime = obj.AttendanceInTime;
                    att.AttendanceOutTime = obj.AttendanceOutTime;
                    if (obj.AttendanceOutTime <= obj.AttendanceInTime)
                    {
                        return Json(new { success = false, message = "Out time should be greater than In time" }, JsonRequestBehavior.AllowGet);
                    }
                    if (obj.AttendanceInTime.Date != obj.AttendanceOutTime.Date)
                    {
                        return Json(new { success = false, message = "In and out time should have same date" }, JsonRequestBehavior.AllowGet);
                    }
                    att.Employeeid = obj.Employeeid;
                    db.Emp_Attendance.Add(att);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployee()), message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public FileResult Export()
        {

            Employee entities = new Employee();
            DataTable dt = new DataTable(string.Format("Employeedetails"));
            dt.Columns.AddRange(new DataColumn[6] { new DataColumn("Employeeid"),
                                            new DataColumn("Name"),
                                            new DataColumn("Position"),
                                            new DataColumn("Office"),
                                            new DataColumn("Age"),
                                            new DataColumn("Salary")});
            using (Entities db = new Entities())
            {
                var Emolyees = db.Employees.ToList();
                foreach (var Emolyee in Emolyees)
                {
                    dt.Rows.Add(Emolyee.EmployeeID, Emolyee.Name, Emolyee.Position, Emolyee.Office, Emolyee.Age, Emolyee.Salary);
                }
            }



            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.Format("Employeedetails_{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmmss")));
                }
            }

        }
    }
}
