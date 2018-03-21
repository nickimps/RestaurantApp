using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Imaging;
using System.IO;

using System.Windows.Controls;
using System.Reflection;
using System.Resources;
using System.Windows;

namespace HelperMethods481
{
    /// <summary>
    /// Helps accessing Embedded Resource build action files within code behind
    /// </summary>
    /// <author>
    /// David Ledo, 2018
    /// </author>
    public class AssemblyManager
    {
        #region Helper Functions

        /// <summary>
        /// Helper method that gets a Bitmap Image from a Stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static BitmapImage ImageFromStream(Stream stream)
        {
            BitmapImage image = new BitmapImage();

            // bitmaps always have a begin and end init
            image.BeginInit();
            image.StreamSource = stream;
            // access the stream when loading to prevent it
            // from accessing it when it might already be closed
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            // we do this so that the image can be
            // used in multiple threads as well as
            // to improve performance
            image.Freeze();

            return image;
        }

        private static Stream GetStreamForEmbeddedResourceFile(string fileName)
        {
            // Assembly requires using System.ComponentModel
            // Stream requires using System.IO
            Assembly executingAssembly = Assembly.GetExecutingAssembly();

            return executingAssembly.GetManifestResourceStream(fileName);
        }

        /// <summary>
        /// Helper function to get all embedded resources (GetAllEmbeddedResourceFilesEndingWith) of a specific string ending
        /// </summary>
        /// <param name="file"></param>
        /// <param name="endings"></param>
        /// <returns></returns>
        private static bool ChainOrEndings(string file, string[] endings)
        {
            foreach (string ending in endings)
            {
                if (file.EndsWith(ending))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Access Methods

        public static List<string> GetAllEmbeddedResourceFiles()
        {
            // Assembly requires using System.ComponentModel
            Assembly executingAssembly = Assembly.GetExecutingAssembly();

            // Make sure images are set as Embedded Resource, Build Action "Copy Always" or "Copy if Newer"
            // Get all the files associated as embedded resources
            List<string> assemblyFiles = executingAssembly.GetManifestResourceNames().ToList<string>();

            return assemblyFiles;
        }

        /// <summary>
        /// Gets all the resources that end with a particular string (e.g. ".txt", ".png") 
        /// </summary>
        /// <param name="endOfFileNames">strings with the file ending (e.g. ".txt") </param>
        /// <returns></returns>
        public static List<string> GetAllEmbeddedResourceFilesEndingWith(params string[] endOfFileNames)
        {
            return GetAllEmbeddedResourceFiles().Where(file => ChainOrEndings(file, endOfFileNames)).ToList<string>();
        }

        /// <summary>
        /// Gets Embedded Resource File from a string
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileFromName(string fileName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceNames().ToList().Where(path => path.Contains(fileName)).First();
        }

        /// <summary>
        /// Gets all images that are set as Embedded Resources (build action should be "Copy if Newer")
        /// </summary>
        /// <returns></returns>
        public static List<BitmapImage> GetImagesFromEmbeddedResources()
        {
            List<BitmapImage> images = new List<BitmapImage>();

            List<string> imageFiles = GetAllEmbeddedResourceFilesEndingWith(".jpg", ".png");

            foreach (string fileName in imageFiles)
            {
                Stream streamFromFileName = GetStreamForEmbeddedResourceFile(fileName);
                BitmapImage imageToLoad = new BitmapImage();
                imageToLoad.BeginInit();
                imageToLoad.StreamSource = streamFromFileName;
                imageToLoad.EndInit();
                imageToLoad.DecodePixelWidth = 50;
                imageToLoad.DecodePixelHeight = 50;

                images.Add(imageToLoad);
            }

            return images;
        }

        public static Image GetImageFromEmbeddedResources(string fileName)
        {
            Stream streamFromFileName = GetStreamForEmbeddedResourceFile(GetFileFromName(fileName));
            Image imageToLoad = new Image();
            imageToLoad.BeginInit();
            imageToLoad.Source = ImageFromStream(streamFromFileName);
            imageToLoad.EndInit();
            imageToLoad.Width = 100;
            imageToLoad.Height = 100;
            return imageToLoad;
        }

        #endregion

    }
}