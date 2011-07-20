using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using AgileFx.ORM;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace canyoucode.Core.Utils
{
    public static class FileUtil
    {
        private static string PUBLIC_DATA_STORE = "PublicDataStore";

        public static string GetImage(int width, string path)
        {
            var extn = Path.GetExtension(path);
            return path.Replace(extn, "-" + width + extn);
        }

        //Creates File Path from Url
        //public static Func<string, string> MapPath { get; set; }

        //Creates a Url from a File Path
        //public static Func<string, string> GetUrlFromPath { get; set; }

        public static string SaveFile(HttpPostedFileBase file, string uploadDirectory)
        {
            return SaveFile(file, Common.RandomString(), uploadDirectory);
        }

        /* ----- old file storage --------------
        private static string SaveFile(HttpPostedFileBase file, string fileNameWithoutExtension, string uploadFolder)
        {
            var validFileTypes = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".txt", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", 
                ".pdf", ".odt" };

            var timeStamp = DateTime.UtcNow.ToString("yyyyMMdd");

            var saveFolder = MapPath(Path.Combine(uploadFolder, timeStamp));

            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);

            var ext = Path.GetExtension(file.FileName).ToLower();

            if (!validFileTypes.Contains(ext))
                throw new Exception("Invalid file type.");

            var filename = fileNameWithoutExtension + ext;
            var filePath = Path.Combine(saveFolder, filename);

            file.SaveAs(filePath);

            return filePath;
        }
          */

        public static string SaveResizedImage(HttpPostedFileBase file, string uploadDirectory, int maxWidth, int maxHeight, bool saveOriginal, bool upscale = false)
        {
            var fileName = Common.RandomString();
            string originalPath = string.Empty;
            if (saveOriginal)
            {
                originalPath = SaveFile(file, fileName, uploadDirectory);
            }
            var newPath = ResizeImage(file, uploadDirectory, fileName, maxWidth, maxHeight, upscale);
            return saveOriginal ? originalPath : newPath;
        }

        private static string ResizeImage(HttpPostedFileBase file, string storename, string newFileNameWithoutExtension, int maxWidth, int maxHeight, bool upscale = false)
        {
            //For now we scale down only to JPEGs
            var extension = Path.GetExtension(file.FileName).ToLower();

            var timeStamp = DateTime.UtcNow.ToString("yyyyMMdd");
            var imageFormat = GetImageFormat(extension);
            var filename = timeStamp + "_" + newFileNameWithoutExtension + "-" + maxWidth + extension;

            var image = Image.FromStream(file.InputStream);

            return ResizeImage(image, maxWidth, maxHeight, storename, filename, upscale, imageFormat);
        }

        private static string ResizeImage(Image image, int maxWidth, int maxHeight, string storename, string filename,
            bool upscale, ImageFormat outputFormat)
        {
            float scaleFactor = 1.0f;

            //Do we need to scale?
            //  1. if h > hMax OR w > wMax (scale down)
            //  2. if hMax > h AND wMax > w (scale up)

            //To find scaleFactor, always choose the bigger dimension.

            if ((image.Width > maxWidth || image.Height > maxHeight)
                || (image.Width < maxWidth && image.Height < maxHeight))
            {
                scaleFactor = ((float)image.Width / (float)maxWidth > (float)image.Height / (float)maxHeight) ?
                    (float)maxWidth / (float)image.Width : (float)maxHeight / (float)image.Height;
            }

            string fileUrl = string.Empty;
            if (scaleFactor < 1 || (scaleFactor > 1 && upscale))
            {
                int newHeight = (int)((scaleFactor * image.Height) + 0.5);
                int newWidth = (int)((scaleFactor * image.Width) + 0.5);

                Bitmap bitmap = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
                bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.Clear(Color.White);
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    graphics.DrawImage(image,
                        new Rectangle(0, 0, newWidth, newHeight),
                        new Rectangle(0, 0, image.Width, image.Height),
                        GraphicsUnit.Pixel);
                }

                if (outputFormat == ImageFormat.Jpeg)
                {
                    fileUrl = SaveJpeg(storename, filename, bitmap.Save);
                }
                else
                {
                    fileUrl = SaveImage(storename, filename, outputFormat, bitmap.Save);
                }
            }
            else
            {
                if (outputFormat == ImageFormat.Jpeg)
                {
                    fileUrl = SaveJpeg(storename, filename, image.Save);
                }
                else
                {
                    fileUrl = SaveImage(storename, filename, outputFormat, image.Save);
                }
            }

            return fileUrl;
        }

        private static string SaveImage(string storename, string filename, ImageFormat format,
            Action<Stream, ImageFormat> saveFunc)
        {
            MemoryStream imageStream = new MemoryStream();
            saveFunc(imageStream, format);
            imageStream.Position = 0;

            var blob = SaveBlob(storename, filename, imageStream, "resized-image", format.ToString());
            return blob.Uri.ToString();
        }

        private static string SaveJpeg(string storename, string filename,
            Action<Stream, ImageCodecInfo, EncoderParameters> saveFunc)
        {
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder quality = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters encoderParameters = new EncoderParameters(1);
            EncoderParameter encoderParameter = new EncoderParameter(quality, 85L);
            encoderParameters.Param[0] = encoderParameter;
            
            MemoryStream imageStream = new MemoryStream();
            saveFunc(imageStream, jgpEncoder, encoderParameters);
            imageStream.Position = 0;

            var blob = SaveBlob(storename, filename, imageStream, "resized-image", "image/jpeg");
            return blob.Uri.ToString();
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }


        private static ImageFormat GetImageFormat(string extension)
        {
            var imageFormat = ImageFormat.Jpeg;
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    {
                        imageFormat = ImageFormat.Jpeg;
                        break;
                    }
                case ".png":
                    {
                        imageFormat = ImageFormat.Png;
                        break;
                    }
                case ".gif":
                    {
                        imageFormat = ImageFormat.Gif;
                        break;
                    }
                case ".bmp":
                    {
                        imageFormat = ImageFormat.Bmp;
                        break;
                    }
            }

            return imageFormat;
        }

        private static CloudBlobContainer GetPublicContainer(string name)
        {
            //CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            //        configSetter(ConfigurationManager.AppSettings[configName]));

            var cloudStorageAccount = CloudStorageAccount.FromConfigurationSetting(PUBLIC_DATA_STORE);

            var blobClient = cloudStorageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(name);

            container.CreateIfNotExist();

            var permissions = new BlobContainerPermissions();
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;
            container.SetPermissions(permissions);

            return container;
        }

        private static string SaveFile(HttpPostedFileBase file, string fileNameWithoutExtension, string storeName)
        {
            var validFileTypes = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".txt", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx", 
                ".pdf", ".odt" };

            var ext = Path.GetExtension(file.FileName).ToLower();
            if (!validFileTypes.Contains(ext))
                throw new Exception("Invalid file type.");

            var timeStamp = DateTime.UtcNow.ToString("yyyyMMdd");

            var blob = SaveBlob(storeName, timeStamp + "_" + fileNameWithoutExtension + ext, file.InputStream,
                file.FileName, file.ContentType);

            return blob.Uri.ToString();
        }

        private static CloudBlob SaveBlob(string storeName, string filename, Stream stream, string originalFilename, string contentType)
        {
            var blob = GetPublicContainer(storeName).GetBlobReference(filename);
            blob.UploadFromStream(stream);
            blob.Metadata["FileName"] = originalFilename;
            blob.SetMetadata();
            blob.Properties.ContentType = contentType;
            blob.SetProperties();

            return blob;
        }
        
    }
}
