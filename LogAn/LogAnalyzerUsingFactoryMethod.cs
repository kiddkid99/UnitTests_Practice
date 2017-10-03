namespace LogAn
{
    public class LogAnalyzerUsingFactoryMethod
    {
        public bool IsValidLogFileName(string fileName)
        {
            return GetManager().IsValid(fileName);
        }

        public virtual IFileExtensionManager GetManager()
        {
            return new FileExtensionManager();
        }
    }
}
