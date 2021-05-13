using System;
using System.Collections.Generic;
using System.Linq;

namespace lab5
{
    public class DataProcessor
    {
        public List<Course> courses;
        public DataProcessor(List<Course> courses)
        {
            this.courses = courses;
        }

        public List<Course> SortByEnrolled()
        {
            var sortedCourses = courses.OrderByDescending(c => c.enrolled);
            return sortedCourses.ToList();
        }

        public int GetTotalPages()
        {
            const int pageSize = 10;
            return (int)Math.Ceiling(courses.Count / (double)pageSize);
        }

        public List<Course> GetPage(int pageNumber)
        {
            List<Course> coursesOnThePage = new List<Course>();
            int firstCourseIndex = pageNumber * 10 - 10;
            int lastCourseIndex = pageNumber == GetTotalPages() ? courses.Count - 1 : (firstCourseIndex + 9);
            for (int i = firstCourseIndex; i <= lastCourseIndex; i++)
            {
                coursesOnThePage.Add(courses[i]);
            }

            return coursesOnThePage;
        }

        public HashSet<string> GetUniquePrefixes()
        {
            HashSet<string> uniquePrefixes = new HashSet<string>();
            foreach (Course course in courses)
            {
                if (course.prefix != "")
                {
                    uniquePrefixes.Add(course.prefix);
                }
            }
            return uniquePrefixes;
        }

        public HashSet<string> GetCourseTitlesFromTheBuilding(string building)
        {
            HashSet<string> titles = new HashSet<string>();
            foreach (Course course in courses)
            {
                if (course.place.building == building && course.place.building != "")
                {
                    titles.Add(course.title);
                }
            }
            return titles;
        }

        public HashSet<string> GetInstructors()
        {
            HashSet<string> instructors = new HashSet<string>();
            foreach (Course course in courses)
            {
                if (course.instructor != "")
                {
                    instructors.Add(course.instructor);
                }
            }
            return instructors;
        }
    }
}
