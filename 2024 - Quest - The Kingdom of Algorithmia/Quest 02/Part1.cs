namespace Quest_02
{
    public class Part1 : IEverybodyCodesProblem
    {
        public string ProblemNumber => Helpers.GetQuestFromNamespace(this);
        public string PartNumber => Helpers.GetPartFromClass(this);

        public static string ProblemTitle = "The Runes of Power";
        public string ProblemName { get => $"Day {ProblemNumber}: {ProblemTitle}. {Helpers.AddSpacesBeforeDigits(PartNumber)}"; }

        public void Run()
        {
            Solve(ParseInput($"Quest {ProblemNumber}/{PartNumber}.input.txt"));
        }

        public void RunTest()
        {
            Solve(ParseInput($"Quest {ProblemNumber}/{PartNumber}.test-input.txt"));
        }

        public void Solve((List<string> runes, string sentence) input)
        {
            var totalRuneCount = 0;
            foreach (var rune in input.runes)
            {
                var runeCount = input.sentence.Count(rune);
                totalRuneCount += runeCount;
            }

            Log.Information("Total runes in sentence is {totalRuneCount}", totalRuneCount);
        }

        public static (List<string> runes, string sentence) ParseInput(string filePath)
        {
            var inputText = File.ReadAllText(filePath).NormalizeLineEndings();

            var parts = inputText.Split(Environment.NewLine + Environment.NewLine);
            var runes = parts[0].Split(':')[1].Split(',');
            var sentence = parts[1];

            return (runes.ToList(), sentence);
        }
    }
}