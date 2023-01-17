using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipScreenSpaceUI : MonoBehaviour
{
    public RectTransform canvasRect;
    public RectTransform tailRect;

    private RectTransform _rect;
    private RectTransform _backgroundRect;
    private TextMeshProUGUI _textMeshPro;

    private Vector2 _anchoredPosition;
    private Vector2 _anchoredPositionTail;

    private readonly float _heightHeader = 48f;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _backgroundRect = transform.Find("Background").GetComponent<RectTransform>();
        _textMeshPro = transform.Find("Text").GetComponent<TextMeshProUGUI>();

        SetText(_textMeshPro.text);
    }

    private void Update()
    {
        BackgroundAnchorPosition();
        FlipTail();
    }

    private void BackgroundAnchorPosition()
    {
        _anchoredPosition = Input.mousePosition + new Vector3(-20, tailRect.sizeDelta.y - 1f);
        
        TailAnchorPosition();
        
        if (_anchoredPosition.x + _backgroundRect.rect.width > canvasRect.rect.width)
        {
            _anchoredPosition.x = canvasRect.rect.width - _backgroundRect.rect.width;
        }
        
        if (_anchoredPosition.y + _backgroundRect.rect.height + _heightHeader > canvasRect.rect.height) {
            _anchoredPosition.y = canvasRect.rect.height - _backgroundRect.rect.height - _heightHeader;
        }

        _rect.anchoredPosition = _anchoredPosition;
    }
    
    private void TailAnchorPosition()
    {
         _anchoredPositionTail = Input.mousePosition;

        if (_anchoredPositionTail.x + tailRect.rect.width > canvasRect.rect.width)
        {
            _anchoredPositionTail.x = canvasRect.rect.width - tailRect.rect.width;
        }
        
        if (_anchoredPosition.y + _backgroundRect.rect.height + _heightHeader > canvasRect.rect.height) {
            _anchoredPositionTail.y = canvasRect.rect.height - tailRect.rect.height - _heightHeader - _backgroundRect.sizeDelta.y + 1f;
        }

        tailRect.anchoredPosition = _anchoredPositionTail;
    }

    private void FlipTail()
    {
        var rotate = tailRect.rotation;
        if (_anchoredPositionTail.x - _anchoredPosition.x > _backgroundRect.sizeDelta.x / 2f)
        {
            
            rotate.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            rotate.eulerAngles = new Vector3(0, 0, 0);
        }

        tailRect.rotation = rotate;
    }

    private void SetText(string tooltipText)
    {
        _textMeshPro.SetText(tooltipText);
        _textMeshPro.ForceMeshUpdate();

        Vector2 textSize = _textMeshPro.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(18, 8);
        _backgroundRect.sizeDelta = textSize + paddingSize;
    }
}
