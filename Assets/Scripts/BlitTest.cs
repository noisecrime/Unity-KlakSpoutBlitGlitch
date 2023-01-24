using UnityEngine;

namespace KlakSpoutBlitGlitch
{
    public class BlitTest : MonoBehaviour
    {
        [SerializeField]
        private Material        m_material;        
        [SerializeField]
        private Texture2D       m_texture2D;
        [SerializeField, Range(0f, 1f)]
        private float           m_blitChance = 0.7f;
        [SerializeField]
        private bool            m_wrapCallToBlit = false;


        private RenderTexture   m_renderTexture;

        void Start()
        {
            m_renderTexture = new RenderTexture( 512, 512, 0 );
            m_renderTexture.wrapMode = TextureWrapMode.Clamp;
            m_renderTexture.filterMode = FilterMode.Point;
            m_renderTexture.name = "bodyIndexTexture";
        }

        private void OnDestroy()
        {
            m_renderTexture.Release();
        }

        void Update()
        {
            if ( Random.value > m_blitChance )
                CustomBlit( m_texture2D, m_renderTexture, m_material, m_wrapCallToBlit );
        }

        public static void CustomBlit( Texture src, RenderTexture dst, Material mat, bool wrapCallToBlit = false )
        {
            RenderTexture previousActive = null;

            string activeName   = (RenderTexture.active == null) ? "null" : RenderTexture.active.name;
            string srcName      = (src == null) ? "null" : src.name;
            string dstName      = (dst == null) ? "null" : dst.name;
            string matName      = (mat == null) ? "null" : mat.name;
            string output       = "";

            output += $"Active RenderTexture - Before: active: [{activeName}] src: {srcName}  dst: {dstName}  Mat: {matName}\n";

            if ( wrapCallToBlit )
                previousActive = RenderTexture.active;

            Graphics.Blit( src, dst, mat );

            activeName = ( RenderTexture.active == null ) ? "null" : RenderTexture.active.name;
            output += $"Active RenderTexture - Blit : active: [{activeName}] src: {srcName}  dst: {dstName}  Mat: {matName}\n";

            if ( wrapCallToBlit )
                RenderTexture.active = previousActive;

            activeName = ( RenderTexture.active == null ) ? "null" : RenderTexture.active.name;
            output += $"Active RenderTexture - After: active: [{activeName}] src: {srcName}  dst: {dstName}  Mat: {matName}\n";

            Debug.Log( output );
        }
    }
}