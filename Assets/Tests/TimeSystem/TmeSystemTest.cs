using UnityEngine;
namespace ShootingEditor2D.Tests
{
    public class TmeSystemTest : MonoBehaviour
    {
        private void Start()
        {
            Debug.Log(Time.time);
            ShootingEditor2D.Interface.GetSystem<ITimeSystem>().AddTimeDelay(3,()=> Debug.Log(Time.time));
        }
    }
}
