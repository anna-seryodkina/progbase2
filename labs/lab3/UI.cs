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
                    try
                    {
                        ProcessAdd(input, a, b, logger);
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex.Message);
                    }
                }
                else if(input.Contains("contains"))
                {
                    try
                    {
                        ProcessContains(input, a, b, logger);
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex.Message);
                    }
                }
                else if(input.Contains("remove"))
                {
                    try
                    {
                        ProcessRemove(input, a, b, logger);
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex.Message);
                    }
                }
                else if(input.Contains("clear"))
                {
                    try
                    {
                        ProcessClear(input, a, b, logger);
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex.Message);
                    }
                }
                else if(input.Contains("log"))
                {
                    try
                    {
                        ProcessLog(input, a, b, logger);
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex.Message);
                    }
                }
                else if(input.Contains("count"))
                {
                    try
                    {
                        ProcessCount(input, a, b, logger);
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex.Message);
                    }
                }
                else if(input.Contains("read")) 
                {
                    try
                    {
                        ProcessRead(input, a, b, logger);
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex.Message);
                    }
                }
                else if(input.Contains("write"))
                {
                    try
                    {
                        ProcessWrite(input, a, b, logger);
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex.Message);
                    }
                }
                else if(input == "Overlaps")
                {
                    try
                    {
                        bool toBeOrNotToBe = a.Overlaps(b);
                        logger.Log($"{toBeOrNotToBe}.");
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex.Message);
                    }
                }
                else if (input == "SymmetricExceptWith")
                {
                    try
                    {
                        ProcessSymmetricExceptWith(input, a, b, logger);
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex.Message);
                    }
                }
                else
                {
                    logger.LogError("incorrect input.");
                }
            }
        }

        static void ProcessAdd(string input, ISetInt a, ISetInt b, ILogger logger)
        {
            string[] inputParts = input.Split();
            int number;
            if(inputParts[0] != "a" && inputParts[0] != "b")
            {
                logger.LogError("incorrect set.");
            }
            else if (!int.TryParse(inputParts[2], out number))
            {
                logger.LogError("value should be a number.");
            }
            else
            {
                bool added;
                if(inputParts[0] == "a")
                {
                    added = a.Add(number);
                }
                else
                {
                    added = b.Add(number);
                }
                //
                if(added)
                {
                    logger.Log($"[{number}] was added to the set.");
                }
                else
                {
                    logger.Log($"[{number}] NOT added.");
                }
            }
        }

        static void ProcessContains(string input, ISetInt a, ISetInt b, ILogger logger)
        {
            string[] inputParts = input.Split();
            int number;
            if(inputParts[0] != "a" && inputParts[0] != "b")
            {
                logger.LogError("incorrect set.");
            }
            else if (!int.TryParse(inputParts[2], out number))
            {
                logger.LogError("value should be a number.");
            }
            else
            {
                bool contains;
                if(inputParts[0] == "a")
                {
                    contains = a.Contains(number);
                }
                else
                {
                    contains = b.Contains(number);
                }
                //
                if(contains)
                {
                    logger.Log($"set '{inputParts[0]}' contains [{number}].");
                }
                else
                {
                    logger.Log($"[{number}] NOT found.");
                }
            }
        }

        static void ProcessRemove(string input, ISetInt a, ISetInt b, ILogger logger)
        {
            string[] inputParts = input.Split();
            int number;
            if(inputParts[0] != "a" && inputParts[0] != "b")
            {
                logger.LogError("incorrect set.");
            }
            else if (!int.TryParse(inputParts[2], out number))
            {
                logger.LogError("value should be a number.");
            }
            else
            {
                bool removed;
                if(inputParts[0] == "a")
                {
                    removed = a.Remove(number);
                }
                else
                {
                    removed = b.Remove(number);
                }
                //
                if(removed)
                {
                    logger.Log($"[{number}] was removed from set '{inputParts[0]}'.");
                }
                else
                {
                    logger.Log($"[{number}] NOT removed.");
                }
            }
        }

        static void ProcessClear(string input, ISetInt a, ISetInt b, ILogger logger)
        {
            string[] inputParts = input.Split();
            if(inputParts[0] != "a" && inputParts[0] != "b")
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
            if(inputParts[0] != "a" && inputParts[0] != "b")
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
                logger.Log(CreateOutputString(array));
            }
        }

        static void ProcessCount(string input, ISetInt a, ISetInt b, ILogger logger)
        {
            string[] inputParts = input.Split();
            if(inputParts[0] != "a" && inputParts[0] != "b")
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
            if(inputParts[0] != "a" && inputParts[0] != "b")
            {
                logger.LogError("incorrect set.");
            }
            else
            {
                string path = inputParts[2];
                if(inputParts[0] == "a")
                {
                    a = ReadWrite.ReadSet(path, a);
                }
                else
                {
                    b = ReadWrite.ReadSet(path, b);
                }
            }
        }

        static void ProcessWrite(string input, ISetInt a, ISetInt b, ILogger logger)
        {
            string[] inputParts = input.Split();
            if(inputParts[0] != "a" && inputParts[0] != "b")
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

        static string CreateOutputString(int[] array)
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < array.Length; i++)
            {
                sb.Append($"[{array[i]}] ");
            }
            return sb.ToString();
        }

        static void ProcessSymmetricExceptWith (string input, ISetInt a, ISetInt b, ILogger logger)
        {
            a.SymmetricExceptWith(b);
            int[] array = new int[a.GetCount];
            a.CopyTo(array);
            logger.Log(CreateOutputString(array));
        }

    }
}