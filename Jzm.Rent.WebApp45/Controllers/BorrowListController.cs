using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jzm.Rent.Common;
using Jzm.Rent.WebApp45.DAL;
using Jzm.Rent.WebApp45.Models;

namespace Jzm.Rent.WebApp45.Controllers
{
    public class BorrowListController : Controller
    {
        #region --ShowBorrowDevice
// GET: BorrowList
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult ShowBorrowDevice()
        {


            // query database
            string sqlTxt = "select * from Borrow where IsReturned=@IsReturned order by id DESC ";

            int IsReturned = 0;
            DataTable dt = SqlHelper.ExecuteDataTable(sqlTxt,new SqlParameter("@IsReturned",IsReturned));

            List<Borrow> borrowList= new List<Borrow>();


            foreach (DataRow row in dt.Rows)
            {
               

                // convert each line to object BorrowList

                Borrow borrowInfo=new Borrow();

                borrowInfo.id = Convert.ToInt32(row["id"]);
                borrowInfo.BorrowDateOnUtc = Convert.ToDateTime(row["BorrowDateOnUtc"]);
                borrowInfo.ReturnDateOnUtc = DateTime.Parse(row["ReturnDateOnUtc"]==DBNull.Value?SqlDateTime.MinValue.ToString(): row["ReturnDateOnUtc"].ToString());
                borrowInfo.BorrowDays = Convert.ToInt32(row["BorrowDays"]);
                borrowInfo.UserInfo_id = Convert.ToInt32(row["UserInfo_id"]);
                borrowInfo.DeviceInfo_id = Convert.ToInt32(row["DeviceInfo_id"]);
                borrowInfo.Note = row["Note"] != DBNull.Value ? row["Note"].ToString() : string.Empty;
                borrowInfo.UserLastName =
                    row["UserLastName"] != DBNull.Value ? row["UserLastName"].ToString() : string.Empty;
                borrowInfo.UserFirstName = row["UserFirstName"] != DBNull.Value
                    ? row["UserFirstName"].ToString()
                    : string.Empty;
                borrowInfo.DeviceUID = Convert.ToString(row["DeviceUID"]);
                borrowInfo.DeviceName = row["DeviceName"] != DBNull.Value ? row["DeviceName"].ToString() : string.Empty;
                borrowInfo.IsReturned= Convert.ToBoolean(row["IsReturned"]);
                borrowInfo.BorrowQuantity = Convert.ToInt32(row["BorrowQuantity"]);
                borrowInfo.ReturnQuantity = row["ReturnQuantity"]!=DBNull.Value?Convert.ToInt32(row["ReturnQuantity"]):0;
                borrowList.Add(borrowInfo);
            }


            return View(borrowList);
        }

        #endregion

        #region --EditBorrowDevice

        public ActionResult EditBorrowDevice()
        {


            return Content("OK!!");

        }

        #endregion

        #region Add Borrow device by device

        [HttpGet]
        public ActionResult AddBorrowDevice(int Id)
        {
            string sqlTxt = "select * from DeviceInfo where id=@Id";
            DataTable dt = SqlHelper.ExecuteDataTable(sqlTxt, new SqlParameter("@Id", Id));
            DataRow row = dt.Rows[0];
            DeviceInfo model = new DeviceInfo();
            model.id = Convert.ToInt32(row["id"]);
            model.DeviceUID = Convert.ToString(row["DeviceUID"]);
            model.DeviceName = Convert.ToString(row["DeviceName"]);
            model.Quantity = Convert.ToInt32(row["Quantity"]);
            model.Note = row["Note"] != DBNull.Value ? Convert.ToString(row["Note"]) : string.Empty;
            model.IsActive = Convert.ToBoolean(row["IsActive"]);
            model.IsDeleted = Convert.ToBoolean(row["IsDeleted"]);
            model.CreatedOnUtc = Convert.ToDateTime(row["CreatedOnUtc"]);
            model.UpdateOnUtc = Convert.ToDateTime(row["UpdateOnUtc"]);

            return View(model);

            //return Content("add borrow device");
        }

        #endregion

        #region --Add borrow device by user

        [HttpGet]
        public ActionResult AddBorrowDeviceByUser(int Id)
        {
            
            if ( Id == 0)
            {
                Redirect("~/UserList/ShowUsers");
            }

            JzmUserInfo userDetails=new JzmUserInfo();
            userDetails = UserDbUtils.GetUserById(Id);

            ViewBag.userDetails = userDetails;


            // return Content("by user");
            return View();

        }

        [HttpPost]

