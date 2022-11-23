using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cursor
{
    public sealed class CursorSkin : MonoBehaviour
    {
        public Texture2D Arrow;
        public Texture2D Horizontal;
        public Texture2D Vertical;
        public Texture2D Click;
        public Texture2D Select;
        public Texture2D Hold;
        public Texture2D[] load;

        private bool _isLoadActive;
        private Coroutine _coroutineUseLoad;

        public static CursorSkin Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
        
        public void UseArrow()
        {
            _isLoadActive = false;
            UnityEngine.Cursor.SetCursor(Instance.Arrow, Vector2.zero, CursorMode.Auto);
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
            if(_coroutineUseLoad != null)
                StopCoroutine(_coroutineUseLoad);
            _coroutineUseLoad = StartCoroutine(AnimateCursorLoad());
        }

        private IEnumerator AnimateCursorLoad()
        {
            while (true)
            {
                for (var i = 0; i < load.Length; i++)
                {
                    if (!_isLoadActive) yield break;
                    
                    UnityEngine.Cursor.SetCursor(Instance.load[i], new Vector2(10, 10), CursorMode.Auto);
                    yield return new WaitForFixedUpdate();
                }
            }
        }
    }
}