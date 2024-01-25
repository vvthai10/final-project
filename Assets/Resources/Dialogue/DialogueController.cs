using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
// ? m?i vòng l?p, khi player t?i g?n vùng g?n radio thì âm thanh s? b?t lên
// ?o?n h?i tho?i và audio t??ng ?ng s? ch?y 
// bi?n isSoundPlaying[index] s? set v? true ?? nó ko th? phát l?i 
public class DialogueController : MonoBehaviour
{
    public Conversation[] conversations;
    public int index;
    public AudioClip[] audioClips;
    private bool[] isSoundPlaying;

    public TextMeshProUGUI dialogText;
    public AudioSource audioSource;

    private IEnumerator curScriptPlay;
    void Start()
    {
        isSoundPlaying = new bool[conversations.Length];
        for  (int i = 0; i < conversations.Length; i++)
        {
            isSoundPlaying[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    StartConversation();
        //}
    }

    public bool CanPlayRadio()
    {
        return isSoundPlaying[index] == false && index < conversations.Length && !audioSource.isPlaying;
    }

    private bool isGoToTheNextLoop()
    {
        return index + 1 < conversations.Length && isSoundPlaying[index] == true;
    }

    //g?i khi player b??c qua c?a và b?t ??u 1 vòng l?p m?i
    public void onNextLoop()
    {
        if(isGoToTheNextLoop())
        {
            if(audioSource.isPlaying)
            {
                StopAudio();
            }
            index++;
        }
    }

    //xóa text khi ?o?n radio phát xong
    private void OnFinished()
    {
        ClearText();
    }

    //stop dialouge system when have event
    public void StopAudio()
    {
        if(curScriptPlay != null)
        {
            StopCoroutine(curScriptPlay);
        }
        ClearText();
        if(audioSource.isPlaying)
        {
            audioSource.Stop();
        }
       
    }

    public void StartConversation()
    {
        StopAudio();
        AudioClip clip = audioClips[index];
        Conversation conver = conversations[index];
        isSoundPlaying[index] = true;
        audioSource.clip = clip;
        audioSource.Play();
        ShowDialogue(conver);
    }

    private IEnumerator ShowCo(Conversation conversation)
    {
        dialogText.text = "";
        float waitTime = calculateShowTextSpeech();
        yield return new WaitForSeconds(4.32f);
        foreach (string paragraph in conversation.paragraph)
        {
            dialogText.text = "";
            
            foreach (string letter in paragraph.Split(' '))
            {
                dialogText.text += letter;
                dialogText.text += " ";
                yield return new WaitForSeconds(waitTime);
            }
        }
        yield return new WaitForSeconds(1);
        StopAudio();
    }

    private void ShowDialogue(Conversation conversation)
    {
        curScriptPlay = ShowCo(conversation);
        StartCoroutine(curScriptPlay);
    }
    private void ClearText()
    {
        dialogText.text = "";
    }

    //Tinh thoi gian giua cac t? ???c show 
    private float calculateShowTextSpeech()
    {
        float lengthInSecOfAudio = audioClips[index].length;
        float numberOfLetter = 0;
        foreach (var item in conversations[index].paragraph)
        {
            var size = item.Split(' ').Length;
            numberOfLetter += size;
        }
        return lengthInSecOfAudio / numberOfLetter;
    }
}
