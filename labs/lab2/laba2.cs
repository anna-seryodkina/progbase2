using System;
using static System.Console;
using Microsoft.Data.Sqlite;
using System.IO;


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
        public PlanetRepository(SqliteConnection connection)
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
                INSERT INTO planets (name, size, color) 
                VALUES ($name, $size, $color);
            
                SELECT last_insert_rowid();
            ";
            
            command.Parameters.AddWithValue("$name", planet.name);
            command.Parameters.AddWithValue("$size", planet.size);
            command.Parameters.AddWithValue("$color", planet.color);
            
            long newId = (long)command.ExecuteScalar();
            
            connection.Close();
            return (int)newId;
        }

        public long GetTotalPages()
        {
            connection.Open();

            SqliteCommand command = connection.CreateCommand();
            command.CommandText = 
            @"
                SELECT COUNT(*) FROM planets;
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

        public ListPlanet GetPage(int pageNumber) 
        {
            ListPlanet plList = new ListPlanet();
            //
            connection.Open();
            //
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = 
            @"
                SELECT * FROM planets LIMIT 10 OFFSET ($pageN-1)*10;
            ";
            command.Parameters.AddWithValue("$pageN", pageNumber);

            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {     
                Planet p = new Planet();
                p.id = int.Parse(reader.GetString(0));
                p.name = reader.GetString(1);
                p.size = Convert.ToDouble(reader.GetString(2));
                p.color = reader.GetString(3);
                plList.Add(p);
            }
            reader.Close();
            //
            connection.Close();
            return plList;
        }

        public ListPlanet GetExport(int valueX) 
        {
            ListPlanet plList = new ListPlanet();
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM planets WHERE id > $valueX";
            command.Parameters.AddWithValue("$valueX", valueX);

            SqliteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Planet p = new Planet();
                p.id = int.Parse(reader.GetString(0));
                p.name = reader.GetString(1);
                p.size = Convert.ToDouble(reader.GetString(2));
                p.color = reader.GetString(3);
                plList.Add(p);
            }
            reader.Close();
            connection.Close();

            return plList;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string databaseFileName = "galaxy.db";
            SqliteConnection connection = new SqliteConnection($"Data Source={databaseFileName}");
            PlanetRepository galaxyRemoteControl = new PlanetRepository(connection);
            //
            while(true)
            {
                Write("> ");
                string input = ReadLine();
                if(input.StartsWith("getById"))
                {
                    if(GetByIdCommand(input, galaxyRemoteControl) == 0)
                    {
                        continue;
                    }
                }
                else if (input.StartsWith("deleteById"))
                {
                    if(DeleteByIdCommand(input, galaxyRemoteControl) == 0)
                    {
                        continue;
                    }
                }
                else if (input.StartsWith("insert"))
                {
                    if(InsertCommand(input, galaxyRemoteControl) == 0)
                    {
                        continue;
                    }
                }
                else if (input == "getTotalPages")
                {
                    long pages = galaxyRemoteControl.GetTotalPages();
                    WriteLine($">> There are {pages} pages.");
                }
                else if (input.StartsWith("getPage"))
                {
                    if(GetPageCommand(input, galaxyRemoteControl) == 0)
                    {
                        continue;
                    }
                }
                else if (input.StartsWith("export"))
                {
                    if(ExportCommand(input, galaxyRemoteControl) == 0)
                    {
                        continue;
                    }
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

        static int GetByIdCommand(string input, PlanetRepository grc)
        {
            if(input.Split().Length != 2)
            {
                WriteLine(">> incorrect input. (some parts of the command are missed)");
                return 0;
            }
            string mayBeId = input.Split()[1];
            int id = 0;
            if(!int.TryParse(mayBeId, out id))
            {
                WriteLine(">> incorrect input.");
                return 0;
            }
            Planet p = grc.GetById(id);
            if(p == null)
            {
                WriteLine("PLanet NOT found.");
            }
            else
            {
                WriteLine("PLanet found:\n" + p.ToString());
            }
            return 1;
        }

        static int DeleteByIdCommand(string input, PlanetRepository grc)
        {
            if(input.Split().Length != 2)
            {
                WriteLine(">> incorrect input. (some parts of the command are missed)");
                return 0;
            }
            string mayBeId = input.Split()[1];
            int id = 0;
            if(!int.TryParse(mayBeId, out id))
            {
                WriteLine(">> incorrect input.");
                return 0;
            }
            int nChanged = grc.DeleteById(id);
            if (nChanged == 0)
            {
                WriteLine(">> Planet NOT deleted.");
            }
            else 
            {
                WriteLine(">> Planet deleted.");
            }
            return 1;
            throw new NotImplementedException();
        }

        static int InsertCommand(string input, PlanetRepository grc)
        {
            if(input.Split().Length != 2)
            {
                WriteLine(">> incorrect input. (some parts of the command are missed)"); 
                return 0;
            }
            string csvRow = input.Split()[1];
            Planet p = CSVRowToPlanet(csvRow);
            if(p == null)
            {
                WriteLine(">> incorrect input.");
                return 0;
            }
            int newId = grc.Insert(p);
            if (newId == 0)
            {
                WriteLine(">> Planet NOT added.");
            }
            else 
            {
                WriteLine(">> Planet added. New id: " + newId);
            }
            return 1;
        }

        static int GetPageCommand(string input, PlanetRepository grc)
        {
            if(input.Split().Length != 2)
            {
                WriteLine(">> incorrect input. (some parts of the command are missed)");
                return 0;
            }
            string mayBePageN = input.Split()[1];
            int n = 0;
            if(!int.TryParse(mayBePageN, out n))
            {
                WriteLine(">> incorrect input.");
                return 0;
            }
            if(n <= 0 || n > grc.GetTotalPages())
            {
                WriteLine(">> There is no such page");
                return 0;
            }
            //
            ListPlanet planetList = grc.GetPage(n);
            PrintListInfo(planetList);
            return 1;
        }

        static int ExportCommand(string input, PlanetRepository grc)
        {
            if(input.Split().Length != 2)
            {
                WriteLine(">> incorrect input. (some parts of the command are missed)");
                return 0;
            }
            string mayBeV = input.Split()[1];
            int valueX = 0;
            if(!int.TryParse(mayBeV, out valueX))
            {
                WriteLine(">> incorrect input.");
                return 0;
            }
            //
            ListPlanet list = grc.GetExport(valueX);
            string path = "./export.csv";
            WriteAllPlanets(path, list);
            WriteLine($"File name: 'export.csv'; number of lines: {list.Count}");
            return 1;
        }

        static void WriteAllPlanets(string path, ListPlanet planets)
        {
            StreamWriter sw = new StreamWriter(path);
            string s = ""; 
            int i = 0;
            while (true)
            {
                if (i == planets.Count)
                {
                    break;
                }
                s = PlanetToCSV (planets[i]);
                sw.WriteLine(s);
                i++;
            }
            sw.Close();
        }

        static string PlanetToCSV (Planet p1)
        {
            string[] colsInRow = new string[4];
            colsInRow[0] = Convert.ToString(p1.id);
            colsInRow[1] = p1.name;
            colsInRow[2] = Convert.ToString(p1.size);
            colsInRow[3] = p1.color;
            string csvRow = String.Join(',', colsInRow);
            return csvRow;
        }

        static Planet CSVRowToPlanet (string s)
        {
            string[] colsInRow = s.Split(',');
            if(colsInRow.Length != 3)
            {
                return null;
            }
            Planet p1 = new Planet();
            p1.name = colsInRow[0];
            string forSize = colsInRow[1];
            double newSize;
            if(!double.TryParse(forSize, out newSize))
            {
                return null;
            }
            p1.size = newSize;
            p1.color = colsInRow[2];
            return p1;
        }

        static void PrintListInfo(ListPlanet list)
        {
            for(int i = 0; i < list.Count; i++)
            {
                WriteLine(list[i].ToString());
            }
        }
    }
}
