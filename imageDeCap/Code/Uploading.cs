using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//me
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

using System.Net;
using System.IO;
using System.Text.RegularExpressions;

using System.Collections.Specialized;
using System.ComponentModel;

using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API;
using Imgur.API.Models;

using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace imageDeCap
{
    public static class Uploading
    {
        public static void UploadImage_Imgur(object sender, DoWorkEventArgs e)
        {
            byte[] FileData = (byte[])e.Argument;

            try
            {
                var client = new ImgurClient("da05117bbfa9bda");
                var endpoint = new ImageEndpoint(client);
                IImage image = endpoint.UploadImageBinaryAsync(FileData).GetAwaiter().GetResult();
                e.Result = (image.Link, FileData);
            }
            catch (ImgurException imgurEx)
            {
                e.Result = ("failed, " + imgurEx.Message, FileData);
            }
        }

        public static void UploadGif_Webmshare(object sender, DoWorkEventArgs e)
        {
            byte[] FileData = (byte[])e.Argument;
            var result = Webmshare.Upload(FileData);
            if (result.UploadSuccessfull)
            {
                e.Result = (result.Message, FileData);
            }
            else
            {
                e.Result = ("failed, " + result.Message, FileData);
            }
        }

        public static void UploadGif_Gfycat(object sender, DoWorkEventArgs e)
        {
            byte[] FileData = (byte[])e.Argument;
            var result = Gfycat.Upload(FileData);
            if (result.UploadSuccessfull)
            {
                e.Result = (result.Message, FileData);
            }
            else
            {
                e.Result = ("failed, " + result.Message, FileData);
            }
        }

        public static void UploadGif_Imgur(object sender, DoWorkEventArgs e)
        {
            //byte[] FileData = (byte[])e.Argument;
            //
            //
            //var client = new ImgurClient("da05117bbfa9bda");
            //var endpoint = new ImageEndpoint(client);
            //
            //endpoint.UploadImageUrlAsync();
            //var endpoint = new ImageEndpoint(client);
            //IImage image = endpoint.UploadImageBinaryAsync(FileData).GetAwaiter().GetResult();
            //e.Result = (image.Link, FileData);

        }

        public static void UploadToFTP(object sender, DoWorkEventArgs e)
        {
            object[] arguments = (object[])e.Argument;
            string url = (string)arguments[0];
            string username = (string)arguments[1];
            string password = (string)arguments[2];
            byte[] filedata = (byte[])arguments[3];
            string filename = (string)arguments[4];

            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url + (url.EndsWith("/") ? "" : "/") + filename);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = new NetworkCredential(username, password);

            // Copy the contents of the file to the request stream.
            byte[] fileContents = filedata;
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            response.Close();
        }

        public static void UploadPastebin(object sender, DoWorkEventArgs e)
        {
            //private string ILoginURL = "http://pastebin.com/api/api_login.php";
            string IPostURL = "http://pastebin.com/api/api_post.php";
            string IDevKey = "4d1c2c0bb6fa2e5c1403cedccc50bfd5";
            string IUserKey = null;

            string IBody = (string)e.Argument;
            string ISubj = Preferences.PastebinSubjectLine;

            if (string.IsNullOrEmpty(IBody.Trim())) { e.Result = "failed"; }
            if (string.IsNullOrEmpty(ISubj.Trim())) { e.Result = "failed"; }

            NameValueCollection IQuery = new NameValueCollection();

            IQuery.Add("api_dev_key", IDevKey);
            IQuery.Add("api_option", "paste");
            IQuery.Add("api_paste_code", IBody);
            IQuery.Add("api_paste_private", "0");
            IQuery.Add("api_paste_name", ISubj);
            IQuery.Add("api_paste_expire_date", "N");
            IQuery.Add("api_paste_format", "text");
            IQuery.Add("api_user_key", IUserKey);

            string IResponse = "";

            using (WebClient IClient = new WebClient())
            {
                try
                {
                    IResponse = Encoding.UTF8.GetString(IClient.UploadValues(IPostURL, IQuery));
                }
                catch (Exception ee)
                {
                    IResponse = "failed, " + ee.Message;
                }
            }
            e.Result = IResponse;
        }
    }
}
