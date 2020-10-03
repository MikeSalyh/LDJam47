using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerGridPattern : MonoBehaviour
{
    public float GetCellSize()
    {
        return GetComponentInChildren<Image>().GetComponent<RectTransform>().sizeDelta.x;
    }

    public Vector3[] GetPositions()
    {
        Image[] allCells = GetComponentsInChildren<Image>();
        Vector3[] gridPositions = new Vector3[allCells.Length];
        for (int i = 0; i < allCells.Length; i++)
        {
            gridPositions[i] = allCells[i].GetComponent<RectTransform>().position;
        }
        return gridPositions;
    }
}
