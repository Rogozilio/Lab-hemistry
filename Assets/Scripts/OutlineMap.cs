using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public class OutlineMap
{
    [Serializable]
    public struct Condition
    {
        public GameObject target;
        public Component component;
        public string nameProperty;
        public int indexComponent;
        public int indexProperty;
        public Operator sign;
        public string requiredValue;

        public bool GetResult()
        {
            var properties = component.GetType().GetProperty(nameProperty);
            var valueProperty = properties.GetValue(component);

            var res = 0;
            if (valueProperty is bool)
                res = (bool)valueProperty ? 1 : 0;
            if (valueProperty is Enum)
                res = (int)valueProperty;
            if (valueProperty is int)
                res = (int)valueProperty;

            if (requiredValue.Contains(" "))
            {
                var values = requiredValue.Split(" ");

                foreach (var value in values)
                {
                    if (ResultCondition(res, int.Parse(value), sign)) return true;
                }

                return false;
            }

            return ResultCondition(res, int.Parse(requiredValue), sign);
        }

        private bool ResultCondition(int value1, int value2, Operator sign)
        {
            switch (sign)
            {
                case Operator.Less: return value1 < value2;
                case Operator.LessEquals: return value1 <= value2;
                case Operator.Equally: return value1 == value2;
                case Operator.MoreEquals: return value1 >= value2;
                case Operator.More: return value1 > value2;
                case Operator.NotEqually: return value1 != value2;
            }

            return false;
        }
    }

    [Serializable]
    public struct OutlineMapItem
    {
        public Outline value;
        public List<Condition> conditions;

        public bool GetResult()
        {
            foreach (var condition in conditions)
            {
                if (!condition.GetResult()) return false;
            }

            return true;
        }
    }

    public Vector3 offsetPoint;
    public List<OutlineMapItem> outlineMapItem;

    public void Show(Vector3 position)
    {
        foreach (var item in outlineMapItem)
        {
            if (item.GetResult()) ShowOutlineItem(item.value, position + offsetPoint);
        }
    }

    public void Clear()
    {
        foreach (var item in outlineMapItem)
        {
            var outline = item.value;
            outline.OutlineWidth = 3f;
            outline.enabled = false;
        }
    }

    private void ShowOutlineItem(Outline outline, Vector3 position)
    {
        if (!outline.enabled) outline.enabled = true;
        var distance = Vector3.Distance(position, outline.transform.position);
        outline.OutlineWidth = Math.Clamp(1 / distance, 0, 12f);
    }
}