namespace EverybodyCodes
{
    public interface IEverybodyCodesProblem
    {
        public void Run();
        public void RunTest();

        public string ProblemName { get; }

        public string ProblemNumber { get; }
        public string PartNumber { get; }
    }
}