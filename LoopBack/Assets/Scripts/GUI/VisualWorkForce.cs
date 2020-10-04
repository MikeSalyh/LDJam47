using DG.Tweening;
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
        public PopupWorkerGraphic popup;
        public AudioClip newWorkerSFX, firedSFX, winSFX;
        public AudioSource workAudioSource;
        private AudioSource audioSrc;


        private void Awake()
        {
            audioSrc = GetComponent<AudioSource>();
        }

        protected override Worker AddWorker()
        {
            Worker w = base.AddWorker();
            GameObject.Instantiate(workerVisualObject, workerParent.transform).GetComponent<VisualWorker>().SetWorker(w, workers.Count-1);
            audioSrc.PlayOneShot(newWorkerSFX);
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
            workerParent.DOFade(0f, 0.5f);
            w.visualRepresentation.GetComponent<CanvasGroup>().alpha = 0f;
            audioSrc.PlayOneShot(firedSFX);
            yield return popup.DoGameOver(w.visualRepresentation.GetComponentInChildren<VisualWorker>(), w.visualRepresentation.GetComponent<RectTransform>(), 0.5f);
            yield return new WaitForSeconds(1.5f);
        }

        protected override IEnumerator DoWinAnimation(Worker w)
        {
            workerParent.DOFade(0f, 0.5f);
            audioSrc.PlayOneShot(winSFX);
            w.visualRepresentation.GetComponent<CanvasGroup>().alpha = 0f;
            yield return popup.DoGameWin(w.visualRepresentation.GetComponentInChildren<VisualWorker>(), w.visualRepresentation.GetComponent<RectTransform>(), 0.5f);
            yield return new WaitForSeconds(1.5f);
        }

        protected override void HandleWordFinished(Worker w)
        {
            base.HandleWordFinished(w);
            workAudioSource.PlayOneShot(workAudioSource.clip, 0.5f);
        }

        protected override void HandleWorkDone()
        {
            base.HandleWorkDone();
            workAudioSource.pitch = Random.Range(0.8f, 1.2f);
            workAudioSource.volume = Random.Range(0.2f, 0.3f);
            workAudioSource.Play();
        }
    }
}
