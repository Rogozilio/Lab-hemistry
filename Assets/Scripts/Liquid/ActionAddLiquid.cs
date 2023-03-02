using System;
using System.Collections.Generic;
using UnityEngine;

namespace Liquid
{
    public class ActionAddLiquid<T>
    {
        private partial struct ConditionActionAddLiquid
        {
            public T equalState;
            public TypeLiquid equalTypeLiquid;
            public Operator sign;
            public int countLiquid;
            public Color32 colorDropLiquid;
            public T newState;
            public Action action;
            public Action<Color32> actionWithColor;
        }

        private List<ConditionActionAddLiquid> _conditions;

        public ActionAddLiquid()
        {
            _conditions = new List<ConditionActionAddLiquid>();
        }
        
        public void AddAction(T equalState, TypeLiquid equalTypeLiquid, Operator sign, int countLiquid, T newState, Action action = null)
        {
            _conditions.Add(new ConditionActionAddLiquid()
            {
                equalState = equalState,
                equalTypeLiquid = equalTypeLiquid,
                sign = sign,
                countLiquid = countLiquid,
                newState = newState,
                action = action
            });
        }
        
        public void AddAction(T nowState, TypeLiquid equalTypeLiquid, Operator sign, int countLiquid, Action action = null)
        {
            _conditions.Add(new ConditionActionAddLiquid()
            {
                equalState = nowState,
                equalTypeLiquid = equalTypeLiquid,
                sign = sign,
                countLiquid = countLiquid,
                newState = nowState,
                action = action
            });
        }
        
        public void AddAction(T equalState, TypeLiquid equalTypeLiquid, Operator sign, int countLiquid, T newState, Action<Color32> action = null)
        {
            _conditions.Add(new ConditionActionAddLiquid()
            {
                equalState = equalState,
                equalTypeLiquid = equalTypeLiquid,
                sign = sign,
                countLiquid = countLiquid,
                newState = newState,
                actionWithColor = action
            });
        }
        
        public void AddAction(T nowState, TypeLiquid equalTypeLiquid, Operator sign, int countLiquid, Action<Color32> action = null)
        {
            _conditions.Add(new ConditionActionAddLiquid()
            {
                equalState = nowState,
                equalTypeLiquid = equalTypeLiquid,
                sign = sign,
                countLiquid = countLiquid,
                newState = nowState,
                actionWithColor = action
            });
        }

        public void Launch(ref T state, TypeLiquid typeLiquid, int countLiquid, Color32 dropColor = default)
        {
            for (int i = 0; i < _conditions.Count; i++)
            {
                if (Equals(state, _conditions[i].equalState) && typeLiquid == _conditions[i].equalTypeLiquid)
                {
                    var isTrueCondition = false;

                    switch (_conditions[i].sign)
                    {
                        case Operator.Less:
                            isTrueCondition = countLiquid < _conditions[i].countLiquid;
                            break;
                        case Operator.LessEquals:
                            isTrueCondition = countLiquid <= _conditions[i].countLiquid;
                            break;
                        case Operator.Equally:
                            isTrueCondition = countLiquid == _conditions[i].countLiquid;
                            break;
                        case Operator.MoreEquals:
                            isTrueCondition = countLiquid >= _conditions[i].countLiquid;
                            break;
                        case Operator.More:
                            isTrueCondition = countLiquid > _conditions[i].countLiquid;
                            break;
                    }

                    if (!isTrueCondition) continue;
                    state = _conditions[i].newState;
                    _conditions[i].action?.Invoke();
                    _conditions[i].actionWithColor?.Invoke(dropColor);
                    return;
                }
            }
        }
    }
}