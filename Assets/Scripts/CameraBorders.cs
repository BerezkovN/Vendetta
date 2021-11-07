using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class CameraBorders : MonoBehaviour
{
    [SerializeField] private RectTransform _topImage;
    [SerializeField] private RectTransform _bottomImage;
    [SerializeField] private float _speed = 0.2f;

    private Canvas _canvas;

    private Coroutine _currentAction;
    private float _referenceHeight;

    private void Start()
    {
        _canvas = GetComponent<Canvas>();

        _referenceHeight = _canvas.pixelRect.height;

        _topImage.localPosition = new Vector3(0, _referenceHeight);
        _topImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _referenceHeight);

        _bottomImage.localPosition = new Vector3(0, -_referenceHeight);
        _bottomImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _referenceHeight);
    }

    public void Show(float percent)
    {
        if (_currentAction != null)
            StopCoroutine(_currentAction);

        _currentAction = StartCoroutine(StartAnimation(percent));
    }

    public void Hide()
    {
        if (_currentAction != null)
            StopCoroutine(_currentAction);

        _currentAction = StartCoroutine(StartAnimation(0));
    }

    [ContextMenu("Test Show 25%")]
    public void TestShow25()
    {
        Show(.25f);
    }

    [ContextMenu("Test Show 35%")]
    public void TestShow35()
    {
        Show(.35f);
    }

    [ContextMenu("Test Show 100%")]
    public void TestShow100()
    {
        Show(1);
    }

    [ContextMenu("Test Hide")]
    public void TestHide()
    {
        Hide();
    }

    private IEnumerator StartAnimation(float percent)
    {
        float startTopY = _topImage.localPosition.y;
        float startBottomY = _bottomImage.localPosition.y;

        float destTopY = _referenceHeight - (percent / 2 * _referenceHeight);
        float destBottomY = -destTopY;
        float completePercent = 0;

        while (completePercent != 1)
        {
            completePercent = Mathf.Min(1, completePercent + _speed * Time.deltaTime); 

            _topImage.localPosition = new Vector3(_topImage.localPosition.x, Mathf.Lerp(startTopY, destTopY, completePercent));
            _bottomImage.localPosition = new Vector3(_bottomImage.localPosition.x, Mathf.Lerp(startBottomY, destBottomY, completePercent));

            yield return null; 
        }
    }
}
