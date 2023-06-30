using System.Collections;
using ERA;
using UnityEngine;

public class P_tube : MonoBehaviour, IRestart
{
    public enum StatePTube
    {
        balance,
        frozen,
        warmed
    }

    public float step = 0.01f;

    private Renderer _gradientTube;
    private StatePTube _state;

    private UIStagesControl _uiStagesControl;

    private MoveMouseItem _moveMouseItem;

    private Coroutine _coroutineWithIceWater;
    private Coroutine _coroutineBalance;
    private Coroutine _coroutineWithHotWater;

    private float _originEdgeLeftCenter;
    private float _originEdgeCenterRight;
    private float _originSmoothLeftCenter;
    private float _originSmoothCenterRight;
    private Color _originColorRight;

    private float _edgeLeftCenter
    {
        get => _gradientTube.material.GetFloat("_EdgeLeft_Center");
        set => _gradientTube.material.SetFloat("_EdgeLeft_Center", Mathf.Clamp(value, 0f, 1f));
    }

    private float _edgeCenterRight
    {
        get => _gradientTube.material.GetFloat("_EdgeCenter_Right");
        set => _gradientTube.material.SetFloat("_EdgeCenter_Right", Mathf.Clamp(value, 0f, 1f));
    }

    private float _smoothLeftCenter
    {
        get => _gradientTube.material.GetFloat("_SmoothLeft_Center");
        set => _gradientTube.material.SetFloat("_SmoothLeft_Center", Mathf.Clamp(value, 0f, 1f));
    }

    private float _smoothCenterRight
    {
        get => _gradientTube.material.GetFloat("_SmoothCenter_Right");
        set => _gradientTube.material.SetFloat("_SmoothCenter_Right", Mathf.Clamp(value, 0f, 1f));
    }

    private Color _colorRight
    {
        get => _gradientTube.material.GetColor("_ColorRight");
        set => _gradientTube.material.SetColor("_ColorRight", value);
    }

    public StatePTube GetState => _state;

    void Awake()
    {
        _gradientTube = GetComponent<Renderer>();
        _uiStagesControl = FindObjectOfType<UIStagesControl>();
        _moveMouseItem = transform.parent.GetComponent<MoveMouseItem>();

        _originEdgeLeftCenter = _edgeLeftCenter;
        _originEdgeCenterRight = _edgeCenterRight;
        _originSmoothLeftCenter = _smoothLeftCenter;
        _originSmoothCenterRight = _smoothCenterRight;
        _originColorRight = _colorRight;
    }

    public void StartReactionInIceWater()
    {
        if (_coroutineBalance != null) StopCoroutine(_coroutineBalance);
        if (_coroutineWithHotWater != null) StopCoroutine(_coroutineWithHotWater);
        _coroutineWithIceWater = StartCoroutine(ReactionInIceWater());
    }

    public void StartReactionInHotWater()
    {
        if (_coroutineBalance != null) StopCoroutine(_coroutineBalance);
        if (_coroutineWithIceWater != null) StopCoroutine(_coroutineWithIceWater);
        _coroutineWithHotWater = StartCoroutine(ReactionInHotWater());
    }

    private IEnumerator ReactionInIceWater()
    {
        _uiStagesControl.NextStep();

        _state = StatePTube.frozen;

        CursorSkin.Instance.isUseClock = true;
        _moveMouseItem.enabled = false;

        while (_edgeLeftCenter < 0.35f)
        {
            _smoothCenterRight = Mathf.Clamp(_smoothCenterRight + step, 0f, 0.1f);

            if (_smoothCenterRight >= 0.1f)
            {
                _edgeCenterRight = Mathf.Clamp(_edgeCenterRight + step, 0f, 1f);
            }

            if (_edgeCenterRight >= 0.35f)
            {
                _smoothLeftCenter = Mathf.Clamp(_smoothLeftCenter + step, 0f, 0.1f);
            }

            if (_smoothLeftCenter >= 0.1f)
            {
                _edgeLeftCenter = Mathf.Clamp(_edgeLeftCenter + step, 0f, 0.35f);
            }

            yield return new WaitForFixedUpdate();
        }

        CursorSkin.Instance.isUseClock = false;
        _moveMouseItem.enabled = true;
        _uiStagesControl.NextStep();
    }

    private IEnumerator ReactionInHotWater()
    {
        _uiStagesControl.NextStep();

        _state = StatePTube.warmed;

        CursorSkin.Instance.isUseClock = true;
        _moveMouseItem.enabled = false;

        _colorRight = new Color32(123, 62, 15, 83);

        while (_edgeLeftCenter > 0)
        {
            _edgeCenterRight = Mathf.Clamp(_edgeCenterRight - step, 0.55f, 1f);

            if (_edgeCenterRight <= 0.55f)
            {
                _edgeLeftCenter = Mathf.Clamp(_edgeLeftCenter - step, 0f, 1f);
            }

            yield return new WaitForFixedUpdate();
        }

        CursorSkin.Instance.isUseClock = false;
        _uiStagesControl.NextStep();
    }

    public void Restart()
    {
        _state = StatePTube.balance;
        _edgeLeftCenter = _originEdgeLeftCenter;
        _edgeCenterRight = _originEdgeCenterRight;
        _smoothLeftCenter = _originSmoothLeftCenter;
        _smoothCenterRight = _originSmoothCenterRight;
        _colorRight = _originColorRight;
        _moveMouseItem.enabled = true;
    }
}