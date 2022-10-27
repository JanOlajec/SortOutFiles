using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace SortOutFiles
{
    /// <summary>
    /// Sorting picture files in to new created folders with name based on taken date of the particular picture
    /// </summary>
    public class SortOutPhotoFiles
    {
        /// <summary>
        /// Input folder with all picture files for string 
        /// </summary>
        private readonly string sourcePath;

        /// <summary>
        /// Destination folder. In this folder will be created sub-folders with DateTaken names. 
        /// Picture files will be copied in to this created sub-folders
        /// </summary>
        private readonly string destPath;

        /// <summary>
        /// Sorting picture files in to new created folders with name based on taken date of the particular picture
        /// </summary>
        /// <param name="sourceFolderPath"></param>
        /// <param name="destFolderPath"></param>
        public SortOutPhotoFiles(string sourceFolderPath, string destFolderPath)
        {
            /// HOW TO: 
            /// Obtain file creation date (DateTaken)
            /// Create folder with name derived from file creation date if not exist
            /// Move file to the folder - COPY is safer

            this.PathFullTest(sourceFolderPath);
            this.sourcePath = sourceFolderPath;

            this.StringTest(destFolderPath);
            this.destPath = destFolderPath;

            // OPTIONS1: Only top (defined) folder is processed:
            //string[] allFilesPaths = Directory.GetFiles(this.sourcePath);
            // OPTIONS2: Top (defined) folder and sub-folders are processed:
            string[] allFilesPaths = Directory.GetFiles(this.sourcePath, "*.*", SearchOption.AllDirectories);

            if(allFilesPaths.Length == 0)
            {
                throw new ArgumentException("Source path is empty. There are no JPG files for sorting", sourcePath);
            }

            foreach (var oneFilePath in allFilesPaths)
            {
                string dateTaken = this.GetDateTaken(oneFilePath);

                string folderName;

                if (dateTaken == null)
                {
                    // DateTake do not exist in file. Use create date instead.
                    dateTaken = GetCreationDate(oneFilePath);
                    folderName = FolderNameBasedOnDate(dateTaken);
                    folderName = Path.Combine("_NoDateTaken", folderName);
                }
                else
                {
                    folderName = FolderNameBasedOnDate(dateTaken);
                }

                this.PictureFileCopy(oneFilePath, folderName);
            }
        }

        /// <summary>
        /// Text string testing for null or empty
        /// </summary>
        /// <param name="text"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private void StringTest(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(text, "Text string is null or empty");
            }
        }

        /// <summary>
        /// Path string testing for null, empty and existence
        /// </summary>
        /// <param name="path"></param>
        /// <exception cref="ArgumentException"></exception>
        private void PathFullTest(string path)
        {
            this.StringTest(path);

            if (!Directory.Exists(path))
            {
                throw new ArgumentException(string.Format($"Provided path string is not valid: '{path}'"), path);
            }
        }

        /// <summary>
        /// Get taken date and time from picture file
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Date and time</returns>
        private string GetDateTaken(string path)
        {
            string dateTaken = null;
            FileInfo fileInformation = new FileInfo(path);
            FileStream fileStream = new FileStream(fileInformation.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            
            try
            {
                // If file format is not supported e.g. TXT, DOC, then exception is throw. If possible creation date is used on upper level.
                BitmapSource imgeSource = BitmapFrame.Create(fileStream);
                BitmapMetadata imageMetaData = (BitmapMetadata)imgeSource.Metadata;
                dateTaken = imageMetaData.DateTaken;
            }
            catch (NotSupportedException)
            {
                Console.WriteLine($"WARNING: not supported file type: {path}");
            }

            return dateTaken;
        }

        private string GetCreationDate(string path)
        {
            FileInfo fileInformation = new FileInfo(path);
            var creationDate = fileInformation.CreationTime.ToString();
            return creationDate;
        }

        /// <summary>
        /// Setup specific folder name derived from date: YYYY.MM.DD
        /// </summary>
        /// <param name="dateTaken"></param>
        /// <returns></returns>
        private static string FolderNameBasedOnDate(string dateTaken)
        {
            DateTime dateTime = DateTime.Parse(dateTaken);
            string year = dateTime.Year.ToString();
            string month = dateTime.Month.ToString().PadLeft(2, '0');
            string day = dateTime.Day.ToString().PadLeft(2, '0'); ;

            string folderName = year + "." + month + "." + day;
            return folderName;
        }

        /// <summary>
        /// Copy picture file to the destination folder. 
        /// Destination folder is a SortOut folder with specific name derived from taken date
        /// </summary>
        /// <param name="pictureFilePath"></param>
        /// <param name="destFolderName"></param>
        private void PictureFileCopy(string pictureFilePath, string destFolderName)
        {
            string outPath = Path.Combine(this.destPath, destFolderName);

            if (!Directory.Exists(outPath))
            {
                try
                {
                    Directory.CreateDirectory(outPath);
                }
                catch (Exception e)
                {
                    // If output directory is not created, e.g. due to access rights, it throw 'UnauthorizedAccessException'.
                    // Try it for next Output directory in the list.
                    Console.WriteLine($"WARNING: Output folder not created: {outPath}");
                    Console.WriteLine($"Reason is exception:\n{e.Message}\n");
                    
                    // File copy is skipped, simply return.
                    return;
                }

                Directory.CreateDirectory(outPath);
            }

            string destPath = Path.Combine(outPath, Path.GetFileName(pictureFilePath));

            try
            {
                File.Copy(pictureFilePath, destPath);
            }
            catch (Exception e)
            {
                // If file exist in output folder the System.IO.IOException is throw
                Console.WriteLine($"WARNING: File is not copied to the destination: {outPath}");
                Console.WriteLine($"Reason is exception:\n{e.Message}\n");

                // File copy is skipped, simply return.
                return;
            }
        }
    }
}
