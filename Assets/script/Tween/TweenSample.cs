using UnityEngine;
using DG.Tweening;
using TMPro;
public class TweenSample : MonoBehaviour
{
    [Header("효과를 위한 UI/Object 타겟")]
    public RectTransform UItarget;
    public GameObject Objecttarget;

    [Header("글자 연출 타겟")]
    public TMP_Text countText;
    public int currentValue = 0;
    public int addValue = 100;

    private int targetValue;

    [Header("색 변형 연출 예시")]
    public Color fiashColor = Color.yellow;

    private Color originalColor;

    [Header("페이드 UI 그룹")]
    public CanvasGroup fadeTarget;

    void Start()
    {
        countText.text = currentValue.ToString();

        originalColor = countText.color;

        fadeTarget.alpha = 0;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayPunchUIScale();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayPunchObjectScale();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayUIShake();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayCountUp();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlayColorFlash();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            PlayFade();
        }
    }

    public void PlayPunchUIScale()
    {
        if (UItarget == null) return;
        UItarget.DOKill();
        UItarget.localScale = Vector3.one;
        UItarget.DOPunchScale(Vector3.one * 0.3f, 0.25f, 8, 1.0f);
    }

    public void PlayPunchObjectScale()
    {
        if (UItarget == null) return;
        UItarget.DOKill();
        UItarget.localScale = Vector3.one;
        UItarget.DOPunchScale(Vector3.one * 0.3f, 0.25f, 8, 1.0f);
    }

    public void PlayUIShake()
    {
        if (UItarget == null) return;
        UItarget.DOKill();
        UItarget.DOShakeAnchorPos(0.3f, 20f, 20, 90f);
    }

    public void PlayCountUp()
    {
        if (countText == null) return;

        targetValue += addValue;
        DOTween.Kill("CountTween", true);

        DOTween.To(
            () => currentValue,
            value =>
            {
                currentValue = value;
                countText.text = currentValue.ToString();
            },
            targetValue,
            0.5f
        )
        .SetEase(Ease.OutQuad)
        .SetId("CountTween");
    }

    public void PlayColorFlash()
    {
        if (countText == null) return;
        countText.DOKill();
        countText.color = originalColor;

        countText.DOColor(fiashColor, 0.1f)
            .OnComplete(() =>
            {
                countText.DOColor(originalColor, 0.2f);
            });
    }

    public void PlayFade()
    {
        if (fadeTarget == null) return;
        fadeTarget.DOKill();
        fadeTarget.alpha = 0;

        Sequence seq = DOTween.Sequence();

        seq.Append(fadeTarget.DOFade(1f, 0.2f));
        seq.AppendInterval(0.5f);
        seq.Append(fadeTarget.DOFade(0f, 0.3f));

    }

}
