﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Work.GUI {
    public class VisualBreakPrompt : MonoBehaviour
    {
        public TextMeshProUGUI count;
        public Slider progressToNext;
        public CanvasGroup cg;
        public Button takeBreakButton;
        public Workforce workforce;

        // Start is called before the first frame update
        void Start()
        {
            cg = GetComponent<CanvasGroup>();
        }

        // Update is called once per frame
        void Update()
        {
            progressToNext.value = workforce.NextBreakPercent;
            count.text = workforce.BreaksAvailable.ToString();
        }
    }
}