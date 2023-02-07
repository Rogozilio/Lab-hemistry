using UnityEngine;
using UnityEngine.Events;


namespace VirtualLab
{
    public class RestartDialog : MonoBehaviour
    {
        public delegate void Callback();

        Callback callback;

        //  Events  ----------------------------------------------------- 
        public UnityEvent onShow;
        public UnityEvent onHide;

        public void OnRestart()
        {
            if (callback != null) callback();
            Hide();
        }

        public void OnContinue()
        {
            Hide();
        }

        //  Visibility  ------------------------------------------------- 
        public void Show(Callback onRestart)
        {
            callback = onRestart;
            gameObject.SetActive(true);
            onShow.Invoke();
        }

        void Hide()
        {
            gameObject.SetActive(false);
            onHide.Invoke();
        }
    }
}