using System.Text;
using System.Collections.Generic;
using System.Transactions;
namespace CourseEnrollmentSystem
{
    internal class Program
    {
        static Dictionary<string , HashSet<string>> CoursesDictionary = new Dictionary<string, HashSet<string>>();
        static Dictionary<string, int> CourseSeats = new Dictionary<string, int>();
        static List<(string CourseName, string StudentName)> WaitList = new List<(string CourseName, string StudentName)>();
        static void Main(string[] args)
        {
            InitializeStartupData();
            MainMenu();
        }

        static void MainMenu()
        {
            string Menu = "Welcome to Codeline University" +
                "\nSelect an option:" +
                "\n1. Add New Course." +
                "\n2. Remove a Course." +
                "\n3. Enroll a Student to a Course." +
                "\n4. Remove a Student From a Course." +
                "\n5. Display Students in a Specified Course." +
                "\n6. Display Students in All Courses." +
                "\n7. Display Common Students Between Two Courses." +
                "\n8. Withdraw a Student From All Courses." +
                "\n9. View Waiting List." +
                "\n\n0. Exit.";
            bool ExitApp  = false;
            do
            {
                Console.Clear();
                Console.WriteLine(Menu);
                int Choice;
                while ((!int.TryParse(Console.ReadLine(), out Choice)) || (Choice > 9) || (Choice < 0))
                {
                    Console.Clear();
                    Console.WriteLine(Menu);
                    Console.WriteLine("\nInvalid input, please try again.\n");
                }
                Console.Clear();
                switch (Choice)
                {
                    case 0:
                        Console.WriteLine("Thank you for using our system.");
                        ExitApp  = true;
                        break;

                    case 1:
                        AddNewCourse();
                        break;

                    case 2:
                        RemoveCourse();
                        break;

                    case 3:
                        EnrollStudent();
                        break;

                    case 4:
                        RemoveStudentFromCourse();
                        break;

                    case 5:
                        DisplayStudentsInCourse();
                        break;

                    case 6:
                        DisplayStudentsInAllCourses();
                        break;

                    case 7:
                        CommonStudentsInCourses();
                        break;

                    case 8:
                        WithdrawStudentFromAllCourses();
                        break;

                    case 9:
                        ViewWaitingList();
                        break;

                    default:
                        Console.WriteLine("\nInvalid input, please try again.");
                        break;
                }
                Console.WriteLine("\n\nPress any key to continue...");
                Console.ReadKey();
            } 
            while (!ExitApp);
        }

