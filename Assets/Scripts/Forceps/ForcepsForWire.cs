
    using System;
    using UnityEngine;

    public class ForcepsForWire : MonoBehaviour, IRestart
    {
        public enum StateForcepsForWire
        {
            Empty,
            WithWire,
            WithWireHeat,
            NotActive
        }

        private StateForcepsForWire _stateForceps;
        private Transform _leftHalfForceps;
        private Transform _rightHalfForceps;
        private Quaternion _originRotateLeftHalfForceps;
        private Quaternion _originRotateRightHalfForceps;
        private MoveToPoint _rotateToLeftForceps;
        private MoveToPoint _rotateToRightForceps;
        private StateItem _stateItem;
        private bool _isOpenForceps = true;

        public StateForcepsForWire GetState => _stateForceps;

        public int SetState
        {
            set => _stateForceps = (StateForcepsForWire)value;
        }
        
        private void Awake()
        {
            _leftHalfForceps = transform.GetChild(0);
            _rightHalfForceps = transform.GetChild(1);
            _stateItem = GetComponent<StateItem>();
            _originRotateLeftHalfForceps = _leftHalfForceps.rotation;
            _originRotateRightHalfForceps = _rightHalfForceps.rotation;
            _rotateToLeftForceps = new MoveToPoint(_leftHalfForceps);
            _rotateToRightForceps = new MoveToPoint(_rightHalfForceps);
            _rotateToLeftForceps.SetSpeedTRS = new Vector3(0f, 7f, 0f);
            _rotateToRightForceps.SetSpeedTRS = new Vector3(0f, 7f, 0f);
        }

        public void StartOpenForceps(int newState)
        {
            ActionForceps(true, newState);
        }

        public void StartCloseForceps(int newState)
        {
            ActionForceps(false, newState);
        }

        private void ActionForceps(bool isOpenForceps, int newState)
        {
            if (_isOpenForceps == isOpenForceps) return;

            _isOpenForceps = isOpenForceps;
            _stateForceps = (StateForcepsForWire)newState;

            var delta = isOpenForceps ? -4 : 4;

            var newRotateLeftForceps = Quaternion.Euler(_leftHalfForceps.localRotation.eulerAngles.x,
                _leftHalfForceps.localRotation.eulerAngles.y, _leftHalfForceps.localRotation.eulerAngles.z + delta);
            var newRotateRightForceps = Quaternion.Euler(_rightHalfForceps.localRotation.eulerAngles.x,
                _rightHalfForceps.localRotation.eulerAngles.y, _rightHalfForceps.localRotation.eulerAngles.z - delta);

            _rotateToLeftForceps.SetTargetPosition(_leftHalfForceps.position);
            _rotateToRightForceps.SetTargetPosition(_rightHalfForceps.position);
            _rotateToLeftForceps.SetTargetLocalRotation(newRotateLeftForceps);
            _rotateToRightForceps.SetTargetLocalRotation(newRotateRightForceps);

            StartCoroutine(_rotateToLeftForceps.StartAsync());
            StartCoroutine(_rotateToRightForceps.StartAsync(() =>
            {
                _stateItem.ChangeState(StateItems.BackToMouse);
            }));
        }

        public void Restart()
        {
            _isOpenForceps = true;
            _stateForceps = StateForcepsForWire.Empty;
            _leftHalfForceps.rotation = _originRotateLeftHalfForceps;
            _rightHalfForceps.rotation = _originRotateRightHalfForceps;
        }
    }