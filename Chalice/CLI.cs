#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.

using System.CommandLine;
using System.Diagnostics;

namespace EverybodyCodes
{
    public static class CLI
    {
        public static Logger DefaultLogger { get; set; }
        public static Logger IndentLogger { get; set; }

        public static int Process(string[] args)
        {
            InitializeLoggers();

            Command runCommand = new("run")
            {
                Description = "Run a specified Problem in the Quest/Story."
            };
            Argument<string> problemArgument = new("problem")
            {
                Description = "The Problem in this Quest/Story to run.",
                DefaultValueFactory = parseResult => "01"
            };
            runCommand.Arguments.Add(problemArgument);
            runCommand.SetAction(parseResult =>
            {
                var problemName = parseResult.GetValue(problemArgument);
                if (string.IsNullOrWhiteSpace(problemName))
                {
                    Log.Warning("This is not a proper input for problem: {p}", problemName);
                }

                RunOne(problemName!);
            });

            Command runAllCommand = new("runall")
            {
                Description = "Run all Problems in the Quest/Story.",
            };
            runAllCommand.SetAction(parseResult =>
            {
                RunAll();
            });

            Command runLastCommand = new("runlast")
            {
                Description = "Run the last Problem in the Quest/Story."
            };
            Option<bool> exampleArgument = new("--example")
            {
                Description = "Run the example instead of the real input.",
                DefaultValueFactory = parseResult => false
            };
            runLastCommand.Options.Add(exampleArgument);
            runLastCommand.SetAction(parseResult =>
            {
                RunLast(parseResult.GetValue(exampleArgument));
            });

            RootCommand rootCommand = new("Everybody Codes Solver");
            rootCommand.Subcommands.Add(runCommand);
            rootCommand.Subcommands.Add(runAllCommand);
            rootCommand.Subcommands.Add(runLastCommand);

            ParseResult parseResult = rootCommand.Parse(args);
            return parseResult.Invoke();
        }

        public static void InitializeLoggers()
        {
            const string defaultLoggerTemplate = "[{Timestamp:HH:mm:ss ffff} {Level:u3}] {Message}{NewLine}{Exception}";
            DefaultLogger = new LoggerConfiguration()
                                     .MinimumLevel.Information()
                                     .WriteTo.Console(outputTemplate: defaultLoggerTemplate)
                                     .CreateLogger();

            const string indentLoggerTemplate = "[{Timestamp:HH:mm:ss ffff} {Level:u3}] \t{Message}{NewLine}{Exception}";
            IndentLogger = new LoggerConfiguration()
                                     .MinimumLevel.Information()
                                     .WriteTo.Console(outputTemplate: indentLoggerTemplate)
                                     .CreateLogger();
            [Conditional("DEBUG")]
            static void DebugLoggers()
            {
                const string defaultLoggerTemplate = "[{Timestamp:HH:mm:ss ffff} {Level:u3}] {Message}{NewLine}{Exception}";
                DefaultLogger = new LoggerConfiguration()
                                     .MinimumLevel.Verbose()
                                     .WriteTo.Console(outputTemplate: defaultLoggerTemplate)
                                     .CreateLogger();

                const string indentLoggerTemplate = "[{Timestamp:HH:mm:ss ffff} {Level:u3}] \t{Message}{NewLine}{Exception}";
                IndentLogger = new LoggerConfiguration()
                                         .MinimumLevel.Verbose()
                                         .WriteTo.Console(outputTemplate: indentLoggerTemplate)
                                         .CreateLogger();
            }

            DebugLoggers();

            Log.Logger = DefaultLogger;
        }

        private static void RunAll()
        {
            var problems = FindProblems();

            if (problems.Count == 0)
            {
                Log.Error("No problems found. Did you write any?");
            }

            foreach (var quest in problems.GroupBy(p => p.FullName?.Split(".")[0]))
            {
                foreach (var problem in quest)
                {
                    var part = (IEverybodyCodesProblem)Activator.CreateInstance(problem);

                    ProblemRunner(part!);
                }
            }
        }

        private static void RunLast(bool runExample = false)
        {
            var problems = FindProblems();

            if (problems.Count == 0)
            {
                Log.Error("No problems found. Did you write any?");
            }

            var groupedProblems = problems.GroupBy(p => p.FullName?.Split(".")[0]);

            foreach (var problem in groupedProblems.Last())
            {
                var part = (IEverybodyCodesProblem)Activator.CreateInstance(problem);
                ProblemRunner(part!, runExample);
            }
        }

        private static void RunOne(string dayName)
        {
            var problems = FindProblems().Where(d => d.FullName.Contains(dayName, StringComparison.OrdinalIgnoreCase)).ToList();

            if (problems.Count == 0)
            {
                Log.Error("No problems found. Did you write any?");
            }

            var groupedProblems = problems.GroupBy(p => p.FullName?.Split(".")[0]);

            foreach (var problem in groupedProblems.Last())
            {
                var part = (IEverybodyCodesProblem)Activator.CreateInstance(problem);
                ProblemRunner(part!);
            }
        }

        private static List<Type> FindProblems()
        {
            var problemInterface = typeof(IEverybodyCodesProblem);
            var problems = AppDomain.CurrentDomain.GetAssemblies()
                            .SelectMany(a => a.GetTypes())
                            .Where(t => problemInterface.IsAssignableFrom(t) && t.Name != "IEverybodyCodesProblem")
                            .OrderBy(p => p.FullName);

            return [.. problems];
        }

        public static void ProblemRunner(IEverybodyCodesProblem problem, bool example = false)
        {
            Console.WriteLine();
            Log.Information("Running {ProblemName}", problem.ProblemName);
            Log.Logger = IndentLogger;

            if (!example)
            {
                problem.Run();
            }
            else
            {
                problem.RunTest();
            }

            Log.Logger = DefaultLogger;
        }
    }
}