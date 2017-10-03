using System;
using System.Web;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace WebApp.Services.Media
{

    public static class PictureService
    {
        ////Đường dẫn vật lý trên server
        //public static string FullPathProduct = HttpContext.Current.Server.MapPath("~/images/products");
        //public static string FullPathProductThumbs = HttpContext.Current.Server.MapPath("~/images/products/thumbs");


        ////Đường dẫn tương đối trên server
        //public static string PathProduct = "/images/products";
        //public static string PathProductThumbs = "/images/products/thumbs";
        //public static string PictureDefault = "/images/products/noDefaultImage.gif";

        //Đường dẫn vật lý trên server
        public static string FullPathImage = HttpContext.Current.Server.MapPath("~/images/logos");
        public static string FullPathImageThumbs = HttpContext.Current.Server.MapPath("~/images/logos/thumbs");


        //Đường dẫn tương đối trên server
        public static string PathImage = "/images/logos";
        public static string PathImageThumbs = "/images/logos/thumbs";
        //public static string PictureDefault = "/images/logos/noDefaultImage.png";
        public static string PictureDefault = "noDefaultImage.png";

        //Chất lượng ảnh khi nén
        public static long PictureQuality = 100L;


        /// <summary>
        /// Calculates picture dimensions whilst maintaining aspect
        /// </summary>
        /// <param name="originalSize">The original picture size</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <returns></returns>
        public static System.Drawing.Size CalculateDimensions(System.Drawing.Size originalSize, int targetSize)
        {
            if (targetSize==0) // lấy lại kích thước thực tế
            {
                return originalSize;
            }
            var newSize = new System.Drawing.Size();
            if (originalSize.Height > originalSize.Width) // portrait 
            {
                newSize.Width = (int)(originalSize.Width * (float)(targetSize / (float)originalSize.Height));
                newSize.Height = targetSize;
            }
            else // landscape or square
            {
                newSize.Height = (int)(originalSize.Height * (float)(targetSize / (float)originalSize.Width));
                newSize.Width = targetSize;
            }
            return newSize;
        }

        /// <summary>
        /// Returns the first ImageCodecInfo instance with the specified mime type.
        /// </summary>
        /// <param name="mimeType">Mime type</param>
        /// <returns>ImageCodecInfo</returns>
        public static ImageCodecInfo GetImageCodecInfoFromMimeType(string mimeType)
        {
            var info = ImageCodecInfo.GetImageEncoders();
            foreach (var ici in info)
                if (ici.MimeType.Equals(mimeType, StringComparison.OrdinalIgnoreCase))
                    return ici;
            return null;
        }

        /// <summary>
        /// Returns the first ImageCodecInfo instance with the specified extension.
        /// </summary>
        /// <param name="fileExt">File extension</param>
        /// <returns>ImageCodecInfo</returns>
        public static string GetContentTypeFromExtension(string fileExt)
        {
            //fileExt = fileExt.TrimStart(".".ToCharArray()).ToLower().Trim();
            //contentType is not always available 
            //that's why we manually update it here
            //http://www.sfsu.edu/training/mimetype.htm
            string contentType = "";
            switch (fileExt)
            {
                case ".bmp":
                    contentType = "image/bmp";
                    break;
                case ".gif":
                    contentType = "image/gif";
                    break;
                case ".jpeg":
                case ".jpg":
                case ".jpe":
                case ".jfif":
                case ".pjpeg":
                case ".pjp":
                    contentType = "image/jpeg";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".tiff":
                case ".tif":
                    contentType = "image/tiff";
                    break;
                default:
                    break;
            }
            return contentType;
        }

        public static ImageCodecInfo GetImageCodecInfoFromExtension(string fileExt)
        {
            fileExt = fileExt.TrimStart(".".ToCharArray()).ToLower().Trim();
            switch (fileExt)
            {
                case "jpg":
                case "jpeg":
                    return GetImageCodecInfoFromMimeType("image/jpeg");
                case "png":
                    return GetImageCodecInfoFromMimeType("image/png");
                case "gif":
                    //use png codec for gif to preserve transparency
                    //return GetImageCodecInfoFromMimeType("image/gif");
                    return GetImageCodecInfoFromMimeType("image/png");
                default:
                    return GetImageCodecInfoFromMimeType("image/jpeg");
            }
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <param name="useSsl">Value indicating whether to get SSL protected picture URL; null to use the same value as the current page</param>
        /// <returns>Picture URL</returns>
        public static string GetPictureUrl(string filename, int targetSize = 0)
        {


            if (String.IsNullOrEmpty(filename))
            {
                return string.Format("{0}/{1}", PathImage, PictureDefault);
            }
            if (filename.Contains("http://") || filename.Contains("https://"))
            {
                return filename;
            }
            string url = string.Empty;

            var fileExtension = System.IO.Path.GetExtension(filename);


            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            //gen tên file của thumbs tùy theo kích thước

            string filenamethumbs = string.Format("{0}_{1}{2}", filename.ToLower().Replace(fileExtension, ""), targetSize, fileExtension);

            //nếu không chỉ định đường dẫn hình thì lấy ngầm định này - sẽ xử lý khai báo sau

            if (System.IO.File.Exists(Path.Combine(FullPathImage, filename)))
            {
                //Nếu chưa có hình nhỏ thì lưu vào thư mục hình nhỏ
                string filePath = Path.Combine(FullPathImage, filename);
                if (!System.IO.File.Exists(Path.Combine(FullPathImageThumbs, filenamethumbs)))
                {
                    var b = new Bitmap(filePath);

                    var newSize = CalculateDimensions(b.Size, targetSize);

                    if (newSize.Width < 1)
                        newSize.Width = 1;
                    if (newSize.Height < 1)
                        newSize.Height = 1;

                    var newBitMap = new Bitmap(newSize.Width, newSize.Height);
                    var g = Graphics.FromImage(newBitMap);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.DrawImage(b, 0, 0, newSize.Width, newSize.Height);
                    var ep = new EncoderParameters(1);
                    ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, PictureQuality);
                    ImageCodecInfo ici = GetImageCodecInfoFromExtension(fileExtension);
                    if (ici == null)
                        ici = GetImageCodecInfoFromMimeType("image/jpeg");
                    //contentType
                    string fullfilename = Path.Combine(FullPathImageThumbs, filenamethumbs);
                    newBitMap.Save(fullfilename, ici, ep);
                    newBitMap.Dispose();
                    b.Dispose();
                }

                url = string.Format("{0}/{1}", PathImageThumbs, filenamethumbs);

            }
            return url;
        }

        /// <summary>
        /// Get a picture URL
        /// </summary>
        /// <param name="picture">Picture instance</param>
        /// <param name="targetSize">The target picture size (longest side)</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <param name="useSsl">Value indicating whether to get SSL protected picture URL; null to use the same value as the current page</param>
        /// <returns>Picture URL</returns>
        public static string GetPictureUrl(string filename, int width = 0,int height=0 )
        {


            if (String.IsNullOrEmpty(filename))
            {
                //return PictureDefault;
               filename = PictureDefault;
            }
            if (filename.Contains("http://") || filename.Contains("https://"))
            {
                return filename;
            }
            string url = string.Empty;

            var fileExtension = System.IO.Path.GetExtension(filename);


            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            //gen tên file của thumbs tùy theo kích thước

            string filenamethumbs = string.Format("{0}_{1}{2}{3}", filename.ToLower().Replace(fileExtension, ""), width,height, fileExtension);

            //nếu không chỉ định đường dẫn hình thì lấy ngầm định này - sẽ xử lý khai báo sau

            if (System.IO.File.Exists(Path.Combine(FullPathImage, filename)))
            {
                //Nếu chưa có hình nhỏ thì lưu vào thư mục hình nhỏ
                string filePath = Path.Combine(FullPathImage, filename);
                if (!System.IO.File.Exists(Path.Combine(FullPathImageThumbs, filenamethumbs)))
                {
                    var b = new Bitmap(filePath);

                    var newSize = CalculateDimensions(b.Size, width);

                    if (newSize.Width < 1)
                        newSize.Width = 1;
                    if (newSize.Height < 1)
                        newSize.Height = 1;

                    newSize.Width = width;
                    newSize.Height = height;

                    var newBitMap = new Bitmap(newSize.Width, newSize.Height);
                    var g = Graphics.FromImage(newBitMap);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.DrawImage(b, 0, 0, newSize.Width, newSize.Height);
                    var ep = new EncoderParameters(1);
                    ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, PictureQuality);
                    ImageCodecInfo ici = GetImageCodecInfoFromExtension(fileExtension);
                    if (ici == null)
                        ici = GetImageCodecInfoFromMimeType("image/jpeg");
                    //contentType
                    string fullfilename = Path.Combine(FullPathImageThumbs, filenamethumbs);
                    newBitMap.Save(fullfilename, ici, ep);
                    newBitMap.Dispose();
                    b.Dispose();
                }

                url = string.Format("{0}/{1}", PathImageThumbs, filenamethumbs);

            }
            return url;
        }

        public static string GenName(string filename)
        {
            var fileExtension = System.IO.Path.GetExtension(filename);

            string _filename =string.Format("{0}{1}", DateTime.UtcNow.ToString("yyyyMMddhhss"),fileExtension);

            return _filename;
        }

        public static bool SavePicture(string filename, System.IO.Stream stream)
        {
            try
            {
                //Lưu file hình với tham số là stream và filename
                var contentType = "";
                var fileBinary = new byte[stream.Length];
                stream.Read(fileBinary, 0, fileBinary.Length);

                var fileExtension = System.IO.Path.GetExtension(filename);
                if (!String.IsNullOrEmpty(fileExtension))
                    fileExtension = fileExtension.ToLowerInvariant();

                contentType = PictureService.GetContentTypeFromExtension(fileExtension);

                string LocalImagePath = PictureService.FullPathImage;
                //Lưu vào thư mục hình
                System.IO.File.WriteAllBytes(System.IO.Path.Combine(LocalImagePath, filename), fileBinary);

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}