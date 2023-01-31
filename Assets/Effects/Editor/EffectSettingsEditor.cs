using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EffectSettings))]
public class EffectSettingsEditor : Editor
{
    private enum ParticleMode
    {
        Always,
        [InspectorName("One-time")] OneTime
    }

    private enum ShapeSteam
    {
        Cone,
        Cylinder
    }
    
    private enum ShapeSmoke
    {
        Cone,
        Cylinder,
        Box
    }

    private enum ShapeFire
    {
        Circle,
        Rectangle
    }

    private EffectSettings _effectSettings;

    private SerializedProperty _propertyTypeEffect;
    private SerializedProperty _propertyTypeEffects;
    private SerializedProperty _propertyEffects;

    private ParticleMode _particleMode;
    private ShapeSteam _shapeSteam;
    private ShapeSmoke _shapeSmoke;
    private ShapeFire _shapeFire;
    private bool _isUseVelocityDirection;

    private void OnEnable()
    {
        _effectSettings = (EffectSettings)target;

        _propertyTypeEffect = serializedObject.FindProperty("typeEffect");
        _propertyTypeEffects = serializedObject.FindProperty("typeEffects");
        _propertyEffects = serializedObject.FindProperty("effects");

        _particleMode = _effectSettings.effect.main.loop ? ParticleMode.Always : ParticleMode.OneTime;
        _shapeSteam = _effectSettings.effect.sizeOverLifetime.enabled ? ShapeSteam.Cone : ShapeSteam.Cylinder;
        
        _shapeFire = _effectSettings.effect.shape.shapeType == ParticleSystemShapeType.Circle
            ? ShapeFire.Circle
            : ShapeFire.Rectangle;
        
        _isUseVelocityDirection = _effectSettings.effect.velocityOverLifetime.enabled;
    }

    public override void OnInspectorGUI()
    {
        _effectSettings = (EffectSettings)target;

        var isRootPrefab = !PrefabUtility.GetCorrespondingObjectFromSource(_effectSettings.gameObject);

        if (isRootPrefab && !Application.isPlaying)
        {
            for (var i = 0; i < _effectSettings.effects.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_propertyTypeEffects.GetArrayElementAtIndex(i), GUIContent.none);
                EditorGUILayout.PropertyField(_propertyEffects.GetArrayElementAtIndex(i), GUIContent.none);
                if (GUILayout.Button("X"))
                {
                    _propertyTypeEffects.DeleteArrayElementAtIndex(i);
                    _propertyEffects.DeleteArrayElementAtIndex(i);
                    break;
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("Add"))
            {
                _effectSettings.typeEffects ??= new List<EffectSettings.TypeEffect>();
                _effectSettings.effects ??= new List<GameObject>();

                _propertyTypeEffects.arraySize++;
                _propertyEffects.arraySize++;
            }

            serializedObject.ApplyModifiedProperties();
            return;
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_propertyTypeEffect);
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            _effectSettings.LoadEffect();
            _particleMode = ParticleMode.Always;
        }