        public ActionResult AddBorrowDeviceByUser(FormCollection collection)
        {
            string DeviceUID = Request["DeviceUID"];

            if( string.IsNullOrEmpty(DeviceUID))
            {
                return  RedirectToAction("showUsers","UserList");
            }

            string FirstName = Request["FirstName"];
            string LastName = Request["LastName"];
            int UserInfo_Id = Convert.ToInt32(Request["UserId"]);
            
            DateTime BorrowDateOnUtc = Convert.ToDateTime(Request["BorrowDateOnUtc"]);
            int BorrowDays = Convert.ToInt32(Request["BorrowDays"]);
            int BorrowQuantity = Convert.ToInt32(Request["BorrowQuantity"]);

            DeviceInfo obj = new DeviceInfo();

            obj = UserDbUtils.GetDeviceInfoByUid(DeviceUID);

            int DeviceInfo_id = obj.id;
            string DeviceName = obj.DeviceName;
            //insert into dbo.Borrow (BorrowDays, UserInfo_id,deviceInfo_id,[UserFirstName],UserLastName, DeviceName,BorrowQuantity)  values (5, 5,5,'jzm', 'last','desk computer',3);

            string sqlTxt = "insert into dbo.Borrow (BorrowDays, UserInfo_id,DeviceInfo_id,[UserFirstName],UserLastName, DeviceUID,DeviceName,BorrowQuantity)  values (@BorrowDays,@UserInfo_id,@DeviceInfo_id,@UserFirstName,@UserLastName,@DeviceUID,@DeviceName,@BorrowQuantity) ";
            string sqlUpdateQuantity = "update dbo.DeviceInfo set Quantity=Quantity-@BorrowQuantity where id=@DeviceInfo_id";
            SqlHelper.ExecuteNonQuery(sqlTxt,
                new SqlParameter("@BorrowDays", BorrowDays),
                new SqlParameter("@UserInfo_id", UserInfo_Id),
                new SqlParameter("@DeviceInfo_id", DeviceInfo_id),
                new SqlParameter("@UserLastName", LastName),
                new SqlParameter("@DeviceName", DeviceName),
                new SqlParameter("@DeviceUID",DeviceUID),
                new SqlParameter("@BorrowQuantity", BorrowQuantity),

                new SqlParameter("@UserFirstName", FirstName));

            SqlHelper.ExecuteNonQuery(sqlUpdateQuantity, new SqlParameter("@BorrowQuantity", BorrowQuantity),new SqlParameter("@DeviceInfo_id",DeviceInfo_id));


            return Redirect("/UserList/ShowUsers");


            //return Content("Post successfully!");
        }



        #endregion

        #region --Delete Device

        public ActionResult DeleteDevice(Borrow model)
        {
            string sqlTxt = "delete Borrow  where id=@id";
            SqlHelper.ExecuteNonQuery(sqlTxt, new SqlParameter("@id", model.id));
            return Redirect("~/BorrowList/ShowBorrowDevice");

        }

        #endregion

        #region --Return Device
        /// <summary>
        /// return Device module
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult ReturnDevice(int id)
        {
            Borrow model = new Borrow();

            model = UserDbUtils.GetBorrowDeviceById(id);
            return View(model);

           

        }


        [HttpPost]

        public ActionResult ReturnDevice(Borrow model)
        {
            int IsRetutned = 1;
            // update Borrow table
            string sqlTxt = "update Borrow set " +
                "IsReturned=@IsReturned, " +
                "ReturnDateOnUtc=@ReturnDateOnUtc, " +
                "ReturnQuantity=@ReturnQuantity," +
                "Note=@Note " +
                "where id=@id";

            if (model.Note is null)
            {
                model.Note = "No";
            }


            SqlHelper.ExecuteNonQuery(sqlTxt,
                new SqlParameter("@ReturnDateOnUtc", model.ReturnDateOnUtc),
                new SqlParameter("@ReturnQuantity", Convert.ToInt32(model.ReturnQuantity)),
                new SqlParameter("@IsReturned",IsRetutned),
                new SqlParameter("@Note", model.Note),
                new SqlParameter("@id", model.id));

            // at mean time, Updtae DeviceInfo table

            string sqlUpdateDevice = "update DeviceInfo set " +
                                     " Quantity=Quantity+@ReturnQuantity  " +
                                     "where DeviceUID=@DeviceUID";
            SqlHelper.ExecuteNonQuery(sqlUpdateDevice, new SqlParameter("@ReturnQuantity",
                Convert.ToInt32(model.ReturnQuantity)),
                new SqlParameter("@DeviceUID",model.DeviceUID));

            return Redirect("~/DeviceList/ShowDevice");


            //return Content("Post OK!!");



        }



        #endregion

        #region --GetAllDevice for Ajax
        [HttpGet]

        public ActionResult GetAllDevice()
        {
            List< DeviceInfo> deviceInfo=new List<DeviceInfo>();

            deviceInfo = UserDbUtils.GetAllDevice();

            return Json(deviceInfo,JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region GetDeviceUid for Ajax
        /// <summary>
        /// Get device info by DeviceUID
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDeviceByUID(string DeviceUID)
        {
           DeviceInfo deviceInfo=new DeviceInfo();

            deviceInfo = UserDbUtils.GetDeviceInfoByUid(DeviceUID);

            return Json(deviceInfo, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region --show All Borrow deivce list

        public ActionResult ShowAllBorrowDevice()
        {
            List<Borrow> model = new List<Borrow>();

            //model = UserDbUtils.GetAllBorrowDevice();

            int pageIndex;
            if (!int.TryParse(Request["pageIndex"], out pageIndex))
            {
                pageIndex = 1;
            }
            int pageSize = 5;
            
            BorrowDbUtils borrowDbUtils=new BorrowDbUtils();
            model = borrowDbUtils.GetBorrowPageList(pageIndex, pageSize);
            ViewData["pageIndex"] = pageIndex;
            ViewData["pageCount"] = borrowDbUtils.GetPageCount(pageSize);
            return View(model);
        }

        #endregion
    } //end class
}