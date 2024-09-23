using System.Text;

namespace CourseEnrollmentSystem
{
    internal class Program
    {
        static Dictionary<string , HashSet<string>> CoursesDictionary = new Dictionary<string, HashSet<string>>();
        static List<(string CourseName, string StudentName)> WaitList = new List<(string CourseName, string StudentName)>();
        static void Main(string[] args)
        {
            MainMenu();
        }

        static void MainMenu()
        {
            //AddNewCourse();
            //StringBuilder sb = new StringBuilder();
            //foreach (string Key in CoursesDictionary.Keys)
            //{
            //    sb.Append(Key);
            //}
            //Console.WriteLine("Courses: " + sb.ToString());
            //RemoveCourse();
            //sb.Clear();
            //foreach (string Key in CoursesDictionary.Keys)
            //{
            //    sb.Append(Key);
            //}
            //Console.WriteLine("Courses: " + sb.ToString());
            //EnrollStudent();
            //string CourseName = "math";
            //sb.Clear();
            //foreach (string value in CoursesDictionary[CourseName])
            //{
            //    sb.Append(value);
            //}
            //Console.WriteLine("Math: " + sb.ToString());
        }

        static void AddNewCourse()
        {
            do
            {
                string CourseCode;
                Console.WriteLine("\nEnter the new course ID, or type \"x\" to exit:\n");
                while (string.IsNullOrEmpty(CourseCode = Console.ReadLine().ToLower().Trim()) 
                    || CoursesDictionary.ContainsKey(CourseCode.ToLower().Trim()) 
                    || (CourseCode == "x"))
                {
                    if(CourseCode == "x")
                    {
                        return;
                    }
                    Console.Clear();
                    if (CoursesDictionary.ContainsKey(CourseCode))
                    {
                        Console.WriteLine("\nCourse already exists, please try again.\n");
                    }
                    else
                    {
                        Console.WriteLine("\nInvalid course ID, please try again.\n");
                    }
                    Console.WriteLine("\nEnter the new course ID:\n");
                }
                CoursesDictionary.Add(CourseCode, new HashSet<string>(){ });
            } while (true);
        }

        static void RemoveCourse()
        {
            do
            {
                string CourseToRemove;
                Console.WriteLine("\nEnter the course ID to remove, or type \"x\" to exit:\n");
                while (string.IsNullOrEmpty(CourseToRemove = Console.ReadLine().ToLower().Trim()) 
                    || (!CoursesDictionary.ContainsKey(CourseToRemove.ToLower().Trim()))
                    || (CoursesDictionary.TryGetValue(CourseToRemove, out _ )) 
                    || (CourseToRemove == "x"))
                {
                    Console.Clear();
                    if (CourseToRemove == "x")
                    {
                        return;
                    }
                    else if (!CoursesDictionary.ContainsKey(CourseToRemove))
                    {
                        Console.WriteLine("\nCourse does not exist, please try again.\n");
                    }
                    else if (string.IsNullOrEmpty(CourseToRemove.ToLower().Trim()))
                    {
                        Console.WriteLine("\nInvalid course ID, please try again.\n");
                    }
                    else
                    {
                        Console.WriteLine("\nCourse has enrolled students, please try again.\n");
                    }
                    Console.WriteLine("\nEnter the course ID to remove, or type \"x\" to exit:\n");
                }
                CoursesDictionary.Remove(CourseToRemove);
            } while(true);
        }

        static void EnrollStudent()
        {
            Console.WriteLine("Enter the student name to enroll:\n");
            string EnrollStudentName;
            while (string.IsNullOrEmpty(EnrollStudentName = Console.ReadLine()))
            {
                Console.Clear();
                Console.WriteLine("\nInvalid name, please try again.");
                Console.WriteLine("Enter the student name to enroll:\n");
            }
            Console.WriteLine("\nEnter the course ID to enroll in:\n");
            string EnrollCourse;
            while (string.IsNullOrEmpty(EnrollCourse = Console.ReadLine()) || (!CoursesDictionary.ContainsKey(EnrollCourse)))
            {
                Console.Clear();
                Console.WriteLine("\nInvalid name, please try again.");
                Console.WriteLine("Enter the Course ID to enroll the student:\n");
            }
            CoursesDictionary[EnrollCourse].Add(EnrollStudentName);
        }
    }
}
