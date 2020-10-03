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

        private static readonly string workStartString = "<color=yellow>";
        private static readonly string workEndString = "</color>";

        private void Start()
        {
            defaultColor = background.color;
        }

        // Update is called once per frame
        void Update()
        {
            if (AssociatedWorker != null)
            {
                if (!AssociatedWorker.Fired)
                {
                    timeRemaining.value = 1 - Mathf.InverseLerp(AssociatedWorker.AskDuration, 0f, AssociatedWorker.TimeRemainingOnAsk);
                    timeLeftLabel.text = AssociatedWorker.TimeRemainingOnAsk.ToString("0");
                    wordLabel.text = GenerateLabel();
                }
                else
                {
                    background.color = Color.red;
                }
            }
        }

        public void SetWorker(Worker w)
        {
            AssociatedWorker = w;
        }

        private string GenerateLabel()
        {
            string output = workStartString;
            bool closedColor = false;
            for (int i = 0; i < AssociatedWorker.RequestedWord.Length; i++)
            {
                if (AssociatedWorker.WorkDone <= i && !closedColor)
                {
                    closedColor = true;
                    output += workEndString;
                }
                output += AssociatedWorker.RequestedWord[i];
            }
            return output;
        }
    }
}