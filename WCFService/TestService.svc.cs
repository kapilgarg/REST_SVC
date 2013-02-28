using System;
using System.IO;
using System.ServiceModel.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace WCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TestService" in code, svc and config file together.
    public class TestService : ITestService
    {
        [WebInvoke(Method = "POST", UriTemplate = "UploadFile?fileName={fileName}")]
        public string UploadFile(string fileName, Stream fileContents)
        {
            //save file
            try
            {
                string absFileName = string.Format("{0}\\FileUpload\\{1}"
                                        , AppDomain.CurrentDomain.BaseDirectory
                                        , fileName);
                using (FileStream fs = new FileStream(absFileName, FileMode.Create))
                {
                    fileContents.CopyTo(fs);
                    fileContents.Close();
                }
                return "Upload OK";
            }
            catch(Exception ex)
            {
                return "FAIL ==> " + ex.Message;
            }
        }


        [WebInvoke(Method = "POST", UriTemplate = "PostData?data={data}")]
        public string PostData(string data)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            MyData obj = (MyData)ser.Deserialize(data, typeof(MyData));
            return obj.Id + ": " + obj.Desc;
        }

        [WebInvoke(Method = "POST", UriTemplate = "PostDataWithFormat?data={data}&format={format}")]
        public string PostDataWithFormat(string data, string format)
        {
            string result = string.Empty;
            if (format == "json")
                result = PostData(data);
            else if (format == "xml")
            {
                MyData myData = null;
                XmlSerializer ser = new XmlSerializer(typeof(MyData));
                using (StringReader strReader = new StringReader(data))
                {
                    using (XmlTextReader xmlReader = new XmlTextReader(strReader))
                    {
                        myData = (MyData) ser.Deserialize(xmlReader);
                    }
                }
                result = myData.Id + ": " + myData.Desc;
            }
            else
            {
                throw new Exception("format not available");
            }
            return result;
        }


        [WebGet]
        public Stream DownloadFile(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
