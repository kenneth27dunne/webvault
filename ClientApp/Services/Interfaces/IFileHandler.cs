using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Services.Interfaces
{
    public interface IFileHandler
    {
        void HandleReceivedFile(string filePath);
        void SetFilePathInBlazor(string filePath);
    }
}
