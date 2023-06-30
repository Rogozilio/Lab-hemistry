using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


public sealed class CursorSkin : MonoBehaviour
    {
        public Texture2D arrow;
        public Texture2D notInteractive;
        public Texture2D horizontal;
        public Texture2D vertical;
        public Texture2D click;
        public Texture2D select;
        public Texture2D hold;
        public Texture2D[] load;
        public Texture2D[] clock;
        [Space] 
        public UnityEvent onArrow;
        public UnityEvent onNotInteractive;
        public UnityEvent onHorizontal;
        public UnityEvent onVertical;
        public UnityEvent onClick;
        public UnityEvent onSelect;
        public UnityEvent onHold;
        public UnityEvent onLoad;
        public UnityEvent onClock;

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
                onClock?.Invoke();
            }
            else
            {
                UnityEngine.Cursor.SetCursor(Instance.arrow, Vector2.zero, CursorMode.Auto);
                onArrow?.Invoke();
            }
        }
        
        public void UseNotInteractive()
        {
            _isLoadActive = false;
            UnityEngine.Cursor.SetCursor(Instance.notInteractive, new Vector2(10, 10), CursorMode.Auto);
            onNotInteractive?.Invoke();
        }

        public void UseHorizontal()
        {
            _isLoadActive = false;
            UnityEngine.Cursor.SetCursor(Instance.horizontal, new Vector2(10, 0), CursorMode.Auto);
            onHorizontal?.Invoke();
        }

        public void UseVertical()
        {
            _isLoadActive = false;
            UnityEngine.Cursor.SetCursor(Instance.vertical, new Vector2(0, 10), CursorMode.Auto);
            onVertical?.Invoke();
        }

        public void UseClick()
        {
            _isLoadActive = false;
            UnityEngine.Cursor.SetCursor(Instance.click, new Vector2(10, 0), CursorMode.Auto);
            onClick?.Invoke();
        }

        public void UseSelect()
        {
            _isLoadActive = false;
            UnityEngine.Cursor.SetCursor(Instance.select, new Vector2(10, 10), CursorMode.Auto);
            onSelect?.Invoke();
        }

        public void UseHold()
        {
            _isLoadActive = false;
            UnityEngine.Cursor.SetCursor(Instance.hold, new Vector2(10, 10), CursorMode.Auto);
            onHold?.Invoke();
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
            onLoad?.Invoke();
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