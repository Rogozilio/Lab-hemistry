using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



namespace VirtualLab.Tooltips
{

/// <summary> 
///     Элемент данных выноски. Содержит тип данных, и данные в виде строки или числа.
/// </summary> 
[System.Serializable]
public class Data 
{
    public enum Type { Title, Text, Progress }

    [Tooltip("Тип данных. Title - заголовок выноски, Text - текст выноски, Progress - линия прогресса")]
    [SerializeField] Type   _type       = Type.Text;
    [Tooltip("Данные в виде строки, используется как заголок или текст выноски, а также описание полосы прогресса")]
    [SerializeField] string _stringData = "";
    [Tooltip("Данные в виде числа, используется как значение уровня полосы прогресса")]
    [SerializeField] float  _numberData = 0;



    public Data (Type type, string stringData, float numberData) 
    {
        this._type       = type;
        this._stringData = stringData;
        this._numberData = numberData;
    }

    public Data (ERA.Tooltips.Core.TooltipsInfo.Data info) 
    {
        _type = info.type switch 
        {
            "title"    => Type.Title,
            "text"     => Type.Text,
            "progress" => Type.Progress,
            _          => Type.Text
        };

        _stringData = info.stringData;
        _numberData = info.numberData;
    }



    //  Data  -------------------------------------------------------
    /// <summary> 
    ///     Тип данных. <br/> 
    ///     Title - заголовок выноски <br/> 
    ///     Text - текст выноски <br/>
    ///     Progress - линия прогресса 
    /// </summary> 
    public Type type => _type;

    /// <summary> 
    ///     Данные в виде строки, используется как заголок или текст выноски, а также описание полосы прогресса
    /// </summary> 
    public string stringData 
    {
        get => _stringData;
        set => _stringData = value;
    }

    /// <summary> 
    ///     Данные в виде числа, используется как значение уровня полосы прогресса
    /// </summary> 
    public float numberData 
    {
        get => _numberData;
        set => _numberData = value;
    }



    //  Info  -------------------------------------------------------
    public bool Match (ERA.Tooltips.Core.TooltipsInfo.Data info) 
    {
        return 
            info.type == "title"    && type == Type.Title || 
            info.type == "text"     && type == Type.Text || 
            info.type == "progress" && type == Type.Progress;
    }

}

}
