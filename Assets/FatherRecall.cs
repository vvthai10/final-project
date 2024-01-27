using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FatherRecall : MonoBehaviour
{
    public static bool IsDone = false;
    public GameObject uiPanel;
    public Text uiText;
    public float typingSpeed = 0.35f;

    private bool isTyping = false;
    [SerializeField] private GameObject _ghost;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTyping)
        {
            Time.timeScale = 0f;
            isTyping = true;
            uiPanel.SetActive(true);

            StartCoroutine(TypeText());
        }
    }

    IEnumerator TypeText()
    {
        float timeElapsed = 0f;
        int currentIndex = 0;
        string fullText = "Through the eerie hallways of memory, I recall the evening that my father inadvertently let loose a dark force. Eerie incantations surged through the chamber, bringing shadows closer. The line dividing the worlds became less distinct as mysterious symbols filled the atmosphere. The ceremony, an outlaw dance, tied me to an ungodly force. My innocence was devoured in the symphony of demonic laughter, making me a receptacle for an evil I did not create. It was me, not him, who was chosen.";

        while (currentIndex < fullText.Length)
        {
            uiText.text += fullText[currentIndex];
            currentIndex++;

            // Chờ theo thời gian thực mà không bị ảnh hưởng bởi Time.timeScale
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        FatherRecall.IsDone = true;
        _ghost.SetActive(false);
    }
}
