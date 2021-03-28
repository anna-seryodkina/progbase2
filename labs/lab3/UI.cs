using System;
using System.Text;
using static System.Console;
namespace lab3
{
    public class UI
    {
        public static void ProcessSets(ILogger logger)
        {
            ISetInt a = new ArraySetInt();
            ISetInt b = new ArraySetInt();
            while(true)
            {
                Write("> ");
                string input = ReadLine();
                if(input == "")
                {
                    break;
                }
                if (input == "exit")
                {
                    break;
                }
                //
                if(input.Contains("add"))
                {
                    ProcessAdd(input, a, b, logger);
                }
                else if(input.Contains("contains"))
                {
                    ProcessContains(input, a, b, logger);
                }
                else if(input.Contains("remove"))
                {
                    ProcessRemove(input, a, b, logger);
                }
                else if(input.Contains("clear"))
                {
                    ProcessClear(input, a, b, logger);
                }
                else if(input.Contains("log"))
                {
                    ProcessLog(input, a, b, logger);
                }
                else if(input.Contains("count"))
                {
                    ProcessCount(input, a, b, logger);
                }
                else if(input.Contains("read"))
                {
                    ProcessRead(input, a, b, logger);
                }
                else if(input.Contains("write"))
                {
                    ProcessWrite(input, a, b, logger);
                }
                else if(input == "Overlaps")
                {
                    ProcessOverlaps(input, a, b, logger);
                }
                else if (input == "SymmetricExceptWith")
                {
                    ProcessSymmetricExceptWith(input, a, b, logger);
                }
                else
                {
                    logger.LogError("incorrect input.");
                }
                break;
            }
        }

        static void ProcessAdd(string input, ISetInt a, ISetInt b, ILogger logger) // add try catch ???
        {
            string[] inputParts = input.Split();
            int number;
            if(inputParts[0] != "a" || inputParts[0] != "b")
            {
                logger.LogError("incorrect set.");
            }
            else if (!int.TryParse(inputParts[2], out number))
            {
                logger.LogError("value should be a number.");
            }
            else
            {
                if(inputParts[0] == "a")
                {
                    logger.Log($"add to set a. result: {a.Add(number).ToString()}");
                }
                else
                {
                    logger.Log($"add to set b. result: {b.Add(number).ToString()}");
                }
            }
        }

        static void ProcessContains(string input, ISetInt a, ISetInt b, ILogger logger)
        {
            string[] inputParts = input.Split();
            int number;
            if(inputParts[0] != "a" || inputParts[0] != "b")
            {
                logger.LogError("incorrect set.");
            }
            else if (!int.TryParse(inputParts[2], out number))
            {
                logger.LogError("value should be a number.");
            }
            else
            {
                if(inputParts[0] == "a")
                {
                    logger.Log($"search in A. result: {a.Contains(number)}");
                }
                else
                {
                    logger.Log($"search in B. result: {b.Contains(number)}");
                }
            }
        }

        static void ProcessRemove(string input, ISetInt a, ISetInt b, ILogger logger)
        {
            string[] inputParts = input.Split();
            int number;
            if(inputParts[0] != "a" || inputParts[0] != "b")
            {
                logger.LogError("incorrect set.");
            }
            else if (!int.TryParse(inputParts[2], out number))
            {
                logger.LogError("value should be a number.");
            }
            else
            {
                if(inputParts[0] == "a")
                {
                    logger.Log($"remove from A. result: {a.Remove(number)}");
                }
                else
                {
                    logger.Log($"remove from B. result: {b.Remove(number)}");
                }
            }
        }

        static void ProcessClear(string input, ISetInt a, ISetInt b, ILogger logger)
        {
            string[] inputParts = input.Split();
            if(inputParts[0] != "a" || inputParts[0] != "b")
            {
                logger.LogError("incorrect set.");
            }
            else
            {
                if(inputParts[0] == "a")
                {
                    a.Clear();
                }
                else
                {
                    b.Clear();
                }
            }
        }

        static void ProcessLog(string input, ISetInt a, ISetInt b, ILogger logger)
        {
            string[] inputParts = input.Split();
            if(inputParts[0] != "a" || inputParts[0] != "b")
            {
                logger.LogError("incorrect set.");
            }
            else
            {
                int[] array;
                if(inputParts[0] == "a")
                {
                    array = new int[a.GetCount];
                    a.CopyTo(array);
                }
                else
                {
                    array = new int[b.GetCount];
                    b.CopyTo(array);
                }
                StringBuilder sb = new StringBuilder();
                for(int i = 0; i < array.Length; i++)
                {
                    sb.Append(array[i]);
                    sb.Append(" ");
                }
                logger.Log(sb.ToString());
            }
        }

        static void ProcessCount(string input, ISetInt a, ISetInt b, ILogger logger)
        {
            string[] inputParts = input.Split();
            if(inputParts[0] != "a" || inputParts[0] != "b")
            {
                logger.LogError("incorrect set.");
            }
            else
            {
                if(inputParts[0] == "a")
                {
                    logger.Log($"there are {a.GetCount} elements in A.");
                }
                else
                {
                    logger.Log($"there are {b.GetCount} elements in B.");
                }
            }
        }

        static void ProcessRead(string input, ISetInt a, ISetInt b, ILogger logger)
        {
            string[] inputParts = input.Split();
            if(inputParts[0] != "a" || inputParts[0] != "b")
            {
                logger.LogError("incorrect set.");
            }
            else
            {
                string path = inputParts[2];
                if(inputParts[0] == "a")
                {
                    a = ReadWrite.ReadSet(path);
                }
                else
                {
                    b = ReadWrite.ReadSet(path);
                }
            }
        }

        static void ProcessWrite(string input, ISetInt a, ISetInt b, ILogger logger)
        {
            string[] inputParts = input.Split();
            if(inputParts[0] != "a" || inputParts[0] != "b")
            {
                logger.LogError("incorrect set.");
            }
            else
            {
                string path = inputParts[2];
                if(inputParts[0] == "a")
                {
                    ReadWrite.WriteSet(path, a);
                }
                else
                {
                    ReadWrite.WriteSet(path, b);
                }
            }
        }

        static void ProcessOverlaps (string input, ISetInt a, ISetInt b, ILogger logger)
        {
            logger.Log($"overlaps happened here. result: {a.Overlaps(b)} :D");
        }

        static void ProcessSymmetricExceptWith (string input, ISetInt a, ISetInt b, ILogger logger)
        {
            a.SymmetricExceptWith(b);
            WriteLine("SymmetricExceptWith happened here. :D");
        }

    }
}