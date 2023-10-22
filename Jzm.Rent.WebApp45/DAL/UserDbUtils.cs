using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Jzm.Rent.WebApp45.Models;
using Jzm.Rent.Common;
using System.Data.SqlTypes;
using System.Drawing;
using System.Runtime.Remoting.Messaging;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Ajax.Utilities;

namespace Jzm.Rent.WebApp45.DAL
{
    public class UserDbUtils
    {

        #region --get all users

        /// <summary>
        /// get all users
        /// </summary>
        /// <param name="AvailableForIssue"></param>
        /// <returns></returns>

        public  static  List<JzmUserInfo> GetAllUsers()
        {
            
            List<JzmUserInfo> userInfoList = new List<JzmUserInfo>();
            
                string sqlTxt = "select [id], [UserID], [FirstName], [LastName], [Email], [MobilePhone], [IsActive], [IsDeleted], [CreatedOnUtc], [UpdateOnUtc] from UserInfo where [IsDeleted]=@IsDeleted  ORDER BY id DESC";
                int IsDeleted = 0;
                DataTable dt = SqlHelper.ExecuteDataTable(sqlTxt, new SqlParameter("@IsDeleted", IsDeleted));
                
                foreach (DataRow row in dt.Rows)
                {
                     JzmUserInfo userInfo=new JzmUserInfo();
                    userInfo.id = Convert.ToInt32(row["id"]);
                    userInfo.FirstName = Convert.ToString(row["FirstName"]);
                    userInfo.LastName= Convert.ToString(row["LastName"]);
                    userInfo.Email = Convert.ToString(row["Email"]);
                    userInfo.MobilePhone = Convert.ToString(row["MobilePhone"]);
                    userInfo.IsActive = Convert.ToBoolean(row["IsActive"]);
                    userInfo.IsDeleted = Convert.ToBoolean(row["IsDeleted"]);
                    userInfo.CreatedOnUtc = Convert.ToDateTime(row["CreatedOnUtc"]);
                    userInfo.UpdateOnUtc= DateTime.Parse(row["UpdateOnUtc"] == DBNull.Value ? SqlDateTime.MinValue.ToString() : row["UpdateOnUtc"].ToString());


                userInfoList.Add(userInfo);
               
                }
             return userInfoList;
        }


        #endregion


        #region --GetUserByID
        /// <summary>
        /// Bet user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static JzmUserInfo GetUserById(int id)
    {
        string sqlTxt = "select * from UserInfo where id=@id";
        DataTable dt = SqlHelper.ExecuteDataTable(sqlTxt, new SqlParameter("@Id", id));
        DataRow row = dt.Rows[0];
        JzmUserInfo obj = new JzmUserInfo();
        obj.id = Convert.ToInt32(row["id"]);
        obj.FirstName = Convert.ToString(row["FirstName"]);
        obj.LastName = Convert.ToString(row["LastName"]);
        obj.Email = Convert.ToString(row["Email"]);
        obj.MobilePhone = Convert.ToString(row["MobilePhone"]);


        return obj;
        }

        #endregion





        #region --GetALldevice
        /// <summary>
        ///
        /// 
        /// </summary>
        /// <returns></returns>

        public static List<DeviceInfo> GetAllDevice()
        {

            List<DeviceInfo> deviceInfoList = new List<DeviceInfo>();
            int IsDeleted = 0;
            string sqlTxt = "select * from DeviceInfo where [IsDeleted]=@IsDeleted  ORDER BY id DESC";
            
            DataTable dt = SqlHelper.ExecuteDataTable(sqlTxt, new SqlParameter("@IsDeleted", IsDeleted));

            foreach (DataRow row in dt.Rows)
            {
                //JzmUserInfo userInfo = new JzmUserInfo();

                DeviceInfo deviceInfo=new DeviceInfo();
                deviceInfo.id = Convert.ToInt32(row["id"]);
                deviceInfo.DeviceUID = Convert.ToString(row["DeviceUID"]);
                deviceInfo.DeviceName = Convert.ToString(row["deviceName"]);
               

                //userInfoList.Add(userInfo);
                deviceInfoList.Add(deviceInfo);
            }
            return deviceInfoList;
        }


        #endregion


        #region GetDeviceInfoBy DeviceUID

