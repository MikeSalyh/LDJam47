using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

namespace Work.GUI
{
    public class WordPromptGUI : MonoBehaviour
    {
        private CanvasGroup cg;
        public TextMeshProUGUI wordLabel;
        public Slider timeRemaining;
        public Image background;

        // Start is called before the first frame update
        void Awake()
        {
            cg = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            background.color = Color.white;
            cg.alpha = 0f;
            cg.DOKill();
            cg.DOFade(1f, 0.25f);
        }
    }
}
