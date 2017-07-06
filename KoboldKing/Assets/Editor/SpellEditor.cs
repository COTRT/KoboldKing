using Assets.Scripts.Magic.Effects;
using Assets.Scripts.Magic.Spells;
using Assets.Scripts.Managers;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Assets.Scripts.Events;

[CustomEditor(typeof(Spell))]
public class SpellEditor : Editor
{
    bool ShowSpellStopper;

    bool ShowEffects;
    bool CasterIsComboed;
    List<ComboedEffectItem> CasterEffects = new List<ComboedEffectItem>();
    bool TargetIsComboed;
    List<ComboedEffectItem> TargetEffects = new List<ComboedEffectItem>();


    public static Dictionary<string, Effect> _effects;
    public static Dictionary<string, Effect> Effects
    {
        get
        {
            if (_effects == null)
            {
                try
                {
                    _effects = GameController.GetManager<EffectManager>().Prefabs;

                }
                catch (InvalidOperationException)
                {
                    //We are not currently in-game, so we can't use managers.  We need to load the prefabs ourselves instead.
                    _effects = ResourceLoader.LoadNamedPrefabs<Effect>();
                };
            }
            return _effects;
        }
    }

    private void Awake()
    {
        Messenger.AddListener(ResourceEvent.PREFABS_RELOADING, ReloadEffectPrefabs);
    }
    private void ReloadEffectPrefabs()
    {
        //If _effects is null, the lazy loader above will handle all loading for us.
        _effects = null;
    }

    public override void OnInspectorGUI()
    {
        Spell spell = (Spell)target;

        //Spell Stopper --------
        ShowSpellStopper = EditorGUILayout.Foldout(ShowSpellStopper, "Spell Stopper");
        if (ShowSpellStopper)
        {
            spell.spellStopper = (SpellStopper)EditorGUILayout.EnumPopup("Spell Stopper", spell.spellStopper);
            switch (spell.spellStopper)
            {
                case Assets.Scripts.Magic.Spells.SpellStopper.Lifespan:
                    spell.lifespan = EditorGUILayout.FloatField("Lifespan (Seconds)", spell.lifespan);
                    break;
                //case Assets.Scripts.Magic.Spells.SpellStopper.UserAction:
                //    break;
                //case Assets.Scripts.Magic.Spells.SpellStopper.Event:
                //    break;
                default:
                    EditorGUILayout.HelpBox("Sorry, this Spell Stopper is not yet supported.", MessageType.Warning);
                    break;
            }
        }

        //Effects ----------
        ShowEffects = EditorGUILayout.Foldout(ShowEffects, "Effects");
        if (ShowEffects)
        {

            EditorGUILayout.LabelField("Caster:  ");
            spell.casterEffect = DrawEffectSelector(spell.casterEffect, ref spell.hasCasterEffect, ref CasterIsComboed, ref CasterEffects);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Target:  ");
            spell.targetEffect = DrawEffectSelector(spell.targetEffect, ref spell.hasTargetEffect, ref TargetIsComboed, ref TargetEffects);
        }
    }

    private Effect DrawEffectSelector(Effect effect, ref bool hasEffect, ref bool isComboed, ref List<ComboedEffectItem> effects)
    {
        hasEffect = EditorGUILayout.Toggle("Has Effect", hasEffect);
        if (hasEffect)
        {
            isComboed = EditorGUILayout.Toggle("Multi-Effect", isComboed);
            if (isComboed)
            {
                bool wasChanged = false;
                //Not foreach so we can nab effect references (foreach makes copies, I believe)...(Wait, never mind that, I think that no copies would be made in this instance.  I'm not entirely sure though, so I'm pulling a better-safe-than-why-the-heck-doens't-it-work.
                for (int i = 0; i < effects.Count; i++)
                {
                    bool wasChangedThisLoop;
                    string effectDisplayName;
                    if (effects[i].Effect == null)
                    {
                        effectDisplayName = "New Effect";
                    }
                    else
                    {
                        effectDisplayName = effects[i].Effect.name;
                    }
                    effects[i].Toggled = EditorGUILayout.Foldout(effects[i].Toggled, effectDisplayName);
                    EditorGUI.indentLevel++;
                    if (effects[i].Toggled)
                    {
                        if (GUILayout.Button("Remove"))
                        {
                            effects.RemoveAt(i);
                            i--;
                        }
                        else
                        {
                            DrawSingleEffectSelector(effects[i], out wasChangedThisLoop);
                            wasChanged = wasChanged || wasChangedThisLoop;
                        }
                    }
                    EditorGUI.indentLevel--;
                }

                if (GUILayout.Button("Add"))
                {
                    effects.Add(new ComboedEffectItem() { NameOverride = false, Toggled = false });
                }

                if (wasChanged)
                {
                    return new ComboEffect(effects.Select(e => e.Effect).ToArray());
                }
                else
                {
                    return effect;
                }
            }
            else
            {
                bool wasChanged = false;
                if (effects.Count == 0)
                {
                    effects.Add(DrawSingleEffectSelector(new ComboedEffectItem() { NameOverride = false, Toggled = false }, out wasChanged));
                }
                else
                {
                    effects[0] = DrawSingleEffectSelector(effects[0], out wasChanged);
                }
                return effects[0].Effect;
            }
        }
        else
        {
            return effect;
        }
    }

    private static ComboedEffectItem DrawSingleEffectSelector(ComboedEffectItem cei, out bool wasChanged)
    {
        cei.NameOverride = EditorGUILayout.Toggle("Name Override", cei.NameOverride);
        wasChanged = false;
        string currentEffectName;
        if (cei.NameOverride)
        {
            currentEffectName = cei.CurrentEffectNameOverride ?? string.Empty;
        }
        else if (cei.Effect == null)
        {
            currentEffectName = string.Empty;
        }
        else
        {
            currentEffectName = cei.Effect.name;
        }
        if (cei.NameOverride)
        {
            var newEffectName = EditorGUILayout.TextField("Effect Name", currentEffectName);
            if (Effects.ContainsKey(newEffectName))
            {
                if (newEffectName != currentEffectName)
                {
                    cei.Effect = Instantiate(Effects[newEffectName]);
                    wasChanged = true;
                }
            }
            else
            {
                EditorGUILayout.HelpBox("This Effect does not exist", MessageType.Warning);
            }
            cei.CurrentEffectNameOverride = currentEffectName;
        }
        else
        {
            var newEffect = EditorGUILayout.ObjectField("Effect", cei.Effect, typeof(Effect), false);
            string newEffectName;
            //This is where C# 6 null property accessors would be really nice...
            if (newEffect == null)
            {
                newEffectName = string.Empty;
            }
            else
            {
                newEffectName = newEffect.name;
            }
            if (newEffectName != currentEffectName)
            {
                cei.Effect = (Effect)Instantiate(newEffect);
                wasChanged = true;
            }
        }

        return cei;
    }
}

internal class ComboedEffectItem
{
    public ComboedEffectItem()
    {

    }

    public Effect Effect { get; set; }
    public bool NameOverride { get; set; }
    public bool Toggled { get; set; }
    public string CurrentEffectNameOverride { get; set; }
}