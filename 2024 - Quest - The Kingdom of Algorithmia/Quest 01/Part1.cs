namespace Quest_01
{
    public class Part1 : IEverybodyCodesProblem
    {
        public string ProblemNumber => Helpers.GetQuestFromNamespace(this);
        public string PartNumber => Helpers.GetPartFromClass(this);
        public string ProblemName { get => $"Day {ProblemNumber}: The Battle for the Farmlands. {PartNumber}"; }

        private readonly Dictionary<char, int> _potionCost = new() { { 'A', 0 }, { 'B', 1 }, { 'C', 3 } };

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
            var potionCount = 0;

            foreach (char c in input)
            {
                potionCount += _potionCost[c];
            }

            Log.Information("Total potions used for encounters is {potionCount}", potionCount);
        }

        public static string ParseInput(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}