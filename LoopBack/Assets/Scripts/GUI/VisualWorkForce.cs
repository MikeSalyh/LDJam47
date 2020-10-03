using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Work.GUI
{
    public class VisualWorkForce : Workforce
    {
        public GameObject workerVisualObject;
        public GameObject workerParent;
        public WorkerGridReference workerGrid;

        protected override Worker AddWorker()
        {
            Worker w = base.AddWorker();
            GameObject.Instantiate(workerVisualObject, workerParent.transform).GetComponent<VisualWorker>().SetWorker(w);
            UpdateGrid();
            return w;
        }

        private void UpdateGrid()
        {
            Vector3[] newGridPositions = workerGrid.GetGridFor(workers.Count);
            float newGridSize = workerGrid.GetCellSizeFor(workers.Count);
            for (int i = 0; i < workers.Count; i++)
            {
                workers[i].visualRepresentation.GetComponent<VisualWorker>().Reposition(newGridPositions[i], newGridSize);
            }
        }


        protected override void ReleaseActiveWorker(Worker w)
        {
            if (activeWorker != null)
            {
                VisualWorker vw = activeWorker.visualRepresentation.GetComponent<VisualWorker>();
                vw.ResetBackground();
            }
            base.ReleaseActiveWorker(w);
        }
    }
}
