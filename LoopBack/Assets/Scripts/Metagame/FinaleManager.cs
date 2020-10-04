using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinaleManager : MonoBehaviour
{
    public void GoToMainMenu()
    {
        MetagameManager.instance.GoToMenu();
    }
}
