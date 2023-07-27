using System;
using UnityEngine;

namespace Granule
{
    public enum TypeGranule
    {
        Zn,
        Sn
    }
    
    public class Granule : MonoBehaviour, IRestart
    {
        public TypeGranule typeGranule;
        public ParticleSystem bubble;
        public ParticleSystem bubbleMesh;
        
        private Vector3 _originPosition;
        private Quaternion _originRotation;
        private Transform _originParent;
        private ConnectTransform _connectTransform;

        private float _bubbleSimulationSpeedOrigin;
        private float _bubbleEmissionCountOrigin;
        private float _bubbleShapeRadiusOrigin;
        private float _bubbleMeshEmissionCountOrigin;

        private void Awake()
        {
            _originPosition = transform.localPosition;
            _originRotation = transform.localRotation;
            _originParent = transform.parent;
            _connectTransform = GetComponent<ConnectTransform>();

            _bubbleSimulationSpeedOrigin = bubble.main.simulationSpeed;
            _bubbleEmissionCountOrigin = bubble.emission.rateOverTime.constant;
            _bubbleShapeRadiusOrigin = bubble.shape.radius;
            _bubbleMeshEmissionCountOrigin = bubbleMesh.emission.rateOverTime.constant;
        }

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }

        public void FixedGranuleIn(Transform target)
        {
            _connectTransform.target = target;
            _connectTransform.enabled = true;
        }

        public void PlayBubble()
        {
            bubble.Play();
            bubbleMesh.Play();
        }

        public void ChangeBubbleSpeed(float speed)
        {
            var main = bubble.main;
            main.simulationSpeed = speed;
        }

        public void ChangeBubbleCountEmission(float count)
        {
            var emission = bubble.emission;
            emission.rateOverTime = count;
        }
        
        public void ChangeBubbleMeshCountEmission(float count)
        {
            var emission = bubbleMesh.emission;
            emission.rateOverTime = count;
        }

        public void ChangeBubbleRadius(float radius)
        {
            var shape = bubble.shape;
            shape.radius = radius;
        }

        public void Restart()
        {
            ChangeBubbleSpeed(_bubbleSimulationSpeedOrigin);
            ChangeBubbleRadius(_bubbleShapeRadiusOrigin);
            ChangeBubbleCountEmission(_bubbleEmissionCountOrigin);
            ChangeBubbleMeshCountEmission(_bubbleMeshEmissionCountOrigin);
            SetParent(_originParent);
            transform.localPosition = _originPosition;
            transform.localRotation = _originRotation;
            if (_connectTransform)
            {
                _connectTransform.target = null;
                _connectTransform.enabled = false;
            }
            bubble.Stop();
            bubbleMesh.Stop();
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}