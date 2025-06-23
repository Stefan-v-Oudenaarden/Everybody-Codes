namespace Quest_01
{
    public class Part2 : IEverybodyCodesProblem
    {
        public string ProblemNumber => Helpers.GetQuestFromNamespace(this);
        public string PartNumber => Helpers.GetPartFromClass(this);
        public string ProblemName { get => $"Day {ProblemNumber}: The Battle for the Farmlands {PartNumber}"; }

        private readonly Dictionary<char, int> _potionCost = new() { { 'A', 0 }, { 'B', 1 }, { 'C', 3 }, { 'D', 5 }, { 'x', 0 } };

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

            var pairs = Enumerable.Range(0, input.Length)
                         .GroupBy(x => x / 2)
                         .Select(x => new string(x.Select(y => input[y]).ToArray()))
                         .ToList();

            foreach (string pair in pairs)
            {
                var battlePotionCount = 0;
                battlePotionCount += _potionCost[pair[0]];
                battlePotionCount += _potionCost[pair[1]];

                if (!pair.Contains("x"))
                {
                    battlePotionCount += 2;
                }

                potionCount += battlePotionCount;
            }

            Log.Information("Total potions used for encounters is {potionCount}", potionCount);
        }

        public static string ParseInput(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}