using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using ERA.Tooltips.Core;



namespace ERA
{

/// <summary>
///     Основной компонент для информационной выноски
/// </summary>
public class Tooltip : ERA.Tooltips.Core.Updatable
{
    [SerializeField] TooltipDataList data;

    [SerializeField] MainSettings     mainSettings     = new MainSettings();
    [SerializeField] UISettings       uiSettings       = new UISettings();
    [SerializeField] GeometrySettings geometrySettings = new GeometrySettings();
    [SerializeField] LogicSettings    logicSettings    = new LogicSettings();



    //  Data  -------------------------------------------------------
    /// <summary>
    ///     Имя выноски. Используется для соотношения данных в json с объектами выносок в проекте 
    /// </summary>
    public new string name => mainSettings.name;
    /// <summary>
    ///     Список данных выноски. Используя этот список можно добавлять, удалять и изменять данные выноски.
    ///     Отображение данных обновляется в реальном времени 
    /// </summary>
    public TooltipDataList dataList => data;

    public Transform setTarget
    {
        set => geometrySettings.worldPoint = value;
    }

    public Vector3 OffsetLocalSpace
    {
        set => geometrySettings.offsetLocalSpace = value;
    }

    public bool SetAlwaysVisible
    {
        set => __visibilitySettings.alwaysVisible = value;
    }

    public Dictionary<int, string> DataFromFile;

    //  Edit mode  --------------------------------------------------
    protected override void AwakeEditMode () 
    {
        InitSettings();
        CreateParts();
    }

    protected override void UpdateEditMode () 
    {
        if (!settingsReady) InitSettings();

        if (settingsReady) 
        {
            if (!partsReady) CreateParts();
            UpdateParts();
        }
    }
    
    public void UpdateReadFileTooltips()
    {
        FindObjectOfType<ReadFiles>().StringLoaderForTooltips();
    }



    //  Play mode  --------------------------------------------------
    protected override void AwakePlayMode () 
    {
        InitSettings();
        CreateParts();
    }

    protected override void UpdatePlayMode () 
    {
        UpdateParts();
    }



    //  Settings  ---------------------------------------------------
    bool settingsReady => 
        mainSettings    .isReady && 
        uiSettings      .isReady &&
        geometrySettings.isReady && 
        logicSettings   .isReady;

    void InitSettings () 
    {
        mainSettings    .Init(this);
        uiSettings      .Init(this);
        geometrySettings.Init(this);
        logicSettings   .Init(this);
    }



    //  Parts  ------------------------------------------------------
    bool partsReady => 
        geometry   != null && 
        ui         != null && 
        dataViews  != null && 
        visibility != null;

    Geometry   geometry;
    UIPart     ui;
    DataViews  dataViews;
    Visibility visibility;

    void CreateParts () 
    {
        geometry   = new Geometry(mainSettings, uiSettings, geometrySettings);
        ui         = new UIPart(uiSettings, geometrySettings, geometry);
        dataViews  = new DataViews(mainSettings, uiSettings, dataList, geometry);
        visibility = new Visibility(uiSettings, logicSettings);
    }

    void UpdateParts () 
    {
        dataViews.Update();
        UpdateUILayout();

        geometry.Update();
        ui.Update();
        visibility.Update();
    }



    //  Geometry  ---------------------------------------------------
    /// <summary>
    ///     Объект, к которому привязана выноска. 
    /// </summary>
    public Transform worldPoint 
    {
        get => geometrySettings.worldPoint;
        set => geometrySettings.worldPoint = value;
    }



    //  Visibility  -------------------------------------------------
    /// <summary>
    ///     Отображается ли сейчас выноска.
    /// </summary>
    /// <value>
    ///     <c>true</c>: выноска отображется полностью, появляется или исчезает. <br/>
    ///     <c>false</c>: выноска полностью скрыта (полностью прозрачная).
    /// </value>
    public bool isVisible => visibility.isVisible;
    /// <summary>
    ///     Отображается ли сейчас выноска полностью.
    /// </summary>
    /// <value>
    ///     <c>true</c>: выноска отображется полностью (полностью непрозрачная). <br/>
    ///     <c>false</c>: выноска полностью скрыта.
    /// </value>
    public bool isFullyVisible => visibility.isFullyVisible;

    /// <summary>
    ///     Показать выноску. Запускается анимация появления выноски.
    /// </summary>
    public void Show () 
    {
        if (!this) return;
        visibility.Show();
    }
    /// <summary>
    ///     Спрятать выноску. Запускается анимация исчезновения выноски.
    /// </summary>
    public void Hide () 
    {
        if (!this) return;
        visibility.Hide();
    }
    /// <summary>
    ///     Показать выноску. Выноска появляется мгновенно.
    /// </summary>
    public void ShowInstantly () 
    {
        if (!this) return;
        visibility.ShowInstantly();
    }
    /// <summary>
    ///     Спрятать выноску. Выноска исчезает мгновенно.
    /// </summary>
    public void HideInstantly () 
    {
        if (!this) return;
        visibility.HideInstantly();
    }



    //  Tech  -------------------------------------------------------
    void UpdateUILayout () 
    {
        LayoutGroup layout = transform.GetComponentInParent<LayoutGroup>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(layout.GetComponent<RectTransform>());
    }



    //  Editor  -----------------------------------------------------
    public bool __settingsReady => 
        mainSettings      .isReady && 
        geometrySettings  .isReady && 
        logicSettings.isReady;

    public MainSettings     __mainSettings       => mainSettings;
    public UISettings       __uiSettings         => uiSettings;
    public GeometrySettings __geometrySettings   => geometrySettings;
    public LogicSettings    __visibilitySettings => logicSettings;

}

}
