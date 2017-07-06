using UnityEngine;
using UnityEditor;
using Assets.Scripts.Magic.Effects;

[CustomEditor(typeof(Effect))]
public class EffectEditor : Editor
{
    public static void DrawCustomEditor(Effect t)
    {

    }
    public override void OnInspectorGUI()
    {
        DrawCustomEditor((Effect)target);
    }
}