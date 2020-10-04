using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void GoToGameplay()
    {
        MetagameManager.instance.GoToGameplay();
    }
}
