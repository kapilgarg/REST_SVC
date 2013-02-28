using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using MVCServer.Models;

namespace MVCServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Upload(HttpPostedFileBase file)
        {
            string result = UploadFileToService(file);
            ViewBag.Result = result;
            return View("Index");
        }

        private string UploadFileToService(HttpPostedFileBase file)
        {
            //specify the url with fileName parameter
            string url = @"http://localhost:8080/TestService.svc/REST/UploadFile?fileName=" + file.FileName;
            var uri = new Uri(url);

            //create HttpWebRequest
            var request = (HttpWebRequest) WebRequest.Create(uri);
            request.ContentType = "application/octet-stream";
            request.Method = WebRequestMethods.Http.Post;
            
            //set request stream (Content)
            using (var requestStream = request.GetRequestStream())
            {
                byte[] fileDataInByte = null;
                using (BinaryReader binaryReader = new BinaryReader(file.InputStream))
                {
                    fileDataInByte = binaryReader.ReadBytes(file.ContentLength);
                }
                requestStream.Write(fileDataInByte, 0, fileDataInByte.Length);
            }

            //Call REST and get a response back
            using (var response = request.GetResponse())
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }

        public ActionResult XMLData()
        {
            return View();
        }

        public ActionResult SendXMLData()
        {
            string result = SendXML();
            ViewBag.Result = result;
            return View("XMLData");
        }

        private string SendXML()
        {
            //Send data in JSON format
            MyData dataToSend = new MyData() { Id = 2, Desc = "My XML Desc", StartDate = null };
            XmlSerializer ser = new XmlSerializer(dataToSend.GetType());
            StringWriter Writer = new StringWriter(); 
            ser.Serialize(Writer, dataToSend);
            string dataXML = Writer.ToString();
            Writer.Close();

            var uri = new Uri("http://localhost:8080/TestService.svc/REST/PostDataWithFormat?data=" + dataXML + "&format=xml");
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.ContentType = "text/xml";
            request.Method = WebRequestMethods.Http.Post;

            //In the config file it shouldn't have TransferMode = Streamed
            using (var response = request.GetResponse())
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }


        public ActionResult JSONData()
        {
            return View();
        }

        public ActionResult SendJSONData()
        {
            string result = SendJSON();
            ViewBag.Result = result;
            return View("JSONData");
        }


        private string SendJSON()
        {
            //Send data in JSON format
            MyData dataToSend = new MyData() { Id = 1, Desc = "My JSON Desc", StartDate = null };
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string dataJson = ser.Serialize(dataToSend);

            var uri = new Uri("http://localhost:8080/TestService.svc/REST/PostData?data=" + dataJson);
            var request = (HttpWebRequest)WebRequest.Create(uri);
            
            request.ContentType = "text/json";
            request.Method = WebRequestMethods.Http.Post;

            //In the config file it shouldn't have TransferMode = Streamed
            using (var response = request.GetResponse())
            {
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }
    }
}
