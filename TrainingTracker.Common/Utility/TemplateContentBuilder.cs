using System.Collections.Generic;

namespace TrainingTracker.Common.Utility
{
    public class TemplateContentBuilder
    {
        private string _templateText;

        public TemplateContentBuilder(string template)
        {
            _templateText = template;
        }

        public void Fill(Dictionary<string, string> tags)
        {
            foreach (var t in tags)
            {
                _templateText = _templateText.Replace(string.Format("[[[{0}]]]", t.Key), t.Value);
            }
        }

        public string GetText()
        {
            return _templateText;
        }
    }
}