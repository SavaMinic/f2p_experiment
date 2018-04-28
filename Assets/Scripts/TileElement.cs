using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileElement : MonoBehaviour
{

    #region Fields

    [SerializeField]
    private Color normalColor;

    [SerializeField]
    private Color selectedColor;

    [SerializeField]
    private Image backgroundImage;

    #endregion
    
    #region Mono

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void Start()
    {
        TileController.Instance.OnSelectionTileChanged += OnSelectionTileChanged;
    }

    #endregion

    #region Events handlers

    private void OnClick()
    {
        TileController.Instance.OnTileElementClick(this);
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
