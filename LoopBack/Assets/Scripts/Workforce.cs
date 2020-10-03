using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workforce : MonoBehaviour
{
    public List<Worker> workers;

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateWorkers();
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AddWorker();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            for (int i = 0; i < workers.Count; i++)
            {
                workers[i].DoWork();
            }
        }
    }

    protected virtual void UpdateWorkers()
    {
        for (int i = 0; i < workers.Count; i++)
        {
            workers[i].UpdateWorker();
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
