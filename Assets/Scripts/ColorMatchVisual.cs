using UnityEngine;
using Random = UnityEngine.Random;

public class ColourMatchVisual : MonoBehaviour
{
    [SerializeField] private Renderer basketballRenderer;
    [SerializeField] private Renderer netRenderer;

    [SerializeField] private float nextChangeColor = 5f;
    [SerializeField] private Color[] colors;

    private MaterialPropertyBlock propertyBlock;
    private float currentTime;
    private int currentColorIndex;
    private int lastChosenColorIndex;
    
    private static readonly int BaseColor =
        Shader.PropertyToID("_BaseColor");

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
    }

    public void SetColour(Color colour)
    {
        SetRendererColour(basketballRenderer, colour);
        SetRendererColour(netRenderer, colour);
    }

    private void SetRendererColour(Renderer renderer, Color colour)
    {
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(BaseColor, colour);
        renderer.SetPropertyBlock(propertyBlock);
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= nextChangeColor)
        {
            SetColour(GetRandomColor());
            currentTime = 0;
            Debug.Log($"Color has Changed");
        }
    }

    private Color GetRandomColor()
    {
        if (colors == null) return Color.white;
        var length = colors.Length - 1;
        var randomIndex = Random.Range(0, length );

        return colors[randomIndex];
    }
}