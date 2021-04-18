using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ConsoleProject
{
    static class ConsoleUserInterface
    {
        public static void Run()
        {
            Root root = new Root();
            while(true)
            {
                Console.Write("> ");
                string command = Console.ReadLine();
                //
                if(command == "exit" || command == "")
                {
                    break;
                }
                root = ProcessCommand(command, root);
            }
        }

        private static Root ProcessLoad(string command)
        {
            string[] commandParts = command.Split();
            if(commandParts.Length != 2)
            {
                throw new Exception(">> incorrect input.");
            }
            string filename = commandParts[1];
            FileExists(filename);
            //
            return MySerializer.Deserialize(filename);
        }

        private static void ProcessPrint(string command, Root root)
        {
            string[] commandParts = command.Split();
            if(commandParts.Length != 2)
            {
                throw new Exception(">> incorrect input.");
            }
            int pageNum;
            if(!int.TryParse(commandParts[1], out pageNum))
            {
                throw new Exception($">> invalid page number: {commandParts[1]}.");
            }
            int pageSize = 10;
            Console.WriteLine($">> page size: {pageSize}");
            //
            int pages = 0;
            // count pages
            if(root.courses.Count % pageSize != 0)
            {
                pages = (root.courses.Count / pageSize) + 1;
            }
            else
            {
                pages = root.courses.Count / pageSize;
            }
            Console.WriteLine($">> number of pages: {pages}");
            //
            List<Course> page = XmlDataProcessor.GetPage(root, pageNum, pageSize);
            Console.WriteLine($">> page {pageNum}:");
            PrintCoursesList(page);
        }

        private static void FileExists(string filename)
        {
            if(!System.IO.File.Exists(filename))
            {
                throw new Exception($"File does not exist: {filename}");
            }
        }

        private static void ProcessSave(string command, Root root)
        {
            string[] commandParts = command.Split();
            if(commandParts.Length != 2)
            {
                throw new Exception(">> incorrect input.");
            }
            string filename = commandParts[1];
            MySerializer.Serialize(filename, root);
        }

        private static void ProcessExport(string command, Root root)
        {
            string[] commandParts = command.Split();
            if(commandParts.Length != 3)
            {
                throw new Exception(">> incorrect input.");
            }
            int n;
            string maybeN = commandParts[1];
            if(!int.TryParse(maybeN, out n))
            {
                throw new Exception(">> incorrect input. (N)");
            }
            string filename = commandParts[2];
            FileExists(filename);
            //
            List<Course> nCourses = XmlDataProcessor.GetFirstN(root, n);
            Root nRoot = new Root()
            {
                courses = nCourses,
            };
            MySerializer.Serialize(filename, nRoot);
        }

        private static void ProcessInstructors(Root root)
        {
            List<string> instructorsList = XmlDataProcessor.GetUniqueInstructorsList(root);
            PrintList(instructorsList);
        }

        private static void ProcessSubjects(Root root)
        {
            List<string> subjects = XmlDataProcessor.GetUniqueSubjList(root);
            PrintList(subjects);
        }

        private static void PrintList(List<string> list)
        {
            foreach(string item in list)
            {
                Console.WriteLine($"[{item}]");
            }
        }

        private static void PrintCoursesList(List<Course> list)
        {
            foreach(Course item in list)
            {
                Console.WriteLine($"[{item}]"); // TO DO: normal output
            }
        }

        private static void ProcessSubject(string command, Root root)
        {
            string[] commandParts = command.Split();
            if(commandParts.Length != 2)
            {
                throw new Exception(">> incorrect input.");
            }
            string subj = commandParts[1];
            List<string> titlesList = XmlDataProcessor.GetTitlesList(root, subj);
            PrintList(titlesList);
        }

        private static void ProcessImage(string command, Root root)
        {
            string[] commandParts = command.Split();
            if(commandParts.Length != 2)
            {
                throw new Exception(">> incorrect input.");
            }
            string filename = commandParts[1];
            FileExists(filename);
            PlotMaker.CreateGraph(root, filename);
        }

        private static Root ProcessCommand(string command, Root root)
        {
            if(command.StartsWith("load"))
            {
                root = ProcessLoad(command);
            }
            else if(command.StartsWith("print"))
            {
                ProcessPrint(command, root);
            }
            else if(command.StartsWith("save"))
            {
                ProcessSave(command, root);
            }
            else if(command.StartsWith("export"))
            {
                ProcessExport(command, root);
            }
            else if(command == "subjects")
            {
                ProcessSubjects(root);
            }
            else if(command.StartsWith("subject"))
            {
                ProcessSubject(command, root);
            }
            else if(command == "instructors")
            {
                ProcessInstructors(root);
            }
            else if(command.StartsWith("image"))
            {
                ProcessImage(command, root);
            }
            else
            {
                throw new Exception($">> not supported command: [{command}]");
                // OR: Console.WriteLine($">> not supported command: {command}"); return;
            }
            return root;
        }
    }
}