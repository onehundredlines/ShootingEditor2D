using UnityEngine;
namespace ShootingEditor2D
{
    public class TriggerCheck : MonoBehaviour
    {
        [SerializeField]
        private LayerMask TargetLayers;
        [SerializeField]
        private int EnterCount;
        //类似引用计数器
        public bool Triggered => EnterCount > 0;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (IsInLayerMask(other.gameObject, TargetLayers))
            {
                EnterCount++;
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (IsInLayerMask(other.gameObject, TargetLayers))
            {
                EnterCount--;
            }
        }
        private bool IsInLayerMask(GameObject obj, LayerMask mask)
        {
            // 根据Layer数值进行移位获得用于运算的Mask值
            var objLayerMask = 1 << obj.layer;
            return (mask.value & objLayerMask) > 0;
        }
    }
}