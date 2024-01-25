using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PassCode : MonoBehaviour
{
    public string Code = "2110";
    string Nr = null;
    int NrIndex = 0;
    string alpha;
    public TextMeshProUGUI UiText = null;
    public Button[] ListBtn;
    public Button YesBtn;
    public Button ClearBtn;

    public void CodeFunction(string Numbers)
    {
        if (NrIndex >= 4) return;
        NrIndex++;
        Nr = Nr + Numbers;
        UiText.text = Nr;

    }
    public void Enter()
    {
        if (Nr == Code)
        {
            FindAnyObjectByType<PressKeyOpenDoorBathroom>().Unlock();
        }
    }
    public void Delete()
    {
        NrIndex=0;
        Nr = null;
        UiText.text = Nr;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            ListBtn[0].onClick.Invoke();
        } else if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ListBtn[1].onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ListBtn[2].onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ListBtn[3].onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ListBtn[4].onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ListBtn[5].onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ListBtn[6].onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ListBtn[7].onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ListBtn[8].onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ListBtn[9].onClick.Invoke();
        }
        else if(Input.GetKeyDown(KeyCode.Y))
        {
            YesBtn.onClick.Invoke();
            YesBtn.image.CrossFadeColor(YesBtn.colors.normalColor, YesBtn.colors.fadeDuration, true, true);
        }   
        else if(Input.GetKeyDown(KeyCode.C))
        {
            ClearBtn.onClick.Invoke();
        }
    }
}
