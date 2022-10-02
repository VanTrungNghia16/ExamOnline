using QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using static System.Net.WebRequestMethods;

namespace QuizApp.Controllers
{
    public class HomeController : Controller
    {
        QuizAppEntities db = new QuizAppEntities();

        //part 6

         [HttpGet]
         public ActionResult sregister()
         {

            return View();
         }

        [HttpPost]
        public ActionResult sregister(student svm, HttpPostedFileBase imgfile)
        {
            student s = new student();

            try
            {
                s.std_name = svm.std_name;
                s.std_password = svm.std_password;
                s.std_image = uploadImage(imgfile);
                db.students.Add(s);
                db.SaveChanges();

                return RedirectToAction("slogin");
            }
            catch(Exception)
            {
                ViewBag.msg = "Data could not be inserted...";
            }
 

            return View();
        }

        public string uploadImage(HttpPostedFileBase file)
        {
            Random r = new Random();

            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {

                string extension = Path.GetExtension(file.FileName);

                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))

                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("/Content/img/"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);

                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);

                        //    ViewBag.Message = "File uploaded successfully";

                    }

                    catch (Exception ex)

                    {

                        path = "-1";

                    }

                }
                else

                {
                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                }

            }
            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");

                path = "-1";
            }

            return path;
        }


        [HttpGet]
        public ActionResult LogOut()
        {
            Session.Abandon();
            Session.RemoveAll();

            return RedirectToAction("Index");
        }

        public ActionResult tlogin()
        {

            return View();
        }

        [HttpPost]
        public ActionResult tlogin(tbl_admin a)
        {

            tbl_admin ad = db.tbl_admin.Where(x => x.ad_name == a.ad_name && x.ad_pass == a.ad_pass).SingleOrDefault();

            if (ad != null)
            {
                Session["ad_id"] = ad.ad_id;
                return RedirectToAction("Dashboard");
            }

            else
            {
                ViewBag.msg = "Invalid User Name or Password";
            }
            return View();
        }

        public ActionResult slogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult slogin(student s)
        {
            student sv = db.students.Where(x => x.std_name == s.std_name && x.std_password == s.std_password).SingleOrDefault();

            if(sv == null)
            {
                ViewBag.msg = "Invalid User Name or Password";
            }
            else
            {
                return RedirectToAction("ExamDashboard");
            }

            return View();
        }

        public ActionResult ExamDashboard()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add_Category()
        {
            //Session["ad_id"] = 2; //remove

            int ad_id = Convert.ToInt32(Session["ad_id"].ToString());

            List<tbl_category> catLi = db.tbl_category.Where(x=>x.cat_fk_ad_id == ad_id).OrderByDescending(x=>x.cat_id).ToList();
            ViewData["list"] = catLi;

            return View();
        }

        [HttpPost]
        public ActionResult Add_Category(tbl_category cat)
        {
            List<tbl_category> catLi = db.tbl_category.OrderByDescending(x => x.cat_id).ToList();
            ViewData["list"] = catLi;
            tbl_category c = new tbl_category();
            c.cat_name = cat.cat_name;
            c.cat_fk_ad_id = Convert.ToInt32(Session["ad_id"].ToString());

            db.tbl_category.Add(c);
            db.SaveChanges();

            return RedirectToAction("Add_Category");


        }

        [HttpGet]
        public ActionResult AddQuestions()
        {
            int s_id = Convert.ToInt32(Session["ad_id"]);
            List<tbl_category> li = db.tbl_category.Where(x => x.cat_fk_ad_id == s_id).ToList();
            ViewBag.list = new SelectList(li, "cat_id", "cat_name");

            return View();
        }

        [HttpPost]
        public ActionResult AddQuestions(tbl_questions q)
        {
            int s_id = Convert.ToInt32(Session["ad_id"]);
            List<tbl_category> li = db.tbl_category.Where(x => x.cat_fk_ad_id == s_id).ToList();
            ViewBag.list = new SelectList(li, "cat_id", "cat_name");

            tbl_questions qa = new tbl_questions();
            qa.q_text = q.q_text;
            qa.QA = q.QA;

            qa.QB = q.QB;
            qa.QC = q.QC;
            qa.QD = q.QD;
            qa.QCorrectAns = q.QCorrectAns;

            qa.q_fk_catid = q.q_fk_catid;
            db.tbl_questions.Add(qa);
            db.SaveChanges();
            TempData["ms"] = "Questions successfully Added";
            TempData.Keep();

            return RedirectToAction("AddQuestions");
        }

        public ActionResult Index()
        {
            if(Session["ad_id"]!=null)
            {
                return RedirectToAction("Dashboard");
            }
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
    }
}