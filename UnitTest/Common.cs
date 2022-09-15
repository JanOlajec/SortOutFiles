using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public static class Common
    {
        public const string FILE1 = "SNY09199.jpg";

        private static string GetProjectPath()
        {
            string projectPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            int StepUpInPath = 2; // 2-last folders will be removed: \bin\Debug

            // remove defined number of folders from the path
            for (int i = 0; i < StepUpInPath; i++)
            {
                // remove last folder in path
                projectPath = Path.GetDirectoryName(projectPath);
            }

            return projectPath;
        }

        public static string GetTestDataPath()
        {
            return Path.Combine(GetProjectPath(), "TestData");
        }

        public static string GetPhotosPath()
        {
            return Path.Combine(GetTestDataPath(), "Photos");
        }

        public static string GetOutputPath()
        {
            return Path.Combine(GetTestDataPath(), "Output");
        }
    }
}
