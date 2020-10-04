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
        yield return new WaitForSeconds(0.25f);
        workforce.TryToAddWorker();
        yield return new WaitForSeconds(1f);
        while (workforce.workers.Count < Workforce.MAX_WORKERS)
        {
            int pointsToNextHire = MetagameManager.instance.score + (workforce.workers.Count * 2);  //you need to do X words / person
            yield return new WaitUntil(()=> MetagameManager.instance.score > pointsToNextHire);
            yield return new WaitUntil(() => !workforce.TryingToAddWorker);
            workforce.TryToAddWorker();
        }

        yield break;
        //Handle winning end-game.
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
        return 12f;
    }
}
