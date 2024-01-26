using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCanvasManager : MonoBehaviour
{
    public static BookCanvasManager instance;
    private CanvasGroup _canvasGroup;

    private bool isShowing = false;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Update()
    {
        if (isShowing)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
    public void Show()
    {
        _canvasGroup.alpha = 1;
        isShowing = true;
    }
    public void Hide()
    {
        isShowing = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _canvasGroup.alpha = 0;
    }
}
