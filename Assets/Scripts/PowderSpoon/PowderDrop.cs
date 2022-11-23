using UnityEngine;

namespace Liquid
{
    public enum TypePowder
    {
        NH4CI,
        CH3COONa
    }
    public class PowderDrop : MonoBehaviour
    {
        [HideInInspector] public TypePowder typePowder;
    }
}