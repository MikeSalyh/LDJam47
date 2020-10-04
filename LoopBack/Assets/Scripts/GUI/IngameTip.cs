using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class IngameTip : MonoBehaviour
{
    private float gameStartTime;
    private bool displayed = false;
    public float timeTilTip = 6f;
    private CanvasGroup cg;
    private TextMeshProUGUI label;

    // Start is called before the first frame update
    void Start()
    {
        cg = GetComponent<CanvasGroup>();
        label = GetComponent<TextMeshProUGUI>();
        gameStartTime = Time.time;
        cg.alpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!displayed && Time.time > gameStartTime + timeTilTip)
        {
            //Check if the player's started playing:
            if (MetagameManager.instance.score > 0)
            {
                Debug.Log("The player doesn't need a tip");
                GameObject.Destroy(this.gameObject);
            }
            else
            {
                StartCoroutine(ToolTipRoutine());
            }
        }
    }

    private IEnumerator ToolTipRoutine()
    {
        displayed = true;
        cg.DOFade(1f, 1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil( ()=> MetagameManager.instance.score > 0);
        cg.DOFade(0f, 0.25f);
        yield return new WaitForSeconds(0.25f);
        GameObject.Destroy(this.gameObject);
    }
}