        //------Settings-----------
        switch (_effectSettings.typeEffect)
        {
            case EffectSettings.TypeEffect.None:
                break;
            case EffectSettings.TypeEffect.Steam:
                SettingSteamEffect();
                break;
            case EffectSettings.TypeEffect.Smoke:
                SettingSmokeEffect();
                break;
            case EffectSettings.TypeEffect.Fog:
                SettingFogEffect();
                break;
            case EffectSettings.TypeEffect.Fire:
                SettingFireEffect();
                break;
            case EffectSettings.TypeEffect.Flame:
                SettingFlameEffect();
                break;
            case EffectSettings.TypeEffect.Explosion:
                SettingExplosionEffect();
                break;
            case EffectSettings.TypeEffect.Bubble:
                SettingBubbleEffect();
                break;
            case EffectSettings.TypeEffect.Liquid:
                SettingLiquidEffect();
                break;
            case EffectSettings.TypeEffect.LiquidInVessel:
                InfoLiquidInVessel();
                break;
        }
    }

    private void SettingSteamEffect()
    {
        var main = _effectSettings.effect.main;
        var emission = _effectSettings.effect.emission;
        var shapeModule = _effectSettings.effect.shape;
        var velocityModule = _effectSettings.effect.velocityOverLifetime;

        SetMainPlayOnAwake(main, "Play On Awake");

        ChangeParticleMode(main);

        SetMainSimulationSpeed(main, "Speed");
        SetEmissionParticleCount(emission, "Count", 1f, 500f);
        SetMainStartSpeed(main, "Height", 0.1f, 5f);
        SetMainColor(main, "Color");
        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();
        _shapeSteam = (ShapeSteam)EditorGUILayout.EnumPopup("Shape ", _shapeSteam);
        if (EditorGUI.EndChangeCheck())
        {
            var sizeOverLifeTime = _effectSettings.effect.sizeOverLifetime;
            sizeOverLifeTime.enabled = _shapeSteam == ShapeSteam.Cone;
            shapeModule.angle = _shapeSteam == ShapeSteam.Cone ? shapeModule.angle : 0f;
        }

        if (_shapeSteam == ShapeSteam.Cone)
            SetShapeAngle(shapeModule, "Angle");
        SetShapeRadius(shapeModule, "Radius", 0.01f);

        SetVelocityOrbital(velocityModule, "Around Axis");
    }
    
    private void SettingSmokeEffect()
    {
        var main = _effectSettings.effect.main;
        var emission = _effectSettings.effect.emission;
        var shapeModule = _effectSettings.effect.shape;
        var velocityModule = _effectSettings.effect.velocityOverLifetime;

        SetMainPlayOnAwake(main, "Play On Awake");

        ChangeParticleMode(main);

        SetMainSimulationSpeed(main, "Speed", 0.1f, 10f);
        SetEmissionParticleCount(emission, "Count", 1f, 500f);
        SetMainStartSpeed(main, "Height", 0.1f, 5f);
        SetMainColor(main, "Color");
        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();
        _shapeSmoke = (ShapeSmoke)EditorGUILayout.EnumPopup("Shape ", _shapeSmoke);
        if (EditorGUI.EndChangeCheck())
        {
            shapeModule.angle = _shapeSmoke == ShapeSmoke.Cone ? shapeModule.angle : 0f;
            shapeModule.scale = _shapeSmoke == ShapeSmoke.Box ? shapeModule.scale : Vector3.one;
            shapeModule.shapeType = (_shapeSmoke == ShapeSmoke.Box)
                ? ParticleSystemShapeType.Rectangle
                : ParticleSystemShapeType.Cone;
        }

        switch (_shapeSmoke)
        {
            case ShapeSmoke.Cone:
                SetShapeAngle(shapeModule, "Angle");
                SetShapeRadius(shapeModule, "Radius");
                break;
            case ShapeSmoke.Cylinder:
                SetShapeRadius(shapeModule, "Radius");
                break;
            case ShapeSmoke.Box:
                SetShapeScale(shapeModule, "Size Rectangle", true);
                break;
        }

        SetVelocityOrbital(velocityModule, "Around Axis");
    }

    private void SettingFogEffect()
    {
        var main = _effectSettings.effect.main;
        var emission = _effectSettings.effect.emission;
        var shapeModule = _effectSettings.effect.shape;
        var velocityModule = _effectSettings.effect.velocityOverLifetime;

        SetMainPlayOnAwake(main, "Play On Awake");
        
        ChangeParticleMode(main);

        SetMainSimulationSpeed(main, "Speed", 0.1f, 5f);
        SetEmissionParticleCount(emission, "Count", 1f, 500f);
        SetMainStartSpeed(main, "Expend");
        SetMainStartSize3D(main, "Volume");
        SetMainColor(main, "Color");
        SetShapeScale(shapeModule, "Size Box");

        SetVelocity(velocityModule, "World Axis");
    }

    private void SettingFireEffect()
    {
        var main = _effectSettings.effect.main;
        var emission = _effectSettings.effect.emission;
        var shapeModule = _effectSettings.effect.shape;
        var velocityModule = _effectSettings.effect.velocityOverLifetime;

        SetMainPlayOnAwake(main, "Play On Awake");

        ChangeParticleMode(main);

        SetMainSimulationSpeed(main, "Speed", 1f, 10f);
        SetMainStartSize(main, "Height", 1f, 10f);
        SetEmissionParticleCount(emission, "Count", 1f, 500f);
        SetMainColor(main, "Color");
        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();
        _shapeFire = (ShapeFire)EditorGUILayout.EnumPopup("Shape ", _shapeFire);
        if (EditorGUI.EndChangeCheck())
        {
            shapeModule.shapeType = (_shapeFire == ShapeFire.Circle)
                ? ParticleSystemShapeType.Circle
                : ParticleSystemShapeType.Rectangle;
            if(shapeModule.shapeType == ParticleSystemShapeType.Circle)
                shapeModule.scale = Vector3.one;
        }

        EditorGUI.indentLevel++;
        if(_shapeFire == ShapeFire.Rectangle)
        {
            SetShapeScale(shapeModule, "Size Rectangle", true);
        }
        else if(_shapeFire == ShapeFire.Circle)
        {
            SetShapeRadius(shapeModule, "Radius", 0.1f, 20f);
        }
        EditorGUI.indentLevel--;
        SetVelocity(velocityModule, "World Axis");
    }

    private void SettingFlameEffect()
    {
        var main = _effectSettings.effect.main;
        var velocityModule = _effectSettings.effect.velocityOverLifetime;

        SetMainPlayOnAwake(main, "Play On Awake");

        ChangeParticleMode(main);

        SetMainSimulationSpeed(main, "Speed", 0.1f, 5f);
        SetMainStartSpeed(main, "Height", 5f, 50f);
        SetMainStartSize(main, "Volume");
        SetMainColor(main, "Color");

        SetVelocityOrbital(velocityModule, "Around Axis");
    }

    private void SettingExplosionEffect()
    {
        var main = _effectSettings.effect.main;
        var emission = _effectSettings.effect.emission;

        SetMainPlayOnAwake(main, "Play On Awake");

        EditorGUILayout.PropertyField(serializedObject.FindProperty("repeatEffect"),
            new GUIContent("Repeat Effect (Only Editor)"));
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();

        SetMainSimulationSpeed(main, "Speed", 1f, 5f);
        SetMainStartLifetime(main, "Duration", 0.5f, 2f);
        SetEmissionBurstCount(emission, "Count", 1f, 300f);
        SetMainStartSize3D(main, "Volume");
        SetMainColor(main, "Color");
    }

    private void SettingBubbleEffect()
    {
        var main = _effectSettings.effect.main;
        var emission = _effectSettings.effect.emission;
        var shapeModule = _effectSettings.effect.shape;
        var velocityModule = _effectSettings.effect.velocityOverLifetime;

        SetMainPlayOnAwake(main, "Play On Awake");

        ChangeParticleMode(main);

        SetEmissionParticleCount(emission, "Count", 1f, 500f);
        SetShapeRadius(shapeModule, "Radius");
        SetMainSimulationSpeed(main, "Speed", 0.1f, 5f);
        SetMainStartSpeed(main, "Height", 0.1f, 5f);
        SetMainStartSize(main, "Size", 0.01f, 0.5f);
        SetMainColor(main, "Color");

        SetVelocity(velocityModule, "World Axis");
    }

    private void SettingLiquidEffect()
    {
        var main = _effectSettings.effect.main;
        var emission = _effectSettings.effect.emission;
        var shapeModule = _effectSettings.effect.shape;
        var collision = _effectSettings.effect.collision;

        SetMainPlayOnAwake(main, "Play On Awake");
        
        ChangeParticleMode(main);
        
        SetEmissionParticleCount(emission, "Count");
        SetMainSimulationSpeed(main, "Speed", 0.1f, 5f);
        SetMainStartSpeed(main, "Force", 0.1f, 20f);
        SetMainStartLifetime(main, "Lifetime", 0.1f, 2f);
        SetMainStartSize(main, "Size", 0.01f, 0.3f);
        SetMainColor(main, "Color");
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Shape: ");
        EditorGUI.indentLevel++;
        SetShapeAngle(shapeModule, "Angle");
        SetShapeRadius(shapeModule, "Radius", 0.01f);
        EditorGUI.indentLevel--;
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Collision: ");
        EditorGUI.indentLevel++;
        SetCollisionDampen(collision, "Dampen");
        SetCollisionBounce(collision, "Bounce");
        EditorGUI.indentLevel--;
    }

    private void InfoLiquidInVessel()
    {
        if (_effectSettings.transform.parent)
        {
            EditorGUILayout.HelpBox("Make sure the gameObject is a child of the vessel gameObject.",
                MessageType.Warning);
            EditorGUILayout.HelpBox("Liquid settings are done in the parent game object.", MessageType.Info);
        }
        else
            EditorGUILayout.HelpBox("Make sure the gameObject is a child of the vessel gameObject.", MessageType.Error);
    }

    private void SetMainStartSpeed(ParticleSystem.MainModule main, string name, float min = 1f, float max = 10f)
    {
        var startSpeed = main.startSpeed;
        EditorGUI.BeginChangeCheck();
        startSpeed.constant = EditorGUILayout.Slider(name, startSpeed.constant, min, max);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_effectSettings.effect, "Changed effect var " + name);
            main.startSpeed = startSpeed;
        }
    }

    private void SetMainStartLifetime(ParticleSystem.MainModule main, string name, float min = 0.1f, float max = 10f)
    {
        var startLifetime = main.startLifetime;
        EditorGUI.BeginChangeCheck();
        startLifetime.constant = EditorGUILayout.Slider(name, startLifetime.constant, min, max);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_effectSettings.effect, "Changed effect var " + name);
            main.startLifetime = startLifetime;
        }
    }

    private void SetMainStartSize(ParticleSystem.MainModule main, string name, float min = 1f, float max = 20f)
    {
        var startSize = main.startSize;
        EditorGUI.BeginChangeCheck();
        switch (startSize.mode)
        {
            case ParticleSystemCurveMode.Constant:
                startSize.constant = EditorGUILayout.Slider(name, startSize.constant, min, max);
                break;
            case ParticleSystemCurveMode.TwoConstants:
                var minValue = startSize.constantMin;
                var maxValue = startSize.constantMax;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(name + " Min-Max", GUILayout.Width(EditorGUIUtility.labelWidth));
                minValue = EditorGUILayout.FloatField(minValue, GUILayout.Width(EditorGUIUtility.fieldWidth));
                EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, min, max);
                maxValue = EditorGUILayout.FloatField(maxValue, GUILayout.Width(EditorGUIUtility.fieldWidth));
                EditorGUILayout.EndHorizontal();
                startSize.constantMin = minValue;
                startSize.constantMax = maxValue;
                break;
        }
       
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_effectSettings.effect, "Changed effect var " + name);
            main.startSize = startSize;
        }
    }

    private void SetMainStartSize3D(ParticleSystem.MainModule main, string name, float min = 1f, float max = 20f)
    {
        var startSizeX = main.startSizeX;
        var startSizeY = main.startSizeY;
        var startSizeZ = main.startSizeZ;
        EditorGUI.BeginChangeCheck();
        var startSize = EditorGUILayout.Slider(name, startSizeX.constant, min, max);
        startSizeX.constant = startSize;
        startSizeY.constant = startSize;
        startSizeZ.constant = startSize;
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_effectSettings.effect, "Changed effect var " + name);
            main.startSizeX = startSizeX;
            main.startSizeY = startSizeY;
            main.startSizeZ = startSizeZ;
        }
    }

    private void SetMainColor(ParticleSystem.MainModule main, string name)
    {
        var startColor = main.startColor;
        EditorGUI.BeginChangeCheck();
        switch (startColor.mode)
        {
            case ParticleSystemGradientMode.Color:
                startColor.color = EditorGUILayout.ColorField(name, startColor.color);
                break;
            case ParticleSystemGradientMode.TwoColors:
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(name + " Between", GUILayout.Width(EditorGUIUtility.labelWidth));
                startColor.colorMin = EditorGUILayout.ColorField(startColor.colorMin);
                EditorGUILayout.LabelField(" - ", GUILayout.Width(12f));
                startColor.colorMax = EditorGUILayout.ColorField(startColor.colorMax);
                EditorGUILayout.EndHorizontal();
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_effectSettings.effect, "Changed effect var " + name);
            main.startColor = startColor;
        }
    }

    private void SetMainPlayOnAwake(ParticleSystem.MainModule main, string name)
    {
        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();
        var isPlayOnAwake = EditorGUILayout.Toggle(name, main.playOnAwake);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_effectSettings.effect, "Changed effect var " + name);
            main.playOnAwake = isPlayOnAwake;
        }

        EditorGUILayout.Space();
    }

    private void SetMainSimulationSpeed(ParticleSystem.MainModule main, string name, float min = 1f, float max = 20f)
    {
        EditorGUI.BeginChangeCheck();
        var simulationSpeed = EditorGUILayout.Slider(name, main.simulationSpeed, min, max);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_effectSettings.effect, "Changed effect var " + name);
            main.simulationSpeed = simulationSpeed;
        }
    }

    private void SetShapeAngle(ParticleSystem.ShapeModule shape, string name)
    {
        EditorGUI.BeginChangeCheck();
        var angle = EditorGUILayout.Slider(name, shape.angle, 0f, 90f);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_effectSettings.effect, "Changed effect var " + name);
            shape.angle = angle;
        }
    }

    private void SetShapeRadius(ParticleSystem.ShapeModule shape, string name, float min = 0.1f, float max = 10f)
    {
        EditorGUI.BeginChangeCheck();
        var radius = EditorGUILayout.Slider(name, shape.radius, min, max);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_effectSettings.effect, "Changed effect var " + name);
            shape.radius = radius;
        }
    }

    private void SetShapeScale(ParticleSystem.ShapeModule shape, string name, bool is2D = false)
    {
        Vector3 scale = Vector3.zero;
        EditorGUI.BeginChangeCheck();
        if (is2D)
        {
            scale = EditorGUILayout.Vector2Field(name, shape.scale);
            scale = new Vector3(scale.x, scale.y, 1f);
        }
        else
        {
            scale = EditorGUILayout.Vector3Field(name, shape.scale);
        }
        
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_effectSettings.effect, "Changed effect var " + name);
            shape.scale = scale;
        }
    }

    private void SetVelocity(ParticleSystem.VelocityOverLifetimeModule velocity, string name)
    {
        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();
        _isUseVelocityDirection = EditorGUILayout.Toggle("Velocity Direction", _isUseVelocityDirection);
        if (EditorGUI.EndChangeCheck())
        {
            var velocityOverLifetime = _effectSettings.effect.velocityOverLifetime;
            velocityOverLifetime.enabled = _isUseVelocityDirection;
        }

        if (!_isUseVelocityDirection) return;

        EditorGUILayout.BeginHorizontal();
        EditorGUI.indentLevel++;
        EditorGUI.BeginChangeCheck();
        var multiplier =
            EditorGUILayout.Vector3Field(name,
                new Vector3(velocity.xMultiplier, velocity.yMultiplier, velocity.zMultiplier));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_effectSettings.effect, "Changed effect var " + name);
            velocity.xMultiplier = multiplier.x;
            velocity.yMultiplier = multiplier.y;
            velocity.zMultiplier = multiplier.z;
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndHorizontal();
    }

    private void SetVelocityOrbital(ParticleSystem.VelocityOverLifetimeModule velocity, string name)
    {
        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();
        _isUseVelocityDirection = EditorGUILayout.Toggle("Velocity Orbital", _isUseVelocityDirection);
        if (EditorGUI.EndChangeCheck())
        {
            var velocityOverLifetime = _effectSettings.effect.velocityOverLifetime;
            velocityOverLifetime.enabled = _isUseVelocityDirection;
        }

        if (!_isUseVelocityDirection) return;

        EditorGUILayout.BeginHorizontal();
        EditorGUI.indentLevel++;
        EditorGUI.BeginChangeCheck();
        var multiplier =
            EditorGUILayout.Vector3Field(name,
                new Vector3(velocity.orbitalXMultiplier, velocity.orbitalYMultiplier, velocity.orbitalZMultiplier));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_effectSettings.effect, "Changed effect var " + name);
            velocity.orbitalXMultiplier = multiplier.x;
            velocity.orbitalYMultiplier = multiplier.y;
            velocity.orbitalZMultiplier = multiplier.z;
        }

        EditorGUI.indentLevel--;
        EditorGUILayout.EndHorizontal();
    }

    private void SetEmissionParticleCount(ParticleSystem.EmissionModule emission, string name, float min = 1f,
        float max = 1000f)
    {
        var rateOverTime = emission.rateOverTime;
        EditorGUI.BeginChangeCheck();
        rateOverTime.constant = (int)EditorGUILayout.Slider(name, rateOverTime.constant, min, max);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_effectSettings.effect, "Changed effect var " + name);
            emission.rateOverTime = rateOverTime;
        }
    }

    private void SetEmissionBurstCount(ParticleSystem.EmissionModule emission, string name, float min = 1f,
        float max = 1000f)
    {
        var burst = emission.GetBurst(0);
        var count = burst.count;
        EditorGUI.BeginChangeCheck();
        count.constant = (int)EditorGUILayout.Slider(name, count.constant, min, max);
        burst.count = count;
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_effectSettings.effect, "Changed effect var " + name);
            emission.SetBurst(0, burst);
        }
    }

    private void ChangeParticleMode(ParticleSystem.MainModule main)
    {
        EditorGUI.BeginChangeCheck();
        _particleMode = (ParticleMode)EditorGUILayout.EnumPopup("Mode ", _particleMode);
        if (EditorGUI.EndChangeCheck())
        {
            main.loop = _particleMode == ParticleMode.Always;
            _effectSettings.repeatEffect = _particleMode == ParticleMode.OneTime;
            _effectSettings.effect.Play();
        }

        if (_particleMode == ParticleMode.OneTime)
        {
            SetMainDuration(main, "Duration");
        }

        EditorGUILayout.Space();
    }

    private void SetMainDuration(ParticleSystem.MainModule main, string name, float min = 0.1f, float max = 10f)
    {
        EditorGUI.BeginChangeCheck();
        var duration = EditorGUILayout.Slider(name, main.duration, min, max);
        if (EditorGUI.EndChangeCheck())
        {
            _effectSettings.effect.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            main.duration = duration;
            _effectSettings.effect.Play();
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("repeatEffect"),
            new GUIContent("Repeat Effect (Only Editor)"));
        serializedObject.ApplyModifiedProperties();
    }

    private void SetCollisionDampen(ParticleSystem.CollisionModule collision, string name, float min = 0f, float max = 1f)
    {
        var dampen = collision.dampen;
        EditorGUI.BeginChangeCheck();
        dampen.constant = EditorGUILayout.Slider(name, dampen.constant, min, max);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_effectSettings.effect, "Changed effect var " + name);
            collision.dampen = dampen;
        }
    }
    
    private void SetCollisionBounce(ParticleSystem.CollisionModule collision, string name, float min = 0f, float max = 1f)
    {
        var bounce = collision.bounce;
        EditorGUI.BeginChangeCheck();
        bounce.constant = EditorGUILayout.Slider(name, bounce.constant, min, max);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_effectSettings.effect, "Changed effect var " + name);
            collision.bounce = bounce;
        }
    }
}