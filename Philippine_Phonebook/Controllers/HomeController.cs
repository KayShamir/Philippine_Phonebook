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
            string status = Request["status"];
            var zip_code = Convert.ToInt32(Request["zip_code"]);

            bool isUnique = true;

            using (var db = new SqlConnection(conn_str))
            {
                db.Open();
                using (var cmdCheck = db.CreateCommand())
                {
                    cmdCheck.CommandType = CommandType.Text;
                    cmdCheck.CommandText = "SELECT COUNT(*) FROM PHONEBOOK WHERE MOBILE_NUM = @PhoneNumber";
                    cmdCheck.Parameters.AddWithValue("@PhoneNumber", mobile_num);
                    int count = (int)cmdCheck.ExecuteScalar();
                    if (count > 0)
                    {
                        isUnique = false;
                    }
                }

                if (isUnique)
                {
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO PHONEBOOK (NAME, AREA_CODE, PHONE_NUM, MOBILE_NUM, HOUSE_NUM, STREET, CITY, PROVINCE, ZIP_CODE, EMAIL_ADD, STATUS) " +
                            "VALUES (@name, @area_code, @phone_num, @mobile_num, @house_num, @street, @city, @province, @zip_code, @email_add, @status)";
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
                        cmd.Parameters.AddWithValue("@status", status);

                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Phone number already exists. Please enter a unique phone number." }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = true, message = "Product added successfully." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IsPhoneNumberUnique(string phoneNumber)
        {
            bool isUnique = true;
            using (var db = new SqlConnection(conn_str))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT COUNT(*) FROM PHONEBOOK WHERE MOBILE_NUM = @PhoneNumber";
                    cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        isUnique = false;
                    }
                }
            }
            return Json(isUnique);
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
                            result.Append("<table class='table table-striped'><thead class='bg-secondary text-white'><tr><th style=\"text-align:center\">Name</th><th>Phone Number</th><th>Mobile Number</th><th>Status</th><th class=\"ml-5\"></th></tr></thead><tbody>");
                            while (reader.Read())
                            {
                                string mobileNum = reader["MOBILE_NUM"].ToString();
                                result.Append("<tr>");
                                result.Append($"<td style=\"text-align:center\">{reader["NAME"]}</td>");
                                result.Append($"<td>{reader["PHONE_NUM"]}</td>");
                                result.Append($"<td>{mobileNum}</td>");
                                result.Append($"<td>{reader["STATUS"]}</td>");
                                result.Append("<td>");
                                result.Append($"<button style=\"margin-left:20%\" class='btn btn-primary btn_update width='10%'' data-mobile='{mobileNum}'>Update</button>");
                                result.Append($"<button style=\"margin-left:5%\" class='btn btn-warning btn_soft_delete' data-mobile='{mobileNum}'>Soft Delete</button>");
                                result.Append($"<button style=\"margin-left:5%\" class='btn btn-danger btn_hard_delete' data-mobile='{mobileNum}'>Hard Delete</button>");
                                result.Append("</td>");
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


        [HttpPost]
        public ActionResult UpdatePhoneNumber(string oldMobileNumber, string newMobileNumber)
        {
            using (var db = new SqlConnection(conn_str))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE PHONEBOOK SET MOBILE_NUM = @NewMobileNumber WHERE MOBILE_NUM = @OldMobileNumber";
                    cmd.Parameters.AddWithValue("@NewMobileNumber", newMobileNumber);
                    cmd.Parameters.AddWithValue("@OldMobileNumber", oldMobileNumber);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Content("Mobile number updated successfully.");
                    }
                    else
                    {
                        return Content("Failed to update mobile number.");
                    }
                }
            }
        }
        [HttpPost]
        public ActionResult SoftDeletePhoneNumber(string mobileNumber)
        {
            using (var db = new SqlConnection(conn_str))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE PHONEBOOK SET STATUS = 'Inactive' WHERE MOBILE_NUM = @MobileNumber";
                    cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Content("Phone number status updated to inactive.");
                    }
                    else
                    {
                        return Content("Failed to update phone number status.");
                    }
                }
            }
        }
        [HttpPost]
        public ActionResult HardDeletePhoneNumber(string mobileNumber)
        {
            using (var db = new SqlConnection(conn_str))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM PHONEBOOK WHERE MOBILE_NUM = @MobileNumber";
                    cmd.Parameters.AddWithValue("@MobileNumber", mobileNumber);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Content("Phone number deleted successfully.");
                    }
                    else
                    {
                        return Content("Failed to delete phone number.");
                    }
                }
            }
        }
    }
}