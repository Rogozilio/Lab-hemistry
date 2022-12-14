using System;
using Liquid;
using UnityEngine;

namespace Mini_test_tube
{
    public class MiniTestTube : MonoBehaviour
    {
        public GameObject Liquid;
        public GameObject LiquidFlow;
        public Transform StartLiquidFlow;

        protected LevelLiquid _levelLiquid;
        protected int _countLiquid;
        protected int _countPowder;
        protected float _step;

        private Renderer _rendererLiquid;
        
        private LiquidFlow _liquidFlowScript;
        private StateItem _stateItem;
        private bool _isReadyFlowLiquid;
        

        public Renderer rendererLiquid => _rendererLiquid;
        public LevelLiquid levelLiquid => _levelLiquid;
        public StateItem stateItem => _stateItem;

        public LiquidFlow liquidFlowScript => _liquidFlowScript;

        public bool isReadyFlowLiquid
        {
            get => _isReadyFlowLiquid;
            set => _isReadyFlowLiquid = value;
        }

        public void Awake()
        {
            _stateItem = GetComponent<StateItem>();
            _rendererLiquid = Liquid.GetComponent<Renderer>();
            _levelLiquid = Liquid.GetComponent<LevelLiquid>();
            _liquidFlowScript = LiquidFlow.GetComponent<LiquidFlow>();
            _liquidFlowScript.actionInEnd += () =>
            {
                _isReadyFlowLiquid = false;
                var oldLinearValue = GetComponent<LinearRotate>().linearValue;
                var newLinearValue = new LinearValue()
                {
                    axis = oldLinearValue.axis,
                    axisInput = oldLinearValue.axisInput,
                    edge = new Vector2(-oldLinearValue.edge.y, -oldLinearValue.edge.x)
                };
                if(stateItem.State == StateItems.Interacts)
                    stateItem.ChangeState(StateItems.LinearRotate, newLinearValue);
            };

            _step = 1f / 60f;
        }

        public virtual void SetStateMiniTestTube(int index)
        {
            
        }

        public virtual void AddLiquid(LiquidDrop liquid)
        {
            _countLiquid++;

            if (!_levelLiquid.gameObject.activeSelf)
                _levelLiquid.gameObject.SetActive(true);
            _levelLiquid.level += _step;
        }

        public virtual void AddPowder(PowderDrop powder)
        {
            _countPowder++;
        }

        protected virtual void ChangeColorLiquid(Color newColor, byte step = 1)
        {
            ChangeColorLiquid(_rendererLiquid, newColor, step);
        }
        
        protected virtual void ChangeColorLiquid(Renderer renderLiquid, Color newColor, byte step = 1)
        {
            var liquidColor = renderLiquid.material.GetColor("_LiquidColor");
            var stepColor = new Color();

            if (step > 0)
            {
                stepColor = (newColor-liquidColor) / step;
            }

            renderLiquid.material.SetColor("_LiquidColor", liquidColor + stepColor);
        }

        protected virtual void PourOutLiquid(float step, float howMach, LevelLiquid sediment = null)
        {
            if (!_isReadyFlowLiquid) return;

            if (transform.rotation.eulerAngles.x >= 352f)
            {
                if (!LiquidFlow.activeSelf)
                {
                    stateItem.ChangeState(StateItems.Interacts);
                    var sign = (transform.up.y > -transform.up.y) ? 0 : -2;
                    
                    LiquidFlow.SetActive(true);
                    _liquidFlowScript.SetColorFlow = rendererLiquid.material.GetColor("_LiquidColor");
                    _liquidFlowScript.SetColor = rendererLiquid.material.GetColor("_LiquidColor");
                    if (sediment)
                    {
                        _liquidFlowScript.SetColorSediment 
                            = sediment.GetComponent<Renderer>().material.GetColor("_LiquidColor");
                        _liquidFlowScript.stepSediment = step * (sediment.level / levelLiquid.level);
                    }
                        
                    StartLiquidFlow.localPosition += new Vector3(0, StartLiquidFlow.localPosition.y * sign, 0);
                    _liquidFlowScript.SetPositionStart(StartLiquidFlow.position);
                    StartLiquidFlow.localPosition += new Vector3(0, StartLiquidFlow.localPosition.y * sign, 0);
                    _liquidFlowScript.step = step;
                    _liquidFlowScript.howMach = howMach;
                }

                if (_liquidFlowScript.stateFlowLiquid == StateFlowLiquid.Pour)
                {
                    _liquidFlowScript.PourOutLiquid(levelLiquid, sediment);
                }
            }
            else
            {
                _liquidFlowScript.ChangeStateFlowLiquid(3);
            }
        }
        
        protected bool _isStir;
        private Vector3 _prevPosition;

        public virtual void Stir(Transform stick)
        {
            _isStir = stick.position != _prevPosition;
            _prevPosition = stick.position;
        }
    }
}