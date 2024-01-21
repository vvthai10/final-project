using UnityEngine;

public class ChangeTexture : MonoBehaviour
{
    public Texture newTexture; // Texture mới mà bạn muốn sử dụng
    public Renderer picture;

    void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem đối tượng đi vào trigger có phải là người chơi hay không
        if (other.CompareTag("Player"))
        {
            // Gọi hàm để thay đổi texture khi đi vào trigger
            ChangeObjectTexture(newTexture);
        }
    }

    // Hàm để thay đổi texture của Material
    void ChangeObjectTexture(Texture newTexture)
    {
        if (picture != null && newTexture != null)
        {
            picture.material.mainTexture = newTexture;
        }
        else
        {
            Debug.LogError("Renderer or newTexture is null.");
        }
    }
}
