using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Work.GUI
{
    public class TestVisualWorker : MonoBehaviour
    {
        public Worker AssociatedWorker { get; private set; }
        public TextMeshProUGUI label;
        public Slider timeRemaining;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (AssociatedWorker != null)
            {
                timeRemaining.value = 1- Mathf.InverseLerp(AssociatedWorker.AskDuration, 0f, AssociatedWorker.TimeRemainingOnAsk);
                label.text = AssociatedWorker.requestedWord;
            }
        }

        public void SetWorker(Worker w)
        {
            AssociatedWorker = w;
        }
    }
}