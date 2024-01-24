using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCanvasManager : MonoBehaviour
{
    public static ItemCanvasManager instance;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Show()
    {
        _canvasGroup.alpha = 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PlayerControl.PlayerController.instance.Freeze();
    }
    public void Hide()
    {
        _canvasGroup.alpha = 0;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        PlayerControl.PlayerController.instance.Unfreeze();
    }
}