        public static DeviceInfo GetDeviceInfoByUid( string DeviceUID)
        {
            string sqlTxt = "select * from DeviceInfo where DeviceUID=@DeviceUID";
            DataTable dt = SqlHelper.ExecuteDataTable(sqlTxt, new SqlParameter("DeviceUID", DeviceUID));
            DeviceInfo obj = new DeviceInfo();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                
                obj.id = Convert.ToInt32(row["id"]);
                obj.DeviceUID = Convert.ToString(row["DeviceUID"]);
                obj.DeviceName = Convert.ToString(row["DeviceName"]);
                obj.Quantity = Convert.ToInt32(row["Quantity"]);


            }

            return obj;

        }

        #endregion


        #region --Get BorrowDeviceByID


        public static Borrow GetBorrowDeviceById(int id)
        {
            string sqlTxt = "select * from Borrow where id=@id";
            DataTable dt = SqlHelper.ExecuteDataTable(sqlTxt, new SqlParameter("@Id", id));
            DataRow row = dt.Rows[0];
            // obj = new JzmUserInfo();

            Borrow obj=new Borrow();
            obj.id = Convert.ToInt32(row["id"]);
            obj.BorrowQuantity = Convert.ToInt32(row["BorrowQuantity"]);
            obj.ReturnQuantity = row["ReturnQuantity"] != DBNull.Value ? Convert.ToInt32(row["ReturnQuantity"]) : 0;
            obj.BorrowDays = Convert.ToInt32(row["BorrowDays"]);
            obj.UserInfo_id = Convert.ToInt32(row["UserInfo_id"]);
            obj.DeviceInfo_id = Convert.ToInt32(row["DeviceInfo_id"]);
            
            obj.DeviceName = Convert.ToString(row["DeviceName"]);
            obj.DeviceUID = Convert.ToString(row["DeviceUID"]);
            obj.IsReturned =  Convert.ToBoolean(row["IsReturned"]);


            obj.UserLastName = Convert.ToString(row["UserLastName"]);
            obj.UserFirstName = Convert.ToString(row["UserFirstName"]);
            obj.Note = row["Note"] != DBNull.Value ? Convert.ToString(row["Note"]) : string.Empty;

            obj.BorrowDateOnUtc = DateTime.Parse(row["BorrowDateOnUtc"].ToString());
            obj.ReturnDateOnUtc = DateTime.Parse(row["ReturnDateOnUtc"] != DBNull.Value
                ? row["ReturnDateOnUtc"].ToString():SqlDateTime.MinValue.ToString());



            return obj;



        }

        #endregion

        #region --GetAllBorrowDevice 

        public static List<Borrow> GetAllBorrowDevice()
        {
            // query database
            string sqlTxt = "select * from Borrow  order by id DESC ";

            DataTable dt = SqlHelper.ExecuteDataTable(sqlTxt);

            List<Borrow> borrowList = new List<Borrow>();


            if (dt != null && dt.Rows.Count > 0)
            {

                foreach (DataRow row in dt.Rows)
                {


                    // convert each line to object BorrowList

                    Borrow borrowInfo = new Borrow();

                    borrowInfo.id = Convert.ToInt32(row["id"]);
                    borrowInfo.BorrowDateOnUtc = Convert.ToDateTime(row["BorrowDateOnUtc"]);
                    borrowInfo.ReturnDateOnUtc = DateTime.Parse(row["ReturnDateOnUtc"] == DBNull.Value
                        ? SqlDateTime.MinValue.ToString()
                        : row["ReturnDateOnUtc"].ToString());
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
                    borrowInfo.DeviceName =
                        row["DeviceName"] != DBNull.Value ? row["DeviceName"].ToString() : string.Empty;
                    borrowInfo.IsReturned = Convert.ToBoolean(row["IsReturned"]);
                    borrowInfo.BorrowQuantity = Convert.ToInt32(row["BorrowQuantity"]);
                    borrowInfo.ReturnQuantity =
                        row["ReturnQuantity"] != DBNull.Value ? Convert.ToInt32(row["ReturnQuantity"]) : 0;
                    borrowList.Add(borrowInfo);
                }

             }
             return borrowList;
        }

        #endregion



    } //end class
    

    }
   


   

   