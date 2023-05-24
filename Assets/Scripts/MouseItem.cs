using UnityEngine;

[RequireComponent(typeof(StateItem))]
[RequireComponent(typeof(Outline))]
public class MouseItem : MonoBehaviour
{
    public int priorityOutline;
    
    protected bool _isActive;
    protected bool _isReadyToAction;
    private StateItem _stateItem;

    public bool IsActive => _isActive;

    public bool IsReadyToAction
    {
        get => _isReadyToAction;
        set => _isReadyToAction = value;
    }

    public StateItem StateItem => _stateItem;

    private Outline _outline;

    protected void Awake()
    {
        _outline = GetComponent<Outline>();
        _stateItem = GetComponent<StateItem>();
    }

    private void OnDisable()
    {
        _isActive = false;
    }

    public void ShowOutline()
    {
        _isReadyToAction = true;

        if (!enabled)
        {
            CursorSkin.Instance.UseNotInteractive();
            _outline.enabled = true;
            return;
        }

        if(!IsActive && GetType() == typeof(ClickMouseItem) && _stateItem.State != StateItems.Interacts)
            CursorSkin.Instance.UseClick();

        if (_outline.enabled) return;
        
        _outline.enabled = true;

        if (GetType() == typeof(MoveMouseItem))
            CursorSkin.Instance.UseSelect();
    }

    public void HideOutline(bool isChangeCursor = true)
    {
        _isReadyToAction = false;
        
        if (!_outline.enabled) return;
        
        _outline.enabled = false;
        
        if(IsActive) return;
        
        if(_stateItem.State != StateItems.Interacts && isChangeCursor)
            CursorSkin.Instance.UseArrow();
    }
}