using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class FinaleManager : MonoBehaviour
{
    public TextMeshProUGUI[] finaleStrings;
    public GameObject winGraphic, loseGraphic;

    private void Start()
    {
        bool won = MetagameManager.instance.NumWorkers >= Workforce.MAX_WORKERS;
        winGraphic.SetActive(won);
        loseGraphic.SetActive(!won);

        for (int i = 0; i < finaleStrings.Length; i++)
        {
            finaleStrings[i].text = string.Format(finaleStrings[i].text, MetagameManager.instance.score.ToString(), MetagameManager.instance.NumWorkers.ToString());
        }
    }

    public void GoToMainMenu()
    {
        MetagameManager.instance.GoToMenu();
    }
}
