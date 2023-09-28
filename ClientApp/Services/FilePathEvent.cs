using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Services
{
    public static class FilePathEvent
    {
        public static event Action<string> OnFilePathSet;

        public static void RaiseFilePathSet(string filePath) => OnFilePathSet?.Invoke(filePath);
    }
}
