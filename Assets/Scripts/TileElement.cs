using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileElement : MonoBehaviour
{

    #region Mono

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    #endregion

    #region Events handlers

    private void OnClick()
    {
        TileController.Instance.OnTileElementClick(this);
    }

    #endregion

}
