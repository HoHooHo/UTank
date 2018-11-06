using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.AnimatedValues;

//自定义Tset脚本
[CustomEditor(typeof(ColorEffect), true)]
[CanEditMultipleObjects]
public class ColorEffectEditor : Editor
{
    SerializedProperty m_Gray;
    SerializedProperty m_BlendType;
    SerializedProperty m_BlendSrc;
    SerializedProperty m_BlendDst;
    SerializedProperty m_ColorFilterType;
    SerializedProperty m_Brightness;
    SerializedProperty m_Saturation;
    SerializedProperty m_Hue;
    SerializedProperty m_Contrast;
    SerializedProperty m_TintColor;
    SerializedProperty m_TintAmount;

    AnimBool m_ShowCustomBlend;
    AnimBool m_ShowColorFilterAdjust;
    AnimBool m_ShowColorFilterTint;

    GUIContent m_GrayContent;
    GUIContent m_BlendTypeContent;
    GUIContent m_ColorFilterTypeContent;
    GUIContent m_BrightnessContent;
    GUIContent m_SaturationContent;
    GUIContent m_HueContent;
    GUIContent m_ContrastContent;

    protected virtual void OnEnable()
    {
        m_GrayContent = new GUIContent("置灰");
        m_BlendTypeContent = new GUIContent("混合方式");
        m_ColorFilterTypeContent = new GUIContent("颜色特效");
        m_BrightnessContent = new GUIContent("亮度");
        m_SaturationContent = new GUIContent("饱和度");
        m_HueContent = new GUIContent("色调");
        m_ContrastContent = new GUIContent("对比度");

        m_Gray = serializedObject.FindProperty("m_Gray");

        m_BlendType = serializedObject.FindProperty("m_BlendType");
        m_BlendSrc = serializedObject.FindProperty("m_BlendSrc");
        m_BlendDst = serializedObject.FindProperty("m_BlendDst");

        m_ColorFilterType = serializedObject.FindProperty("m_ColorFilterType");
        m_Brightness = serializedObject.FindProperty("m_Brightness");
        m_Saturation = serializedObject.FindProperty("m_Saturation");
        m_Hue = serializedObject.FindProperty("m_Hue");
        m_Contrast = serializedObject.FindProperty("m_Contrast");
        m_TintColor = serializedObject.FindProperty("m_TintColor");
        m_TintAmount = serializedObject.FindProperty("m_TintAmount");

        ColorEffect.BlendType blendType = (ColorEffect.BlendType)m_BlendType.enumValueIndex;
        m_ShowCustomBlend = new AnimBool(!m_BlendType.hasMultipleDifferentValues && blendType == ColorEffect.BlendType.Custom);
        m_ShowCustomBlend.valueChanged.AddListener(Repaint);

        ColorEffect.ColorFilterType colorFilterType = (ColorEffect.ColorFilterType)m_ColorFilterType.enumValueIndex;
        m_ShowColorFilterAdjust = new AnimBool(!m_BlendType.hasMultipleDifferentValues && colorFilterType == ColorEffect.ColorFilterType.AdJust);
        m_ShowColorFilterTint = new AnimBool(!m_BlendType.hasMultipleDifferentValues && (colorFilterType == ColorEffect.ColorFilterType.Tint || colorFilterType == ColorEffect.ColorFilterType.Tint2));
        m_ShowColorFilterAdjust.valueChanged.AddListener(Repaint);
        m_ShowColorFilterTint.valueChanged.AddListener(Repaint);
    }

    protected virtual void OnDisable()
    {
        m_ShowCustomBlend.valueChanged.RemoveListener(Repaint);
        m_ShowColorFilterAdjust.valueChanged.RemoveListener(Repaint);
        m_ShowColorFilterTint.valueChanged.RemoveListener(Repaint);
    }


    //在这里方法中就可以绘制面板。
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_Gray, m_GrayContent);

        EditorGUILayout.PropertyField(m_BlendType, m_BlendTypeContent);
        ColorEffect.BlendType typeEnum = (ColorEffect.BlendType)m_BlendType.enumValueIndex;
        m_ShowCustomBlend.target = (!m_BlendType.hasMultipleDifferentValues && typeEnum == ColorEffect.BlendType.Custom);
        if (EditorGUILayout.BeginFadeGroup(m_ShowCustomBlend.faded))
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_BlendSrc);
            EditorGUILayout.PropertyField(m_BlendDst);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();

        EditorGUILayout.PropertyField(m_ColorFilterType, m_ColorFilterTypeContent);
        ColorEffect.ColorFilterType colorFilterType = (ColorEffect.ColorFilterType)m_ColorFilterType.enumValueIndex;
        m_ShowColorFilterAdjust.target = !m_BlendType.hasMultipleDifferentValues && colorFilterType == ColorEffect.ColorFilterType.AdJust;
        if (EditorGUILayout.BeginFadeGroup(m_ShowColorFilterAdjust.faded))
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_Brightness, m_BrightnessContent);
            EditorGUILayout.PropertyField(m_Saturation, m_SaturationContent);
            EditorGUILayout.PropertyField(m_Hue, m_HueContent);
            EditorGUILayout.PropertyField(m_Contrast, m_ContrastContent);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();

        m_ShowColorFilterTint.target = !m_BlendType.hasMultipleDifferentValues && (colorFilterType == ColorEffect.ColorFilterType.Tint || colorFilterType == ColorEffect.ColorFilterType.Tint2);
        if (EditorGUILayout.BeginFadeGroup(m_ShowColorFilterTint.faded))
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_TintColor);
            EditorGUILayout.PropertyField(m_TintAmount);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFadeGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
