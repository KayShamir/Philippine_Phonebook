using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace Philippine_Phonebook.Controllers
{
    public class HomeController : Controller
    {
        string conn_str = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kaysh\source\repos\Philippine_Phonebook\Philippine_Phonebook\App_Data\Phonebook.mdf;Integrated Security=True";
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Dashboard()
        {
            return View();

        }
        public ActionResult PostCreate()
        {
            var data = new List<object>();
            string name = Request["name"].ToUpper();
            string area_code = Request["area_code"];
            string phone_num = Request["phone_num"];
            string mobile_num = Request["mobile_num"];
            string email_add = Request["email_add"];
            string house_num = Request["house_num"];
            string street = Request["street"];
            string city = Request["city"];
            string province = Request["province"];
            var zip_code = Convert.ToInt32(Request["zip_code"]);

            using (var db = new SqlConnection(conn_str))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO PHONEBOOK (NAME, AREA_CODE, PHONE_NUM, MOBILE_NUM, HOUSE_NUM, STREET, CITY, PROVINCE, ZIP_CODE, EMAIL_ADD) " +
                        "VALUES (@name, @area_code, @phone_num, @mobile_num, @house_num, @street, @city, @province, @zip_code, @email_add)";
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@area_code", area_code);
                    cmd.Parameters.AddWithValue("@phone_num", phone_num);
                    cmd.Parameters.AddWithValue("@mobile_num", mobile_num);
                    cmd.Parameters.AddWithValue("@house_num", house_num);
                    cmd.Parameters.AddWithValue("@street", street);
                    cmd.Parameters.AddWithValue("@city", city);
                    cmd.Parameters.AddWithValue("@province", province);
                    cmd.Parameters.AddWithValue("@zip_code", zip_code);
                    cmd.Parameters.AddWithValue("@email_add", email_add);

                    cmd.ExecuteNonQuery();
                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult SearchPhonebook(string phoneNumber)
        {
            StringBuilder result = new StringBuilder();
            using (var db = new SqlConnection(conn_str))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM PHONEBOOK WHERE REPLACE(PHONE_NUM, ' ', '') = REPLACE(@PhoneNumber, ' ', '')";
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            result.Append("<table class='table table-striped'><thead class='bg-secondary text-white'><tr><th>Name</th><th>Phone Number</th><th>Mobile Number</th><th>Email Address</th><th>House Number</th><th>Street</th><th>City</th><th>Province</th><th>Zip Code</th><th>Area Code</th></tr></thead><tbody>");
                            while (reader.Read())
                            {
                                result.Append("<tr>");
                                result.Append($"<td>{reader["NAME"]}</td>");
                                result.Append($"<td>{reader["PHONE_NUM"]}</td>");
                                result.Append($"<td>{reader["MOBILE_NUM"]}</td>");
                                result.Append($"<td>{reader["EMAIL_ADD"]}</td>");
                                result.Append($"<td>{reader["HOUSE_NUM"]}</td>");
                                result.Append($"<td>{reader["STREET"]}</td>");
                                result.Append($"<td>{reader["CITY"]}</td>");
                                result.Append($"<td>{reader["PROVINCE"]}</td>");
                                result.Append($"<td>{reader["ZIP_CODE"]}</td>");
                                result.Append($"<td>{reader["AREA_CODE"]}</td>");
                                result.Append("</tr>");
                            }
                            result.Append("</tbody></table>");
                        }
                        else
                        {
                            result.Append("<p>No records found.</p>");
                        }
                    }
                }
            }
            return Content(result.ToString());
        }

    }
}