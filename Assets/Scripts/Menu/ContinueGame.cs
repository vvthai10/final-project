﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ContinueGame : MonoBehaviour {

 public Button startButton;


public void Start()
{
	startButton.interactable = intToBool(PlayerPrefs.GetInt("continueGame", 0));
}


int boolToInt(bool val)
{
    if (val)
        return 1;
    else
        return 0;
}

bool intToBool(int val)
{
    if (val != 0)
        return true;
    else
        return false;
}
		
public void HiddenButton()
{
    PlayerPrefs.SetInt("continueGame", boolToInt(startButton.interactable));
	PlayerPrefs.Save();
}

}

