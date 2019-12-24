namespace CSGOStats.Services.HistoryParse.Config.Settings
{
    public class LogSetting
    {
        public string MessageTemplate { get; }

        public string Filename { get; }

        public long FileSizeLimit { get; }

        public LogSetting(string messageTemplate, string filename, long fileSizeLimit)
        {
            MessageTemplate = messageTemplate;
            Filename = filename;
            FileSizeLimit = fileSizeLimit;
        }
    }
}