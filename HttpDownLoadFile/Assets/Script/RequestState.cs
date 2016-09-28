using UnityEngine;
using System.Collections;
using System.Text;
using System.Net;
using System.IO;

/// <summary>
/// 网络数据
/// </summary>
public class RequestState
{
    // 缓冲区大小
    const int m_buffetSize = 1024;
    // 读取流缓冲区
    public byte[] m_bufferRead;
    // 请求
    public HttpWebRequest m_request;
    // 响应
    public HttpWebResponse m_response;
    // 响应流
    public Stream m_streamResponse;
    // 文件存放路径
    public string filePath = null;
    // 网络读取路径
    public string netPath = null;
    // 读取文件流
    public FileStream fileStream = null;

    /// <summary>
    /// 构造
    /// </summary>
    public RequestState()
    {
        m_bufferRead = new byte[m_buffetSize];
        m_request = null;
        m_streamResponse = null;
    }
}
