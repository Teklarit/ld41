using System;

namespace UnityEngine.PostProcessing
{
    [Serializable]
    public class FadeModel : PostProcessingModel
    {
        [Serializable]
        public struct Settings
        {
            [ColorUsage(false)]
            [Tooltip("Fade color.")]
            public Color color;

            public static Settings defaultSettings
            {
                get
                {
                    return new Settings
                    {
                        color = new Color(0f, 0f, 0f, 1f),
                    };
                }
            }
        }

        [SerializeField]
        Settings m_Settings = Settings.defaultSettings;
        public Settings settings
        {
            get { return m_Settings; }
            set { m_Settings = value; }
        }

        public override void Reset()
        {
            m_Settings = Settings.defaultSettings;
        }
    }
}
