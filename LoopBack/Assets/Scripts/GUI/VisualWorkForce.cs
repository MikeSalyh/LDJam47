using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Work.GUI
{
    public class VisualWorkForce : Workforce
    {
        public GameObject workerVisualObject;
        public Transform workerParent;

        protected override Worker AddWorker()
        {
            Worker w = base.AddWorker();
            GameObject.Instantiate(workerVisualObject, workerParent).GetComponent<TestVisualWorker>().SetWorker(w);
            return w;
        }
    }
}
