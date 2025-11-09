using UnityEngine;
using UnityEngine.UI;

public class MenuBackgroundParallax : MonoBehaviour
{
    [SerializeField] private RectTransform background;
    [SerializeField] private float parallaxAmount = 20f; 

    private Vector2 initialPosition;

    void Start()
    {
        if (background == null)
            background = GetComponent<RectTransform>();

        initialPosition = background.anchoredPosition;
    }

    void Update()
    {
        float moveX = (Input.mousePosition.x / Screen.width - 0.5f) * 2;
        float moveY = (Input.mousePosition.y / Screen.height - 0.5f) * 2;

        Vector2 offset = new Vector2(moveX, moveY) * parallaxAmount;
        background.anchoredPosition = Vector2.Lerp(
            background.anchoredPosition,
            initialPosition + offset,
            Time.deltaTime * 3f
        );
    }
}