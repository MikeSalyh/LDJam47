using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleAnimLoop : MonoBehaviour
{
    public Sprite[] sprites;
    public float animDuration = 1f;
    private int currentFrame = 0;
    private Image img;

    // Start is called before the first frame update
    void Awake()
    {
        img = GetComponent<Image>();
    }

    private void Start()
    {
        Play();
    }

    public void Play()
    {
        StartCoroutine(PlayRoutine());
    }

    private IEnumerator PlayRoutine()
    {
        float timePerFrame = animDuration / sprites.Length;
        int i = 0;
        while (true)
        {
            img.sprite = sprites[i];
            i++;
            i %= sprites.Length;
            yield return new WaitForSeconds(timePerFrame);
        }
    }
}
