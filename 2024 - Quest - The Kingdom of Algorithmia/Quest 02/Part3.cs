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

        public void Solve((List<string> runes, List<List<char>> grid) input)
        {
            var rows = input.grid.Count;
            var cols = input.grid[0].Count;
            var markedPositions = new HashSet<(int row, int col)>();

            var directions = new[]
            {
                (0, 1),   // left
                (0, -1),  // right
                (1, 0),   // up
                (-1, 0)   // down
            };

            foreach (var rune in input.runes)
            {
                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < cols; col++)
                    {
                        foreach (var (horizontalDirection, verticalDirection) in directions)
                        {
                            if (FindRuneAtPosition(input.grid, rune, row, col, horizontalDirection, verticalDirection, rows, cols))
                            {
                                for (int i = 0; i < rune.Length; i++)
                                {
                                    var currentRow = row + (i * horizontalDirection);
                                    var currentCol = (col + (i * verticalDirection) + cols) % cols; // Horizontal wrap

                                    if (currentRow >= 0 && currentRow < rows)
                                    {
                                        markedPositions.Add((currentRow, currentCol));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Log.Information("Total runes in text is {totalRuneCount}", markedPositions.Count);
        }

        private bool FindRuneAtPosition(List<List<char>> grid, string rune, int startRow, int startCol, int horizontalDirection, int verticalDirection, int rows, int cols)
        {
            for (int i = 0; i < rune.Length; i++)
            {
                var currentRow = startRow + (i * horizontalDirection);
                var currentCol = (startCol + (i * verticalDirection) + cols) % cols; // Handle horizontal wrapping

                if (currentRow < 0 || currentRow >= rows)
                    return false;

                if (char.ToUpper(grid[currentRow][currentCol]) != char.ToUpper(rune[i]))
                    return false;
            }
            return true;
        }

        public static (List<string> runes, List<List<char>> grid) ParseInput(string filePath)
        {
            var inputText = File.ReadAllText(filePath).NormalizeLineEndings();

            var parts = inputText.Split(Environment.NewLine + Environment.NewLine);
            var runes = parts[0].Split(':')[1].Split(',').Select(r => r.Trim()).ToList();

            var gridLines = parts[1].Split(Environment.NewLine);
            var grid = gridLines.Select(line => line.ToList()).ToList();

            return (runes, grid);
        }
    }
}