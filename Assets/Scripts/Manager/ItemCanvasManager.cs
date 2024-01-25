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
        player.GetComponent<PlayerControl.PlayerController>().Freeze();
        FPSController playerFPSController = player.GetComponent<FPSController>();
        playerFPSController.Freeze();
    }
    public void Hide()
    {
        _canvasGroup.alpha = 0;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<PlayerControl.PlayerController>().Unfreeze();
        FPSController playerFPSController = player.GetComponent<FPSController>();
        playerFPSController.Unfreeze();
        if (_ghost)
        {
            _ghost.SetActive(true);
            GhostController.instance.StartChasing();
        }
    }
}
