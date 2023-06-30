using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wire
{
    public enum TypeWire
    {
        Cu,
        Fe,
        Pb,
        steel,
        Al
    }

    public enum TypePassivate
    {
        No,
        Passivated,
        PassivatedAndWashH2O
    }

    public enum TypeHeatTreatment
    {
        No,
        Fire
    }

    public class Wire : MonoBehaviour, IRestart
    {
        public TypeWire typeWire;
        public TypePassivate typePassivate;
        public TypeHeatTreatment typeHeatTreatment;
        public bool isFormedAmalgam;
        public List<ParticleSystem> effects;

        private StateItem _stateItem;
        private Granule.Granule _withGranule;
        private ConnectTransform _connectTransform;
        private MoveMap _moveMap;
        private MoveMouseItem _moveMouseItem;
        private Vector3 _originPosition;
        private Quaternion _originRotation;
        private Quaternion _originMoveMouseRotation;
        private Material _partWire;
        private WirePartEffects[] _wirePartEffects;

        public Granule.Granule GetGranule => _withGranule;
        public TypeWire GetTypeWire => typeWire;
        public TypePassivate GetTypePassivate => typePassivate;
        public TypeHeatTreatment GetTypeHeatTreatment => typeHeatTreatment;

        public TypeWire SetTypeWire
        {
            set => typeWire = value;
        }

        public int SetTypeHeatTreatment
        {
            set => typeHeatTreatment = (TypeHeatTreatment)value;
        }

        public bool IsFormedAmalgam
        {
            set => isFormedAmalgam = value;
            get => isFormedAmalgam;
        }

        public MoveMouseItem MoveMouseItem => _moveMouseItem;
        public MoveMap MoveMap => _moveMap;
        public StateItems GetStateItem => _stateItem.State;

        private void Awake()
        {
            _stateItem = GetComponent<StateItem>();
            _withGranule = GetComponentInChildren<Granule.Granule>();
            _connectTransform = GetComponent<ConnectTransform>();
            _moveMouseItem = GetComponent<MoveMouseItem>();
            _moveMap = GetComponent<MoveMap>();
            if (GetComponent<Renderer>().materials.Length > 1)
            {
                _wirePartEffects = GetComponents<WirePartEffects>();
                _partWire = GetComponent<Renderer>().materials[1];
            }

            _originPosition = transform.position;
            _originRotation = transform.rotation;
            _originMoveMouseRotation = _moveMouseItem.StartRotation;
        }

        public void FixedWireIn(Transform transform)
        {
            _moveMouseItem.enabled = false;
            _connectTransform.target = transform;
            _connectTransform.enabled = true;
        }

        public void UnfixedWire()
        {
            _moveMouseItem.enabled = true;
            _connectTransform.enabled = false;
        }

        public void ResetSpawnPointIfConnect()
        {
            if (_connectTransform.enabled)
                _moveMouseItem.ResetPointRespawn();
        }

        public bool SetMoveMouseItemEnable
        {
            set => _moveMouseItem.enabled = value;
        }

        public void PlayExistEffects()
        {
            foreach (var effect in effects)
            {
                effect.Play();
            }
        }

        public void SwitchOnLinearState()
        {
            LinearValue value = new LinearValue()
            {
                axis = Axis.Y,
                axisInput = AxisInput.Y,
                edge = new Vector2(0, 0.125f)
            };
            _moveMouseItem.SetState(StateItems.LinearMove, value);
            _moveMouseItem.StartRotation = Quaternion.identity;
        }

        public void StartWirePartEffect(string indexs)
        {
            StartWirePartEffect(0, indexs);
        }

        public void StartWirePartEffect(int materialIndex, string indexs)
        {
            if (indexs.Contains('-'))
            {
                var startIndex = int.Parse(indexs.Split('-')[0]);
                var endIndex = int.Parse(indexs.Split('-')[1]);

                _wirePartEffects[materialIndex].StartQueue(startIndex, endIndex);
            }
            else if (indexs.Contains(' '))
            {
                var stringIndexs = indexs.Split(' ');
                var arrayIndexs = new int[stringIndexs.Length];
                for (var i = 0; i < stringIndexs.Length; i++)
                {
                    arrayIndexs[i] = int.Parse(stringIndexs[i]);
                }
                _wirePartEffects[materialIndex].StartQueue(arrayIndexs);
            }
            else 
                _wirePartEffects[materialIndex].Launch(int.Parse(indexs));
        }

        public void Restart()
        {
            typePassivate = TypePassivate.No;
            typeHeatTreatment = TypeHeatTreatment.No;
            isFormedAmalgam = false;
            transform.position = _originPosition;
            transform.rotation = _originRotation;
            _moveMouseItem.StartRotation = _originMoveMouseRotation;
            _moveMouseItem.SetState(StateItems.Drag);
            UnfixedWire();
            foreach (var effect in effects)
            {
                effect.Stop();
            }
        }
    }
}