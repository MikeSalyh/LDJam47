using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gallery : MonoBehaviour
{
    public WorkerAnimation anim;
    public TextMeshProUGUI label;
    private string labelString = "{0}/{1}";
    public int currentIndex = 0;
    public GameObject hider;

    // Start is called before the first frame update
    void Start()
    {
        hider.gameObject.SetActive(false);
        anim.SetCharacter(0);
    }

    // Update is called once per frame
    void Update()
    {
        label.text = string.Format(labelString, currentIndex + 1, Workforce.MAX_WORKERS);
    }

    public void MoveForward()
    {
        currentIndex++;
        currentIndex %= Workforce.MAX_WORKERS;
        anim.SetCharacter(currentIndex);
        anim.PlayLoop();
        hider.SetActive(currentIndex >= MetagameManager.instance.MaxWorkersUnlocked);
    }

    public void MoveBackwards()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = Workforce.MAX_WORKERS - 1;
        }
        anim.SetCharacter(currentIndex);
        anim.PlayLoop();
        hider.SetActive(currentIndex >= MetagameManager.instance.MaxWorkersUnlocked);
    }
}
