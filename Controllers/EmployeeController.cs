using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrudDapper.Models;
using CrudDapper.Repository;
using System.IO;
using Rotativa;
using OfficeOpenXml;
using PagedList;
using System.Data;
using ClosedXML.Excel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace CrudDapper.Controllers
{
    public class EmployeeController : Controller
    {
        public int Page { get; private set; }

        // GET: Employee/GetAllEmpDetails
        //public ActionResult GetAllEmpDetails()
        //{
        //    EmpRepository EmpRepo = new EmpRepository();
        //    return View(EmpRepo.GetAllEmployees());


        //}
        public ActionResult GetAllEmpDetails(string Sorting_Order, string searchString, int Page = 1)
        {
            EmpRepository EmpRepo = new EmpRepository();
            var employees = EmpRepo.GetAllEmployees();
            ViewBag.TotalPages = Math.Ceiling(employees.Count() / 10.0);
            employees = employees.Skip((Page - 1) * 10).Take(10).ToList();


            ViewBag.SortingName = String.IsNullOrEmpty(Sorting_Order) ? "Name" : "";
            ViewBag.SortingEmail = String.IsNullOrEmpty(Sorting_Order) ? "Email" : "";
            ViewBag.SortingGender = String.IsNullOrEmpty(Sorting_Order) ? "Gender" : "";
            ViewBag.SortingPhone = String.IsNullOrEmpty(Sorting_Order) ? "Phone" : "";
            ViewBag.SortingLanguage = String.IsNullOrEmpty(Sorting_Order) ? "Language" : "";



            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.Name.Contains(searchString) || e.Email.Contains(searchString)).ToList();
            }

            switch (Sorting_Order)
            {
                case "Name":
                    employees = employees.OrderByDescending(e => e.Name).ToList();
                    break;
                case "Email":
                    employees = employees.OrderByDescending(e => e.Email).ToList();
                    break;

                case "Phone":
                    employees = employees.OrderByDescending(e => e.Phone).ToList();
                    break;
                case "Gender":
                    employees = employees.OrderByDescending(e => e.Gender).ToList();
                    break;
                case "Language":
                    employees = employees.OrderBy(e => e.Language).ToList();
                    break;
                default:
                    employees = employees.OrderBy(e => e.Name).ToList();
                    break;

            }
            return View(employees);
        }


        // GET: Employee/AddEmployee

        public ActionResult AddEmployee()
        {
            return View();
        }
        // POST: Employee/AddEmployee
        [HttpPost]
        public ActionResult AddEmployee(EmpModel Emp)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    EmpRepository EmpRepo = new EmpRepository();
                    EmpRepo.AddEmployee(Emp);
                    //ViewBag.Message = "Records added successfully.";
                    //ViewBag.Name = Emp.Name; 
                    return RedirectToAction("GetAllEmpDetails");


                }
                else
                {

                    return View();
                }


            }

            catch
            {
                return View();
            }

        }
        [HttpGet]
        // GET: Bind controls to Update details
        public ActionResult EditEmpDetails(int Id)
        {
            EmpRepository EmpRepo = new EmpRepository();
            return View(EmpRepo.GetAllEmployees().Find(Emp => Emp.EmpId == Id));
        }
        // POST:Update the details into database
        [HttpPost]
        public ActionResult EditEmpDetails(int Id, EmpModel obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EmpRepository EmpRepo = new EmpRepository();
                    EmpRepo.UpdateEmployee(obj);
                    return RedirectToAction("GetAllEmpDetails");
                }
                else
                {
                    return View();
                }

            }
            catch
            {
                return View();
            }
        }
        // GET: Delete  Employee details by id
        public ActionResult DeleteEmp(int Id)
        {
            try
            {
                EmpRepository EmpRepo = new EmpRepository();
               if( EmpRepo.DeleteEmployee(Id))
                {
                   ViewBag.AlertMsg = "Employee details deleted successfully";

                }
                return RedirectToAction("GetAllEmpDetails");

            }
            catch
            {
                return RedirectToAction("GetAllEmpDetails");
            }
        }


        // GET: Employee/RestoreEmp




        public ActionResult RestoredEmp()
        {
            try
            {
                EmpRepository EmpRepo = new EmpRepository();
                // Get the employee details
                var employee = EmpRepo.GetDeletedEmployees();

                if (employee == null)
                {
                    ViewBag.ErrorMsg = "No deleted record found";
                    return RedirectToAction("GetAllEmpDetails");
                }


                return View(employee);
            }
            catch
            {
                return RedirectToAction("GetAllEmpDetails");
            }
        }

        public ActionResult Restore(int Id)
        {
            try
            {
                EmpRepository objRepo = new EmpRepository();
                objRepo.RestoreDeletedEmployee(Id);
                return RedirectToAction("GetAllEmpDetails");
            }
            catch
            {
                return RedirectToAction("GetAllEmpDetails");
            }
        }

        //public ActionResult PrintAllReport()
        //{
        //    var report = new ActionAsPdf("GetAllEmpDetails");
        //    return report;

        //}
        public ActionResult PrintPDF()
        {
            EmpRepository EmpRepo = new EmpRepository();
            var employees = EmpRepo.GetAllEmployees();

            return new PartialViewAsPdf("EmpData", employees)
            {
                FileName = "TestPartialViewAsPdf.pdf"
            };
        }



        public void ExportListUsingEPPlus()
        {

            EmpRepository EmpRepo = new EmpRepository();
            var data = EmpRepo.GetAllEmployees();
            ExcelPackage excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Sheet1");
            workSheet.Cells[1, 1].LoadFromCollection(data, true);
            using (var memoryStream = new System.IO.MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=Employees_List.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }




        }





    }
}
