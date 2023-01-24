using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

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

    public Vector3 SetPositionTooltip
    {
        set => _positionTooltip = Camera.main.WorldToScreenPoint(value);
    } 

    public Dictionary<int, string> Tooltips
    {
        get
        {
            if(_tooltips == null || _tooltips.Count == 0)
                WriteTooltipsFromPath();
            return _tooltips;
        }
    }

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _imageBackground = backgroundRect.GetComponent<Image>();
        _imageBackgroundOutline = backgroundRect.GetChild(0).GetComponent<Image>();
        _imageTail = tailRect.GetComponent<Image>();
        _imageTailOutline = tailRect.GetChild(0).GetComponent<Image>();

        SetText(textMeshPro.text);

        WriteTooltipsFromPath();
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
        
        _coroutineShowOrHideTooltip = StartCoroutine(CoroutineShowOrHideTooltip());
        SetText(text);
    }

    public void HideTooltip()
    {
        if (_coroutineShowOrHideTooltip != null)
        {
            StopCoroutine(_coroutineShowOrHideTooltip);
        }
            
        _coroutineShowOrHideTooltip = StartCoroutine(CoroutineShowOrHideTooltip(false));
    }

    private void SwitchActiveTooltip(bool value)
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(value);
        }
        tailRect.gameObject.SetActive(value);
    }

    private IEnumerator CoroutineShowOrHideTooltip(bool isShow = true)
    {
        Color32 color = _imageBackground.color;
        byte alpha = color.a;
        byte edge = (byte)(isShow ? 255 : 0);

        if (isShow)
        {
            SwitchActiveTooltip(true);
            ChangeAlphaTooltip(0);
            yield return new WaitForSeconds(0.5f);
        }

        while (alpha != edge)
        {
            alpha = (alpha < edge) ? (byte)(alpha + 85) : (byte)(alpha - 85);
            alpha = Math.Clamp(alpha, (byte)0, (byte)255);
            ChangeAlphaTooltip(alpha);
            yield return new WaitForFixedUpdate();
        }

        if (!isShow)
        {
            SwitchActiveTooltip(false);
        }
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
        string path = Application.streamingAssetsPath + "/Tooltips.txt";

        var allLines = File.ReadAllLines(path).ToList();

        _tooltips ??= new Dictionary<int, string>();

        for (var i = 0; i < allLines.Count; i++)
        {
            var key = int.Parse(allLines[i].Split('[')[1].Split(']')[0]);
            var value = allLines[i].Split('<')[1].Split('>')[0];
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
             canvasRect.rect.height - _heightHeader - backgroundRect.sizeDelta.y - tailRect.rect.height  + 1f);

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