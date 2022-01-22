using QFramework;
using UnityEngine;
namespace ShootingEditor2D
{
    public abstract class ShootingEditor2DBaseController : MonoBehaviour, IController
    {
        IArchitecture IBelongToArchitecture.GetArchitecture() => ShootingEditor2D.Interface;
    }
}