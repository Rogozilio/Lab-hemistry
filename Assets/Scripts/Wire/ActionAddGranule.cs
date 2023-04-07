using System;
using System.Collections.Generic;
using UnityEngine;
using Wire;

namespace Granule
{
    public class ActionAddWire<T>
    {
        private partial struct ConditionActionAddWire
        {
            public T equalState;
            public TypeWire equalTypeWire;
            public T newState;
            public Action action;
            public Action<Wire.Wire> actionGranule;
        }

        private List<ConditionActionAddWire> _conditions;

        public ActionAddWire()
        {
            _conditions = new List<ConditionActionAddWire>();
        }
        
        public void AddAction(T equalState, TypeWire equalTypeWire, T newState, Action action = null)
        {
            _conditions.Add(new ConditionActionAddWire()
            {
                equalState = equalState,
                equalTypeWire = equalTypeWire,
                newState = newState,
                action = action
            });
        }
        
        public void AddAction(T nowState, TypeWire equalTypeWire, Action action = null)
        {
            _conditions.Add(new ConditionActionAddWire()
            {
                equalState = nowState,
                equalTypeWire = equalTypeWire,
                newState = nowState,
                action = action
            });
        }
        
        public void AddAction(T equalState, TypeWire equalTypeWire, T newState, Action<Wire.Wire> action = null)
        {
            _conditions.Add(new ConditionActionAddWire()
            {
                equalState = equalState,
                equalTypeWire = equalTypeWire,
                newState = newState,
                actionGranule = action
            });
        }
        
        public void AddAction(T nowState, TypeWire equalTypeWire, Action<Wire.Wire> action = null)
        {
            _conditions.Add(new ConditionActionAddWire()
            {
                equalState = nowState,
                equalTypeWire = equalTypeWire,
                newState = nowState,
                actionGranule = action
            });
        }

        public void Launch(ref T state, Wire.Wire wire)
        {
            for (var i = 0; i < _conditions.Count; i++)
            {
                if (Equals(state, _conditions[i].equalState) && wire.typeWire == _conditions[i].equalTypeWire)
                {
                    state = _conditions[i].newState;
                    _conditions[i].action?.Invoke();
                    _conditions[i].actionGranule?.Invoke(wire);
                    return;
                }
            }
        }
    }
}