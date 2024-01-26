using PlayerControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCanvasManager : MonoBehaviour
{
    public Transform player;
    public static ItemCanvasManager instance;
    private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _ghost;
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
        _canvasGroup.alpha = 0;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (_ghost)
        {
            _ghost.SetActive(true);
            GhostController.instance.StartChasing();
        }
    }
}
