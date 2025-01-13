namespace Bot.Logic.Builder
{
    public class Report
    {
        private string _report { get; set; }

        public void SetReport(string repotr)
        {
            _report = repotr;
        }

        public string GetReport()
        {
            return _report;
        }
    }
}
