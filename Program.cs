namespace AnyQuestions
{
    internal class Program
    {
        static void Main()
        {
            Console.CursorVisible = false;

            loadStatistic();

            int cursorPos = 1;

            bool choosing = true;

            bool isAnswering = false;

            while (choosing)
            {
                drawMenu(ref cursorPos, ref choosing);
            }

        }

        // Menu (HUD)
        private static void drawMenu(ref int cursorPos, ref bool choosing)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" Choose Path: (To choose press enter) ");
            Console.WriteLine("  1. Use recent path");
            Console.WriteLine("  2. Enter file path");
            Console.WriteLine("  3. Statistic");
            Console.WriteLine("  4. Load test questions");
            Console.WriteLine("  5. Exit");
            Console.SetCursorPosition(0, cursorPos);
            Console.Write(">");

            if (choosingPos(ref cursorPos) == true)
            {
                Console.Clear();
                choosing = false;
                choose(cursorPos);
            }
        }

        private static bool choosingPos(ref int cursorPos)
        {
            ConsoleKey key = Console.ReadKey().Key;
            switch (key)
            {
                case ConsoleKey.DownArrow:
                    if (cursorPos >= 5)
                        cursorPos = 1;
                    else cursorPos++;
                    break;
                case ConsoleKey.UpArrow:
                    if (cursorPos <= 1)
                        cursorPos = 5;
                    else cursorPos--;
                    break;
                case ConsoleKey.Enter:
                    return true;
            }
            return false;
        }

        private static void choose(int position)
        {
            switch (position)
            {
                case 1:
                    QAHUD();
                    break;
                case 2:
                    enter(); // enter new path
                    QAHUD(); // after run this path

                    break;
                case 3:
                    showStatistic();
                    break;
                case 4:
                    loadTest();
                    break;
                case 5:
                    Environment.Exit(0);
                    break;
            }
        }

        private static void QAHUD()
        {
            // 60 - time, now isn't important
            questions Questions = new questions(60, readConfig());
            Questions.Main();

        }

        private static string readConfig()
        {
            return File.ReadAllText("config.txt");
        }


        private static void loadTest()
        {

            string qatest = "1 + 1 ?\n2\nwrite Hello\nhello";

            File.WriteAllText("qatest.txt", qatest);

            string qatestPath = $"{Environment.CurrentDirectory}\\qatest.txt";

            Console.WriteLine($"Testing file was created: {qatestPath}");

            // Saving in the latest path
            File.WriteAllText("config.txt", qatestPath);
        }

        private static string enter()
        {
            // Using while to avoid stack overflow caused by recursion (if {...} else {enter()})
            while (true)
            {
                string directory = Console.ReadLine();

                // checking correct file name
                if (directory.Substring(directory.Length - 4, 4) != ".txt")
                    directory = $"{directory}.txt";

                if (File.Exists(directory))
                {
                    // Saving in the latest path
                    File.WriteAllText("config.txt", directory);
                    return directory;
                }
                else
                {
                    Console.WriteLine("Path error. Try again.");
                }
            }

        }

        private static void loadStatistic()
        {
            if (!File.Exists("statistic.txt"))
                File.WriteAllText("statistic.txt", "0");
        }

        private static void showStatistic()
        {
            Console.WriteLine(File.ReadAllText("statistic.txt"));

            Console.WriteLine("Press any button to continue...");

            Console.ReadKey();

            // NOT good method :(
            Main();
        }

    }
}
