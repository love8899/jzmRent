using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Channels;


namespace Jzm.Rent.Common
{
    public class SqlHelper
    {
        #region --getConnectionString
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
        }

        public static readonly string connStr = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;

        #endregion
        #region SqlConnection


        public static SqlConnection OpenConnection()
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            return conn;
        }
         
        #endregion

        #region ExecuteNonQuery
        public static int ExecuteNonQuery(SqlConnection conn, String cmdText, params SqlParameter[] parameters)
        {
            //using (SqlCommand cmd = conn.CreateCommand())
            //{
            //    cmd.CommandText = cmdText;
            //    cmd.Parameters.AddRange(parameters);
            //    return cmd.ExecuteNonQuery();
            //}

            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);

                return cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                //Console.WriteLine(ex.Message);
                //throw;

                string errorInfo = "call ExecuteNonQuery() error occured" + ex.Message;

                WriteLog(errorInfo);
                throw new Exception(errorInfo);
            }
            finally
            {
                conn.Close();
            }

        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(String cmdText, params SqlParameter[] parameters)
        {
            #region using--

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                return ExecuteNonQuery(conn, cmdText, parameters);

            }


            #endregion
            //SqlConnection conn = new SqlConnection(connStr);
            //try
            //{
               
            //    conn.Open();
            //    return ExecuteNonQuery(conn, cmdText, parameters);
            //}
            //catch (Exception ex)
            //{
            //    WriteLog(ex.Message);
            //}
            
        }



        #endregion

        #region execcutescalar

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the result set returned by the query
        /// Additional columns or rows are ignored.
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string cmdText, params SqlParameter[] parameters)
        {

            //using (SqlConnection conn = new SqlConnection(connStr))
            //{
            //    conn.Open();
            //    using (SqlCommand cmd = conn.CreateCommand())
            //    {
            //        cmd.CommandText = cmdText;
            //        cmd.Parameters.AddRange(parameters);
            //        return cmd.ExecuteScalar();
            //    }
            //}

            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = conn.CreateCommand();

            try
            {
                conn.Open();
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                string errorInfo = "call ExecuteScalar() error occured" + ex.Message;

                WriteLog(errorInfo);
                throw new Exception(errorInfo);
            }

            finally
            {
                conn.Close();
            }
        }

        #endregion


        #region ExecuteDataTable

        public static DataTable ExecuteDataTable(string cmdText, params SqlParameter[] parameters)
        {
            //using (SqlConnection conn = new SqlConnection(connStr))
            //{
            //    conn.Open();
            //    using (SqlCommand cmd = conn.CreateCommand())
            //    {
            //        cmd.CommandText = cmdText;
            //        if (parameters != null)
            //        {
            //            cmd.Parameters.AddRange(parameters); 

            //        }

            //        DataTable dt = new DataTable();
            //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            //        adapter.Fill(dt);
            //        return dt;//这四行与上面处理是不同的
            //    }
            //}

            SqlConnection conn = new SqlConnection(connStr);
            SqlCommand cmd = conn.CreateCommand();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            cmd.CommandText = cmdText;
            DataTable dt = new DataTable();
            if (parameters != null)
            {

                cmd.Parameters.AddRange(parameters);
            }


            try
            {
                conn.Open();
                adapter.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                string errorInfo = "call ExecuteDataTable() error occured" + ex.Message;

                WriteLog(errorInfo);
                throw new Exception(errorInfo);
            }

            finally
            {
                conn.Close();
            }


        }

        #endregion

        #region --handle database error

        private static void WriteLog(string errLog)
        {
            // string strFolder = .MapPath("./");
            //string path = Server.map;
            //string path = Server.MapPath("~/Images/BookCover/");
            string fileName = @"D:\sqlLog\sqlHelper.log";
             using(FileStream fs = new FileStream(fileName, FileMode.Append)) 
             {
                 
                 StreamWriter sw = new StreamWriter(fs);
                 sw.WriteLine(DateTime.Now.ToString() + "   " + errLog);
                 //sw.Close();
                 //fs.Close();

             }
        }
        

        #endregion



    }
}