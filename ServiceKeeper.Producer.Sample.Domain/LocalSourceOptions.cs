namespace ServiceKeeper.Producer.Sample.Domain
{
    public class LocalSourceOptions
    {
        public LocalSourceOptions() { }
        public LocalSourceOptions(string rootDir, string serviceConfigSaveName, string taskEntitiesSaveName)
        {
            RootDir = rootDir;
            ServiceConfigSaveName = serviceConfigSaveName;
            TaskEntitiesSaveName = taskEntitiesSaveName;
        }
        public string RootDir { get; set; } = "";
        public string ServiceConfigSaveName { get; set; } = "";
        public string TaskEntitiesSaveName { get; set; } = "";
    }
}