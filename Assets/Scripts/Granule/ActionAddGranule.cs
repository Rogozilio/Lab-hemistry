using System;
using System.Collections.Generic;
using UnityEngine;

namespace Granule
{
    public class ActionAddGranule<T>
    {
        private partial struct ConditionActionAddGranule
        {
            public T equalState;
            public TypeGranule equalTypeGranule;
            public T newState;
            public Action action;
            public Action<Granule> actionGranule;
        }

        private List<ConditionActionAddGranule> _conditions;

        public ActionAddGranule()
        {
            _conditions = new List<ConditionActionAddGranule>();
        }
        
        public void AddAction(T equalState, TypeGranule equalTypeGranule, T newState, Action action = null)
        {
            _conditions.Add(new ConditionActionAddGranule()
            {
                equalState = equalState,
                equalTypeGranule = equalTypeGranule,
                newState = newState,
                action = action
            });
        }
        
        public void AddAction(T nowState, TypeGranule equalTypeGranule, Action action = null)
        {
            _conditions.Add(new ConditionActionAddGranule()
            {
                equalState = nowState,
                equalTypeGranule = equalTypeGranule,
                newState = nowState,
                action = action
            });
        }
        
        public void AddAction(T equalState, TypeGranule equalTypeGranule, T newState, Action<Granule> action = null)
        {
            _conditions.Add(new ConditionActionAddGranule()
            {
                equalState = equalState,
                equalTypeGranule = equalTypeGranule,
                newState = newState,
                actionGranule = action
            });
        }
        
        public void AddAction(T nowState, TypeGranule equalTypeGranule, Action<Granule> action = null)
        {
            _conditions.Add(new ConditionActionAddGranule()
            {
                equalState = nowState,
                equalTypeGranule = equalTypeGranule,
                newState = nowState,
                actionGranule = action
            });
        }

        public void Launch(ref T state, Granule granule)
        {
            for (var i = 0; i < _conditions.Count; i++)
            {
                if (Equals(state, _conditions[i].equalState) && granule.typeGranule == _conditions[i].equalTypeGranule)
                {
                    state = _conditions[i].newState;
                    _conditions[i].action?.Invoke();
                    _conditions[i].actionGranule?.Invoke(granule);
                    return;
                }
            }
        }
    }
}