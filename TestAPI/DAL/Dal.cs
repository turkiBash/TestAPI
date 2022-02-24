using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using TestAPI.API.Models;

namespace TestAPI.DAL
{
    public class Dal
    {
        private DbConnectionFactory _dbConnectionFactory;

        public Dal(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public List<Activity> GetAllData()
        {
            List<Activity> activities = new List<Activity>();
            string selectQuery = "SELECT * FROM Activity";

            using (MySqlConnection con = _dbConnectionFactory.MySqlConnection())
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand(selectQuery, con))
                {
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Activity activity = new Activity();
                        activity.Id = reader.GetInt32("idActivity");
                        activity.Name = reader["Name"].ToString();
                        activity.CurrentDate = reader.GetDateTime("Date");
                        activities.Add(activity);
                    }

                    reader.Close();
                }
            }
            return activities;
        }
        public bool InsertData(string Name, DateTime CurrentDate)
        { 
            using (MySqlConnection con = _dbConnectionFactory.MySqlConnection())
            {
                con.Open();
                using (MySqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "INSERT INTO Activity(Name, Date) VALUES(@Name, @CurrentDate)";
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@CurrentDate", CurrentDate);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public Activity EditActivity(int Id, string Name, DateTime CurrentDate)
        {
            using (MySqlConnection con = _dbConnectionFactory.MySqlConnection())
            {
                con.Open();
                using (MySqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "UPDATE Activity SET Name = @Name WHERE idActivity = @Id";
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Name", Name);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        var activity = new Activity();
                        activity.Id = Id;
                        activity.Name = Name;
                        activity.CurrentDate = DateTime.Now;
                        return activity;
                    }
                    return null;
                }
            }
        }
        public bool DeleteActivity(int Id)
        {
            using (MySqlConnection con = _dbConnectionFactory.MySqlConnection())
            {
                con.Open();
                using (MySqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Activity WHERE idActivity = @Id;";
                    cmd.Parameters.AddWithValue("@Id", Id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
        public bool CreateSession(long PersonId, string SessionId)
        {
            using (MySqlConnection con = _dbConnectionFactory.MySqlConnection())
            {
                con.Open();
                using (MySqlCommand cmd = con.CreateCommand())
                {
                    
                    cmd.CommandText = "INSERT INTO Session_Table(SessionId, PersonId,Expiration_Date) VALUES(@SessionId, @PersonId,@Expiration_Date)";
                    cmd.Parameters.AddWithValue("@SessionId", SessionId);
                    cmd.Parameters.AddWithValue("@PersonId", PersonId);
                    cmd.Parameters.AddWithValue("@Expiration_Date", DateTime.Now.AddMinutes(1));
                    return cmd.ExecuteNonQuery() > 0;
                    
                }

                // return SessionId;
            }
        }
        public Session CheckSession(string SessionId)
        {
            // List<Session> sessions = new List<Session>();
            using (MySqlConnection con = _dbConnectionFactory.MySqlConnection())
            {
                con.Open();
                using (MySqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = "SELECT SessionId,Expiration_Date FROM Session_Table WHERE SessionId = @SessionId";
                    cmd.Parameters.AddWithValue("@SessionId", SessionId);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Session session = new Session();
                        session.SessionId = reader["SessionId"].ToString();
                        session.ExpirationDate = reader.GetDateTime("Expiration_Date");
                        // sessions.Add(session);
                        return session;

                    }
                    reader.Close();

                }
                return null;
            }
        }
    }
}