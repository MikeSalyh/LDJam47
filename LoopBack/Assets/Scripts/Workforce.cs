using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Workforce : MonoBehaviour
{
    public List<Worker> workers;
    private Worker activeWorker;
    public int wordLength = 3;

    // Update is called once per frame
    protected virtual void Update()
    {
        UpdateWorkers();
        HandleInput();

        //Debug:
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
        newWorker.OnRequestNewWord += GenerateNewWord;
        newWorker.OnFinishWord += ReleaseActiveWorker;
        GenerateNewWord(newWorker);
        workers.Add(newWorker);
        return newWorker;
    }

    protected virtual void GenerateNewWord(Worker w)
    {
        List<char> bannedCharacters = new List<char>();
        for (int i = 0; i < workers.Count; i++)
        {
            if (!workers[i].Fired && !string.IsNullOrEmpty(workers[i].CurrentWord))
                bannedCharacters.Add(workers[i].CurrentWord[0]);
        }
        w.SetWord(WordParser.GetRandomWord(wordLength, bannedCharacters.ToArray()));
    }

    protected virtual void ReleaseActiveWorker(Worker w)
    {
        activeWorker = null;
    }

    private void HandleInput()
    {
        string reqChar;
        if (activeWorker != null && !activeWorker.Fired && !activeWorker.WordComplete)
        {
            reqChar = activeWorker.NextLetter.ToString().ToLowerInvariant();
            if (!string.IsNullOrEmpty(reqChar) && Input.GetKeyDown(reqChar))
            {
                activeWorker.DoWork();
            }
        } else {
            for (int i = 0; i < workers.Count; i++)
            {
                if (workers[i].Fired || workers[i].WordComplete)
                    continue;

                reqChar = workers[i].NextLetter.ToString().ToLowerInvariant();
                if (string.IsNullOrEmpty(reqChar))
                    continue;

                if (Input.GetKeyDown(reqChar))
                {
                    activeWorker = workers[i];
                    if(workers[i].OnSelected != null)
                        workers[i].OnSelected.Invoke(); //This is a little janky, no?
                    activeWorker.DoWork();
                    break;
                }
            }
        }
    }
}
