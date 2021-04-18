using System.Collections.Generic;

namespace ConsoleProject
{
    class XmlDataProcessor
    {
        private static double FindMaxUnits(Root root)
        {
            double max = -1.0;
            foreach(Course course in root.courses)
            {
                if(course.units > max)
                {
                    max = course.units;
                }
            }
            return max;
        }

        public static List<Course> GetFirstN(Root root, int n)
        {
            List<Course> nCourses = new List<Course>();
            double maxUnits = FindMaxUnits(root);
            foreach(Course course in root.courses)
            {
                if(course.units == maxUnits && nCourses.Count != n)
                {
                    nCourses.Add(course);
                }
            }
            return nCourses;
        }

        public static List<Course> GetPage(Root root, int pageNum, int pageSize)
        {
            List<Course> page = new List<Course>();
            int startIndex = (pageNum-1)*pageSize;
            for(int i = 0; i < pageSize; i++)
            {
                page.Add(root.courses[startIndex]);
                startIndex++;
            }
            return page;
        }

        public static List<string> GetUniqueSubjList(Root root)
        {
            HashSet<string> subjects = new HashSet<string>();
            List<string> subjList = new List<string>();
            foreach(Course course in root.courses) 
            {
                if(subjects.Add(course.subj))
                {
                    subjList.Add(course.subj);
                }
            }
            return subjList;
        }

        public static List<string> GetUniqueInstructorsList(Root root)
        {
            HashSet<string> set = new HashSet<string>();
            List<string> instructorsList = new List<string>();
            foreach(Course course in root.courses)
            {
                if(set.Add(course.instructor))
                {
                    instructorsList.Add(course.instructor);
                }
            }
            return instructorsList;
        }

        public static List<string> GetTitlesList(Root root, string subj)
        {
            List<string> titlesList = new List<string>();
            foreach(Course course in root.courses)
            {
                if(course.subj == subj)
                {
                    titlesList.Add(course.title);
                }
            }
            return titlesList;
        }

        public static double CountSubjUnits(Root root, string subj)
        {
            double units = 0;
            foreach(Course course in root.courses)
            {
                if(course.subj == subj)
                {
                    units += course.units;
                }
            }
            return units;
        }
    }
}
