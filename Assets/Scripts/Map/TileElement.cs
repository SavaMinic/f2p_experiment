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

    [Header("Animations")]
    [SerializeField]
    private float jumpAnimationTime = 0.1f;
    
    [SerializeField]
    private float twitchAnimationTime = 0.1f;
    
    [SerializeField]
    private float spawnAnimationTime = 0.1f; 

    [SerializeField]
    private GoEaseType spawnEaseType;
    
    [SerializeField]
    private float explosionAnimationTime = 0.1f;

    [SerializeField]
    private GoEaseType explodeEaseType;

    private IEnumerator crossFadeCoroutine;

    private bool isExploding;

    #endregion

    #region Properties

    public ElementType Type { get; private set; }

    public bool IsEmpty
    {
        get { return Type == ElementType.None || isExploding; }
    }
    
    public Location Location { get; private set; }
    
    public Sprite Sprite
    {
        get { return GameSettings.I.elementSprites[(int) Type - 1]; }
    }
    
    public Button Button { get; private set; }
    public RectTransform RectTransform { get; private set; }

    #endregion
    
    #region Mono

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClick);
        SetType(ElementType.None);
        Unselect();
    }

    #endregion

    #region Public api

    public void Initialize(int x, int y)
    {
        backgroundImage.color = normalColor;
        Location = new Location(x, y);
        SetType(ElementType.None);
    }

    public void SetType(ElementType type)
    {
        Type = type;
        elementImage.rectTransform.localScale = Vector3.one;
        if (type == ElementType.None)
        {
            elementImage.enabled = false;
        }
        else
        {
            elementImage.sprite =  GameSettings.I.elementSprites[(int) type - 1];
            elementImage.enabled = true;
        }
    }

    public void Spawn(ElementType type)
    {
        SetType(type);
        DoSpawnAnimation();
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

    public int Explode(int index)
    {
        if (isExploding)
            return 0;

        isExploding = true;
        StartCoroutine(DoExplodeAnimation(index));
        
        // TODO: return score depending on level of element
        return 10;
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

    private void DoSpawnAnimation()
    {
        elementImage.rectTransform.localScale = Vector3.zero;
        Go.to(elementImage.rectTransform, spawnAnimationTime, 
            new GoTweenConfig().vector3Prop("localScale", Vector3.one).setEaseType(spawnEaseType)
        );
    }

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

    private IEnumerator DoExplodeAnimation(int index)
    {
        yield return new WaitForSecondsRealtime(index * explosionAnimationTime / 3f);
        
        Go.to(elementImage.rectTransform, explosionAnimationTime, 
            new GoTweenConfig().vector3Prop("localScale", Vector3.zero).setEaseType(explodeEaseType)
        );
        
        yield return new WaitForSecondsRealtime(explosionAnimationTime);
        
        SetType(ElementType.None);
        isExploding = false;
    }

    #endregion

}
