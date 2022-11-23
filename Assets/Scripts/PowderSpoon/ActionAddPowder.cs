using System;
using System.Collections.Generic;

namespace Liquid
{
    public class ActionAddPowder<T>
    {
        private struct ConditionActionAddPowder
        {
            public T equalState;
            public TypePowder equalTypePowder;
            public Operator sign;
            public int countPowder;
            public T newState;
            public Action action;
        }

        private List<ConditionActionAddPowder> _conditions;

        public ActionAddPowder()
        {
            _conditions = new List<ConditionActionAddPowder>();
        }

        public void AddAction(T equalState, TypePowder equalTypePowder, Operator sign, int countPowder, T newState,
            Action action = null)
        {
            _conditions.Add(new ConditionActionAddPowder()
            {
                equalState = equalState,
                equalTypePowder = equalTypePowder,
                sign = sign,
                countPowder = countPowder,
                newState = newState,
                action = action
            });
        }

        public void AddAction(T nowState, TypePowder equalTypePowder, Operator sign, int countPowder,
            Action action = null)
        {
            _conditions.Add(new ConditionActionAddPowder()
            {
                equalState = nowState,
                equalTypePowder = equalTypePowder,
                sign = sign,
                countPowder = countPowder,
                newState = nowState,
                action = action
            });
        }

        public void Launch(ref T state, TypePowder typePowder, int countLiquid)
        {
            foreach (var condition in _conditions)
            {
                if (Equals(state, condition.equalState) && typePowder == condition.equalTypePowder)
                {
                    var isTrueCondition = false;

                    switch (condition.sign)
                    {
                        case Operator.Less:
                            isTrueCondition = countLiquid < condition.countPowder;
                            break;
                        case Operator.LessEquals:
                            isTrueCondition = countLiquid <= condition.countPowder;
                            break;
                        case Operator.Equally:
                            isTrueCondition = countLiquid == condition.countPowder;
                            break;
                        case Operator.MoreEquals:
                            isTrueCondition = countLiquid >= condition.countPowder;
                            break;
                        case Operator.More:
                            isTrueCondition = countLiquid > condition.countPowder;
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