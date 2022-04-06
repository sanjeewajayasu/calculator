using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class EmployeeController : ApiController
    {
        public HttpResponseMessage Get()
        {
            string query = @"
                        select EmployeeId,EmployeeName,Department,
                        convert(varchar(10),DateOfJoining,120) as DateOfJoining,
                        PhotoFileName
                        from
                        dbo.Employee
                        ";

            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.
            ConnectionStrings["EmployeeAppDB"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))

            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }
            return Request.CreateResponse(HttpStatusCode.OK, table);
        }
        public string Post(Employee emp)
        {
            try
            {
                string query = @"
                insert into dbo.Employee values
                (
                '" + emp.EmployeeName + @"'
                ,'" + emp.Department + @"'
                ,'" + emp.DateOfJoining + @"'
                ,'" + emp.PhotoFileName + @"'
                )
                ";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.
                ConnectionStrings["EmployeeAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))

                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return "Added Successfully!!";


            }
            catch (Exception)
            {
                return "Fail to Add!!";

            }
        }

        public string Put(Employee emp)
        {
            try
            {
                string query = @"
                update dbo.Employee set 
                 EmployeeName = '" + emp.EmployeeName + @"'
                ,Department = '" + emp.Department + @"'
                ,DateOfJoining = '" + emp.DateOfJoining + @"'
                ,PhotoFileName = '" + emp.PhotoFileName + @"'
                where EmployeeId = " + emp.EmployeeId + @"
                ";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.
                ConnectionStrings["EmployeeAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))

                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return "Update Successfully!!";


            }
            catch (Exception)
            {
                return "Fail to Update!!";

            }
        }

        public string Delete(int id)
        {
            try
            {
                string query = @"
               delete from dbo.Employee
                where EmployeeId = " + id + @"
                ";

                DataTable table = new DataTable();
                using (var con = new SqlConnection(ConfigurationManager.
                ConnectionStrings["EmployeeAppDB"].ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))

                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(table);
                }

                return "Delete Successfully!!";


            }
            catch (Exception)
            {
                return "Fail to Delete!!";

            }
        }


        //Custommethod and api
        [Route("api/Employee/GetAllDepartmentName")]
        [HttpGet]
        public HttpResponseMessage GetAllDepartmentName()
        {
            string query = @"
               select DepartmentName from dbo.Department";


            DataTable table = new DataTable();
            using (var con = new SqlConnection(ConfigurationManager.
            ConnectionStrings["EmployeeAppDB"].ConnectionString))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))

            {
                cmd.CommandType = CommandType.Text;
                da.Fill(table);
            }
            return Request.CreateResponse(HttpStatusCode.OK, table);
        }

        //Custom method and api
        [Route("api/Employee/SaveFile")]
        public string SaveFile()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalpath = HttpContext.Current.Server.MapPath("~/Photos/" + filename);

                postedFile.SaveAs(physicalpath);

                return filename;
            }

            catch (Exception)
            {
                return "anonymous.png";
            }
        }

    }
}


