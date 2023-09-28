using ClientApp.Services;
using ClientApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Dependency(typeof(AndroidFileHandler))]
namespace ClientApp.Services
{
    public class AndroidFileHandler : IFileHandler
    {
        public void HandleReceivedFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                // Log error or notify user that the file path is invalid
                return;
            }

            // Call SetFilePathInBlazor to update the file path in the Blazor component
            SetFilePathInBlazor(filePath);
        }
        public void SetFilePathInBlazor(string filePath)
        {
            FilePathEvent.RaiseFilePathSet(filePath);
        }
    }
}
