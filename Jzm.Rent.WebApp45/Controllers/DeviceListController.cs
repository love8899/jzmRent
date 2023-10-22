using Jzm.Rent.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Jzm.Rent.WebApp45.DAL;
using Jzm.Rent.WebApp45.Models;
using Microsoft.Ajax.Utilities;

namespace Jzm.Rent.WebApp45.Controllers
{
    public class DeviceListController : Controller
    {
        // GET: DeviceList
        public ActionResult Index()
        {
            return View();
        }
        #region   -show device
public ActionResult ShowDevice()
        {
            // query database
            string sqlTxt = "select [id], [DeviceUID], [DeviceName], [Note], [IsActive], [IsDeleted], [CreatedOnUtc], [UpdateOnUtc],[Quantity] from DeviceInfo where [IsDeleted]=@IsDeleted order by id desc ";
            int IsDeleted = 0;
            DataTable dt = SqlHelper.ExecuteDataTable(sqlTxt, new SqlParameter("@IsDeleted", IsDeleted));

            List<DeviceInfo> deviceInfoList=new List<DeviceInfo>();

            foreach (DataRow row in dt.Rows)
            {


                // convert each line to object BorrowList


                DeviceInfo deviceInfo = new DeviceInfo();

                deviceInfo.id = Convert.ToInt32(row["id"]);
                deviceInfo.DeviceName = Convert.ToString(row["deviceName"]);
                deviceInfo.DeviceUID = Convert.ToString(row["DeviceUID"]);
                deviceInfo.Note = row["Note"] != DBNull.Value ? row["Note"].ToString() : string.Empty;
                deviceInfo.IsActive = Convert.ToBoolean(row["IsActive"]);
                deviceInfo.IsDeleted = Convert.ToBoolean(row["IsDeleted"]);
                deviceInfo.Quantity = Convert.ToInt32(row["Quantity"]);
                deviceInfo.CreatedOnUtc = Convert.ToDateTime(row["CreatedOnUtc"]);
                deviceInfo.UpdateOnUtc= DateTime.Parse(row["UpdateOnUtc"] == DBNull.Value ? SqlDateTime.MinValue.ToString() : row["UpdateOnUtc"].ToString());

                deviceInfoList.Add(deviceInfo);
            }

            return View(deviceInfoList);
            
        }

        #endregion

        #region --Add Device
        
        [HttpGet]
        public ActionResult AddDevice()
        {
            
            return View();
        }

        [HttpPost]
        public ActionResult AddDevice(FormCollection collection)
        {
            string DeviceUID = Request["DeviceUID"];
            string DeviceName = Request["DeviceName"];
            int Quantity = Convert.ToInt32(Request["Quantity"]);
            string sqlTxt = "insert into DeviceInfo (DeviceUID,DeviceName,Quantity) values (@DeviceUID,@DeviceName,@Quantity);";
            SqlHelper.ExecuteNonQuery(sqlTxt,
                new SqlParameter("@DeviceUID", DeviceUID),
                new SqlParameter("@Quantity", Quantity),
                new SqlParameter("@DeviceName", DeviceName));


            return Redirect("/DeviceList/ShowDevice");
            //return Content("OK!");

            //return View();
        }
        #endregion

        #region -- Edit Device

        [HttpGet]
        public ActionResult EditDevice(int Id)
        {

           
            string sqlTxt = "select * from DeviceInfo where id=@Id";
            DataTable dt = SqlHelper.ExecuteDataTable(sqlTxt, new SqlParameter("@Id", Id));
            DataRow row = dt.Rows[0];
            DeviceInfo model = new DeviceInfo();
            model.id = Convert.ToInt32(row["id"]);
            model.DeviceUID = Convert.ToString(row["DeviceUID"]);
            model.DeviceName = Convert.ToString(row["DeviceName"]);
            model.Quantity= Convert.ToInt32(row["Quantity"]);
            model.Note = row["Note"] != DBNull.Value ? Convert.ToString(row["Note"]) : string.Empty;
            model.IsActive = Convert.ToBoolean(row["IsActive"]);
            model.IsDeleted = Convert.ToBoolean(row["IsDeleted"]);
            model.CreatedOnUtc = Convert.ToDateTime(row["CreatedOnUtc"]);
            model.UpdateOnUtc = Convert.ToDateTime(row["UpdateOnUtc"]);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditDevice(DeviceInfo model)
        {
            string sqlTxt = "update DeviceInfo set " +
                            "DeviceName=@DeviceName, " +
                            "Quantity=@Quantity, " +
                            "UpdateOnUtc=@UpdateOnUtc, " +
                            "Note=@Note " +
                            "where id=@id";

            //DateTime upDateTime = DateTime.Now;
            if (model.Note is null )
            {
                model.Note = " ";
            }
            SqlHelper.ExecuteNonQuery(sqlTxt, 
                new SqlParameter("@DeviceName", model.DeviceName),
                new SqlParameter("@UpdateOnUtc", DateTime.Now),
                new SqlParameter("@Quantity", Convert.ToInt32(model.Quantity)),
                new SqlParameter("@Note", model.Note),
                new SqlParameter("@id", model.id));
            return Redirect("~/DeviceList/ShowDevice");
            //return View();
        }


        #endregion

        #region -- Delete Device

        public ActionResult DeleteDevice(DeviceInfo model)
        {
            string sqlTxt = "update DeviceInfo set IsDeleted=1 where id=@id";
            SqlHelper.ExecuteNonQuery(sqlTxt, new SqlParameter("@id", model.id));
            return Redirect("~/DeviceList/ShowDevice");
        }

        #endregion

        #region --check deviceUID is existing
        /// <summary>
        /// check DeviceUID if it is available
        /// </summary>
        /// <param name="DeviceUID"></param>
        /// <returns></returns>

        [HttpGet]

        public JsonResult CheckDeviceUid(string DeviceUID)
        {
            bool DeviceExists = false;

           
            string sqlTxt = "select * from DeviceInfo where DeviceUID=@DeviceUID";
            object obj = SqlHelper.ExecuteScalar(sqlTxt, new SqlParameter("@DeviceUID", DeviceUID));
            int temp = Convert.ToInt32(obj);
            if (temp > 0)
            {
                DeviceExists = true;
            }
            else
            {
                DeviceExists = false;
            }

            return Json(!DeviceExists, JsonRequestBehavior.AllowGet);


        }

        #endregion

       


        #region --List Device Demo for testing

        public ActionResult ListDeviceDemo()
        {

            List<DeviceInfo> deviceInfo = new List<DeviceInfo>();

            deviceInfo = UserDbUtils.GetAllDevice();

            return View(deviceInfo);
        }

        #endregion

        #region --Get Device BY DeviceUID demo

        public ActionResult GetdeviceByUID(string DeviceUID)
        {
            bool DeviceExists = false;

            DeviceUID = "PC000115";
            DeviceInfo model = new DeviceInfo();

            model = UserDbUtils.GetDeviceInfoByUid(DeviceUID);
            int temp = Convert.ToInt32(model.id);
            if (temp > 0)
            {
                DeviceExists = true;
            }
            else
            {
                DeviceExists = false;
            }

            return View(model);

        }

        #endregion
    }
}