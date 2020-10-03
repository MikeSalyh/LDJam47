using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workforce : MonoBehaviour
{
    public List<Worker> workers;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateWorkers();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddWorker();
        }
    }

    protected virtual void UpdateWorkers()
    {
        for (int i = 0; i < workers.Count; i++)
        {
            if (workers[i].HasRequestedWord && workers[i].TimeRemainingOnAsk < 0)
            {
                Debug.Log("Time ran out for a worker!");
                workers[i].requestedWord = string.Empty;
                break;
            }
        }
    }

    protected virtual Worker AddWorker()
    {
        Worker newWorker = new Worker();
        newWorker.RequestNewWord();
        workers.Add(newWorker);
        return newWorker;
    }
}
