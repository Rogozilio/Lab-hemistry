using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GlassCup : MonoBehaviour, IRestart
{
    public enum StateGlassCup
    {
        Empty,
        Marble,
        Marble_HCI,
        Marble_HCI_CO2,
        Smoke,
        NotActive
    }

    private StateGlassCup _stateGlassCup = StateGlassCup.Empty;
    private UIStagesControl _uiStagesControl;

    private int _countLiquidHCI;

    private TimerTooltip _timerTooltip;
    public GameObject Cap;
    public GameObject fewPieceMarble;
    public GameObject powder;
    public GameObject bubble;
    public Renderer bottomSmoke;
    public ParticleSystem tubeSmokeParticle;

    public StateGlassCup stateGlassCup => _stateGlassCup;

    public bool IsPlayParticleTubeSmoke => tubeSmokeParticle.isPlaying;

    public int countLiquidHCI => _countLiquidHCI;

    private void Awake()
    {
        _uiStagesControl = FindObjectOfType<UIStagesControl>();
        _timerTooltip = GetComponent<TimerTooltip>();
        _timerTooltip.AddActionStart = () =>
        {
            CursorSkin.Instance.isUseClock = true;
        };
        _timerTooltip.AddActionEnd = () =>
        {
            bubble.SetActive(false);
            _stateGlassCup = StateGlassCup.Marble_HCI_CO2;
            Cap.GetComponent<ClickMouseItem>().enabled = true;
            _uiStagesControl.NextStep();
        
            CursorSkin.Instance.isUseClock = false;
        };
    }

    private IEnumerator ShowBottomSmoke()
    {
        bottomSmoke.gameObject.SetActive(true);
        var newColor = bottomSmoke.material.GetColor("_TintColor");
        while (newColor.a < 0.04f)
        {
            newColor += new Color(0f, 0f, 0f, 0.00015f);
            bottomSmoke.material.SetColor("_TintColor", newColor);
            yield return new WaitForFixedUpdate();
        }


        if (_stateGlassCup == StateGlassCup.Marble_HCI_CO2)
        {
            _uiStagesControl.NextStep();
            _stateGlassCup = StateGlassCup.Smoke;
        }
    }

    public void StartReleaseCO2()
    {
        if (_stateGlassCup == StateGlassCup.Marble_HCI)
        {
            _uiStagesControl.NextStep();
            Cap.GetComponent<ClickMouseItem>().enabled = false;
            bubble.SetActive(true);
            _timerTooltip.StartTimerTooltip(transform, "Стакан", "заполнение углекислым газом");
        }
    }

    public void ChangeStateGlassCup(int index)
    {
        _stateGlassCup = (StateGlassCup)index;
    }

    public void SetTransformFewPieceMarble(Transform transform)
    {
        fewPieceMarble.SetActive(true);
        fewPieceMarble.transform.position = transform.position;
        fewPieceMarble.transform.rotation = transform.rotation;
        _uiStagesControl.NextStep();
    }

    public void AddLiquidHCI()
    {
        _countLiquidHCI++;

        if (!powder.activeSelf)
            powder.SetActive(true);

        powder.transform.localScale = 
            new Vector3(powder.transform.localScale.x, powder.transform.localScale.y, _countLiquidHCI * 0.1f);
        
        if(countLiquidHCI == 10) _uiStagesControl.NextStep();
    }

    public void StartShowBottomSmoke()
    {
        StartCoroutine(ShowBottomSmoke());
    }

    public void Restart()
    {
        _stateGlassCup = StateGlassCup.Empty;
        _countLiquidHCI = 0;
        Cap.GetComponent<ClickMouseItem>().enabled = true;
        fewPieceMarble.SetActive(false);
        powder.SetActive(false);
        powder.transform.localScale = 
            new Vector3(powder.transform.localScale.x, powder.transform.localScale.y, 0f);
        bubble.SetActive(false);
        bottomSmoke.gameObject.SetActive(false);
        var newColor = bottomSmoke.material.GetColor("_TintColor");
        newColor.a = 0;
        bottomSmoke.material.SetColor("_TintColor", newColor);
        tubeSmokeParticle.Stop();
    }
}