#region Imports
using GrimGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
#endregion

namespace GrimGame.Game.Character
{
    /// <summary>
    /// The playable character.
    /// </summary>
    public class Player : GameObject
    {
        /// <summary>
        /// The player's tile position.
        /// </summary>
        public Vector2 TilePosition;
        
        /// <summary>
        /// The origin point of the player's sprite.
        /// </summary>
        private Vector2 Origin { get; set; }

        /// <summary>
        /// The player's sprite
        /// </summary>
        public Texture2D PlayerSprite { get; private set; }

        private MapSystem _mapSystem;
        private OrthographicCamera _camera;

        public Player(MapSystem mapSystem, OrthographicCamera camera,Texture2D playerSprite)
        {
            _mapSystem = mapSystem;
            _camera = camera;
            PlayerSprite = playerSprite;
        }
        
        public override void Init(MainGame g)
        {
            
            Origin = new Vector2(PlayerSprite.Width / 2, PlayerSprite.Height);

            foreach (var objectLayer in _mapSystem.Map.ObjectLayers)
            {
                foreach (var layerObject in objectLayer.Objects)
                {
                    if (layerObject.Name.ToLower().Equals("playerspawn"))
                    {
                        Position = layerObject.Position;
                    }
                }
            }
        }

        public override void Destroy(MainGame g)
        {
            // TODO: Destroy GameObject
        }

        public override void Update(MainGame g)
        {
            Move();

            var x = (ushort) (Position.X / 32);
            var y = (ushort) (Position.Y / 32);
            
            TilePosition = new Vector2(x, y);
        }

        public override void Draw(MainGame g)
        {
            Globals.SpriteBatch.Begin(transformMatrix: Globals.Camera.GetViewMatrix(), samplerState: new SamplerState { Filter = TextureFilter.Point });
            // Drawing of player sprite
            Globals.SpriteBatch.Draw(PlayerSprite, Position, null, Color.White, 0f, Origin, 
                new Vector2(0.5f, 0.5f), SpriteEffects.None, 0.1f);
            Globals.SpriteBatch.End();
        }

        /// <summary>
        /// Moves the player.
        /// </summary>
        private void Move()
        {
            _camera.LookAt(Position);
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Position += new Vector2(0, -1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Position += new Vector2(0, 1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Position += new Vector2(-1, 0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Position += new Vector2(1, 0);
            }
        }
    }
}