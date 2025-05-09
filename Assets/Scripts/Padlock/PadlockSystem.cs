using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadlockSystem : MonoBehaviour
{
    private PadLockPassword padLockPassword;
    private MoveRuller moveRuller;

    public GameObject padlockController;
    public GameObject mainCharacter;

    // Start is called before the first frame update
    public bool bathroomKey = false;
   
    void Start()
    {
        padLockPassword = FindAnyObjectByType<PadLockPassword>();
        moveRuller = FindAnyObjectByType<MoveRuller>();

    }

    // Update is called once per frame
    void Update()
    {

        //if(Input.GetKeyDown(KeyCode.Space) && !isActive)
        //{
        //    OpenPadlock();
        //    isActive = true;
        //    return;
        //}

        //if(Input.GetKeyDown(KeyCode.Space) && isActive)
        //{
        //    ClosePadlock();
        //    isActive = false;
        //    return;
        //}
    }

    public void OpenPadlock()
    {
        padlockController.SetActive(true);
        mainCharacter.SetActive(false);
    }

    public void ClosePadlock()
    {
        padlockController.SetActive(false);
        mainCharacter.SetActive(true);
    }

    public void InvalidPadlockSystem()
    {
       
    }

}
