using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Net;
using System.IO;
using System;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

/// <summary>
/// 使用http下载文件
/// </summary>
public class HttpDownLoadFile : MonoBehaviour
{
    private Image progressT = null;
    private List<RequestState> requestStateList = new List<RequestState>();

    /// <summary>
    /// 起始
    /// </summary>
    void Start()
    {
        // ------------------------测试数据------------------------
        // 进度条
        progressT = this.transform.Find("Image").GetComponent<Image>();

        // 路径 (数据是 永恒征战更新的文件数据)
        List<string> filePathList = new List<string>();
        filePathList.Add(Application.dataPath + "/StreamingAssets/icons.assetbundle");
        filePathList.Add(Application.dataPath + "/StreamingAssets/field.assetbundle");

        List<string> netPathList = new List<string>();
        netPathList.Add("http://dl.mmoup.net/android/assets/0030/data/atlas/icon/icons.assetbundle");
        netPathList.Add("http://dl.mmoup.net/android/assets/0030/data/conf/field.assetbundle");

        

        // ------------------------测试数据------------------------

        Debug.Log("http异步下载文件开始");

        for (int i = 0; i < filePathList.Count; ++i)
        {
            RequestState item = new RequestState();
            item.filePath = filePathList[i];
            item.netPath = netPathList[i];
            item.fileStream = new FileStream(item.filePath, FileMode.Create);

            DownloadMusicAsyn(item);
            requestStateList.Add(item);
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        // 下载进度
        progressSize();
    }

    /// <summary>
    /// 开始异步下载文件
    /// </summary>
    void DownloadMusicAsyn(RequestState requestState_)
    {
        try
        {
            // https验证
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);

            HttpWebRequest myHttpWebRequest = HttpWebRequest.Create(requestState_.netPath) as HttpWebRequest;
            requestState_.m_request = myHttpWebRequest;
            

            //异步获取;
            IAsyncResult result = (IAsyncResult)myHttpWebRequest.BeginGetResponse(new AsyncCallback(RespCallback), requestState_);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }

    /// <summary>
    /// https证书验证
    /// </summary>
    public bool CheckValidationResult(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true; // 总是接受
    }

    /// <summary>
    /// 异步下载回调
    /// </summary>
    void RespCallback(IAsyncResult result)
    {
        Debug.Log("RespCallback 0");

        try
        {
            RequestState myRequestState = (RequestState)result.AsyncState;
            HttpWebRequest myHttpWebRequest = myRequestState.m_request;
            myRequestState.m_response = (HttpWebResponse)myHttpWebRequest.EndGetResponse(result);

            Stream responseStream = myRequestState.m_response.GetResponseStream();
            myRequestState.m_streamResponse = responseStream;

            //开始读取数据;
            IAsyncResult asyncreadresult = responseStream.BeginRead(myRequestState.m_bufferRead, 0, 1024, new AsyncCallback(ReadCallBack), myRequestState);

            return;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }

    /// <summary>
    /// 数据写入回调
    /// </summary>
    void ReadCallBack(IAsyncResult result)
    {
        Debug.Log("ReadCallBack");
        try
        {
            RequestState myRequestState = (RequestState)result.AsyncState;
            Stream responseStream = myRequestState.m_streamResponse;
            int read = responseStream.EndRead(result);
            //Debug.Log("read size =" + read);

            if (read > 0)
            {
                //将接收的数据写入;
                myRequestState.fileStream.Write(myRequestState.m_bufferRead, 0, 1024);
                myRequestState.fileStream.Flush();
                //fileStream.Close();

                //继续读取数据;
                myRequestState.m_bufferRead = new byte[1024];
                IAsyncResult asyncreadresult = responseStream.BeginRead(myRequestState.m_bufferRead, 0, 1024, new AsyncCallback(ReadCallBack), myRequestState);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }

    /// <summary>
    /// 下载进度
    /// </summary>
    private void progressSize()
    {
        float progress = 0;
        // -----------测试数据-------------
        float totle = 73728;
        // -----------测试数据-------------

        for (int i = 0; i < requestStateList.Count; ++i)
        {
            FileStream stream = requestStateList[i].fileStream;
            progress += stream.Length;
        }

        progressT.fillAmount = (progress / totle);

        Debug.Log(progress);
        Debug.Log(Convert.ToDouble((progress / 1024f / 1024f).ToString()).ToString("0.000") + " MB");
    }
}