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
        public WorkerAnimation anim;

        private static readonly string workStartString = "<color=yellow>";
        private static readonly string workEndString = "</color>";
        private bool wasSelected;

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

                if (AssociatedWorker.selected != wasSelected)
                {
                    Color selectedColor = Color.green; //Maybe?

                    wasSelected = AssociatedWorker.selected;
                    background.DOColor(AssociatedWorker.selected ? selectedColor : defaultColor, 0.25f);
                    wordPrompt.background.DOColor(AssociatedWorker.selected ? selectedColor : defaultColor, 0.25f);
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
            wordPrompt.gameObject.SetActive(true);
        }

        public void SetWorker(Worker w, int characterID)
        {
            AssociatedWorker = w;
            w.visualRepresentation = this.gameObject;
            AssociatedWorker.OnFinishWord += HandleWordFinished;
            AssociatedWorker.OnRequestNewWord += HandleNewWord;
            AssociatedWorker.OnFired += HandleFired;
            anim.SetCharacter(characterID);
        }

        private void HandleWordFinished(Worker w)
        {
            wordPrompt.gameObject.SetActive(false);
            anim.Play();
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
            AssociatedWorker.OnFired -= HandleFired;
        }

        private void HandleFired(Worker w)
        {
            wordPrompt.wordLabel.text = "<color=red>FIRED!</color>";
        }
    }
}