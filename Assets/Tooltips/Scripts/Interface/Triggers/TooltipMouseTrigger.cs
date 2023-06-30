using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ERA;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;


namespace ERA
{
    /// <summary> 
    ///     Контроллер для выноски. <br/>
    ///     Показывает выноску при наведении курсора на объект в мире. <br/> 
    ///     Компонент должен находиться на одном объекте с коллайдером. Показывает выноску только если скрипт активен (enabled = true).
    /// </summary> 
    public class TooltipMouseTrigger : MonoBehaviour
    {
        [Tooltip("Объект выноски, которой управляет этот компонент")]
        public Tooltip tooltip;

        public Dictionary<int, string> Tooltips => tooltip.DataFromFile;
        public int index;
        public Vector3 offsetLocalPosition;

        [Tooltip("Задержка перед появлением выноски")]
        public float appearDelay = 0.35f;

        private StateItem _state;

        public void OnEnable()
        {
            tooltip = FindObjectOfType<Tooltip>();
            tooltip.UpdateReadFileTooltips();
            TryGetComponent(out _state);
        }

        void OnDisable()
        {
            StopAllCoroutines();
            tooltip?.Hide();
        }


        //  Events  -----------------------------------------------------
        void OnMouseEnter()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (Input.GetMouseButton(0)) return;

            if (_state != null && _state.State != StateItems.Idle) return;
            if (enabled) ShowAfter(appearDelay);
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButton(0) || _state != null && _state.State != StateItems.Idle)
            {
                tooltip.HideInstantly();
            }
        }

        void OnMouseExit()
        {
            Hide();
        }


        //  Actions  ----------------------------------------------------
        void ShowAfter(float appearDelay)
        {
            StartCoroutine(ShowAfterCoroutine(appearDelay));
        }

        void Show()
        {
            StopAllCoroutines();
            tooltip.Show();
        }

        void Hide()
        {
            StopAllCoroutines();
            tooltip.Hide();
        }


        //  Timer  ------------------------------------------------------
        IEnumerator ShowAfterCoroutine(float appearDelay)
        {
            yield return new WaitForSeconds(appearDelay);
            tooltip.setTarget = transform;
            tooltip.dataList[0].stringData = tooltip.DataFromFile[index];
            tooltip.OffsetLocalSpace = offsetLocalPosition;
            Show();
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(TooltipMouseTrigger))]
public class TooltipMouseTriggerEditor : Editor
{
    private SerializedProperty tooltip;
    private SerializedProperty index;
    private SerializedProperty offsetLocalPosition;
    private SerializedProperty appearDelay;

    private void OnEnable()
    {
        ((TooltipMouseTrigger)target).OnEnable();
        tooltip = serializedObject.FindProperty("tooltip");
        index = serializedObject.FindProperty("index");
        offsetLocalPosition = serializedObject.FindProperty("offsetLocalPosition");
        appearDelay = serializedObject.FindProperty("appearDelay");
    }

    public override void OnInspectorGUI()
    {
        var tooltipMouseTrigger = (TooltipMouseTrigger)target;

        EditorGUILayout.PropertyField(tooltip);
        index.intValue =
            EditorGUILayout.Popup("Tooltip", index.intValue, tooltipMouseTrigger.Tooltips.Values.ToArray());
        EditorGUILayout.PropertyField(offsetLocalPosition);
        EditorGUILayout.PropertyField(appearDelay);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif