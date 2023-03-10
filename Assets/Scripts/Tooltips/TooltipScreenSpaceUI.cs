using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using VirtualLab.ApplicationData;

public class TooltipScreenSpaceUI : MonoBehaviour
{
    public RectTransform canvasRect;
    public RectTransform backgroundRect;
    public RectTransform tailRect;
    public TextMeshProUGUI textMeshPro;

    private RectTransform _rect;

    private Vector2 _anchoredPosition;
    private Vector2 _anchoredPositionTail;
    private Vector2 _positionTooltip;

    private Image _imageBackground;
    private Image _imageBackgroundOutline;
    private Image _imageTail;
    private Image _imageTailOutline;

    private Coroutine _coroutineShowOrHideTooltip;

    private readonly float _heightHeader = 48f;

    private Dictionary<int, string> _tooltips;
    private List<string> _allLines;

    public Vector3 SetPositionTooltip
    {
        set => _positionTooltip = Camera.main.WorldToScreenPoint(value);
    }

    public Dictionary<int, string> Tooltips
    {
        get
        {
            if (_tooltips == null || _tooltips.Count == 0)
                new StringLoader(CreateAllList).Start("Tooltips.txt");
            return _tooltips;
        }
    }

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _allLines = new List<string>();
        _imageBackground = backgroundRect.GetComponent<Image>();
        _imageBackgroundOutline = backgroundRect.GetChild(0).GetComponent<Image>();
        _imageTail = tailRect.GetComponent<Image>();
        _imageTailOutline = tailRect.GetChild(0).GetComponent<Image>();

        SetText(textMeshPro.text);
        
        new StringLoader(CreateAllList).Start("Tooltips.txt");
    }

    private void Update()
    {
        BackgroundAnchorPosition();
        FlipTail();
    }

    public void ShowTooltip(string text)
    {
        if (_coroutineShowOrHideTooltip != null)
        {
            StopCoroutine(_coroutineShowOrHideTooltip);
        }

        _coroutineShowOrHideTooltip = StartCoroutine(CoroutineShowAndHideTooltip());
        SetText(text);
    }

    public void HideTooltip()
    {
        if (_coroutineShowOrHideTooltip != null)
        {
            StopCoroutine(_coroutineShowOrHideTooltip);
            ChangeAlphaTooltip(0);
        }
    }
    
    private void CreateAllList(string data)
    {
        _allLines = data.Split('\n').ToList();
        WriteTooltipsFromPath(); 
    }

    private void SwitchActiveTooltip(bool value)
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(value);
        }

        tailRect.gameObject.SetActive(value);
    }

    private IEnumerator CoroutineShowAndHideTooltip()
    {
        Color32 color = _imageBackground.color;
        byte alpha = color.a;
        byte edge = 255;


        SwitchActiveTooltip(true);
        ChangeAlphaTooltip(0);
        yield return new WaitForSeconds(0.5f);

        while (alpha != edge)
        {
            alpha = (alpha < edge) ? (byte)(alpha + 85) : (byte)(alpha - 85);
            alpha = Math.Clamp(alpha, (byte)0, (byte)255);
            ChangeAlphaTooltip(alpha);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        edge = 0;
        
        while (alpha != edge)
        {
            alpha = (alpha < edge) ? (byte)(alpha + 85) : (byte)(alpha - 85);
            alpha = Math.Clamp(alpha, (byte)0, (byte)255);
            ChangeAlphaTooltip(alpha);
            yield return new WaitForFixedUpdate();
        }
       
        SwitchActiveTooltip(false);
    }

    private void ChangeAlphaTooltip(byte value)
    {
        var colorBackground = (Color32)_imageBackground.color;
        var colorBackgroundOutline = (Color32)_imageBackgroundOutline.color;
        var colorTail = (Color32)_imageTail.color;
        var colorTailOutline = (Color32)_imageTailOutline.color;
        var colorText = textMeshPro.color;

        colorBackground.a = value;
        colorBackgroundOutline.a = value;
        colorTail.a = value;
        colorTailOutline.a = value;
        colorText.a = value;

        _imageBackground.color = colorBackground;
        _imageBackgroundOutline.color = colorBackgroundOutline;
        _imageTail.color = colorTail;
        _imageTailOutline.color = colorTailOutline;
        textMeshPro.color = colorText;
    }

    private void WriteTooltipsFromPath()
    {
        _tooltips ??= new Dictionary<int, string>();

        foreach (var line in _allLines)
        {
            var key = int.Parse(line.Split('[')[1].Split(']')[0]);
            var value = line.Split('<')[1].Split('>')[0];
            
            var isBeginSub = true;
            while (value.Contains('_'))
            {
                var index = value.IndexOf('_');
                value = value.Remove(index, 1)
                    .Insert(index, isBeginSub ? "<size=70%>" : "<size=100%>");
                isBeginSub = !isBeginSub;
            }
            _tooltips.Add(key, value);
        }
    }

    private void BackgroundAnchorPosition()
    {
        _anchoredPosition = _positionTooltip + new Vector2(-20, tailRect.sizeDelta.y - 1f);

        TailAnchorPosition();

        var maxHeightBackground = canvasRect.rect.height - backgroundRect.rect.height - _heightHeader;

        _anchoredPosition.x = Math.Clamp(_anchoredPosition.x, 0, canvasRect.rect.width - backgroundRect.rect.width);
        _anchoredPosition.y = Math.Clamp(_anchoredPosition.y, tailRect.sizeDelta.y - 1f, maxHeightBackground);

        //flip background
        if (_anchoredPosition.y + backgroundRect.rect.height + _heightHeader >= canvasRect.rect.height)
        {
            _anchoredPosition.y = maxHeightBackground - 67f;
        }

        _rect.anchoredPosition = _anchoredPosition;
    }

    private void TailAnchorPosition()
    {
        _anchoredPositionTail = _positionTooltip;

        _anchoredPositionTail.x = Math.Clamp(_anchoredPositionTail.x, tailRect.sizeDelta.x,
            canvasRect.rect.width - tailRect.rect.width);
        _anchoredPositionTail.y = Math.Clamp(_anchoredPositionTail.y, 0,
            canvasRect.rect.height - _heightHeader - backgroundRect.sizeDelta.y - tailRect.rect.height + 1f);

        tailRect.anchoredPosition = _anchoredPositionTail;
    }

    private void FlipTail()
    {
        var scale = tailRect.localScale;
        if (_anchoredPositionTail.x - _anchoredPosition.x > backgroundRect.sizeDelta.x / 2f)
        {
            scale = new Vector3(-1, scale.y, scale.z);
        }
        else
        {
            scale = new Vector3(1, scale.y, scale.z);
        }

        if (_anchoredPositionTail.y > _anchoredPosition.y)
        {
            scale = new Vector3(scale.x, -1, scale.z);
        }
        else
        {
            scale = new Vector3(scale.x, 1, scale.z);
        }

        tailRect.localScale = scale;
    }

    private void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(20, 10);
        backgroundRect.sizeDelta = textSize + paddingSize;
    }
}