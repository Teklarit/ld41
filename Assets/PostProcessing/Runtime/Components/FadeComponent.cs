namespace UnityEngine.PostProcessing
{
    public sealed class FadeComponent : PostProcessingComponentRenderTexture<FadeModel>
    {
        static class Uniforms
        {
            internal static readonly int _Fade_Color = Shader.PropertyToID("_Fade_Color");
        }

        public override bool active
        {
            get
            {
                return model.enabled
                       && !context.interrupted;
            }
        }

        public override void Prepare(Material uberMaterial)
        {
            var settings = model.settings;

            uberMaterial.EnableKeyword("FADE");
            uberMaterial.SetColor(Uniforms._Fade_Color, settings.color);
        }
    }
}
