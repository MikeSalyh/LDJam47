using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Work.GUI
{
    public class BreakPrompt : MonoBehaviour
    {
        public TextMeshProUGUI label;
        private CanvasGroup cg;

        private void Awake()
        {
            cg = GetComponent<CanvasGroup>();
        }

        public void Show(float duration)
        {
            gameObject.SetActive(true);
            StartCoroutine(ShowCoroutine(duration));
        }

        private IEnumerator ShowCoroutine(float totalDuration)
        {
            label.text = "BREAK TIME!";
            cg.alpha = 0f;
            cg.DOFade(1f, 0.5f);
            yield return new WaitForSeconds(totalDuration - 1f);
            label.text = "BACK TO WORK!";
            yield return new WaitForSeconds(1f);
            cg.DOFade(0f, 0.5f);
        }
    }
}