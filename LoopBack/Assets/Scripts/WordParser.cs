using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordParser : MonoBehaviour
{
    public TextAsset[] wordSourceFiles;
    public static string[][] allWords;
    private static int shortedWordLength;

    // Start is called before the first frame update
    void Start()
    {
        GenerateWordsFromCSVs();
    }

    private void GenerateWordsFromCSVs()
    {
        allWords = new string[wordSourceFiles.Length][];
        for (int i = 0; i < wordSourceFiles.Length; i++)
        {
            allWords[i] = wordSourceFiles[i].text.Split('\n');
        }
        shortedWordLength = allWords[0][0].Length;
        Debug.Log("Generated all the words");
    }

    public static string GetRandomWord(int charLength)
    {
        return allWords[charLength - shortedWordLength + 1][UnityEngine.Random.Range(0, allWords[charLength - shortedWordLength + 1].Length)];
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    Debug.Log("Outputting a 2: " + GetRandomWord(2));
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    Debug.Log("Outputting a 3: " + GetRandomWord(3));
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    Debug.Log("Outputting a 4: " + GetRandomWord(4));
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    Debug.Log("Outputting a 5: " + GetRandomWord(5));
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    Debug.Log("Outputting a 6: " + GetRandomWord(6));
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha7))
        //{
        //    Debug.Log("Outputting a 7: " + GetRandomWord(7));
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha8))
        //{
        //    Debug.Log("Outputting a 8: " + GetRandomWord(8));
        //}
    }
}
