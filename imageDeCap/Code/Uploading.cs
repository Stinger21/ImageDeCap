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
            ProgressWindow w = new ProgressWindow();
            w.SetProgress($"Uploading to Imgur", 50, 100);

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
                e.Result = ($"failed, {imgurEx.Message}", FileData);
            }

            w.Close();
        }

        //public static void UploadClip_Webmshare(object sender, DoWorkEventArgs e)
        //{
        //    ProgressWindow w = new ProgressWindow();
        //    w.SetProgress($"Uploading to Webmshare", 50, 100);
        //
        //    byte[] FileData = (byte[])e.Argument;
        //    var result = Webmshare.Upload(FileData);
        //    if (result.UploadSuccessfull)
        //    {
        //        e.Result = (result.Message, FileData);
        //    }
        //    else
        //    {
        //        e.Result = ($"failed, {result.Message}", FileData);
        //    }
        //
        //    w.Close();
        //}
        //
        //public static void UploadClip_Gfycat(object sender, DoWorkEventArgs e)
        //{
        //    ProgressWindow w = new ProgressWindow();
        //    w.SetProgress($"Uploading to Gfycat", 50, 100);
        //
        //
        //    GfycatClient client = new GfycatClient("2_otWpjF", "OrfMf6y_hQWnhQNnZjQ4K3aG5EQpB7-FpD2jqHygvlBsTpC787s3UjD2LmgY3Pcm"); // client authentication happens during first 401
        //    byte[] FileData = (byte[])e.Argument;
        //    var stream = new MemoryStream(FileData);
        //    GfyStatus gfyStatus = client.CreateGfyAsync(stream).GetAwaiter().GetResult();
        //    Gfy completedGfy = gfyStatus.GetGfyWhenCompleteAsync().GetAwaiter().GetResult();
        //
        //    //byte[] FileData = (byte[])e.Argument;
        //    //var result = Gfycat.Upload(FileData);
        //    //if (result.UploadSuccessfull)
        //    //{
        //    //    e.Result = (result.Message, FileData);
        //    //}
        //    //else
        //    //{
        //    //    e.Result = ($"failed, {result.Message}", FileData);
        //    //}
        //    //
        //    w.Close();
        //}
        //
        //public static void UploadClip_Imgur(object sender, DoWorkEventArgs e)
        //{
        //    //byte[] FileData = (byte[])e.Argument;
        //    //
        //    //
        //    //var client = new ImgurClient("da05117bbfa9bda");
        //    //var endpoint = new ImageEndpoint(client);
        //    //
        //    //endpoint.UploadImageUrlAsync();
        //    //var endpoint = new ImageEndpoint(client);
        //    //IImage image = endpoint.UploadImageBinaryAsync(FileData).GetAwaiter().GetResult();
        //    //e.Result = (image.Link, FileData);
        //
        //}

        public static void UploadToFTP(object sender, DoWorkEventArgs e)
        {
            ProgressWindow w = new ProgressWindow();
            w.SetProgress($"Uploading to FTP", 50, 100);

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
            try
            {
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                response.Close();
                e.Result = ($"{Preferences.FTPLink}{filename}", filedata);
            }
            catch
            {
                e.Result = ($"failed, {Preferences.FTPLink}{filename}", filedata);
            }
            w.Close();
        }

        public static void UploadPastebin(object sender, DoWorkEventArgs e)
        {
            ProgressWindow w = new ProgressWindow();
            w.SetProgress($"Uploading to Pastebin", 50, 100);

            //private string ILoginURL = "http://pastebin.com/api/api_login.php";
            string IPostURL = "http://pastebin.com/api/api_post.php";
            string IDevKey = "4d1c2c0bb6fa2e5c1403cedccc50bfd5";
            string IUserKey = null;

            string IBody = (string)e.Argument;
            string ISubj = Preferences.PastebinSubjectLine;

            if (string.IsNullOrEmpty(IBody.Trim())) { e.Result = "failed"; }
            if (string.IsNullOrEmpty(ISubj.Trim())) { e.Result = "failed"; }

            NameValueCollection IQuery = new NameValueCollection
            {
                { "api_dev_key", IDevKey },
                { "api_option", "paste" },
                { "api_paste_code", IBody },
                { "api_paste_private", "0" },
                { "api_paste_name", ISubj },
                { "api_paste_expire_date", "N" },
                { "api_paste_format", "text" },
                { "api_user_key", IUserKey }
            };

            string IResponse = "";

            using (WebClient IClient = new WebClient())
            {
                try
                {
                    IResponse = Encoding.UTF8.GetString(IClient.UploadValues(IPostURL, IQuery));
                }
                catch (Exception ee)
                {
                    IResponse = $"failed, {ee.Message}";
                }
            }
            e.Result = IResponse;

            w.Close();
        }
    }

    //public static class Gfycat
    //{
    //    public static (bool UploadSuccessfull, string Message) Upload(byte[] FileData)
    //    {
    //        var createResponse = Create(new GfycatCreateRequest() { NoMd5 = true }).GetAwaiter().GetResult();
    //
    //        Upload(createResponse.GfyName, FileData).GetAwaiter().GetResult();
    //        int notfounds = 0;
    //        while (true)
    //        {
    //            var statusResponse = Status(createResponse.GfyName).GetAwaiter().GetResult();
    //
    //            if (statusResponse.Task == "NotFoundo")
    //            {
    //                if (notfounds > 10)
    //                    return (false, statusResponse.Task);
    //                notfounds++;
    //            }
    //            else if (statusResponse.Task == "encoding") { }
    //            else if (statusResponse.Task == "complete")
    //                return (true, $"https://gfycat.com/{statusResponse.GfyName}");
    //            else
    //                return (false, statusResponse.Task);
    //
    //            System.Threading.Thread.Sleep(1000);
    //        }
    //    }
    //
    //    static async Task<GfycatCreateResponse> Create(GfycatCreateRequest request)
    //    {
    //        HttpClient client = new HttpClient();
    //        var response = await client.PostAsJsonAsync("https://api.gfycat.com/v1/gfycats", request);
    //        response.EnsureSuccessStatusCode();
    //
    //        return await response.Content.ReadAsAsync<GfycatCreateResponse>();
    //    }
    //
    //    static async Task Upload(string gfyname, byte[] fileData)
    //    {
    //        HttpClient client = new HttpClient();
    //        using (var formData = new MultipartFormDataContent())
    //        {
    //            formData.Add(new StringContent(gfyname), "key", "key");
    //            formData.Add(new ByteArrayContent(fileData), "file", gfyname);
    //
    //            var response = await client.PostAsync("https://filedrop.gfycat.com/", formData);
    //
    //            response.EnsureSuccessStatusCode();
    //        }
    //    }
    //
    //    static async Task<GfycatStatusResponse> Status(string gfyname)
    //    {
    //        HttpClient client = new HttpClient();
    //        var response = await client.GetAsync($"https://api.gfycat.com/v1/gfycats/fetch/status/{gfyname}");
    //        response.EnsureSuccessStatusCode();
    //
    //        return await response.Content.ReadAsAsync<GfycatStatusResponse>();
    //    }
    //
    //    struct GfycatCreateRequest
    //    {
    //        public bool NoMd5 { get; set; }
    //    }
    //
    //    struct GfycatCreateResponse
    //    {
    //        public bool IsOk { get; set; }
    //        public string GfyName { get; set; }
    //        public string Secret { get; set; }
    //        public string UploadType { get; set; }
    //    }
    //
    //    struct GfycatStatusResponse
    //    {
    //        public string Task { get; set; }
    //        public string GfyName { get; set; }
    //        public double Progress { get; set; }
    //    }
    //}
}
