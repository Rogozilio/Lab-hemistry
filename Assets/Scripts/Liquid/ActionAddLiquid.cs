using System;
using System.Collections.Generic;

namespace Liquid
{
    public class ActionAddLiquid<T>
    {
        private struct ConditionActionAddLiquid
        {
            public T equalState;
            public TypeLiquid equalTypeLiquid;
            public Operator sign;
            public int countLiquid;
            public T newState;
            public Action action;
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
        
        public void Launch(ref T state, TypeLiquid typeLiquid, int countLiquid)
        {
            foreach (var condition in _conditions)
            {
                if (Equals(state, condition.equalState) && typeLiquid == condition.equalTypeLiquid )
                {
                    var isTrueCondition = false;
                    
                    switch (condition.sign)
                    {
                        case Operator.Less:
                            isTrueCondition = countLiquid < condition.countLiquid;
                            break;
                        case Operator.LessEquals:
                            isTrueCondition = countLiquid <= condition.countLiquid;
                            break;
                        case Operator.Equally:
                            isTrueCondition = countLiquid == condition.countLiquid;
                            break;
                        case Operator.MoreEquals:
                            isTrueCondition = countLiquid >= condition.countLiquid;
                            break;
                        case Operator.More:
                            isTrueCondition = countLiquid > condition.countLiquid;
                            break;
                    }

                    if (!isTrueCondition) continue;

                    state = condition.newState;
                    condition.action?.Invoke();
                    return;
                }
            }
        }
    }
}