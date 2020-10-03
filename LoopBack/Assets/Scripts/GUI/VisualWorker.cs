using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Work.GUI
{
    public class VisualWorker : MonoBehaviour
    {
        public Worker AssociatedWorker { get; private set; }
        public TextMeshProUGUI wordLabel;
        public TextMeshProUGUI timeLeftLabel;
        public Slider timeRemaining;
        public Image background;
        private Color defaultColor;
        private RectTransform rt;

        private static readonly string workStartString = "<color=yellow>";
        private static readonly string workEndString = "</color>";

        private void Awake()
        {
            rt = GetComponent<RectTransform>();
        }

        private void Start()
        {
            defaultColor = background.color;
            AssociatedWorker.OnRequestNewWord += ResetBackground;
            AssociatedWorker.OnSelected += TintBackground;
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
                        timeRemaining.value = 1 - Mathf.InverseLerp(AssociatedWorker.AskDuration, 0f, AssociatedWorker.TimeRemainingOnAsk);
                        timeLeftLabel.text = AssociatedWorker.TimeRemainingOnAsk.ToString("0");
                    }
                }
                else
                {
                    background.color = Color.red;
                }
            }
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


        public void Reposition(Vector3 newPosition, float newScale)
        {
            rt.sizeDelta = Vector2.one * newScale;
            rt.position = newPosition;
        }

        private void OnDisable()
        {
            AssociatedWorker.OnRequestNewWord -= ResetBackground;
            AssociatedWorker.OnSelected -= TintBackground;
        }
    }
}