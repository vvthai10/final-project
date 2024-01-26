using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FatherRecall : MonoBehaviour
{
    public static bool IsDone = false;
    public GameObject uiPanel;
    public Text uiText;
    public float typingSpeed = 0.1f;

    private bool isTyping = false;

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
        string fullText = "Chữ muốn hiển thị, khi dòng chữ này chạy xong thì mới cho dùng chuột để tắt";
        int currentIndex = 0;

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
    }
}
