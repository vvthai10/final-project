using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursorRecall : MonoBehaviour
{
    public GameObject FatherRitual;
    public GameObject Father;

    public void HideCursor()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Father.SetActive(false);
        FatherRitual.SetActive(false);
    }

}