        static void AddNewCourse()
        {
            do
            {
                int CourseSeatNumber;
                string CourseCode;
                DisplayStudentsInAllCourses();
                Console.WriteLine("\nEnter the new course ID, or type \"x\" to exit:\n");
                while (string.IsNullOrEmpty(CourseCode = Console.ReadLine().ToUpper()) 
                    || CoursesDictionary.ContainsKey(CourseCode) 
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
                Console.Clear();
                Console.WriteLine($"Enter the number of seats available for {CourseCode} (1 to 30):\n");
                while ((!int.TryParse(Console.ReadLine(), out CourseSeatNumber)) || (CourseSeatNumber < 1) || (CourseSeatNumber > 30))
                {
                    Console.Clear();
                    Console.WriteLine($"Enter the number of seats available for {CourseCode} (1 to 30):\n");
                    if ((CourseSeatNumber < 1) || (CourseSeatNumber > 30))
                    {
                        Console.WriteLine("Number of Seats must be from 1 to 30, please try again: \n");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input, please try again:\n");
                    }
                }
                CourseSeats.Add(CourseCode, CourseSeatNumber);
                CoursesDictionary.Add(CourseCode, new HashSet<string>(){});
                Console.Clear();
                Console.WriteLine($"\nCourse {CourseCode} with {CourseSeatNumber} seats available added successfully.");
            } while (true);
        }

        static void RemoveCourse()
        {
            do
            {
                DisplayStudentsInAllCourses();
                string CourseToRemove;
                Console.WriteLine("\nEnter the course ID to remove, or type \"x\" to exit:\n");
                while (string.IsNullOrEmpty(CourseToRemove = Console.ReadLine().ToUpper()) 
                    || (!CoursesDictionary.ContainsKey(CourseToRemove.ToUpper()))
                    || (CoursesDictionary[CourseToRemove].Count > 0) 
                    || (CourseToRemove == "X"))
                {
                    Console.Clear();
                    if (CourseToRemove == "X")
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
                    DisplayStudentsInAllCourses();
                    Console.WriteLine("\nEnter the course ID to remove, or type \"x\" to exit:\n");
                }
                Console.WriteLine($"\nYou are removing the course {CourseToRemove}, Confirm? (1)Yes / (2)No");
                int RemoveConf;
                while((!int.TryParse(Console.ReadLine(), out RemoveConf)) 
                    || (RemoveConf > 2) 
                    || (RemoveConf < 1))
                {
                    Console.Clear();
                    Console.WriteLine($"\nYou are removing the course {CourseToRemove}, Confirm? (1)Yes / (2)No");
                    Console.WriteLine("\nInvalid input, please try again:\n");
                }
                if (RemoveConf == 1)
                {
                    CourseSeats.Remove(CourseToRemove);
                    CoursesDictionary.Remove(CourseToRemove);
                    Console.WriteLine($"\nCourse {CourseToRemove} has been removed successfully.");
                }
                else
                {
                    Console.WriteLine($"\nRemoval of {CourseToRemove} has been cancelled, returning to main menu.");
                }
            } while(true);
        }

        static void EnrollStudent(string? WListName = null, string? WListCourse = null)
        {
            if ((WListName != null) && (WListCourse != null))
            {
                CoursesDictionary[WListCourse].Add(WListName);
                Console.WriteLine($"\n{WListName} has been added to {WListCourse} from the waiting list.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else
            {
                do
                {
                    DisplayStudentsInAllCourses();
                    Console.WriteLine("Enter the student name to enroll, or \"x\" to exit:\n");
                    string EnteredName;
                    while (string.IsNullOrEmpty(EnteredName = Console.ReadLine().ToLower()))
                    {
                        Console.Clear();
                        Console.WriteLine("\nInvalid name, please try again.");
                        Console.WriteLine("Enter the student name to enroll:\n");
                    }
                    if (EnteredName == "x")
                    {
                        return;
                    }
                    string EnrollStudentName = NameFormatting(EnteredName);
                    Console.WriteLine("\nEnter the course ID to enroll student in, or \"x\" to exit:\n");
                    string EnrollCourse;
                    while (string.IsNullOrEmpty(EnrollCourse = Console.ReadLine().ToUpper())
                        || (!CoursesDictionary.ContainsKey(EnrollCourse))
                        || (CoursesDictionary[EnrollCourse].Count >= CourseSeats[EnrollCourse]))
                    {
                        if (EnrollCourse == "X")
                        {
                            return;
                        }
                        Console.Clear();
                        if (!CoursesDictionary.ContainsKey(EnrollCourse))
                        {
                            Console.WriteLine($"\nCourse {EnrollCourse} does not exist, please try again:\n");
                        }
                        else if (CoursesDictionary[EnrollCourse].Count >= CourseSeats[EnrollCourse])
                        {
                            Console.WriteLine($"{EnrollCourse} is currently full, would you like to add you name to a waiting list? (1)Yes / (2)No");
                            int WaitListChoice;
                            while ((!int.TryParse(Console.ReadLine(), out WaitListChoice)) || (WaitListChoice > 2) || (WaitListChoice < 1))
                            {
                                Console.Clear();
                                Console.WriteLine($"{EnrollCourse} is currently full, would you like to add you name to a waiting list? (1)Yes / (2)No");
                                Console.WriteLine("\nInvalid input, please try again:\n");
                            }
                            if (WaitListChoice != 1)
                            {
                                Console.WriteLine("\nEnrollment has been cancelled.");
                                return;
                            }
                            else
                            {
                                if (CoursesDictionary[EnrollCourse].Contains(EnrollStudentName))
                                {
                                    Console.WriteLine("\nStudent is already enrolled.\n");
                                }
                                else
                                {
                                    WaitList.Add((EnrollCourse, EnrollStudentName));
                                    Console.WriteLine($"{EnrollStudentName} has been added on a waiting list for {EnrollCourse}." +
                                        $"\nStudent will be added automatically once there is available seats.");
                                }
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nInvalid name, please try again:\n");
                        }
                        Console.WriteLine("\nEnter the course ID to enroll student in, or \"x\" to exit:\n");
                    }
                    if (!CoursesDictionary[EnrollCourse].Contains(EnrollStudentName))
                    {
                        CoursesDictionary[EnrollCourse].Add(EnrollStudentName);
                        Console.WriteLine($"Student {EnrollStudentName} has been enrolled in {EnrollCourse} successfully.");
                    }
                    else
                    {
                        Console.WriteLine("\nStudent is already enrolled in this course.\n");
                    }
                } while (true);
            }
        }

        static void RemoveStudentFromCourse()
        {
            do
            {
                Console.Clear();
                DisplayStudentsInAllCourses();
                Console.WriteLine("Enter the student name to be removed from a course or \"x\" to exit:\n");
                string EnteredName;
                while (string.IsNullOrEmpty(EnteredName = Console.ReadLine().ToLower()))
                {
                    Console.Clear();
                    Console.WriteLine("\nInvalid name, please try again.");
                    Console.WriteLine("Enter the student name to be removed from a course or \"x\" to exit:\n");
                }
                if (EnteredName == "x")
                {
                    return;
                }
                
                string RemoveStudentName = NameFormatting(EnteredName);
                Console.WriteLine("\nEnter the course ID to remove the student from:\n");
                string RemoveFromCourse;
                while (string.IsNullOrEmpty(RemoveFromCourse = Console.ReadLine().ToUpper()) 
                    || (!CoursesDictionary.ContainsKey(RemoveFromCourse)))
                {
                    Console.Clear();
                    Console.WriteLine("\nInvalid name, please try again.");
                    Console.WriteLine("Enter the Course ID to enroll the student:\n");
                }
                if (CoursesDictionary[RemoveFromCourse].Contains(RemoveStudentName))
                {
                    Console.WriteLine($"\nYou are removing student {RemoveStudentName} from {RemoveFromCourse}, Confirm? (1)Yes / (2)No");
                    int RemoveConf;
                    while ((!int.TryParse(Console.ReadLine(), out RemoveConf))
                        || (RemoveConf > 2)
                        || (RemoveConf < 1))
                    {
                        Console.Clear();
                        Console.WriteLine($"\nYou are removing student {RemoveStudentName} from {RemoveFromCourse}, Confirm? (1)Yes / (2)No");
                        Console.WriteLine("\nInvalid input, please try again:\n");
                    }
                    if (RemoveConf == 1)
                    {
                        CoursesDictionary[RemoveFromCourse].Remove(RemoveStudentName);
                        Console.WriteLine($"\nStudent {RemoveStudentName} has been removed from {RemoveFromCourse} successfully.");
                        for (int i = 0; i < WaitList.Count; i++)
                        {
                            if (WaitList[i].CourseName == RemoveFromCourse)
                            {
                                EnrollStudent(WaitList[i].StudentName, WaitList[i].CourseName);
                                WaitList.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"\nRemoval of {RemoveStudentName} has been cancelled, returning to main menu.");
                    }
                }
                else
                {
                    Console.WriteLine("\nStudent is not enrolled in this course.\n");
                }
            } while (true);
        }

        static void DisplayStudentsInCourse()
        {
            DisplayStudentsInAllCourses();
            Console.WriteLine("Enter the course ID to view enrolled students, or enter \"x\" to exit:\n");
            string CourseToView;
            while ((string.IsNullOrEmpty(CourseToView = Console.ReadLine().ToUpper())) 
                || (!CoursesDictionary.ContainsKey(CourseToView)) 
                || (CoursesDictionary[CourseToView].Count <= 0))
            {
                Console.Clear();
                if(CourseToView == "x")
                {
                    return;
                }
                else if (!CoursesDictionary.ContainsKey(CourseToView))
                {
                    Console.WriteLine("Course does not exist, please try again.\n");
                    Console.WriteLine("Enter the course ID to view enrolled students, or enter \"x\" to exit:\n");
                }
                else if (CoursesDictionary[CourseToView].Count <= 0)
                {
                    Console.WriteLine("Course has no enrolled students, please try again.\n");
                    Console.WriteLine("Enter the course ID to view enrolled students, or enter \"x\" to exit:\n");
                }
                else
                {
                    Console.WriteLine("Invalid course ID, please try again.\n");
                    Console.WriteLine("Enter the course ID to view enrolled students, or enter \"x\" to exit:\n");
                }
            }
            Console.Clear();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Students in course \"{CourseToView}\":");
            foreach (string student in CoursesDictionary[CourseToView])
            {
                sb.AppendLine(student);
            }
            Console.WriteLine(sb.ToString());
        }

        static void DisplayStudentsInAllCourses()
        {
            StringBuilder sb = new StringBuilder();
            string border = new string('-', 60);
            sb.AppendLine("Courses & Students:\n");
            sb.AppendLine($"{"Course",-10}: {"Students",-10}");
            sb.AppendLine(border);
            foreach (var Kvp in CoursesDictionary)
            {
                sb.Append($"{Kvp.Key, -10}: ");
                foreach(string student in Kvp.Value)
                {
                    sb.Append($" {student, -10} ");
                }
                sb.AppendLine();
                sb.AppendLine(border);
            }
            Console.Clear();
            Console.WriteLine(sb.ToString());
        }

        static void CommonStudentsInCourses()
        {
            DisplayStudentsInAllCourses();
            Console.WriteLine("Enter two courses IDs to check common students, or enter \"x\" to exit:\n");
            string FirstCourse;
            string SecondCourse;
            Console.Write("Course 1: ");
            while((string.IsNullOrEmpty(FirstCourse = Console.ReadLine().ToUpper())) 
                || (!CoursesDictionary.ContainsKey(FirstCourse)) 
                || (CoursesDictionary[FirstCourse].Count <= 0))
            {
                Console.Clear();
                if (FirstCourse == "x")
                {
                    return;
                }
                else if (!CoursesDictionary.ContainsKey(FirstCourse))
                {
                    Console.WriteLine("Course does not exist, please try again.\n");
                }
                else if (CoursesDictionary[FirstCourse].Count <= 0)
                {
                    Console.WriteLine("Course does not have enrolled students, please try again.\n");
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again.\n");
                }
                Console.WriteLine("Enter two courses IDs to check common students, or enter \"x\" to exit:\n");
            }
            Console.Clear();
            DisplayStudentsInAllCourses();
            Console.Write("Course 2: ");
            while ((string.IsNullOrEmpty(SecondCourse = Console.ReadLine().ToUpper())) 
                || (!CoursesDictionary.ContainsKey(SecondCourse)) 
                || (CoursesDictionary[SecondCourse].Count <= 0))
            {
                if (SecondCourse == "x")
                {
                    return;
                }
                else if (!CoursesDictionary.ContainsKey(SecondCourse))
                {
                    Console.WriteLine("Course does not exist, please try again.\n");
                }
                else if (CoursesDictionary[SecondCourse].Count <= 0)
                {
                    Console.WriteLine("Course does not have enrolled students, please try again.\n");
                }
                else
                {
                    Console.WriteLine("Invalid input, please try again.\n");
                }
                Console.WriteLine("Enter two courses IDs to check common students, or enter \"x\" to exit:\n");
            }
            Console.Clear();
            StringBuilder sb = new StringBuilder();
            sb.Append($"Common students between {FirstCourse} and {SecondCourse}");
            sb.AppendLine();
            var CommonSet = CoursesDictionary[FirstCourse].Intersect(CoursesDictionary[SecondCourse]);
            foreach ( var Common in CommonSet )
            {
                sb.Append( " | "+Common.ToString() );
            }
            sb.AppendLine();
            Console.WriteLine(sb.ToString());
        }

        static void WithdrawStudentFromAllCourses()
        {
            string StudentNameToRemove;
            bool FoundStudent = false;
            do
            {
                Console.Clear();
                DisplayStudentsInAllCourses();
                Console.WriteLine("Enter the student name to remove from all courses:\nOr Enter \"x\" to exit.\n\n");
                StudentNameToRemove = Console.ReadLine();
                if (StudentNameToRemove == "x")
                {
                    return;
                }
                else if (string.IsNullOrEmpty(StudentNameToRemove))
                {
                    Console.WriteLine("Invalid name, please try again.\n");
                }
                else
                {
                    foreach (var Kvp in CoursesDictionary)
                    {
                        if (CoursesDictionary[Kvp.Key].Contains(StudentNameToRemove))
                        {
                            FoundStudent = true;
                            break;
                        }
                    }
                    if (!FoundStudent)
                    {
                        Console.WriteLine("Student did not enroll in any course.\n");
                    }
                }
            } while (!FoundStudent);

            foreach (var Kvp in CoursesDictionary)
            {
                CoursesDictionary[Kvp.Key].Remove(StudentNameToRemove);
                for (int i = 0; i < WaitList.Count; i++)
                {
                    if (WaitList[i].CourseName == Kvp.Key)
                    {
                        EnrollStudent(WaitList[i].StudentName, WaitList[i].CourseName);
                        WaitList.RemoveAt(i);
                        break;
                    }
                }
            }
            Console.WriteLine($"\nStudent {StudentNameToRemove} removed successfuly.");
        }

        static void ViewWaitingList()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Waiting List:\n");
            sb.AppendLine($"{"Student Name", -20} | {"Course ID", -10}");
            for (int i = 0; i < WaitList.Count; i++)
            {
                sb.AppendLine($"{"",-20} | {"",-10}");
                sb.AppendLine($"{WaitList[i].StudentName,-20} | {WaitList[i].CourseName,-10}");
            }
            Console.WriteLine(sb.ToString());
        }

        static void InitializeStartupData()
        {
            // Example data: Courses and their enrolled students (cross-over students)
            CoursesDictionary["CS101"] = new HashSet<string> { "Alice", "Bob", "Charlie" };   // CS101 has Alice, Bob, Charlie
            CoursesDictionary["MATH202"] = new HashSet<string> { "David", "Eva", "Bob" };     // MATH202 has David, Eva, and Bob (cross-over with CS101)
            CoursesDictionary["ENG303"] = new HashSet<string> { "Frank", "Grace", "Charlie" };// ENG303 has Frank, Grace, and Charlie (cross-over with CS101)
            CoursesDictionary["BIO404"] = new HashSet<string> { "Ivy", "Jack", "David" };     // BIO404 has Ivy, Jack, and David (cross-over with MATH202)

            // Set course capacities (varying)
            CourseSeats["CS101"] = 3;  // CS101 capacity of 3 (currently full)
            CourseSeats["MATH202"] = 5; // MATH202 capacity of 5 (can accept more students)
            CourseSeats["ENG303"] = 3;  // ENG303 capacity of 3 (currently full)
            CourseSeats["BIO404"] = 4;  // BIO404 capacity of 4 (can accept more students)

            // Waitlist for courses (students waiting to enroll in full courses)
            WaitList.Add(("CS101", "Helen"));   // Helen waiting for CS101
            WaitList.Add(("ENG303", "Jack"));   // Jack waiting for ENG303
            WaitList.Add(("BIO404", "Alice"));  // Alice waiting for BIO404
            WaitList.Add(("ENG303", "Eva"));    // Eva waiting for ENG303

            Console.WriteLine("Startup data initialized.");
        }

        static string NameFormatting(string Input)
        {
            bool FirstCharSpace = false;
            StringBuilder FormattedName = new StringBuilder();
            for (int i = 0; i < Input.Length; i++)
            {
                if (Input[i].ToString() == " ")
                {
                    if (i == 0)
                    {
                        FirstCharSpace = true;
                    }
                    for (int j = i + 1; j < Input.Length; j++)
                    {
                        if (Input[j].ToString() == " ")
                        {
                            Input.Remove(j);
                            i++;
                        }
                        else
                        {
                            if (!FirstCharSpace)
                            {
                                FormattedName.Append(" ");
                            }
                            break;
                        }
                    }
                    if((i+1) < Input.Length)
                    {
                        FormattedName.Append(Input[i + 1].ToString().ToUpper());
                        i++;
                    }
                    
                }
                else if (i == 0)
                {
                    FormattedName.Append(Input[i].ToString().ToUpper());
                }
                else
                {
                    FormattedName.Append(Input[i].ToString().ToLower());
                }
            }
            return FormattedName.ToString();
        }
    }
}
