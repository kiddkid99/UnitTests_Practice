using System;

namespace LogAn
{
    public class LogAnalyzer
    {
        private IFileExtensionManager manager;

        public IWebService Service
        {
            get; set;
        }

        public IEmailService Email
        {
            get; set;
        }

        public int MinNameLength
        {
            get; set;
        }


        public LogAnalyzer()
        {
            MinNameLength = 8;
        }

        public LogAnalyzer(IFileExtensionManager manager) : this()
        {
            this.manager = manager;
        }

        public LogAnalyzer(IWebService service, IEmailService email) : this()
        {
            this.Service = service;
            this.Email = email;
        }


        public bool WasLastFileNameValid { get; set; }

        public bool IsValidLogFileName(string fileName)
        {
            WasLastFileNameValid = false;

            if (String.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("filename has to be provided.");
            }

            if (!fileName.EndsWith(".SLF", StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            WasLastFileNameValid = true;
            return true;
        }


        public bool IsValidLogFileNameFromExternal(string fileName)
        {
            try
            {
                return manager.IsValid(fileName);
            }
            catch
            {
                return false;
            }

        }


        public void Analyze(string fileName)
        {
            if (fileName.Length < MinNameLength)
            {
                try
                {
                    Service.LogError("File name too short: " + fileName);
                }
                catch (Exception e)
                {
                    Email.SendEmail("someone@somewhere.com", "can't log", e.Message);
                }

            }
        }
    }
}
