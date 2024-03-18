using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class OptionsController : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Color defaultColor;
    public Color hoverColor;
    private StoryScene scene;
    private TextMeshProUGUI textMesh;
    private OptionsLogicController controller;
    private bool isClicked = false; // Debounce flag

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.color = defaultColor;
    }

    public float GetHeight()
    {
        return textMesh.rectTransform.sizeDelta.y * textMesh.rectTransform.localScale.y;
    }

    public void Setup(ChooseScene.ChooseLabel label, OptionsLogicController controller, float y)
    {
        scene = label.nextScene;
        textMesh.text = label.text;
        this.controller = controller;

        Vector3 position = textMesh.rectTransform.localPosition;
        position.y = y;
        textMesh.rectTransform.localPosition = position;
        
        isClicked = false; // Reset debounce flag when setting up
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isClicked) // Check if not already clicked
        {
            isClicked = true; // Set flag to true to block further clicks
            controller.PerformChoose(scene);
            // Optionally, reset isClicked to false after some time or after certain conditions are met
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textMesh.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textMesh.color = defaultColor;
    }
}
