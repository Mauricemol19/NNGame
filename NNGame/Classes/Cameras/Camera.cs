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
        public OrthographicCamera _camera;

        public float _movementSpeed { get; set; }

        public TiledMap _tiledMap { get; set; }

        public Camera(BoxingViewportAdapter bva)
        {
            _camera = new OrthographicCamera(bva);
        }

        public void Move(Vector2 position)
        {
            _camera.LookAt(position);
        }

        public void Move(Vector2 mvd, Vector2 direction, float seconds)
        {
            _camera.Move(mvd * _movementSpeed * seconds);
        }

        public Matrix GetViewMatrix()
        {
            return _camera.GetViewMatrix();
        }

        public Vector2 GetWXY(MouseState mouseState, Vector2 worldPos)
        {            
            return _camera.ScreenToWorld(mouseState.X, mouseState.Y);                        
        }    
      
        public void GetTileXYAtPoint(int x, int y, out int tileX, out int tileY)
        {
            tileX = x / 32;
            tileY = y / 32;
        }

        public string GetTileText(TiledMap map, int id)
        {
            foreach (var tileSet in map.Tilesets)
            {
                int firstGid = map.GetTilesetFirstGlobalIdentifier(tileSet);

                if ((id >= firstGid) && (id < firstGid + tileSet.TileCount))
                    return $"{tileSet.Name}: {id - firstGid}";
            }

            return "Unknown";
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
