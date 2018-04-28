using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileElement : MonoBehaviour
{

    #region Types
    
    public enum ElementType
    {
		None,
        Cat,
        Chicken,
        Cow,
        Dog,
        Pig,
        Sheep
    }
    
    #endregion

    #region Fields

    [SerializeField]
    private Color normalColor;

    [SerializeField]
    private Color selectedColor;

    [SerializeField]
    private Image backgroundImage;

    [SerializeField]
    private Image elementImage;

    [SerializeField]
    private List<Sprite> elementSprites;

    #endregion

    #region Properties

    public ElementType Type { get; private set; }

    #endregion
    
    #region Mono

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        SetType(ElementType.None);
    }

    private void Start()
    {
        TileController.I.OnSelectionTileChanged += OnSelectionTileChanged;
    }

    #endregion

    #region Public api

    public void SetType(ElementType type)
    {
        Type = type;
        if (type == ElementType.None)
        {
            elementImage.enabled = false;
        }
        else
        {
            elementImage.sprite = elementSprites[(int) type - 1];
            elementImage.enabled = true;
        }
    }

    #endregion

    #region Events handlers

    private void OnClick()
    {
        TileController.I.OnTileElementClick(this);
    }

    private void OnSelectionTileChanged(TileElement oldTile, TileElement newTile)
    {
        if (oldTile == this)
        {
            backgroundImage.color = normalColor;
        }

        if (newTile == this)
        {
            backgroundImage.color = selectedColor;
        }
    }

    #endregion

}
