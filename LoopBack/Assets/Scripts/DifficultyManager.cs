using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DifficultyManager : MonoBehaviour
{
    public int wordLength = 3;
    public static DifficultyManager instance;

    private int previousScore;
    private int wordsPerLevel = 10;
    private int wordsToBeatLevel;
    private float minWordLength = 2f, maxWordLength = 3.5f;
    public Workforce workforce;

    public void Start()
    {
        NewLevel();
        instance = this;
        StartCoroutine(AddWorkers());
    }

    private IEnumerator AddWorkers()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => workforce.currentWorkState == Workforce.WorkState.workday);
        workforce.AddWorker();
        yield return new WaitForSeconds(3f);
        yield return new WaitUntil(() => workforce.currentWorkState == Workforce.WorkState.workday);
        workforce.AddWorker();

        float addInterval = 8f;
        while (workforce.workers.Count < Workforce.MAX_WORKERS)
        {
            yield return new WaitForSeconds(addInterval);
            yield return new WaitUntil(() => workforce.currentWorkState == Workforce.WorkState.workday);
            workforce.AddWorker();
            addInterval += 2;
        }
    }

    private void NewLevel()
    {
        wordsToBeatLevel = wordsPerLevel;
        previousScore = MetagameManager.instance.score;
        Debug.Log("You're on level " + MetagameManager.instance.level);
    }

    public void FinishLevel()
    {
        MetagameManager.instance.level++;

        minWordLength += 0.25f;
        maxWordLength += 0.4f;
        minWordLength = Mathf.Min(8f, minWordLength);
        maxWordLength = Mathf.Min(8f, maxWordLength);
        NewLevel();
    }


    public void Update()
    {
        if (MetagameManager.instance.score > previousScore)
        {
            wordsToBeatLevel -= (MetagameManager.instance.score - previousScore);
            previousScore = MetagameManager.instance.score;

            if (wordsToBeatLevel <= 0)
            {
                FinishLevel();
            }
        }
    }

    public static int GetWordLength()
    {
        return Random.Range(Mathf.RoundToInt(instance.minWordLength), Mathf.RoundToInt(instance.maxWordLength+1));
    }

    public static float GetAskDuration()
    {
        return 10f;

        //float val = Mathf.InverseLerp(10, 0, MetagameManager.instance.level);
        //return Mathf.Lerp(10f, 5f, val);
    }
}
