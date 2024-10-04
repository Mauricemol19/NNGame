using Microsoft.Xna.Framework;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;

namespace NNGame.Classes.Cameras
{
    /// <summary>
    /// Parent Camera container
    /// </summary>
    public class Camera
    {
        private readonly OrthographicCamera _camera;

        public float _movementSpeed { get; set; }

        public TiledMap _tiledMap { get; set; }

        public Camera(BoxingViewportAdapter bva)
        {
            _camera = new OrthographicCamera(bva);
        }

        public void Move(Vector2 mvd, float seconds)
        {
            //mvd.SetX(mvd * _movementSpeed);
           
            _camera.Move(mvd);
        }

        public Matrix GetViewMatrix()
        {
            return _camera.GetViewMatrix();
        }

        public Vector2 GetWXY(MouseState mouseState, Vector2 worldPos)
        {
            /*
            float relativeMouseX = mouseState.X + _camera.Position.X;
            float relativeMouseY = mouseState.Y + _camera.Position.Y;

            float rmx = worldPos.X = relativeMouseX / 32;
            float rmy = worldPos.Y = relativeMouseY / 32;           
   
            return new Vector2((int)rmx, (int)rmy);
            */

            return _camera.ScreenToWorld(mouseState.X, mouseState.Y);                        
        }

        public Vector2 GetWorldPos(MouseState mouseState)
        {            
            float relativeMouseX = mouseState.X + _camera.Position.X;
            float relativeMouseY = mouseState.Y + _camera.Position.Y;

            return new Vector2((int)relativeMouseX, (int)relativeMouseY);
        }

        public Vector2 GetWindowPos(GameWindow window)
        { 
            return new Vector2(window.Position.X, window.Position.Y);
        }
    }
}
