using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCanvasManager : MonoBehaviour
{
    public static BookCanvasManager instance;
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
        Cursor.lockState = CursorLockMode.Confined;
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
