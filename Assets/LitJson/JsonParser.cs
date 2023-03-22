
//json
using LitJson;
using System;
using System.Text.RegularExpressions;   //json 한글 파싱에 필요
using System.Globalization;

/// <summary> json 파싱 도우미 </summary>
public class JsonParser
{
    public JsonParser() { }

    //============================================================================================================
    //유니코드로 저장된 string --> 한글로 변환하기 위한 함수. 꼭 필요.
    /// <summary> static 매서드로 쓸 것  </summary>
    [Obsolete]
    public string Decoder(string value)
    {
        Regex _regex = new Regex(@"\\u(?<Value>[a-zA-Z0-9]{4})");//, RegexOptions..Compiled);

        return _regex.Replace(
            value,
            m => ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString()
        );
    }

    
    /// <summary> 유니코드로 저장된 string --> 한글로 변환하기 위한 함수. 꼭 필요. </summary>
    static public string Decode(string value)
    {
        Regex _regex = new Regex(@"\\u(?<Value>[a-zA-Z0-9]{4})");//, RegexOptions..Compiled);

        return _regex.Replace(
            value,
            m => ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString()
        );
    }

    //============================================================================================================
    static public bool ToBool(JsonData jData)
    {
        if (jData == null)
            return false;

        //bool value = false;
        if (jData.ToString() == "1")
            return true;
        else
        {
            return false;
        }

        //return value;
    }

    /// <summary> Static 메서드로 쓸 것 </summary>
    [Obsolete]
    public string ToString_Bak(JsonData jData)
    {
        //null 값이 들어간 string은 반환을 못한다.. 이걸 써야 함.
        string str = "";
        if (jData != null)
            str = jData.ToString();

        return str;
    }

    static public string ToString(JsonData jData)
    {
        string str = "";
        if (jData != null)
            str = jData.ToString();

        return str;
    }

    //============================================================================================================
    static public long ToLong(JsonData jData)
    {
        if (jData == null)
            return -1;

        long value = 0;
        long.TryParse(jData.ToString(), out value);

        return value;
    }

    /// <summary> static 매서드로 쓸 것 </summary>
    [Obsolete]
    public int ToInt_Bak(JsonData jData)
    {
        if (jData == null)
            return -1;

        int value = 0;
        int.TryParse(jData.ToString(), out value);

        return value;
    }

    static public int ToInt(JsonData jData)
    {
        if (jData == null)
            return -1;

        int value = 0;
        int.TryParse(jData.ToString(), out value);

        return value;
    }

    static public float ToFloat(JsonData jData)
    {
        if (jData == null)
            return -1f;

        float value = 0f;
        float.TryParse(jData.ToString(), out value);

        return value;
    }

    static public int DateTimeToInt(JsonData jData)
    {
        if (jData == null)
            return -1;

        DateTime dateTime = new DateTime();
        DateTime.TryParse(jData.ToString(), out dateTime);
        
        return dateTime.Second;
    }

    static public DateTime ToDateTime(JsonData jData)
    {
        if (jData == null)
            return new DateTime(0);

        DateTime dateTime = new DateTime();
        DateTime.TryParse(jData.ToString(), out dateTime);

        return dateTime;
    }
}

