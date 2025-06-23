namespace Quest_01
{
    public class Part0000 : IEverybodyCodesProblem
    {
        public string ProblemNumber => Helpers.GetQuestFromNamespace(this);
        public string PartNumber => Helpers.GetPartFromClass(this);
        public string ProblemName { get => $"Day {ProblemNumber}: ????. {PartNumber}"; }

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
            return File.ReadAllText(filePath);
        }
    }
}