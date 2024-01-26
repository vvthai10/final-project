using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FatherRecall : MonoBehaviour
{
    public Text uiText;  // Kéo và thả UI Text vào trường này trong Inspector
    public float typingSpeed = 0.1f;  // Tốc độ hiển thị chữ

    private bool isTyping = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTyping)
        {
            isTyping = true;
            StartCoroutine(TypeText());
        }
    }

    IEnumerator TypeText()
    {
        foreach (char letter in "Chữ muốn hiển thị")
        {
            uiText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
