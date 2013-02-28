using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WCFService
{
    [ServiceContract]
    public interface ITestService
    {
        [OperationContract]
        string UploadFile(string fileName, Stream fileContents);

        [OperationContract]
        string PostData(string data);

        [OperationContract]
        Stream DownloadFile(string fileName);

        [OperationContract]
        string PostDataWithFormat(string data, string format);

    }
}
