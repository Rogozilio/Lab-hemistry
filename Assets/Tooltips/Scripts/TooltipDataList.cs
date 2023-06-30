using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace ERA
{

/// <summary> 
/// Список данных для выноски. Свойства и методы такие же, как и у List<TooltipData> 
/// </summary> 
[System.Serializable]
public class TooltipDataList : IList<TooltipData>
{
    [SerializeField] List<TooltipData> data;


    public int Count => data.Count;

    public bool IsReadOnly => false;

    public bool Contains (TooltipData item) => data.Contains(item);

    public int IndexOf (TooltipData item) => data.IndexOf(item);


    public TooltipData this[int i] 
    {
        get => data[i];
        set => data[i] = value;
    }


    public void Add (TooltipData item) => data.Add(item);

    public void CopyTo (TooltipData [] array, int arrayIndex) => data.CopyTo(array, arrayIndex);

    public void Insert (int index, TooltipData item) => data.Insert(index, item);

    public bool Remove (TooltipData item) => data.Remove(item);

    public void RemoveAt (int index) => data.RemoveAt(index);

    public void Clear () => data.Clear();


    IEnumerator IEnumerable.GetEnumerator  () => data.GetEnumerator();
    public IEnumerator<TooltipData> GetEnumerator () => data.GetEnumerator();

}

}