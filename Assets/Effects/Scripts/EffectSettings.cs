using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class EffectSettings : MonoBehaviour
{
    public enum TypeEffect
    {
        None,
        Steam,
        Smoke,
        Fog,
        Fire,
        Flame,
        Explosion,
        Bubble,
        Liquid,
        LiquidInVessel
    }

    [HideInInspector] public List<TypeEffect> typeEffects;
    [HideInInspector] public List<GameObject> effects;

    [HideInInspector] public ParticleSystem effect;
    //[HideInInspector] public LiquidVolume liquidInVessel;

    public TypeEffect typeEffect;

    [HideInInspector] public bool repeatEffect;

    [SerializeField] private Material[] _oldMaterialsParentLiquid;

#if UNITY_EDITOR
    private void Update()
    {
        if (!Application.isPlaying && repeatEffect && effect && !effect.isPlaying)
            effect.Play();
    }

    private void OnDrawGizmos()
    {
        if (typeEffect == TypeEffect.Steam)
        {
            Handles.color = Color.cyan;
            Handles.DrawWireDisc(transform.position, transform.up, effect.shape.radius);
        }
        else if (typeEffect == TypeEffect.Smoke)
        {
            Handles.color = Color.cyan;
            if (effect.shape.shapeType == ParticleSystemShapeType.Rectangle)
            {
                var size = new Vector3(effect.shape.scale.x, effect.main.startSpeed.constant, effect.shape.scale.y);
                var heightCube = new Vector3(0, effect.main.startSpeed.constant, 0);
                Handles.DrawWireCube(transform.position + heightCube / 2f, size);
                return;
            }
            
            var radius = effect.shape.radius;
            var angle = effect.shape.angle;
            Handles.matrix = Gizmos.matrix = transform.localToWorldMatrix;;
            Handles.DrawWireDisc(Vector3.zero, Vector3.up, radius);
            var height = new Vector3(0, effect.main.startSpeed.constant * MathF.Cos(angle * Mathf.Deg2Rad), 0);
            var radiusTop = Math.Clamp(radius * (1 / MathF.Cos(angle * Mathf.Deg2Rad)), radius, 5f);
            Handles.DrawWireDisc(height, Vector3.up, radiusTop);
            var xBase = new Vector3(radius, 0, 0);
            var zBase = new Vector3(0, 0, radius);
            var xTop = new Vector3(radiusTop, 0, 0);
            var zTop = new Vector3(0, 0, radiusTop);
            Handles.DrawLine(xBase, xTop + height);
            Handles.DrawLine(-xBase, -xTop + height);
            Handles.DrawLine(zBase, zTop + height);
            Handles.DrawLine(-zBase, -zTop + height);
        }
        else if (typeEffect == TypeEffect.Fog)
        {
            var size = new Vector3(effect.shape.scale.x, effect.shape.scale.y, effect.shape.scale.z);
            Handles.color = Color.cyan;
            Handles.matrix = Gizmos.matrix = transform.localToWorldMatrix;;
            Handles.DrawWireCube(Vector3.zero, size);
        }
        else if (typeEffect == TypeEffect.Bubble)
        {
            Handles.color = Color.cyan;
            Handles.matrix = Gizmos.matrix = transform.localToWorldMatrix;;
            Handles.DrawWireDisc(Vector3.zero, Vector3.up, effect.shape.radius);
            var height = new Vector3(0, effect.main.startSpeed.constant, 0);
            Handles.DrawWireDisc(height, Vector3.up, effect.shape.radius);
        }
        else if (typeEffect == TypeEffect.Fire)
        {
            var center = transform.position - effect.main.startSize.constant / 3f * Vector3.up;
            Handles.color = Color.cyan;
            if (effect.shape.shapeType == ParticleSystemShapeType.Circle)
            {
                Handles.DrawWireDisc(center, transform.up, effect.shape.radius);
            }
            else if(effect.shape.shapeType == ParticleSystemShapeType.Rectangle)
            {
                var size = new Vector3(effect.shape.scale.x, 0, effect.shape.scale.y);
                Handles.matrix = Gizmos.matrix = transform.localToWorldMatrix;;
                Handles.DrawWireCube(- effect.main.startSize.constant / 3f * Vector3.up, size);
            }
        }
    }
#endif

    public void LoadEffect()
    {
        if (typeEffect == TypeEffect.None)
        {
            DestroyEffect();
            return;
        }

        // if (typeEffect == TypeEffect.LiquidInVessel)
        // {
        //     CreateLiquidInVessel();
        //     return;
        // }

        for (var i = 0; i < typeEffects.Count; i++)
        {
            if (typeEffect != typeEffects[i]) continue;
            CreateEffect(effects[i]);
            break;
        }
    }

    private void DestroyEffect()
    {
        if (gameObject.TryGetComponent(out ParticleSystem particle))
        {
            DestroyImmediate(particle);
        }
        // else if (gameObject.transform.parent && gameObject.transform.parent.TryGetComponent(out LiquidVolume script))
        // {
        //     DestroyImmediate(script);
        //     gameObject.transform.parent.GetComponent<Renderer>().sharedMaterials = _oldMaterialsParentLiquid;
        // }
    }

    private void CreateEffect(GameObject newEffect)
    {
        DestroyEffect();
        var defaultParticle = newEffect.GetComponent<ParticleSystem>();
        effect = gameObject.AddComponent<ParticleSystem>();

        var defaultRender = defaultParticle.GetComponent<Renderer>();
        var newRender = effect.GetComponent<Renderer>();

#if UNITY_EDITOR
        EditorUtility.CopySerialized(defaultParticle, effect);
        EditorUtility.CopySerialized(defaultRender, newRender);
#endif
    }

    // private void CreateLiquidInVessel()
    // {
    //     if (!transform.parent) return;
    //
    //     _oldMaterialsParentLiquid = gameObject.transform.parent.GetComponent<Renderer>().sharedMaterials;
    //     DestroyEffect();
    //     gameObject.transform.parent.AddComponent<LiquidVolume>();
    // }
}