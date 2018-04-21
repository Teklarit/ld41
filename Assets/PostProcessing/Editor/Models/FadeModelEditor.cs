using UnityEngine.PostProcessing;

namespace UnityEditor.PostProcessing
{
    using Settings = FadeModel.Settings;

    [PostProcessingModelEditor(typeof (FadeModel))]
    public class FadeModelEditor : PostProcessingModelEditor
    {
        SerializedProperty m_Color;

        public override void OnEnable()
        {
            m_Color = FindSetting((Settings x) => x.color);
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(m_Color);
        }
    }
}
