using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using UnityEngine;

public class HttpDldFile
{
    public event EventHandler DownloadForCloseUpCompleted;

    /// <summary>Occurs when [card download is completed].</summary>
    public event EventHandler DownloadCardCompleted;
    /// <summary>Occurs when [closeup download is completed].</summary>
    public event EventHandler DownloadCloseupCompleted;
    /// <summary>Occurs when [download is completed].</summary>
    public event EventHandler DownloadFieldCompleted;

    public bool Download(string url, string filename)
    {
        bool flag = false;
        try
        {
            if (!Directory.Exists(Path.GetDirectoryName(filename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
            }

            using (var client = new TimeoutWebClient())
            {
                ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;

                //authorization needed to acces github
                if (Path.GetExtension(filename).Contains("png"))
                {
                    client.Headers.Add(HttpRequestHeader.Authorization, string.Concat("token ", RepoData.GetToken()));
                    client.Timeout = 6500;
                }
                if (Path.GetExtension(filename).Contains("jpg"))
                {
                    client.Timeout = 3500;
                }
                client.DownloadFile(new Uri(url), filename + ".tmp");
            }
            flag = true;
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            File.Move(filename + ".tmp", filename);
        }
        catch (Exception)
        {
            flag = false;
        }
        return flag;
    }


    /// <summary>
    /// Additional Download method used for downloading pictures and closeups (Uses EventHandlers).
    /// </summary>
    /// <param name="url">URL of the website containing the file.</param>
    /// <param name="filename">Full path with filename and extension.</param>
    /// <param name="picture"><see cref="GameTextureManager.PictureResource"/> required for EventHandler.</param>
    public bool Download(string url, string filename, GameTextureManager.PictureResource picture, bool forCloseup)
    {
        var flag = Download(url, filename);

        if (filename.Contains("closeup"))
        {
            EventHandler handler = DownloadCloseupCompleted;
            if (handler != null)
            {
                handler(this, new DownloadPicCompletedEventArgs(picture, flag, filename));
            }
        }
        if (filename.Contains("card") && forCloseup)
        {
            EventHandler handler = DownloadForCloseUpCompleted;
            if (handler != null)
            {
                handler(this, new DownloadPicCompletedEventArgs(picture, flag, filename));
            }
        }
        else if (filename.Contains("card"))
        {
            EventHandler handler = DownloadCardCompleted;
            if (handler != null)
            {
                handler(this, new DownloadPicCompletedEventArgs(picture, flag, filename));
            }
        }
        return flag;
    }

    /// <summary>Download method used for downloading fieldspell.</summary>
    /// <param name="url">The URL.</param>
    /// <param name="filename">The filename.</param>
    /// <param name="player">The player's value</param>
    public bool DownloadField(string url, string filename, int player)
    {
        var flag = Download(url, filename);

        if (filename.Contains("field"))
        {
            EventHandler handler = DownloadFieldCompleted;
            if (handler != null)
            {
                handler(this, new DownloadFieldCompletedEventArgs(player, flag, filename));
            }
        }
        return flag;
    }

    public static bool MyRemoteCertificateValidationCallback(System.Object sender,
    X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        // If there are errors in the certificate chain,
        // look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status == X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    continue;
                }
                chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                bool chainIsValid = chain.Build((X509Certificate2)certificate);
                if (!chainIsValid)
                {
                    isOk = false;
                    break;
                }
            }
        }
        return isOk;
    }
}
public class TimeoutWebClient : WebClient
{
    public event EventHandler DownloadFileCompletedExtended;
    public int Timeout { get; set; }

    public TimeoutWebClient()
    {
        Timeout = 10000;
    }
    public TimeoutWebClient(int timeout)
    {
        Timeout = timeout;
    }

    protected override WebRequest GetWebRequest(Uri address)
    {
        WebRequest request = base.GetWebRequest(address);
        request.Timeout = Timeout;
        return request;
    }

}

/// <summary>
/// custom EventArgs used to give methods required variables when Download is completed.
/// </summary>
public class DownloadPicCompletedEventArgs : EventArgs
{
    public DownloadPicCompletedEventArgs(GameTextureManager.PictureResource pic, bool downloadSuccesful, string filename)
    {
        Pic = pic;
        DownloadSuccesful = downloadSuccesful;
        Filename = filename;
    }

    public GameTextureManager.PictureResource Pic { get; protected set; }
    public bool DownloadSuccesful { get; protected set; }
    public string Filename { get; protected set; }
}

public class DownloadFieldCompletedEventArgs : EventArgs
{
    public DownloadFieldCompletedEventArgs(int player, bool downloadSuccesful, string filename)
    {
        this.Player = player;
        DownloadSuccesful = downloadSuccesful;
        Filename = filename;
    }

    public int Player { get; protected set; }
    public bool DownloadSuccesful { get; protected set; }
    public string Filename { get; protected set; }
}