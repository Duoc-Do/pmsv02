using System;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WebApp.Services.Media;

namespace WebApp.Controllers
{
    public class CommonController : Controller
    {
        //
        // GET: /Common/

        //page not found
        public ActionResult PageNotFound()
        {
            this.Response.StatusCode = 404;
            this.Response.TrySkipIisCustomErrors = true;

            return View();
        }

        public ActionResult Pub(int id)
        {
            WebApp.Models.SenContext sendb = new WebApp.Models.SenContext();
            var sencompany = sendb.SenCompanys.Where(m => m.CompanyId == id).FirstOrDefault();

            if (sencompany == null) return Content("");

            string cnn = sencompany.ConnectionString;



            Areas.Accounting.Models.WebAppAccEntities db = new Areas.Accounting.Models.WebAppAccEntities(cnn);

            if (Request.Params["journalid"] != null)
            {
                int journalid = int.Parse(Request.Params["journalid"]);
                var model = db.GapJournals.SingleOrDefault(m => m.JournalId == journalid);
                //return Content(cnn);

                //return Json(new { gapjournalcares = model.GapJournalCares, gapjournalharvests = model.GapJournalHarvests }, JsonRequestBehavior.AllowGet);

                var modelJson = Newtonsoft.Json.JsonConvert.SerializeObject(model);

                return Json(model, JsonRequestBehavior.AllowGet);
            }



            return Content("");
        }


        [HttpPost]
        public ActionResult AsyncUpload(int size = 125)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.UploadPictures))
            //    return Json(new { success = false, error = "You do not have required permissions" }, "text/plain");

            //we process it distinct ways based on a browser
            //find more info here http://stackoverflow.com/questions/4884920/mvc3-valums-ajax-file-upload
            System.IO.Stream stream = null;
            var fileName = "";
            //var contentType = "";
            if (String.IsNullOrEmpty(Request["qqfile"]))
            {
                // IE
                HttpPostedFileBase httpPostedFile = Request.Files[0];
                if (httpPostedFile == null)
                    throw new ArgumentException("Chưa có tập tin tải lên");//No file uploaded
                stream = httpPostedFile.InputStream;
                fileName = System.IO.Path.GetFileName(httpPostedFile.FileName);
                //contentType = httpPostedFile.ContentType;
            }
            else
            {
                //Webkit, Mozilla
                stream = Request.InputStream;
                fileName = Request["qqfile"];
            }
            fileName = PictureService.GenName(fileName);
            PictureService.SavePicture(fileName, stream);

            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            return Json(new
            {
                success = true,
                imagename = fileName,
                imageurl = PictureService.GetPictureUrl(fileName, size)
            },
                "text/plain");
        }

        #region imgur
        public XDocument Upload(string imageAsBase64String)
        {
            XDocument result = null;
            using (var w = new WebClient())
            {
                var values = new NameValueCollection
                            {
                                {"image",  imageAsBase64String}
                            };

                w.Headers.Add("Authorization", "Client-ID 218e3ad7c482d94"); // user your own client-id of imgur website
                byte[] response = w.UploadValues("https://api.imgur.com/3/upload.xml", values);
                result = XDocument.Load(System.Xml.XmlReader.Create(new MemoryStream(response)));

                //link imgur
                //result.Element("data").Element("link").Value
            }


            return result;
        }

        public Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

        public System.IO.Stream Watermark(System.IO.Stream stream, string filename, bool isscale = true)
        {
            //// HERE we will upload image with watermark LOGO
            //string fileName = Guid.NewGuid() + Path.GetExtension(FU1.FileName);

            var fileExtension = System.IO.Path.GetExtension(filename);
            var streamout = new MemoryStream();
            Image upImage = Image.FromStream(stream);
            Image logoImage = Image.FromFile(Path.Combine(Server.MapPath("~/Content/media/uploads/watermark.png")));

            using (Graphics g = Graphics.FromImage(upImage))
            {
                //g.DrawImage(logoImage, new Point(upImage.Width - logoImage.Width - 10, 10));
                //upImage = this.ScaleImage(upImage, upImage.Width, upImage.Height+53);
                if (isscale)
                {
                    logoImage = this.ScaleImage(logoImage, upImage.Width, upImage.Height);
                }
                g.DrawImage(logoImage, new Point(0, upImage.Height - logoImage.Height));

                //upImage.Save(Path.Combine(Server.MapPath("~/UploadFiles"), fileName));
                //Image1.ImageUrl = "~/UploadFiles" + "//" + fileName;

                upImage.Save(streamout, upImage.RawFormat);
                streamout.Position = 0;
            }

            return streamout;
        }


        [HttpPost]
        public ActionResult AsyncUpload2Imgur(bool isscale = true)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.UploadPictures))
            //    return Json(new { success = false, error = "You do not have required permissions" }, "text/plain");

            //we process it distinct ways based on a browser
            //find more info here http://stackoverflow.com/questions/4884920/mvc3-valums-ajax-file-upload
            System.IO.Stream stream = null;
            string fileName = "";
            string _linkimgur = "";

            //var contentType = "";
            if (String.IsNullOrEmpty(Request["qqfile"]))
            {
                // IE
                HttpPostedFileBase httpPostedFile = Request.Files[0];
                if (httpPostedFile == null)
                    throw new ArgumentException("Chưa có tập tin tải lên");//No file uploaded
                stream = httpPostedFile.InputStream;
                fileName = System.IO.Path.GetFileName(httpPostedFile.FileName);
            }
            else
            {
                //Webkit, Mozilla
                stream = Request.InputStream;
                fileName = Request["qqfile"];
            }

            stream = Watermark(stream, fileName, isscale);
            var fileBinary = new byte[stream.Length];
            stream.Read(fileBinary, 0, fileBinary.Length);
            stream.Close();
            string fileContent = Convert.ToBase64String(fileBinary);
            var response = this.Upload(fileContent);
            _linkimgur = response.Element("data").Element("link").Value;


            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            return Json(new
            {
                success = true,
                imagename = _linkimgur,
                imageurl = _linkimgur
            },
                "text/plain");
        }

        #endregion



    }
}
