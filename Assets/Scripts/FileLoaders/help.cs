using UnityEngine;
// Text を使用するため
using UnityEngine.UI;

public class help : MonoBehaviour
{
    // 公開年
    [SerializeField] string year = string.Empty;
    // コピーライト表示欄
    [SerializeField] Text copyright = null;
    [SerializeField] Text company = null;

    void Start()
    {
        // バージョン
        var version = Application.version;
        // リリース元
        var companyName = Application.companyName;
        // コピーライト書き換え
        copyright.text = $"アプリケーションバージョン:Ver.{version}";
        company.text = $"© {year} {companyName}";
    }
}