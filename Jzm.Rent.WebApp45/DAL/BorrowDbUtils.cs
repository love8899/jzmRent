using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jzm.Rent.WebApp45.Models;
using Jzm.Rent.Common;
using System.Data.SqlTypes;
using System.Data;
using System.Data.SqlClient;
namespace Jzm.Rent.WebApp45.DAL
{
    public class BorrowDbUtils
    {
        #region --GetBorrowDevicePage 
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<Borrow> GetBorrowDevicePage(int start, int end)
        {
            
            string sqlTxt = "select * from(select *,row_number()over(order by id) as num from Borrow) as t where t.num>=@start and t.num<=@end order by id DESC ";

            DataTable dt = SqlHelper.ExecuteDataTable(sqlTxt,new SqlParameter("@start",start),new SqlParameter("@end",end));

            List<Borrow> borrowList = new List<Borrow>();


            if (dt != null && dt.Rows.Count > 0)
            {

                foreach (DataRow row in dt.Rows)
                {


                    // convert each line to object BorrowList

                    Borrow borrowInfo = new Borrow();

                    LoadEntity(borrowInfo, row);
                    borrowList.Add(borrowInfo);
                }

            }
            return borrowList;
        }

        #endregion



        #region --get borrow page list

        public List<Borrow> GetBorrowPageList(int pageIndex, int pageSize)
        {
            int start = (pageIndex - 1) * pageSize + 1;
            int end = pageIndex * pageSize;
            return GetBorrowDevicePage(start, end);

        }

        #endregion

        #region --LoadEntity from Borrow 
        /// <summary>
        /// load Borrow info to Entity
        /// </summary>
        /// <param name="borrowInfo"></param>
        /// <param name="row"></param>
        private void LoadEntity(Borrow borrowInfo, DataRow row)
        {
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
        }



        #endregion



        #region --get total record

        public int GetRecordCount()
        {
            string sqlTxt = "select count(*) from Borrow";
            return Convert.ToInt32(SqlHelper.ExecuteScalar(sqlTxt));
        }


        #endregion

        #region --get page count

        public int GetPageCount(int pageSize)
        {
            int recordCount = GetRecordCount();//获取总的记录数.
            int pageCount = Convert.ToInt32(Math.Ceiling((double)recordCount / pageSize));
            return pageCount;
        }

        #endregion
    }//end class
}