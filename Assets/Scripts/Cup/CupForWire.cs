using ERA;
using UnityEngine;
using Wire;

public class CupForWire : MonoBehaviour, IRestart
{
    public enum StateCupForWire
    {
        Empty,
        WithWire,
        Full
    }

    public GameObject Water;

    private UIStagesControl _uiStagesControl;
    private StateCupForWire _state;
    private Vector3 _originScale;
    private int _countWaterDrop;

    public StateCupForWire GetState => _state;

    private void Awake()
    {
        _uiStagesControl = FindObjectOfType<UIStagesControl>();
        _originScale = Water.transform.localScale;
    }

    public void AddWire()
    {
        _state = StateCupForWire.WithWire;
    }

    public void AddWaterDrop()
    {
        _countWaterDrop++;
        
        if (!Water.activeSelf)
            Water.SetActive(true);

        Water.transform.localScale *= 1.2f;

        if(_countWaterDrop < 3) return;
        
        _state = StateCupForWire.Full;
        _uiStagesControl.NextStep();

        foreach (var wire in FindObjectsOfType<Wire.Wire>())
        {
            if (wire.typePassivate == TypePassivate.Passivated)
            {
                wire.MoveMap.StartToMove(4);
                wire.MoveMouseItem.StartRotation = Quaternion.Euler(0, 270, 0);
                wire.typePassivate = TypePassivate.PassivatedAndWashH2O;

                return;
            }
        }
    }

    public void Restart()
    {
        _state = StateCupForWire.Empty;
        _countWaterDrop = 0;
        Water.transform.localScale = _originScale;
        Water.SetActive(false);
    }
}