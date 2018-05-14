using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Comm
{
    public class WebComm
    {
        public static string UploadMultipart(string url, string file, string paramName, string contentType, NameValueCollection nvc)
        {
            //WebResult result = new WebResult();

            //Console.WriteLine(string.Format("Uploading {0} to {1}", file, url));
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
            string header = string.Format(headerTemplate, paramName, file, contentType);
            byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();

            byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                //Console.WriteLine(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
                //result = JsonConvert.DeserializeObject<WebResult>(reader2.ReadToEnd());
                return reader2.ReadToEnd();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
                //result.result = false;
                //result.message = "ไม่สามารถติดต่อเครื่องเซิร์ฟเวอร์ได้, กรุณาตรวจสอบการเชื่อมต่ออินเทอร์เน็ต";
                MessageBox.Show("ไม่สามารถติดต่อเครื่องเซิร์ฟเวอร์ได้, กรุณาตรวจสอบการเชื่อมต่ออินเทอร์เน็ต", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
            finally
            {
                wr = null;
            }
        }

        public static string PostData(string url, string json_post_data)
        {
            //WebResult result = new WebResult();
            //result.result = false;
            string str_result = string.Empty;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = json_post_data;

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                //result = JsonConvert.DeserializeObject<WebResult>(streamReader.ReadToEnd());
                str_result = streamReader.ReadToEnd();
            }

            return str_result;
        }

        //public static string GetMachineCodeFromToken(string url, string token_key)
        //{
        //    //string machine_code = string.Empty;

            //NameValueCollection nvc = new NameValueCollection();
            //nvc.Add("token_key", token_key);
            //WebResult wr = WebComm.PostData(url, "application/x-www-form-urlencoded", nvc);
            //if (wr.result == true)
            //{
            //    machine_code = wr.message.Trim();
            //}

            //return machine_code;
        //}
    }
}
