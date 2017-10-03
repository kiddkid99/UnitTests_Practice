using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn
{
    public class LogAnalyzer
    {
        private IFileExtensionManager manager;
        private IWebService service;

        public LogAnalyzer()
        {

        }

        public LogAnalyzer(IFileExtensionManager manager)
        {
            this.manager = manager;
        }

        public LogAnalyzer(IWebService service)
        {
            this.service = service;
        }


        public bool WasLastFileNameValid { get; set; }

        public bool IsValidLogFileName(string fileName)
        {
            WasLastFileNameValid = false;

            if(String.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("filename has to be provided.");
            }

            if(!fileName.EndsWith(".SLF", StringComparison.CurrentCultureIgnoreCase))
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
            if(fileName.Length < 8)
            {
                service.LogError("File name too short: " + fileName);
            }
        }
    }
}
