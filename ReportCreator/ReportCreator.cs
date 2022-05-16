using System.Collections.Generic;
using System.IO;
using TemplateEngine.Docx;

namespace ReportCreator
{
    public class WordReportCreator : IReportCreator
    {
        public bool CreateReport(Dictionary<string, string> dataKeyValues, string templatePath, string reportPath)
        {
			bool result = false;

			List<FieldContent> content = new List<FieldContent>();

            foreach (KeyValuePair<string, string> data in dataKeyValues)
            {
				content.Add(new FieldContent(data.Key, data.Value));
			}

			Content reportContent = new Content(content.ToArray());

			File.Copy(templatePath, reportPath, true);

			using (TemplateProcessor outputDocument = new TemplateProcessor(reportPath).SetRemoveContentControls(true))
			{
				outputDocument.SetNoticeAboutErrors(false);
				outputDocument.FillContent(reportContent);
				outputDocument.SaveChanges();
			}

			return result;
		}
    }
}
