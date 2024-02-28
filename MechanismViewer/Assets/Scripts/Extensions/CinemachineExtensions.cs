using Cinemachine;

namespace Extensions
{
    public static class CinemachineExtensions
    {
        public static bool TryGetCurrentCamera<T>(this CinemachineBrain cinemachineBrain, out T currentCamera) 
                where T : ICinemachineCamera
        {
            if (cinemachineBrain.ActiveVirtualCamera is T genericCamera)
            {
                currentCamera = genericCamera;
                return true;
            }
            
            currentCamera = default;
            return false;
        }
    }
}