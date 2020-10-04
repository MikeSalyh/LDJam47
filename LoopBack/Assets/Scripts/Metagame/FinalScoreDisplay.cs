using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class FinalScoreDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public string finaleText = "Your company typed {0} words.";
    void Start()
    {
        SetScore();
    }

    void SetScore()
    {
        GetComponent<TextMeshProUGUI>().text = string.Format(finaleText, MetagameManager.instance.score.ToString());
    }
}
