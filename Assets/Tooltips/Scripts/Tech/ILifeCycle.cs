using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace VirtualLab.Tooltips.Core
{

public interface ILifeCycle 
{
    void StartMe ();
    void UpdateMe ();
    void UpdateSettings ();
}

}
