using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Work.GUI
{
    public class HPDisplay : MonoBehaviour
    {
        public Image[] hearts;
        public Workforce workforce;

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i].color = workforce.LivesLeft > i ? Color.white : Color.black ;
            }
        }
    }
}
