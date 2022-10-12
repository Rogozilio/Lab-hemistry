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
    }
}