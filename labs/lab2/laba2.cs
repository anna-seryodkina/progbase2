using System;
using static System.Console;
using Microsoft.Data.Sqlite;


namespace lab2
{
    class Planet
    {
        public int id;
        public string name;
        public double size;
        public string color;

        public Planet()
        {
            id = 0;
            name = "Earth";
            size = 100.001;
            color = "black";
        }
        public Planet(int planetId, string planetName, double planetSize, string planetColor)
        {
            this.id = planetId;
            this.name = planetName;
            this.size = planetSize;
            this.color = planetColor;
        }
        public override string ToString()
        {            
            return $"\nID: {this.id}\nName: {this.name}\nSize: {this.size}\nColor: {this.color}";
        }
    }

    class ListPlanet
    {
        private Planet[] _items;
        private int _size;
        public ListPlanet()
        {
            _items = new Planet[16];
            _size = 0;
        }

        public void Add(Planet newPlanet)
        {
            EnsureCapacity(this.Count+1);
            _items[_size] = newPlanet;
            _size++;
        }
        
        public void RemoveAt(int index)
        {
            if(index < 0 || index > this.Count)
            {
                throw new Exception("incorrect index.");
            }
            for(int i = index; i < this.Count; i++)
            {
                _items[i] = _items[i+1];
            }
            _size -= 1;
        }
        
        public int Count 
        {
            get 
            {
                return _size;
            }
        }
        public int Capacity 
        {
            get 
            {
                return _items.Length;
            }
        }
        public Planet this[int index]
        {
            get 
            {
                if(index < 0 || index > this.Count)
                {
                    throw new Exception("incorrect index.");
                }
                return _items[index];
            }
            set 
            {
                if(index < 0 || index > this.Count)
                {
                    throw new Exception("incorrect index.");
                }
                _items[index] = value;
            }
        }
        private void EnsureCapacity(int newSize) 
        {
            if(newSize > this.Capacity)
            {
                Array.Resize(ref _items, this.Capacity * 2);
            }
        }
    }

    class PlanetRepository
    {
        private SqliteConnection connection;
        public PlanetRepository(SqliteConnection connection) // some hell is happening here :)
        {
            this.connection = connection;
        }
 
        public Planet GetById(int id) 
        {
            connection.Open();
            
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM planets WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            
            SqliteDataReader reader = command.ExecuteReader();
            Planet p = new Planet();
            if (reader.Read())
            {
                p.id = int.Parse(reader.GetString(0));
                p.name = reader.GetString(1);
                p.size = Convert.ToDouble(reader.GetString(2));
                p.color = reader.GetString(3);
            }
            else
            {
                p = null;
            }            
            reader.Close();            
            connection.Close();

            return p;
        }
        public int DeleteById(int id) 
        {
            connection.Open();
 
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM planets WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            
            int nChanged = command.ExecuteNonQuery();

            connection.Close();

            return nChanged;
        }
        public int Insert(Planet planet) 
        {
            connection.Open();
 
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = 
            @"
                INSERT INTO planets (id, name, size, color) 
                VALUES ($id, $name, $size, $color);
            
                SELECT last_insert_rowid();
            ";
            command.Parameters.AddWithValue("$id", planet.id);
            command.Parameters.AddWithValue("$name", planet.name);
            command.Parameters.AddWithValue("$size", planet.size);
            command.Parameters.AddWithValue("$color", planet.color);
            
            long newId = (long)command.ExecuteScalar();
            
            connection.Close();
            return (int)newId; // check!!!
        }

        public int GetTotalPages()
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = 
            @"
                SELECT COUNT(*) FROM planets;
            ";
            int numOfRows = (int)command.ExecuteScalar();

            connection.Close();

            int pages = 0;
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

        public ListPlanet GetPage(int pageNumber) 
        {
            // in: page num; 
            // out: planet list
            /*
            пройтись по списку (N рядків скіпнуть)
            і коли дійдем до потрібного N+1 го рядка
            то в вайлі M к-ть (10 aбо менше) разів зробити list.add()
            */
            throw new NotImplementedException();
        }
    
        // public ListPlanet GetExport(T valueX) 
        // {
        //     throw new NotImplementedException();
        // }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string databaseFileName = "planets.db";
            SqliteConnection connection = new SqliteConnection($"Data Source={databaseFileName}");
            PlanetRepository galaxyRemoteControl = new PlanetRepository(connection);
            //
            while(true)
            {
                Write("> ");
                string input = ReadLine();
                if(input.StartsWith("getById"))
                {
                    string mayBeId = input.Split()[1];
                    int id = 0;
                    if(!int.TryParse(mayBeId, out id))
                    {
                        WriteLine(">> incorrect input.");
                        continue;
                    }
                    Planet p = galaxyRemoteControl.GetById(id);
                    if(p == null)
                    {
                        WriteLine("PLanet NOT found.");
                    }
                    else
                    {
                        WriteLine("PLanet found:\n" + p.ToString());
                    }
                }
                else if (input.StartsWith("deleteById"))
                {
                    string mayBeId = input.Split()[1];
                    int id = 0;
                    if(!int.TryParse(mayBeId, out id))
                    {
                        WriteLine(">> incorrect input.");
                        continue;
                    }
                    int nChanged = galaxyRemoteControl.DeleteById(id);
                    if (nChanged == 0)
                    {
                        WriteLine(">> Planet NOT deleted.");
                    }
                    else 
                    {
                        WriteLine(">> Planet deleted.");
                    }
                }
                else if (input.StartsWith("insert"))
                {
                    string csvRow = input.Split()[1];
                    Planet p = CSVRowToPlanet(csvRow);
                    if(p == null)
                    {
                        WriteLine(">> wrong num of cols."); // to change message!!!
                        continue;
                    }
                    int newId = galaxyRemoteControl.Insert(p);
                    if (newId == 0)
                    {
                        WriteLine(">> Planet NOT added.");
                    }
                    else 
                    {
                        WriteLine(">> Planet added. New id is: " + newId);
                    }
                }
                else if (input == "getTotalPages")
                {
                    int pages = galaxyRemoteControl.GetTotalPages();
                    WriteLine($">> There are {pages} pages."); // to change message!!!
                }
                else if (input.StartsWith("getPage"))
                {
                    string mayBePageN = input.Split()[1];
                    int n = 0;
                    if(!int.TryParse(mayBePageN, out n))
                    {
                        WriteLine(">> incorrect input.");
                        continue;
                    }
                    if(n <= 0 || n > galaxyRemoteControl.GetTotalPages())
                    {
                        WriteLine(">> There is no such page"); // to change message!!!
                        continue;
                    }
                    // smthn = galaxyRemoteControl.GetPage(n);
                }
                else if (input.StartsWith("export"))
                {
                    WriteLine("Скоро буде.(2)");
                }
                else if(input == "exit")
                {
                    break;
                }
                else
                {
                    WriteLine(">> incorrect input.");
                }
            }
        }

        static Planet CSVRowToPlanet (string s)
        {
            string[] colsInRow = s.Split(',');
            if(colsInRow.Length != 4)
            {
                return null;
            }
            Planet p1 = new Planet();
            p1.id = int.Parse(colsInRow[0]);
            p1.name = colsInRow[1];
            p1.size = double.Parse(colsInRow[2]);
            p1.color = colsInRow[3];
            return p1;
        }
    }
}
