using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Logic.Builder
{
    public class WorkFileBuilder
    {
        private ReportBuilderInterface reportBuilderInterface;

        public void SetBuilder(ReportBuilderInterface reportBuilderInterface)
        {
            this.reportBuilderInterface = reportBuilderInterface;
        }
    }
}
