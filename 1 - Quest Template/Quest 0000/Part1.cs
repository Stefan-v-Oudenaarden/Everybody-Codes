namespace Quest_0000
{
    public class Part1 : IEverybodyCodesProblem
    {
        public string ProblemNumber => Helpers.GetQuestFromNamespace(this);
        public string PartNumber => Helpers.GetPartFromClass(this);

        public static string ProblemTitle = "???";
        public string ProblemName { get => $"Day {ProblemNumber}: {ProblemTitle}. {Helpers.AddSpacesBeforeDigits(PartNumber)}"; }

        public void Run()
        {
            Solve(ParseInput($"Quest {ProblemNumber}/{PartNumber}.input.txt"));
        }

        public void RunTest()
        {
            Solve(ParseInput($"Quest {ProblemNumber}/{PartNumber}.test-input.txt"));
        }

        public void Solve(string input)
        {
            Log.Information("A solution can be found");
        }

        public static string ParseInput(string filePath)
        {
            return File.ReadAllText(filePath).NormalizeLineEndings();
        }
    }
}