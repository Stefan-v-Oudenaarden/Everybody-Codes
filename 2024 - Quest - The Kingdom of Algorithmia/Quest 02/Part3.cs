namespace Quest_02
{
    public class Part3 : IEverybodyCodesProblem
    {
        public string ProblemNumber => Helpers.GetQuestFromNamespace(this);
        public string PartNumber => Helpers.GetPartFromClass(this);

        public string ProblemName { get => $"Day {ProblemNumber}: {Part1.ProblemTitle}. {Helpers.AddSpacesBeforeDigits(PartNumber)}"; }

        public void Run()
        {
            Solve(ParseInput($"Quest {ProblemNumber}/{PartNumber}.input.txt"));
        }

        public void RunTest()
        {
            Solve(ParseInput($"Quest {ProblemNumber}/{PartNumber}.test-input.txt"));
        }

        public void Solve((List<string> runes, List<string> sentences) input)
        {
            var totalRuneCount = 0;
            foreach (var sentence in input.sentences)
            {
                var markedSentence = sentence;
                foreach (var rune in input.runes)
                {
                    markedSentence = ReplaceIgnoreCase(markedSentence, rune, rune.ToLower());
                    //Log.Information("{rune} in {sentence}", rune, markedSentence);
                    markedSentence = ReplaceIgnoreCase(markedSentence.Reverse(), rune, rune.ToLower());
                    markedSentence = markedSentence.Reverse();
                }

                var markedCount = markedSentence.Count(c => Char.IsLower(c));
                totalRuneCount += markedCount;

                ///Log.Verbose("Runinc symbols in {sentence} is {count}", markedSentence, markedCount);
            }

            Log.Information("Total runes in text is {totalRuneCount}", totalRuneCount);
        }

        public static (List<string> runes, List<string> sentences) ParseInput(string filePath)
        {
            var inputText = File.ReadAllText(filePath).NormalizeLineEndings();

            var parts = inputText.Split(Environment.NewLine + Environment.NewLine);
            var runes = parts[0].Split(':')[1].Split(',');
            var sentences = parts[1].Split(Environment.NewLine);

            return (runes.ToList(), sentences.ToList());
        }

        public static string ReplaceIgnoreCase(string source, string oldValue, string newValue)
        {
            return Regex.Replace(source, oldValue, newValue, RegexOptions.IgnoreCase);
        }
    }
}