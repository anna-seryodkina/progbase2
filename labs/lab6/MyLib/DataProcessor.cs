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

        public bool Delete(long activityId)
        {
            this.connection.Open();
 
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM activities WHERE id = $id";
            command.Parameters.AddWithValue("$id", activityId);

            int nChanged = command.ExecuteNonQuery();
            this.connection.Close();

            if (nChanged == 0)
            {
                return false;
            }
            else 
            {
                return true;
            }
        }

        public bool Update(long activityId, Activity newActivity)
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = 
            @"
                UPDATE activities
                SET type = $type, name = $name, comment = $comment, distance = $distance
                WHERE id = $id
            ";
            command.Parameters.AddWithValue("$id", activityId);
            command.Parameters.AddWithValue("$type", newActivity.type);
            command.Parameters.AddWithValue("$name", newActivity.name);
            command.Parameters.AddWithValue("$comment", newActivity.comment);
            command.Parameters.AddWithValue("$distance", newActivity.distance);

            int nChanged = command.ExecuteNonQuery();

            connection.Close();

            return nChanged == 1;
        }

        public long GetTotalPages(int pageSize)
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
            if(numOfRows%pageSize != 0)
            {
                pages = numOfRows/pageSize + 1;
            }
            else
            {
                pages = numOfRows/pageSize;
            }
            return pages;
        }

        public List<Activity> GetPage(int pageNumber, int pageSize)
        {
            if(pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber));
            }
            List<Activity> list = new List<Activity>();

            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = 
            @"
                SELECT * FROM activities LIMIT $pageS OFFSET ($pageN-1)*$pageS;
            ";
            command.Parameters.AddWithValue("$pageN", pageNumber);
            command.Parameters.AddWithValue("$pageS", pageSize);

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
        public long id;
        public string type;
        public string name;
        public string comment;
        public int distance;
        public DateTime createdAt;

        public Activity()
        {
            this.createdAt = DateTime.Now;
        }

        public override string ToString()
        {
            return $"[{id}] {name} | {distance}";
        }
    }
}
