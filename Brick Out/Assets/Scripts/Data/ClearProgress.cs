using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearProgress : MonoBehaviour
{
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
    }
}
