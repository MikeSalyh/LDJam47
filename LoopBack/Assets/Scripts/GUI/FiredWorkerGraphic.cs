using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Work.GUI
{
    public class FiredWorkerGraphic : MonoBehaviour
    {
        public TextMeshProUGUI wordLabel;
        public Image background;
        private RectTransform rt;
        private CanvasGroup cg;
        public float transitionTime = 0.5f;
        private Color defaultColor;
        private float defaultScale;
        private Vector2 defaultPosition;

        private void Awake()
        {
            rt = GetComponent<RectTransform>();
            defaultScale = transform.localScale.x;
            defaultPosition = transform.position;
            defaultColor = background.color;
            cg = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            background.color = defaultColor;
        }

        public void SetToFiredMode()
        {
            background.color = Color.black;
            wordLabel.text = "<color=red>FIRED!</color>";
        }

        public void Appear(string label, RectTransform t, float time = 1f)
        {
            gameObject.SetActive(true);
            wordLabel.text = label;
            transform.position = t.position;
            transform.localScale = t.localScale;
            transform.DOMove(defaultPosition, time);
            transform.DOScale(defaultScale, time);
        }

        public void GoAway(RectTransform t, float time = 1f)
        {
            transform.DOMove(t.position, time);
            transform.DOScale(t.localScale, time).onComplete += () => { gameObject.SetActive(false); };
        }
    }
}
