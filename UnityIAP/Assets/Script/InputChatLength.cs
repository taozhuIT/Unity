using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class InputChatLength : MonoBehaviour 
{
    // 参数
    private string _newName = null;

    // UI
    private InputField _inputFeild = null;
    private Text _debug = null;

    private void Awake()
    {
        _inputFeild = this.transform.Find("InputField").GetComponent<InputField>();
        _debug = this.transform.Find("Debug").GetComponent<Text>();

        //_inputFeild.onValueChange.AddListener(OnInputChat);
    }

	private void Start ()
    {
        _inputFeild.text = SystemInfo.deviceUniqueIdentifier;
	}
	
	private void Update () 
    {
	
	}

    private void OnInputChat(string value_)
    {
        // 正则表达式(匹配字符串中的特殊字符)
        string rep = @"\s";
        // 正则表达是(匹配全部字母、数字、中文)长度不能超过6
        string reg = @"[A-Za-z0-9\u4e00-\u9fa5]{0,32}";

        string value = Regex.Replace(value_, rep, "");
        string namec = Regex.Match(value, reg).Value;

        List<byte> names = new List<byte>(System.Text.Encoding.Default.GetBytes(namec));
        if (names.Count > 16)
            names.RemoveRange(16, names.Count - 16);

        _newName = System.Text.Encoding.Default.GetString(names.ToArray());
        _inputFeild.text = _newName;
        DebugSystem(_newName + "---->" + names.Count);
    }

    /// <summary>
    /// 日志输出回调
    /// </summary>
    private void DebugSystem(object obj_)
    {
        string debug = (string)obj_;
        _debug.text = _debug.text + "\n" + debug;
    }
}
