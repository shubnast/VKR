using System.Collections.Generic;

namespace ReportCreator
{
    public interface IReportCreator
    {
        bool CreateReport(Dictionary<string, string> dataKeyValues, string templatePath, string reportPath);
    }
}
