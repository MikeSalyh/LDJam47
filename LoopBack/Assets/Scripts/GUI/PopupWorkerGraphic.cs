using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace Work.GUI
{
    public class PopupWorkerGraphic : MonoBehaviour
    {
        public CanvasGroup firedGraphics;
        public CanvasGroup victoryGraphics;

        private RectTransform rt;
        private CanvasGroup cg;
        public float transitionTime = 0.5f;
        private float defaultScale;
        private Vector2 defaultPosition;
        public WorkerAnimation anim;
        public Image background;
        private AudioSource src;
        public AudioClip firedSound;

        private void Awake()
        {
            rt = GetComponent<RectTransform>();
            defaultScale = transform.localScale.x;
            defaultPosition = transform.position;
            src = GetComponent<AudioSource>();
            cg = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            firedGraphics.alpha = 0f;
            victoryGraphics.alpha = 0f;
        }

        public IEnumerator DoGameOver(VisualWorker w, RectTransform t, float time = 1f)
        {
            gameObject.SetActive(true);
            transform.position = t.position;
            transform.localScale = t.localScale;
            transform.DOMove(defaultPosition, time);
            transform.DOScale(defaultScale, time);
            anim.SetCharacter(w.anim.CharacterIndex);
            yield return new WaitForSeconds(time + 0.5f);
            src.PlayOneShot(firedSound, 0.35f);
            firedGraphics.alpha = 1f;
            transform.DOScale(transform.localScale * 0.8f, 0.1f).SetEase(Ease.OutQuad);
            yield return new WaitForSeconds(0.1f);
            transform.DOScale(defaultScale, 0.25f).SetEase(Ease.OutQuad);
        }

        public IEnumerator DoGameWin(VisualWorker w, RectTransform t, float time = 1f)
        {
            gameObject.SetActive(true);
            transform.position = t.position;
            transform.localScale = t.localScale;
            transform.DOMove(defaultPosition, time);
            transform.DOScale(defaultScale, time);
            anim.SetCharacter(w.anim.CharacterIndex);
            yield return new WaitForSeconds(time + 0.5f);
            anim.Play();
            yield return new WaitForSeconds(1f);
        }
    }
}
