using UnityEngine;

namespace Zoomies
{
    public class RespawnTriggerZone : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out ZoomiesCat slingshotCat))
            {
                slingshotCat.Respawn(transform.position);
            }
        }
    }
}
