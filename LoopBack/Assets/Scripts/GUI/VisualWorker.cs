﻿using System.Collections;
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
        public TextMeshProUGUI wordLabel;
        public TextMeshProUGUI timeLeftLabel;
        public Slider timeRemaining;
        public Image background;
        private Color defaultColor;
        private RectTransform rt;
        private CanvasGroup cg;
        public float transitionTime = 0.5f;
        private float defaultSize;

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
            AssociatedWorker.OnRequestNewWord += ResetBackground;
            AssociatedWorker.OnSelected += TintBackground;
            AssociatedWorker.OnFired += HandleFired;
        }

        // Update is called once per frame
        void Update()
        {
            if (AssociatedWorker != null)
            {
                if (!AssociatedWorker.Fired)
                {
                    wordLabel.text = GenerateLabel();
                    if (AssociatedWorker.WordComplete)
                    {
                        background.color = Color.green;
                    }
                    else
                    {
                        HandleTimeRemaining();
                        timeLeftLabel.text = AssociatedWorker.TimeRemainingOnAsk.ToString("0");
                    }
                }
            }
        }

        private void HandleTimeRemaining()
        {
            float t = 1 - Mathf.InverseLerp(AssociatedWorker.AskDuration, 0f, AssociatedWorker.TimeRemainingOnAsk);
            timeRemaining.value = t;
        }

        private void ResetBackground(Worker w = null)
        {
            background.color = defaultColor;
        }

        private void TintBackground()
        {
            background.color = Color.cyan;
        }

        public void SetWorker(Worker w)
        {
            AssociatedWorker = w;
            w.visualRepresentation = this.gameObject;
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
            AssociatedWorker.OnRequestNewWord -= ResetBackground;
            AssociatedWorker.OnSelected -= TintBackground;
            AssociatedWorker.OnFired -= HandleFired;
        }

        private void HandleFired()
        {
            background.color = Color.black;
            wordLabel.text = "<color=red>FIRED!</color>";
        }
    }
}