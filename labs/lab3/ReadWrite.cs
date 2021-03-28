using System.IO;

namespace lab3
{
    static class ReadWrite
    {
        public static ISetInt ReadSet(string filePath)
        {
            ISetInt set = new ArraySetInt();
            StreamReader sr = new StreamReader(filePath);
            string s = "";
            while (true)
            {
                s = sr.ReadLine();
                if (s == null)
                {
                    break;
                }
                int number = int.Parse(s);
                set.Add(number);
            }
            sr.Close();
            return set;
        }

        public static void WriteSet(string filePath, ISetInt set)
        {
            int[] array = new int[set.GetCount];
            set.CopyTo(array);
            StreamWriter sw = new StreamWriter(filePath);
            for(int i = 0; i < array.Length; i++)
            {
                sw.WriteLine(array[i]);
            }
            sw.Close();
        }
    }
}