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

    [Header("Animations")]
    [SerializeField]
    private float jumpAnimationTime = 0.1f;
    
    [SerializeField]
    private float twitchAnimationTime = 0.1f;

    private IEnumerator crossFadeCoroutine;

    #endregion

    #region Properties

    public ElementType Type { get; private set; }

    public bool IsEmpty
    {
        get { return Type == ElementType.None; }
    }
    
    public Location Location { get; private set; }
    
    public Sprite Sprite
    {
        get { return elementSprites[(int) Type - 1]; }
    }
    
    public Button Button { get; private set; }

    #endregion
    
    #region Mono

    private void Awake()
    {
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClick);
        SetType(ElementType.None);
        Unselect();
    }

    #endregion

    #region Public api

    public void Initialize(int x, int y)
    {
        Location = new Location(x, y);
        SetType(ElementType.None);
    }

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

    public void Select()
    {
        backgroundImage.color = selectedColor;
        StartCoroutine(DoJumpAnimation());
    }

    public void Unselect()
    {
        backgroundImage.color = normalColor;
    }

    public void CantBeSelected()
    {
        StartCoroutine(DoTwitchAnimation());
    }

    public void CrossFade(float duration)
    {
        if (crossFadeCoroutine != null)
        {
            StopCoroutine(crossFadeCoroutine);
        }
        crossFadeCoroutine = DoCrossFade(duration);
        StartCoroutine(crossFadeCoroutine);
    }

    #endregion

    #region Events handlers

    private void OnClick()
    {
        TileController.I.OnTileElementClick(this);
    }

    #endregion

    #region Private

    private IEnumerator DoCrossFade(float duration)
    {
        Go.to(backgroundImage, duration / 5f * 2f, new GoTweenConfig().colorProp("color", selectedColor));
        
        yield return new WaitForSecondsRealtime(duration / 5f * 3f);
        
        Go.to(backgroundImage, duration / 5f * 2f, new GoTweenConfig().colorProp("color", normalColor));
    }

    #endregion

    #region Animations

    private IEnumerator DoJumpAnimation()
    {
        Go.to(elementImage.rectTransform, jumpAnimationTime, new GoTweenConfig().vector2Prop("offsetMin", new Vector2(0, 10)));
        Go.to(elementImage.rectTransform, jumpAnimationTime, new GoTweenConfig().vector2Prop("offsetMax", new Vector2(0, 10)));
        
        yield return new WaitForSecondsRealtime(jumpAnimationTime);
        
        Go.to(elementImage.rectTransform, jumpAnimationTime, new GoTweenConfig().vector2Prop("offsetMin", Vector3.zero));
        Go.to(elementImage.rectTransform, jumpAnimationTime, new GoTweenConfig().vector2Prop("offsetMax", Vector3.zero));
    }

    private IEnumerator DoTwitchAnimation()
    {
        Go.to(elementImage.rectTransform, twitchAnimationTime, new GoTweenConfig().vector2Prop("offsetMin", new Vector2(0, 10)));
        Go.to(elementImage.rectTransform, twitchAnimationTime, new GoTweenConfig().vector2Prop("offsetMax", new Vector2(0, -10)));
        
        yield return new WaitForSecondsRealtime(twitchAnimationTime);
        
        Go.to(elementImage.rectTransform, twitchAnimationTime, new GoTweenConfig().vector2Prop("offsetMin", Vector3.zero));
        Go.to(elementImage.rectTransform, twitchAnimationTime, new GoTweenConfig().vector2Prop("offsetMax", Vector3.zero));
    }

    #endregion

}
