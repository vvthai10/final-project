using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadlockCanvasController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject BathroomKey;
    public GameObject CombinationPadlock;
    public GameObject UnlockNotify;
    public GameObject Instruction;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UnlockKey()
    {
        BathroomKey.SetActive(true);
        CombinationPadlock.SetActive(false);
        Instruction.SetActive(false);
        UnlockNotify.SetActive(true);
    }
}
