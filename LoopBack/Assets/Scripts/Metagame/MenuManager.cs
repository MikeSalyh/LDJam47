using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    public GameObject instrux;

    public void GoToGameplay()
    {
        MetagameManager.instance.GoToGameplay();
    }

    public void ShowInstrux()
    {
        instrux.gameObject.SetActive(true);
        instrux.GetComponent<CanvasGroup>().alpha = 0f;
        instrux.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
    }

    public void HideInstrux()
    {
        instrux.gameObject.SetActive(false);
    }
}
