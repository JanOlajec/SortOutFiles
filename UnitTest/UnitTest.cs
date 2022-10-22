﻿using SortOutFiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
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

        /// <summary>
        /// Expected destination folder name after sorting based on DateTaken
        /// </summary>
        private const string SORT_OUT_FOLDER1 = "2022.08.11";
        private const string SORT_OUT_FOLDER2 = "2022.08.17";
        private const string SORT_OUT_FOLDER3 = "2022.08.26";
        private const string SORT_OUT_FOLDER4 = "2022.08.29";
        private const string SORT_OUT_FOLDER5 = "2022.08.31";
        private const string SORT_OUT_FOLDER6 = @"NoDateTaken\2022.10.03";
        private const string SORT_OUT_FOLDER7 = @"NoDateTaken\2022.10.03";

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
        };

        /// <summary>
        /// Expected output folders paths
        /// </summary>
        /// <returns></returns>
        private List<string> GetSortOutOutputFoldersPaths() 
        { 
            List<string> result = new List<string>();   

            foreach (var item in SortOutOutputFolders)
            {
                result.Add(Path.Combine(Common.GetOutputPath(), item));
            }
            return result;
        }

        /// <summary>
        /// Expected files paths in specific folders
        /// </summary>
        /// <returns></returns>
        private List<string> GetSortOutOutputFilesPaths()
        {
            List<string> result = new List<string>();

            foreach (var item in sortOutData)
            {
                string folderPath = Path.Combine(Common.GetOutputPath(), item.Value);
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

            foreach (var sortOutFolderPath in GetSortOutOutputFoldersPaths())
            {
                Assert.IsTrue(Directory.Exists(sortOutFolderPath));
            }

            foreach (var sortOutFilesPath in GetSortOutOutputFilesPaths())
            {
                Assert.IsTrue(File.Exists(sortOutFilesPath));
            }
        }

        //TODO files without TakenDate - file RAM00001.jpg is not correctly sorted out. I cant get "Modified date" from properties which correspond with date of file creation
        //TODO not valid path
        //TODO destination not possible to create ???
        //TODO empty source folder
        //TODO mix of different file types
        //TODO file in destination exists
        //TODO the same folder is input and output
    }
}
