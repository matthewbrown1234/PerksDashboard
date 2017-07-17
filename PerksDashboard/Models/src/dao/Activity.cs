using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PerksDashboard.Models.src.dao;
using System.Data.SqlClient;
using System.Data;
using PerksDashboard;
using PerksDashboard.Models.src.dto;

namespace PerksDashboard.Models.src.dao
{
    public class Activity
    {
        private static String SELECT_ACTIVITIES = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY date_time) AS RowNum, a.id as activity_id,a.type as activity_type, h.name as handle_name, a.description as activity_description, a.date_time as activity_date_time, a.verified as activity_verified FROM[dbo].[activity] a join[dbo].[handle] h on a.handle_id = h.id) AS RowConstrainedResult WHERE RowNum >= @startRow AND RowNum <= @endRow ORDER BY RowNum";

        private static String ACTIVITY_COUNT = "select count(*) as count from [dbo].[activity]";

        private static String SELECT_SALES_ACTIVITY = "select h.name, a.type, a.id as activity_id, s.invoice_id, s.id as sales_id, a.description, a.date_time, a.verified as activity_verified from [dbo].[activity] a join [dbo].[sales] s on a.id = s.activity_id join [dbo].[handle] h on h.id = a.handle_id WHERE activity_id = @activityId";

        private static String SELECT_RECOGNITION_ACTIVITY = "select h.name, a.type, r.reason, a.id as activity_id, s.reason_id, s.id as recognition_id, a.description, a.date_time, a.verified as activity_verified from [dbo].[activity] a join [dbo].[recognition] s on a.id = s.activity_id join [dbo].[handle] h on h.id = a.handle_id JOIN [dbo].[recognition_reason] r on r.id = s.reason_id WHERE activity_id = @activityId";

        private static String UPDATE_VERIFY_ACTIVITY = "update [dbo].[activity] set verified = 1 where id ";

        public static List<ActivityDto> getAllActivities(SqlConnection connection, Int32 startRow, Int32 endRow)
        {
            List<ActivityDto> activityList = new List<ActivityDto>();
            try
            {
                SqlCommand command = new SqlCommand(SELECT_ACTIVITIES, connection);
                command.Parameters.Add("startRow", SqlDbType.Int).Value = startRow;
                command.Parameters.Add("endRow", SqlDbType.Int).Value = endRow + startRow;
                connection.Open();
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        activityList.Add(processActivityRow(rdr));
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Dispose();
            }
            return activityList;
        }
        public static void verifyActivity(SqlConnection connection, Int32[] itemList)
        {
            try
            {
                string updateStatement = UPDATE_VERIFY_ACTIVITY;
                updateStatement += "in(";
                for (int i = 0; i < itemList.Length; i ++)
                {
                    updateStatement += "@param" + i;
                    updateStatement += itemList.Length -1 == i ? "" : ",";
                }
                updateStatement += ")";
                SqlCommand command = new SqlCommand(updateStatement, connection);
                for (int i = 0; i < itemList.Length; i ++)
                {
                    command.Parameters.Add("param" + i, SqlDbType.Int).Value = itemList[i];
                }
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Dispose();
            }
        }
        public static Int32 getActivityCount(SqlConnection connection)
        {
            Int32 activityCount;
            try
            {
                SqlCommand command = new SqlCommand(ACTIVITY_COUNT, connection);
                connection.Open();
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    if (!rdr.Read())
                    {
                        throw new InvalidOperationException("No records were returned.");
                    }
                    activityCount = Int32.Parse(rdr["count"].ToString());
                    if (rdr.Read())
                    {
                        throw new InvalidOperationException("Multiple records were returned.");
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Dispose();
            }
            return activityCount;
        }

        public static SalesActivityDto getSalesActivity(SqlConnection connection, Int32 actionId)
        {
            SalesActivityDto salesActivity = new SalesActivityDto();
            try
            {
                SqlCommand command = new SqlCommand(SELECT_SALES_ACTIVITY, connection);
                command.Parameters.Add("activityId", SqlDbType.Int).Value = actionId;
                connection.Open();
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    if (!rdr.Read())
                    {
                        throw new InvalidOperationException("No records were returned.");
                    }
                    salesActivity = processSalesRow(rdr);
                    if (rdr.Read())
                    {
                        throw new InvalidOperationException("Multiple records were returned.");
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Dispose();
            }
            return salesActivity;
        }

        public static RecognitionActivityDto getRecognitionActivity(SqlConnection connection, Int32 actionId)
        {
            RecognitionActivityDto recognitionActivity = new RecognitionActivityDto();
            try
            {
                SqlCommand command = new SqlCommand(SELECT_RECOGNITION_ACTIVITY, connection);
                command.Parameters.Add("activityId", SqlDbType.Int).Value = actionId;
                connection.Open();
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    if (!rdr.Read())
                    {
                        throw new InvalidOperationException("No records were returned.");
                    }
                    recognitionActivity = processRecognitionRow(rdr);
                    if (rdr.Read())
                    {
                        throw new InvalidOperationException("Multiple records were returned.");
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Dispose();
            }
            return recognitionActivity;
        }

        private static RecognitionActivityDto processRecognitionRow(SqlDataReader rdr)
        {
            RecognitionActivityDto recognitionActivity = new RecognitionActivityDto();
            recognitionActivity.RecognitionId = Int32.Parse(rdr["recognition_id"].ToString());
            recognitionActivity.RecognitionReason = rdr["reason"].ToString();
            recognitionActivity.Id = Int32.Parse(rdr["activity_id"].ToString());
            recognitionActivity.Verified = Boolean.Parse(rdr["activity_verified"].ToString());
            recognitionActivity.HandleName = rdr["name"].ToString();
            recognitionActivity.Type = Char.Parse(rdr["type"].ToString());
            recognitionActivity.Description = rdr["description"].ToString();
            recognitionActivity.DateTime = DateTimeOffset.Parse(rdr["date_time"].ToString());
            return recognitionActivity;
        }

        private static SalesActivityDto processSalesRow(SqlDataReader rdr)
        {
            SalesActivityDto salesActivity = new SalesActivityDto();
            salesActivity.SalesId = Int32.Parse(rdr["sales_id"].ToString());
            salesActivity.InvoiceId = rdr["invoice_id"].ToString();
            salesActivity.Id = Int32.Parse(rdr["activity_id"].ToString());
            salesActivity.Type = Char.Parse(rdr["type"].ToString());
            salesActivity.Verified = Boolean.Parse(rdr["activity_verified"].ToString());
            salesActivity.HandleName = rdr["name"].ToString();
            salesActivity.Description = rdr["description"].ToString();
            salesActivity.DateTime = DateTimeOffset.Parse(rdr["date_time"].ToString());
            return salesActivity;
        }

        private static ActivityDto processActivityRow(SqlDataReader rdr)
        {
            ActivityDto activity = new ActivityDto();
            activity.Id = Int32.Parse(rdr["activity_id"].ToString());
            activity.Verified = Boolean.Parse(rdr["activity_verified"].ToString());
            activity.Type = Char.Parse(rdr["activity_type"].ToString());
            activity.HandleName = rdr["handle_name"].ToString();
            activity.Description = rdr["activity_description"].ToString();
            activity.DateTime = DateTimeOffset.Parse(rdr["activity_date_time"].ToString());
            return activity;
        }
    }
}