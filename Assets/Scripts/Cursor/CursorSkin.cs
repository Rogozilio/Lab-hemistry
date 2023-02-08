using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cursor
{
    public sealed class CursorSkin : MonoBehaviour
    {
        public Texture2D Arrow;
        public Texture2D NotInteractive;
        public Texture2D Horizontal;
        public Texture2D Vertical;
        public Texture2D Click;
        public Texture2D Select;
        public Texture2D Hold;
        public Texture2D[] load;
        public Texture2D[] clock;

        private bool _isUseClock;
        private bool _isLoadActive;
        private Coroutine _coroutineUseLoad;
        private CanvasGroup _mainCanvasGroup;
        
        public bool isUseClock
        {
            set
            {
                _isUseClock = value;
                UseArrow();
            }
        }

        public static CursorSkin Instance { get; private set; }

        private void Awake()
        {
            _mainCanvasGroup = FindObjectOfType<Canvas>().GetComponent<CanvasGroup>();
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                UseArrow();
            }
        }
        
        public void UseArrow()
        {
            _isLoadActive = _isUseClock;
            if (_isUseClock)
            {
                if (_coroutineUseLoad != null) StopCoroutine(_coroutineUseLoad);
                _coroutineUseLoad = StartCoroutine(AnimateCursor(clock, 0.05f));
            }
            else
                UnityEngine.Cursor.SetCursor(Instance.Arrow, Vector2.zero, CursorMode.Auto);
        }
        
        public void UseNotInteractive()
        {
            _isLoadActive = false;
            UnityEngine.Cursor.SetCursor(Instance.NotInteractive, new Vector2(10, 10), CursorMode.Auto);
        }

        public void UseHorizontal()
        {
            _isLoadActive = false;
            UnityEngine.Cursor.SetCursor(Instance.Horizontal, new Vector2(10, 0), CursorMode.Auto);
        }

        public void UseVertical()
        {
            _isLoadActive = false;
            UnityEngine.Cursor.SetCursor(Instance.Vertical, new Vector2(0, 10), CursorMode.Auto);
        }

        public void UseClick()
        {
            _isLoadActive = false;
            UnityEngine.Cursor.SetCursor(Instance.Click, new Vector2(10, 0), CursorMode.Auto);
        }

        public void UseSelect()
        {
            _isLoadActive = false;
            UnityEngine.Cursor.SetCursor(Instance.Select, new Vector2(10, 10), CursorMode.Auto);
        }

        public void UseHold()
        {
            _isLoadActive = false;
            UnityEngine.Cursor.SetCursor(Instance.Hold, new Vector2(10, 10), CursorMode.Auto);
        }

        public void UseLoad()
        {
            _isLoadActive = true;
            if (_coroutineUseLoad != null)
            {
                _mainCanvasGroup.interactable = true;
                StopCoroutine(_coroutineUseLoad);
            }
            _coroutineUseLoad = StartCoroutine(AnimateCursor(load));
        }

        private IEnumerator AnimateCursor(Texture2D[] data, float deltaTime = 0.1f)
        {
            _mainCanvasGroup.interactable = false;
            while (true)
            {
                for (var i = 0; i < data.Length; i++)
                {
                    if (!_isLoadActive)
                    {
                        _mainCanvasGroup.interactable = true;
                        yield break;
                    }
                    
                    UnityEngine.Cursor.SetCursor(data[i], new Vector2(10, 10), CursorMode.Auto);
                    yield return new WaitForSeconds(deltaTime);
                }
            }
        }
    }
}