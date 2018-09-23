using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoginRegisterDemo.Models;
using System.IO.MemoryMappedFiles;
using System.Web.Hosting;
using System.Text;
using System.Net.Mail;

namespace LoginRegisterDemo.Controllers
{
    public class RegisterController : Controller
    {
        UserSiteContext _Context = new UserSiteContext();
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }
        // post Method save data to database
        public JsonResult SaveData([Bind(Include = "ID,UserName,Email,Password")] UserSite model)
        {
          
            _Context.newUser.Add(model);
            _Context.SaveChanges();
            // call  Method to Creat Email Template  with id  newUser
            BuildEmail(model.ID);
            return
                Json("Riegister successfuly", JsonRequestBehavior.AllowGet);
        }

        public void BuildEmail(int iDReg) // id NewUser
        {
         
            string body = System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/BuildEmail/") + "Text" + ".cshtml");//path  View
            UserSite RegInfo = _Context.newUser.Where(x => x.ID == iDReg).FirstOrDefault(); // get  user  from DataBase
            var url = "http://localhost:62319" + "/Register/Confirm?regId="+iDReg;
            body = body.Replace("@ViewBag.ConfirmationLink", url);
            body = body.ToString();
            BuildEmail("Your Account Is Successfully Created", body, RegInfo.Email);
        }
        // built Email
        public static void BuildEmail(string subjectText, string bodyText, string sendTo)
        {

            string from, to, bcc, cc, subject, body;
            from = "esraa.dayyat@gmail.com";
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }
            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SendEmail(mail);

        }

        public JsonResult RegisterConfirm(int RegID)
        {
            UserSite USER = _Context.newUser.Where(X => X.ID == RegID).FirstOrDefault();

            _Context.SaveChanges();
            var msg = "Your Email Is Verified!";
            return Json(msg, JsonRequestBehavior.AllowGet);

        }
       
        public ActionResult Confirm(UserSite reginfo)
        {

            ViewBag.regID = reginfo.ID;
            return View();

        }

        public static void SendEmail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("esraa.dayyat@gmail.com", "311091esraa");
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public JsonResult CheckValidUser( UserSite model)
        {
            string result = "Fail";
        UserSite DataItem =_Context.newUser.Where(x => x.Email == model.Email && x.Password == model.Password).SingleOrDefault();
            if (DataItem == null)
            {   
                return Json(result, JsonRequestBehavior.AllowGet);
            
            }
            Session["UserID"] = DataItem.ID.ToString();
            Session["UserName"] = DataItem.UserName.ToString();
            result = "Success";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AfterLogin()
        {
            if (Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Home/Index");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}