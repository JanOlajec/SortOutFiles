using SortOutFiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        /// <summary>
        /// File name for sorting
        /// </summary>
        private const string FILE1 = "SNY09199.jpg";
        private const string FILE2 = "SNY09246.jpg";
        private const string FILE3 = "SNY09361.jpg";
        private const string FILE4 = "SNY09448.jpg";
        private const string FILE5 = "SNY09493.jpg";
        private const string FILE6 = "PLAG0001.jpg";
        private const string FILE7 = "RAM00001.jpg";
        private const string FILE8 = "SNY09685.jpg";
        private const string FILE9 = "SNY09735.jpg";

        /// <summary>
        /// Expected destination folder name after sorting based on DateTaken
        /// </summary>
        private const string SORT_OUT_FOLDER1 = "2022.08.11";
        private const string SORT_OUT_FOLDER2 = "2022.08.17";
        private const string SORT_OUT_FOLDER3 = "2022.08.26";
        private const string SORT_OUT_FOLDER4 = "2022.08.29";
        private const string SORT_OUT_FOLDER5 = "2022.08.31";
        private const string SORT_OUT_FOLDER6 = @"_NoDateTaken\2022.10.03";
        private const string SORT_OUT_FOLDER7 = @"_NoDateTaken\2022.10.03";
        private const string SORT_OUT_FOLDER8 = @"2022.10.23";
        private const string SORT_OUT_FOLDER9 = @"2022.10.23";

        /// <summary>
        /// All destination folders created as result of sort out process
        /// </summary>
        private readonly List<string> SortOutOutputFolders = new List<string> {
                SORT_OUT_FOLDER1,
                SORT_OUT_FOLDER2,
                SORT_OUT_FOLDER3,
                SORT_OUT_FOLDER4,
                SORT_OUT_FOLDER5,
                SORT_OUT_FOLDER6,
             /* SORT_OUT_FOLDER7, is the same name as SORT_OUT_FOLDER6 */
                SORT_OUT_FOLDER8,
             /* SORT_OUT_FOLDER9, is the same name as SORT_OUT_FOLDER8 */
            };

        /// <summary>
        /// Specific file present in the destination folder name
        /// </summary>
        private readonly Dictionary<string, string> sortOutData = new Dictionary<string, string>
        {
            { FILE1, SORT_OUT_FOLDER1},
            { FILE2, SORT_OUT_FOLDER2},
            { FILE3, SORT_OUT_FOLDER3},
            { FILE4, SORT_OUT_FOLDER4},
            { FILE5, SORT_OUT_FOLDER5},
            { FILE6, SORT_OUT_FOLDER6},
            { FILE7, SORT_OUT_FOLDER7},
            { FILE8, SORT_OUT_FOLDER8},
            { FILE9, SORT_OUT_FOLDER9},
        };

        /// <summary>
        /// Expected output folders paths
        /// </summary>
        /// <returns></returns>
        private List<string> GetSortOutOutputFoldersPaths(string outputPath) 
        { 
            List<string> result = new List<string>();   

            foreach (var item in SortOutOutputFolders)
            {
                result.Add(Path.Combine(outputPath, item));
            }
            return result;
        }

        /// <summary>
        /// Expected files paths in specific folders
        /// </summary>
        /// <returns></returns>
        private List<string> GetSortOutOutputFilesPaths(string outputPath)
        {
            List<string> result = new List<string>();

            foreach (var item in sortOutData)
            {
                string folderPath = Path.Combine(outputPath, item.Value);
                result.Add(Path.Combine(folderPath, item.Key));
            }

            return result;
        }

        /// <summary>
        /// Remove Output folders if exist
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            if (Directory.Exists(Common.GetOutputPath()))
            {
                Directory.Delete(Common.GetOutputPath(), true);
            }
        }

        /// <summary>
        /// Main functionality verification
        /// Files should be sorted out base on date when they were taken
        /// </summary>
        [TestMethod]
        public void TestMethodMain()
        {
            string source = Common.GetPhotosPath();
            string dest = Common.GetOutputPath();

            _ = new SortOutPhotoFiles(source, dest);

            Assert.IsTrue(Directory.Exists(dest));

            foreach (var sortOutFolderPath in GetSortOutOutputFoldersPaths(Common.GetOutputPath()))
            {
                Assert.IsTrue(Directory.Exists(sortOutFolderPath));
            }

            foreach (var sortOutFilesPath in GetSortOutOutputFilesPaths(Common.GetOutputPath()))
            {
                Assert.IsTrue(File.Exists(sortOutFilesPath));
            }
        }

        /// <summary>
        /// UseCase when input path do not exist. 
        /// Class SortOutPhotoFiles ends with exception. Invalid path should by handled on top level - Program class.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestPathDoNotExist()
        {
            string source = @"C:\ThisPathDoNotExist";
            string dest = Common.GetOutputPath();

            var sort = new SortOutPhotoFiles(source, dest);
        }

        /// <summary>
        /// UseCase when output path can't be created, e.g. due to access rights. 
        /// In test is used 'c:\Program Files' which should not be accessible for program (it is run without administration rights)
        /// Output directory do not exit.
        /// </summary>
        [TestMethod]
        public void TestOutPathNotCreated()
        {
            var source = Common.GetPhotosPath();
            var dest = @"c:\Program Files\OutputPhoto";

            var sort = new SortOutPhotoFiles(source, dest);

            Assert.IsTrue(!Directory.Exists(dest));
        }

        /// <summary>
        /// UseCase when source folder is empty. There are no JPG files for sorting.
        /// Class SortOutPhotoFiles ends with exception which should be catch on Program.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestEmptySourceFolder()
        {
            string source = Common.GetEmptyFolderPath();
            string dest = Common.GetOutputPath();

            if (!Directory.Exists(source))
            {
                Directory.CreateDirectory(source);
            }

            var sort = new SortOutPhotoFiles(source, dest);
        }

        /// <summary>
        /// UseCase for sorting mixed file types. Files which are not JPG.
        /// </summary>
        [TestMethod]
        public void TestMixedFileTypes()
        {
            string source = Common.GetMixedFileTypes();
            string dest = Common.GetOutputPath();

            var sort = new SortOutPhotoFiles(source, dest);

            Assert.IsTrue(Directory.Exists(dest));
            
            //TODO deeper check for file sorting
        }

        /// <summary>
        /// UseCase if output files already exist in output folder, e.g. when sorting is launched more time
        /// </summary>
        [TestMethod]
        public void TestOutputFilesExists()
        {
            string source = Common.GetPhotosPath();
            string dest = Common.GetOutputPath();

            var sort1 = new SortOutPhotoFiles(source, dest);
            var sort2 = new SortOutPhotoFiles(source, dest);

            //output folder exists, exception is not thrown, only warning is shown in console.
            Assert.IsTrue(Directory.Exists(dest));
        }

        /// <summary>
        /// UseCase files for sorting are in sub-folder
        /// TestCase FAILED - its expected
        /// </summary>
        // [TestMethod] - not executed due to issue with "Modified date"
        public void TestInputOutputFolderIsTheSame()
        {
            //WARNING: Do not broke input folder with picture files!

            string source = Common.GetPhotosPath();
            string dest = Common.GetPhotosWorkingPath();

            if (Directory.Exists(dest))
            {
                Directory.Delete(dest, true);
            }

            Directory.CreateDirectory(dest);

            //Copy all the picture files to the new WORKING source folder which will be used also as destination.
            foreach (string sourceFilePath in Directory.GetFiles(source, "*.*", SearchOption.AllDirectories))
            {
                string destFilePath = sourceFilePath.Replace(source, dest);

                if (!Directory.Exists(Path.GetDirectoryName(destFilePath)))
                {
                    //SubFolder must be crated before files copy
                    Directory.CreateDirectory(Path.GetDirectoryName(destFilePath));
                }

                File.Copy(sourceFilePath, sourceFilePath.Replace(sourceFilePath, destFilePath), true);
            }

            var sort = new SortOutPhotoFiles(dest, dest);

            // Check failed because when you copy file you change "creation time" of the file => compare with expected target failed. It is issue with "Modified date"
            foreach (var sortOutFolderPath in GetSortOutOutputFoldersPaths(Common.GetPhotosWorkingPath()))
            {
                Assert.IsTrue(Directory.Exists(sortOutFolderPath));
            }

            foreach (var sortOutFilesPath in GetSortOutOutputFilesPaths(Common.GetPhotosWorkingPath()))
            {
                Assert.IsTrue(File.Exists(sortOutFilesPath));
            }
        }

        //TODO files without TakenDate - file RAM00001.jpg is not correctly sorted out. I cant get "Modified date" from properties which correspond with date of file creation
    }
}
