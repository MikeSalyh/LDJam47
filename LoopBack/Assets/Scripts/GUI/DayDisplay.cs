using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Work.GUI
{

    public class DayDisplay : MonoBehaviour
    {
        public TextMeshProUGUI label;
        public string labelString = "Level {0}: {1}";
        private string[] daysOfTheWeek = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            label.text = string.Format(labelString, MetagameManager.instance.level+1, daysOfTheWeek[MetagameManager.instance.level % daysOfTheWeek.Length]);
        }
    }
}