#region Imports

using System;
using GrimGame.Engine;
using GrimGame.Game.Character;
using Microsoft.Xna.Framework;

#endregion

namespace GrimGame.Game.Scenes
{
    internal class Level1 : Scene
    {
        private readonly MapSystem _mapSystem;
        private          Player    _player;

        public Level1(string sceneName, string mapName, MainGame mainGame)
            : base(sceneName, mainGame)
        {
            _mapSystem = new MapSystem(mapName);
            SceneManager.AddScene(this);
        }

        public override void Initialize()
        {
            base.Initialize();

            if (GetIsSceneLoaded())
            {
                #region Map System

                _player = new Player(_mapSystem, Globals.Camera)
                {
                    Name = "Player 1",
                    Tag = Globals.ObjectTags.Player,
                    Speed = 2f,
                    RunningSpeed = 3.2f,
                    Enabled = true,
                    Active = true
                };
                _player.Init();

                _mapSystem.Player = _player;

                #endregion

                UiManager = new UIManager(this);

                // Init debugger
                if (Globals.DebugMode)
                {
                    GrimDebugger.Player = _player;
                    GrimDebugger.MapSystem = _mapSystem;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (GetIsSceneLoaded())
            {
                if (_player.Active)
                    _player.Update(this, gameTime);

                _mapSystem.Update(gameTime);

                InputManager.Update();
                UiManager.Update();

                ObjectManager.Update(this, gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (GetIsSceneLoaded())
            {
                // Clear the screen
                Globals.Graphics.GraphicsDevice.Clear(Color.Black);

                // Sort the player's index
                PlayerLayerIndexer();

                UiManager.Draw();
            }

            base.Draw(gameTime);
        }

        private void PlayerLayerIndexer()
        {
            foreach (var (rectangle, isBelowPlayer) in _mapSystem.FrontAndBackWalls)
            {
                if (!isBelowPlayer) _mapSystem.DrawMap(Globals.Camera.GetViewMatrix(), 3);

                if (_player.Sprite.BoxCollider.Bounds.Top >= rectangle.Bottom
                    && _player.Sprite.BoxCollider.Bounds.Top <= rectangle.Bottom + _player.Height / 2
                    && _player.Sprite.BoxCollider.Bounds.Right >= rectangle.Left
                    && _player.Sprite.BoxCollider.Bounds.Left <= rectangle.Right)
                {
                    _mapSystem.DrawMap(Globals.Camera.GetViewMatrix(), 12);
                    _mapSystem.CurrentIndex = Globals.LayerCount;
                }
            }
        }
    }
}