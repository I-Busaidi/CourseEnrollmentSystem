namespace CourseEnrollmentSystem
{
    internal class Program
    {
        static Dictionary<string, HashSet<string>?> CoursesDictionary = new Dictionary<string, HashSet<string>?>();
        static List<(string CourseName, string StudentName)> WaitList = new List<(string CourseName, string StudentName)>();
        static void Main(string[] args)
        {
            MainMenu();
        }

        static void MainMenu()
        {
            AddNewCourse();
        }

        static void AddNewCourse()
        {
            string CourseCode;
            while (string.IsNullOrEmpty(CourseCode = Console.ReadLine()) || CoursesDictionary.ContainsKey(CourseCode))
            {
                if (CoursesDictionary.ContainsKey(CourseCode))
                {
                    Console.WriteLine("\nCourse already exists, please try again:\n");
                }
                else
                {
                    Console.WriteLine("\nInvalid course ID, please try again:\n");
                }
            }

            CoursesDictionary.Add(CourseCode, null);
        }
    }
}
