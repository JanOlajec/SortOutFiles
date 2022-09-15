using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;

namespace UnitTest
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class Concept
    {
        /// <summary>
        /// Remove Output folder if exist
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
        /// Obtain DateTaken metadata information from JPG picture file 
        /// </summary>
        [TestMethod, TestCategory("Concept")]
        public void Concept01GetMetadata()
        {
            string pathToPhotosFolder = Common.GetPhotosPath();

            Console.WriteLine(pathToPhotosFolder);

            var allFilesPaths = Directory.GetFiles(pathToPhotosFolder);

            foreach (var oneFilePath in allFilesPaths)
            {
                Console.WriteLine(Path.GetFileName(oneFilePath));

                FileInfo fileInformation = new FileInfo(oneFilePath);
                FileStream fileStream = new FileStream(fileInformation.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                BitmapSource imgeSource = BitmapFrame.Create(fileStream);
                BitmapMetadata imageMetaData = (BitmapMetadata)imgeSource.Metadata;
                var dateTaken = imageMetaData.DateTaken;

                Console.WriteLine(dateTaken);
            }

            Assert.AreEqual(1, 1);
        }

        /// <summary>
        /// Convert date time string to folder name with defined format
        /// </summary>
        [TestMethod, TestCategory("Concept")]
        public void Concept02DateStringToFolderName()
        {
            string dateTaken = "11. 8. 2022 10:02:29";

            Console.WriteLine(dateTaken);

            DateTime dateTime = DateTime.Parse(dateTaken);

            Console.WriteLine(dateTime);

            string year = dateTime.Year.ToString();
            string month = dateTime.Month.ToString().PadLeft(2, '0');
            string day = dateTime.Day.ToString().PadLeft(2, '0'); ;

            Console.WriteLine(year + "." + month + "." + day);
        }


        /// <summary>
        /// Create output folder with specific format name
        /// Coppy picture file to the destination folder
        /// </summary>
        [TestMethod, TestCategory("Concept")]
        public void Concept03Folder()
        {
            string folderName = "2022.08.11";

            string outPath = Path.Combine(Common.GetOutputPath(), folderName);

            Directory.CreateDirectory(outPath);

            Assert.IsTrue(Directory.Exists(outPath));

            string sourcePath = Path.Combine(Common.GetPhotosPath(), Common.FILE1);
            string destPath = Path.Combine(outPath, Common.FILE1);

            File.Copy(sourcePath, destPath);

            Assert.IsTrue(File.Exists(destPath));
        }
    }
}
