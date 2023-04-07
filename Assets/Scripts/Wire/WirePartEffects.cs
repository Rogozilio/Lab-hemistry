using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Wire
{
    public class WirePartEffects : MonoBehaviour, IRestart
    {
        [Serializable]
        public struct Item
        {
            public float duration;
            public Color32 color;
            public UnityEvent eventStart;
            public UnityEvent eventEnd;
        }

        public int materialIndex = 1;
        public List<Item> Items;
        private Color32 _originColor;

        private Color32 _beginColor;
        private Material _wirePart;

        private Color32 _getColor => _wirePart.GetColor("_LiquidColor");
        private Color32 _setColor
        {
            set => _wirePart.SetColor("_LiquidColor", value);
        }

        private void Awake()
        {
            if (GetComponent<Renderer>().materials.Length > materialIndex)
            {
                _wirePart = GetComponent<Renderer>().materials[materialIndex];
                _originColor = _getColor;
            }
            else
                Debug.LogError("Material Wire Part not found");
        }

        public void Launch(int indexColor)
        {
            _beginColor = _getColor;
            StartCoroutine(SmoothlyChangeColor(Items[indexColor]));
        }

        public void StartQueue(int startIndex, int endIndex)
        {
            var delay = 0f;
            for (var start = startIndex;start <= endIndex; start++)
            {
                StartCoroutine(SmoothlyChangeColor(Items[start], delay));
                delay += Items[start].duration;
            }
        }
        
        public void StartQueue(params int[] indexs)
        {
            var delay = 0f;
            foreach (var index in indexs)
            {
                StartCoroutine(SmoothlyChangeColor(Items[index], delay));
                delay += Items[index].duration;
            }
        }

        private IEnumerator SmoothlyChangeColor(Item item, float delay = 0f)
        {
            item.eventStart?.Invoke();
            
            yield return new WaitForSeconds(delay);
            _beginColor = _getColor;
            var time = item.duration;
            while (time > 0)
            {
                var delta = 1 - (time / item.duration);
                _setColor = Color32.Lerp(_beginColor, item.color, delta);
                yield return new WaitForFixedUpdate();
                time -= Time.fixedDeltaTime;
            }
            
            item.eventEnd?.Invoke();
        }

        public void Restart()
        {
            _setColor = _originColor;
        }
    }
}