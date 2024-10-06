using MonoGame.Extended.ViewportAdapters;

namespace NNGame.Classes.Cameras
{
    /// <summary>
    /// Child camera for the player movement
    /// </summary>
    public class PlayerCamera : Camera
    {
        public float moveSpeed = 60f;       

        public PlayerCamera(BoxingViewportAdapter viewportadapter)  : base(viewportadapter)
        {
            _movementSpeed = moveSpeed;
        }
    }
}
