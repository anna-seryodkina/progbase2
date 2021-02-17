using System;
using static System.Console;
using System.IO;

namespace s2_lab1
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

    class Program
    {
        static void Main(string[] args)
        {
            // ------------- Part 1 -------------
            if(args.Length != 2)
            {
                WriteLine("> error.");
                return;
            }
            int numberOfLinesCSV = 0; 
            if(int.TryParse(args[1], out numberOfLinesCSV) == false)
            {
                WriteLine("> error.");
                return;
            }
            if(numberOfLinesCSV < 0)
            {
                WriteLine("> error.");
                return;
            }
            string filePath = args[0];
            
            GenerateCSV(filePath, numberOfLinesCSV);
            // ------------- Part 2 -------------
            string path1 = "./firstInput.csv";
            GenerateCSV(path1, 20);
            string path2 = "./secondInput.csv";            
            GenerateCSV(path2, 200000);
            //
            ListPlanet list1 = new ListPlanet(); 
            list1 = ReadAllPlanets(path1);
            PrintListInfo(list1);
            ListPlanet list2 = new ListPlanet(); 
            list2 = ReadAllPlanets(path2);
            PrintListInfo(list2);
            ListPlanet coolList = new ListPlanet();
            coolList = FillMainList(list1, list2);
            //
            double listAverage = FindAverage(coolList); 
            coolList = DeleteSomething(coolList, listAverage);
            string path3 = "./outputPart2.csv";
            WriteAllPlanets(path3, coolList);
        }

        static ListPlanet DeleteSomething(ListPlanet mainList, double av)
        {
            for(int r = 0; r < mainList.Count; r++)
            {
                if(mainList[r].size < av)
                {
                    mainList.RemoveAt(r);
                    r-=1;
                }
            }
            return mainList;
        }

        static double FindAverage(ListPlanet planets)
        {
            double av = 0;
            double sum = 0;
            for(int h = 0; h < planets.Count; h++)
            {
                sum += planets[h].size;
            }
            av = sum / (planets.Count);
            return av;
        }

        static ListPlanet FillMainList(ListPlanet list1, ListPlanet list2)
        {
            ListPlanet mainList = new ListPlanet();
            bool[] usedIds = new bool [Math.Max(list1.Count, list2.Count)];
            for(int u = 0; u < list1.Count; u++)
            {
                if(usedIds[list1[u].id])
                {
                    continue;
                }
                mainList.Add(list1[u]);
                usedIds[list1[u].id] = true;            
            }
            
            for(int k = 0; k < list2.Count; k++)
            {
                if(usedIds[list2[k].id])
                {
                    continue;
                }                
                mainList.Add(list2[k]);
                usedIds[list2[k].id] = true;                
            }
            return mainList;
        }

        static void GenerateCSV (string filePath, int numberOfLinesCSV)
        {
            Random random = new Random();
            string[] adjective = new string[]
            {
                "happy", "boring", "super", "cool", "sad", "amazing", "lonely"
            };
            string[] noun = new string[]
            {
                "Apple", "Banana", "Mango", "Watermelon", "Pumpkin", "Tangerine", "Avocado"
            };
            string[] colors = new string[] 
            {
                "blue", "black", "white", "red", "green", "yellow", "pink", "purple", "orange"
            };


            StreamWriter sw = new StreamWriter(filePath);
            sw.WriteLine(nameof(Planet.id) + "," + nameof(Planet.name) + "," + nameof(Planet.size) + "," + nameof(Planet.color));
            while(true)
            {
                if(numberOfLinesCSV == 0)
                {
                    break;
                }
                
                int newId = random.Next(1, numberOfLinesCSV);

                string newName = adjective[random.Next(0, adjective.Length)] + noun[random.Next(0, noun.Length)];
                double newSize = Math.Round(random.Next(100, 500) + random.NextDouble(), 3);
                int forColor = random.Next(0, colors.Length);
                string newColor = colors[forColor];

                Planet randomP = new Planet(newId, newName, newSize, newColor);
                string rPlanet = PlanetToCSV (randomP);
                sw.WriteLine(rPlanet);
                numberOfLinesCSV-=1;
            }
            
            sw.Close();
        }

        static void PrintListInfo(ListPlanet list)
        {
            WriteLine($"> size: {list.Count}");
            for(int i = 0; i < 10; i++)
            {
                WriteLine(list[i].ToString());
            }
        }

        static ListPlanet ReadAllPlanets(string path)
        {
            ListPlanet list1 = new ListPlanet(); 
            StreamReader sr = new StreamReader(path); 
            string s = "";
            while (true)
            {
                s = sr.ReadLine();
                if (s == null)
                {
                    break;
                }

                Planet p1 = CSVRowToPlanet(s);
                if(p1.id == 0)
                {
                    continue;
                }
                list1.Add(p1);
            }
            sr.Close();
            return list1;
        }

        static Planet CSVRowToPlanet (string s)
        {
            string[] colsInRow = s.Split(',');
            if(colsInRow.Length != 4)
            {
                Exception ex = new Exception("incorrect format in csv row");
                throw ex;
            }
            int forPlanetsId = 0;
            if(!int.TryParse(colsInRow[0], out forPlanetsId))
            {
                return new Planet();
            }
            Planet p1 = new Planet();
            p1.id = forPlanetsId;
            p1.name = colsInRow[1];
            p1.size = double.Parse(colsInRow[2]);
            p1.color = colsInRow[3];
            return p1;
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
    }
}