using System.Collections.Generic;
using UnityEngine;

public class ColorProperty : MonoBehaviour
{
    [SerializeField] string baseColorId = "_Color";
    [SerializeField] public Color color;
    Renderer renderer;
    public int materialIndex;


    void OnValidate()
    {
        SetProperty();
    }

    public void ChangeColor(Color color)
    {
        this.color = color;
        SetProperty();
    }

    void SetProperty()
    {
        if (renderer == null) renderer = GetComponent<Renderer>();
        if (color == Color.black)
        {
            List<Material> materials = new List<Material>();
            renderer.GetSharedMaterials(materials);
            color = materials[materialIndex].color;
        }

        var property = new MaterialPropertyBlock();
        property.SetColor(baseColorId, color);
        renderer.SetPropertyBlock(property);
    }

    void Awake()
    {
        SetProperty();
    }
}