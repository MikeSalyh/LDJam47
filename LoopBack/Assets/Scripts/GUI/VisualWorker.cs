using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Work.GUI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class VisualWorker : MonoBehaviour
    {
        public Worker AssociatedWorker { get; private set; }
        public Image background;
        private Color defaultColor;
        private RectTransform rt;
        private CanvasGroup cg;
        public float transitionTime = 0.5f;
        private float defaultSize;
        public WordPromptGUI wordPrompt;

        private static readonly string workStartString = "<color=yellow>";
        private static readonly string workEndString = "</color>";

        private void Awake()
        {
            rt = GetComponent<RectTransform>();
            defaultSize = rt.sizeDelta.x;
            cg = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            defaultColor = background.color;
        }

        // Update is called once per frame
        void Update()
        {
            if (AssociatedWorker != null)
            {
                if (!AssociatedWorker.Fired && !AssociatedWorker.WordComplete)
                {
                    wordPrompt.wordLabel.text = GenerateLabel();
                    HandleTimeRemaining();
                }
            }
        }

        private void HandleTimeRemaining()
        {
            float t = 1 - Mathf.InverseLerp(AssociatedWorker.latestAskDuration, 0f, AssociatedWorker.TimeRemainingOnAsk);
            wordPrompt.timeRemaining.value = t;
        }

        public void HandleNewWord(Worker w = null)
        {
            background.color = defaultColor;
            wordPrompt.gameObject.SetActive(true);
        }

        private void TintBackground(Worker w)
        {
            background.color = Color.magenta;
            wordPrompt.background.color = Color.magenta;
        }

        public void SetWorker(Worker w)
        {
            AssociatedWorker = w;
            w.visualRepresentation = this.gameObject;
            AssociatedWorker.OnFinishWord += HandleWordFinished;
            AssociatedWorker.OnRequestNewWord += HandleNewWord;
            AssociatedWorker.OnSelected += TintBackground;
            AssociatedWorker.OnFired += HandleFired;
        }

        private void HandleWordFinished(Worker w)
        {
            background.color = Color.green;
            wordPrompt.gameObject.SetActive(false);
        }

        private string GenerateLabel()
        {
            string output = workStartString;
            bool closedColor = false;
            for (int i = 0; i < AssociatedWorker.CurrentWord.Length; i++)
            {
                if (AssociatedWorker.WorkDone <= i && !closedColor)
                {
                    closedColor = true;
                    output += workEndString;
                }
                output += AssociatedWorker.CurrentWord[i];
            }
            return output;
        }

        public void Reposition(Vector3 newPosition, float newScale, bool snapToPosition = false)
        {
            float calculatedScale = newScale / defaultSize;
            if (snapToPosition)
            {
                rt.localScale = Vector2.one * calculatedScale;
                rt.position = newPosition;
            }
            else
            {
                rt.DOScale(Vector2.one * calculatedScale, transitionTime).SetEase(Ease.InOutSine);
                rt.DOMove(newPosition, transitionTime).SetEase(Ease.InOutSine);
            }
        }

        private void OnEnable()
        {
            cg.alpha = 0f;
            cg.DOFade(1f, transitionTime);
            rt.localScale = Vector3.zero;
        }

        private void OnDestroy()
        {
            RemoveListeners();
        }

        private void RemoveListeners()
        {
            if (AssociatedWorker == null) return;
            AssociatedWorker.OnRequestNewWord -= HandleNewWord;
            AssociatedWorker.OnSelected -= TintBackground;
            AssociatedWorker.OnFired -= HandleFired;
        }

        private void HandleFired(Worker w)
        {
            background.color = Color.black;
            wordPrompt.wordLabel.text = "<color=red>FIRED!</color>";
        }
    }
}