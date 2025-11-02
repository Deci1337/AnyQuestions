namespace AnyQuestions
{
    internal class questions
    {
        public List<string> quests { get; private set; }

        public List<string> answers { get; private set; }

        public int[] points { get; private set; }

        private int time;

        private string directory;

        internal questions(int time, string directory)
        {
            this.time = time;
            this.directory = directory;
        }

        internal void Main()
        {
            string saveTestPath = @"statistic.txt";
            getquestions(directory);
            if (quests.Count != 0 && answers.Count != 0)
            {
                int time = 180;
                SaveItem(saveTestPath, Print(quests, answers, time));
            }
        }

        internal (List<string>, List<string>) getquestions(string directory)
        {
            if (quests == null)
                quests = new List<string>();

            if (answers == null)
                answers = new List<string>();

            try
            {
                if (!File.Exists(directory))
                {
                    Console.WriteLine($"Can't find: {directory}");

                    return (quests, answers);
                }

                string[] allLines = File.ReadAllLines(directory);

                if (allLines.Length == 0)
                {
                    Console.WriteLine("File is empty.");

                    return (quests, answers);
                }

                for (int i = 0; i < allLines.Length; i++)
                {
                    if (i % 2 == 0)
                        quests.Add(allLines[i]);
                    else
                        answers.Add(allLines[i]);
                }

                Console.WriteLine($"Loaded questions: {quests.Count}, answers: {answers.Count}.");

                if (quests.Count != answers.Count)
                {
                    Console.WriteLine("Attention! The number of questions and answers aren't equal!");
                }

                return (quests, answers);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error for readind file: {ex.Message}");

                return (quests, answers);
            }
        }


        internal int Print(List<string> quests, List<string> answers, int timer)
        {
            int count = 0;

            for (int i = 0; i < quests.Count; i++)
            {
                Console.WriteLine(quests[i]);

                Console.Write("Ответ > ");
                string input = Console.ReadLine();

                string answer = answers[i];

                (input, answer) = Compare(input, answer);

                if (answer == input)
                    count++;
            }

            Console.WriteLine();

            if ((double)count / quests.Count * 100 > 50)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"Your answers: {count} / {quests.Count}");
            Console.WriteLine($"Success: {(double)count / quests.Count * 100} %");
            Console.WriteLine("Show answers? Press Y / N");

            ConsoleKey key = Console.ReadKey().Key;

            Console.WriteLine();

            if (key == ConsoleKey.Y)
            {
                Console.ForegroundColor = ConsoleColor.Green;

                for (int i = 0; i < quests.Count; i++)
                {
                    Console.WriteLine($"{quests[i]} - {answers[i]}");
                }
            }

            Console.ForegroundColor= ConsoleColor.Green;

            Console.WriteLine("Program ended. Repeat? Y / N");

            key = Console.ReadKey().Key;

            Console.WriteLine();

            if (key == ConsoleKey.Y)
            {
                count += Print(quests, answers, timer);
            }

            return count;
        }

        internal void SaveItem(string SavePath, int count)
        {
            if (File.Exists(SavePath))
            {
                int.TryParse(File.ReadAllText(SavePath), out int points);

                int summary = count + points;

                File.WriteAllText(SavePath, Convert.ToString(summary));
            }
            else
                Console.WriteLine($"Error path: {SavePath}");
        }

        // returning lowercase input-answer (for example, "YES" == "yes")
        internal (string, string) Compare(string input, string answer) =>
            (input.ToLower(), answer.ToLower());



    }
}
