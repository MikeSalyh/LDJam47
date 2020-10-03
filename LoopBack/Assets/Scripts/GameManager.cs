using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Work.GUI;

public class GameManager : MonoBehaviour
{
    public VisualWorkForce guiWorkers;
    public int maxBreaks = 3;
    public int BreaksAvailable { get; private set; }
    public int maxLives = 3;
    public int LivesLeft { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
