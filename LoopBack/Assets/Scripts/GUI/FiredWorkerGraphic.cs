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
        public CanvasGroup firedLabel;
        public CanvasGroup firedFade;
        private RectTransform rt;
        private CanvasGroup cg;
        public float transitionTime = 0.5f;
        private float defaultScale;
        private Vector2 defaultPosition;
        public WorkerAnimation anim;

        private void Awake()
        {
            rt = GetComponent<RectTransform>();
            defaultScale = transform.localScale.x;
            defaultPosition = transform.position;
            cg = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            firedLabel.alpha = 0f;
            firedFade.alpha = 0f;
        }

        public IEnumerator Appear(VisualWorker w, RectTransform t, float time = 1f)
        {
            gameObject.SetActive(true);
            transform.position = t.position;
            transform.localScale = t.localScale;
            transform.DOMove(defaultPosition, time);
            transform.DOScale(defaultScale, time);
            anim.SetCharacter(w.anim.CharacterIndex);
            yield return new WaitForSeconds(time + 0.5f);
            firedFade.alpha = 1f;
            firedLabel.alpha = 1f;
            transform.DOScale(transform.localScale * 0.8f, 0.1f).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(0.1f);
            transform.DOScale(defaultScale, 0.25f).SetEase(Ease.OutQuad);
        }
    }
}
