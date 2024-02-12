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
        }
    }
}
