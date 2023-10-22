using Jzm.Rent.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Jzm.Rent.WebApp45.Models;
using Jzm.Rent.WebApp45.DAL;

namespace Jzm.Rent.WebApp45.Controllers
{
    public class UserListController : Controller
    {
        #region --index
// GET: UserList

        public ActionResult Index()
        {
            // query database
            string sqlTxt = "select [id], [UserID], [FirstName], [LastName], [Email], [MobilePhone], [IsActive], [IsDeleted], [CreatedOnUtc], [UpdateOnUtc] from UserInfo ";
            DataTable dt= SqlHelper.ExecuteDataTable(sqlTxt);
            ViewData["dt"] = dt;

            return View();
        }


        #endregion

        #region --show users
         public ActionResult ShowUsers()
        {
            List<JzmUserInfo> userInfo = UserDbUtils.GetAllUsers();
            return View(userInfo);
        }
        

        #endregion

        
        #region --Add User
        [HttpGet]
        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(FormCollection collection)
        {
            string FirstName = Request["FirstName"];
            string LastName = Request["LastName"];
            string Email = Request["Email"];
            string MobilePhone = Request["MobilePhone"];

            string sqlTxt = "insert into UserInfo (FirstName,LastName,Email,MobilePhone) values (@FirstName,@LastName,@Email,@MobilePhone);";
            SqlHelper.ExecuteNonQuery(sqlTxt,
                new SqlParameter("@FirstName", FirstName),
                new SqlParameter("@Email",Email),
                new SqlParameter("@MobilePhone",MobilePhone),
                new SqlParameter("@LastName", LastName));


            return Redirect("/UserList/ShowUsers");
            //return Content("OK!");
        }
        #endregion
        #region --Edit User

        [HttpGet]
        public ActionResult EditUser(int Id)
        {
            //string sqlTxt = "select * from UserInfo where id=@Id";
            //DataTable dt= SqlHelper.ExecuteDataTable(sqlTxt,new SqlParameter("@Id",Id));
            //DataRow row = dt.Rows[0];
            //JzmUserInfo model=new JzmUserInfo();
            //model.id = Convert.ToInt32(row["id"]);
            //model.FirstName = Convert.ToString(row["FirstName"]);
            //model.LastName = Convert.ToString(row["LastName"]);

            JzmUserInfo model = new JzmUserInfo();
            model = UserDbUtils.GetUserById(Id);


            return View(model);
        }

        [HttpPost]
        public ActionResult EditUser(JzmUserInfo model)
        {
            string sqlTxt = "update UserInfo set FirstName=@FirstName,LastName=@LastName,Email=@Email,MobilePhone=@MobilePhone where id=@id";

            SqlHelper.ExecuteNonQuery(sqlTxt,new SqlParameter("@FirstName",model.FirstName),
                new SqlParameter("@LastName", model.LastName),
                new SqlParameter("@Email",model.Email),
                new SqlParameter("@MobilePhone",model.MobilePhone),
                new SqlParameter("@id", model.id));
            return Redirect("~/UserList/ShowUsers");


        }
        #endregion
        #region --Delete User

        public ActionResult DeleteUser(JzmUserInfo model)
        {
            string sqlTxt = "update UserInfo set IsDeleted=1 where id=@id";
            SqlHelper.ExecuteNonQuery(sqlTxt,new SqlParameter("@id", model.id));
            return Redirect("~/UserList/ShowUsers");
        }
        #endregion
    }
}