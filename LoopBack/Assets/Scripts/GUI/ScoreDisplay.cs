using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Work.GUI
{
    public class ScoreDisplay : MonoBehaviour
    {
        public TextMeshProUGUI count;

        // Update is called once per frame
        void Update()
        {
            count.text = MetagameManager.instance.score.ToString();
        }
    }
}