using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerGridReference : MonoBehaviour
{
    private WorkerGridPattern[] positions;

    // Start is called before the first frame update
    void Start()
    {
        positions = GetComponentsInChildren<WorkerGridPattern>();    
    }
}
