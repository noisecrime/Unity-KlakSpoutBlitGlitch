using Klak.Spout;
using UnityEngine;

namespace KlakSpoutBlitGlitch
{
    public class SpoutLauncher : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_sproutPrefab;

        void Start()
        {
            if ( null == FindObjectOfType<SpoutSender>() )
                Instantiate( m_sproutPrefab );

        }
    }
}