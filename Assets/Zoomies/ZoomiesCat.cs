using UnityEngine;

namespace Zoomies
{
    public class ZoomiesCat : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Rigidbody2D _rigidbody;

        public Rigidbody2D Rigidbody => _rigidbody;
        
        public void Respawn(Vector3 worldPosition)
        {
            transform.position = worldPosition;
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.angularVelocity = 0f;
        }
    }
}
