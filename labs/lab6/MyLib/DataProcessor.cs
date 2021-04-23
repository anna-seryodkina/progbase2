using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace MyLib
{
    public class ActivityRepository
    {
        private SqliteConnection connection;
        public ActivityRepository(SqliteConnection connection)
        {
            this.connection = connection;
        }
        public long Insert(Activity activity)
        {
            connection.Open();
 
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = 
            @"
                INSERT INTO activities (type, name, comment, distance, createdAt)
                VALUES ($type, $name, $comment, $distance, $createdAt);
            
                SELECT last_insert_rowid();
            ";
            command.Parameters.AddWithValue("$type", activity.type);
            command.Parameters.AddWithValue("$name", activity.name);
            command.Parameters.AddWithValue("$comment", activity.comment);
            command.Parameters.AddWithValue("$distance", activity.distance);
            command.Parameters.AddWithValue("$createdAt", activity.createdAt.ToString("o"));

            long newId = (long)command.ExecuteScalar();

            connection.Close();
            return newId;
        }

        public bool Delete(int activityId)
        {
            this.connection.Open();
 
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM activities WHERE id = $id";
            command.Parameters.AddWithValue("$id", activityId);

            int nChanged = command.ExecuteNonQuery();
            this.connection.Close();

            if (nChanged == 0)
            {
                return false; // Console.WriteLine("Activity NOT deleted.");
            }
            else 
            {
                return true; // Console.WriteLine("Activity deleted.");
            }
        }

        public bool Update(string value)
        {
            throw new NotImplementedException();
            // connection.Open();

            // SqliteCommand command = connection.CreateCommand();
            // command.CommandText = 
            // @"
            //     UPDATE activities
            //     SET 
            // ";
            // connection.Close();
        }

        public long GetTotalPages() // page size???
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = 
            @"
                SELECT COUNT(*) FROM activities;
            ";
            long numOfRows = (long)command.ExecuteScalar();

            connection.Close();

            long pages = 0;
            if(numOfRows%10 != 0)
            {
                pages = numOfRows/10 + 1;
            }
            else
            {
                pages = numOfRows/10;
            }
            return pages;
        }

        public List<Activity> GetPage(int pageNumber) // pageNum [1...GetPagesNum];   page size???
        {
            List<Activity> list = new List<Activity>();

            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = 
            @"
                SELECT * FROM activities LIMIT 10 OFFSET ($pageN-1)*10;
            ";
            command.Parameters.AddWithValue("$pageN", pageNumber);

            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Activity a = new Activity();
                a.id = int.Parse(reader.GetString(0));
                a.type = reader.GetString(1);
                a.name = reader.GetString(2);
                a.comment = reader.GetString(3);
                a.distance = int.Parse(reader.GetString(4));
                a.createdAt = DateTime.Parse(reader.GetString(5));
                list.Add(a);
            }
            reader.Close();

            connection.Close();
            return list;
        }

        public long GetTotalPages2(string value) // rename function; page size???
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = 
            @"
                SELECT COUNT(*) FROM activities
                WHERE name LIKE '%' || $value || '%';
            ";
            command.Parameters.AddWithValue("$value", value);
            long numOfRows = (long)command.ExecuteScalar();

            connection.Close();

            long pages = 0;
            if(numOfRows%10 != 0)
            {
                pages = numOfRows/10 + 1;
            }
            else
            {
                pages = numOfRows/10;
            }
            return pages;
        }

        public List<Activity> GetPage2(int pageNumber, string value) // rename function; page size???
        {
            List<Activity> list = new List<Activity>();

            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = 
            @"
                SELECT * FROM activities 
                WHERE name LIKE '%' || $value || '%'
                LIMIT 10 OFFSET ($pageN-1)*10;
            ";
            command.Parameters.AddWithValue("$pageN", pageNumber);
            command.Parameters.AddWithValue("$value", value);

            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Activity a = new Activity();
                a.id = int.Parse(reader.GetString(0));
                a.type = reader.GetString(1);
                a.name = reader.GetString(2);
                a.comment = reader.GetString(3);
                a.distance = int.Parse(reader.GetString(4));
                a.createdAt = DateTime.Parse(reader.GetString(5));
                list.Add(a);
            }
            reader.Close();

            connection.Close();
            return list;
        }
    }

    public class Activity
    {
        public int id; // type - long?
        public string type;
        public string name;
        public string comment;
        public int distance;
        public DateTime createdAt;

        public Activity()
        {
            this.createdAt = DateTime.Now;
        }
    }
}
