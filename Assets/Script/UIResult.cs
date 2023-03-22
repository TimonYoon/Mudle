using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIResult : MonoBehaviour {

    public GameObject bestStamp;

    public Text bestScore;
    public Text currentScore;
    

    private void OnEnable()
    {
        bestStamp.SetActive(GameController.Instance.isBestScore);
        bestScore.text = ToStringComma(GameController.Instance.bestScore);
        currentScore.text = ToStringComma(GameController.Instance.score);
    }

    /// <summary> 1,234,567 이런 형식의 콤마를 붙인 텍스트로 바꿔줌 </summary>
    public string ToStringComma(double number)
    {

        string returnData = string.Empty;

        string data = number.ToString();

        char[] charArray = data.ToCharArray().Reverse().ToArray();

        for (int i = 0; i < charArray.Length; i++)
        {
            returnData = charArray[i] + returnData;
            if ((i + 1) % 3 == 0 && (i + 1) < charArray.Length)
                returnData = ',' + returnData;
        }
        return returnData;
    }
}
