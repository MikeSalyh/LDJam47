﻿using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Work.GUI
{
    public class VisualWorkForce : Workforce
    {
        public GameObject workerVisualObject;
        public CanvasGroup workerParent;
        public WorkerGridReference workerGrid;
        public FiredWorkerGraphic firedWorker;

        protected override Worker AddWorker()
        {
            Worker w = base.AddWorker();
            GameObject.Instantiate(workerVisualObject, workerParent.transform).GetComponent<VisualWorker>().SetWorker(w, workers.Count-1);
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
                vw.HandleNewWord();
            }
            base.ReleaseActiveWorker(w);
        }

        protected override IEnumerator DoFiredWorkerAnimation(Worker w)
        {
            workerParent.DOFade(0.5f, 0.5f);
            w.visualRepresentation.GetComponent<CanvasGroup>().alpha = 0f;
            firedWorker.Appear(w.visualRepresentation.GetComponentInChildren<TextMeshProUGUI>().text, w.visualRepresentation.GetComponent<RectTransform>(), 0.5f);
                //This will need to be adjusted once I have the real graphics.
            yield return new WaitForSeconds(0.75f);
            firedWorker.SetToFiredMode();
            yield return new WaitForSeconds(0.5f);
        }
    }
}
