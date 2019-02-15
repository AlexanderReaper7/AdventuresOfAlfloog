using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace AdventuresOfAlfloog
{
    public static class InGame
    {
        private static Camera2D _camera;

        public static List<Enemy> Enemies;
        public static List<TerrainElement> TerrainElements;
        public static TerrainElement Ground;
        private static Player _player;
        private static List<ParallaxElement> _parallaxBackgrounds;
        private static List<Level> _levels;
        // Rectangle where player and enemies can be
        private static Rectangle _playArea;

        public static Rectangle PlayArea => _playArea;



        public static void LoadContent(ContentManager content, Viewport viewport)
        {
            LoadPlayer(content);
            LoadEnemies(content);
            // Load camera
            _camera = new Camera2D(viewport) {Zoom = 1.0f};
            // Load Levels
            _levels = new List<Level>();
            LoadLevel1(content);
            LoadLevel2(content);
            // start first level
            StartLevel(0);

        }

        private static void LoadPlayer(ContentManager content)
        {
            // Load player
            Texture2D playerTexture = content.Load<Texture2D>(@"Textures/PlayerSpriteSheet"); 
            List<Animation> playerAnimations = new List<Animation>() // TODO: Load animations
            {
                new Animation(AnimationStates.Idle.ToString() + AnimationDirections.Right.ToString(), new Rectangle(0,0,50,100), 1, 333),
                new Animation(AnimationStates.Idle.ToString() + AnimationDirections.Left.ToString(), new Rectangle(0,100,50,100), 1, 333),
                new Animation(AnimationStates.Walk.ToString() + AnimationDirections.Right.ToString(), new Rectangle(0,200,50,100), 1, 333),
                new Animation(AnimationStates.Walk.ToString() + AnimationDirections.Left.ToString(), new Rectangle(0,300,50,100), 1, 333),
                new Animation(AnimationStates.Attack.ToString() + AnimationDirections.Right.ToString(), new Rectangle(0,400,50,100), 1, 333),
                new Animation(AnimationStates.Attack.ToString() + AnimationDirections.Left.ToString(), new Rectangle(0,500,50,100), 1, 333),
            };

            _player = new Player(playerTexture, Vector2.Zero, playerAnimations);

        }

        private static void LoadEnemies(ContentManager content)
        {
            Texture2D pesantTexture = content.Load<Texture2D>(@"Textures/Character");
            List<Animation> pesantAnimations = new List<Animation>()
            {
                new Animation(AnimationStates.Walk.ToString() + AnimationDirections.Right.ToString(), Rectangle.Empty, 4, 40),
                new Animation(AnimationStates.Walk.ToString() + AnimationDirections.Left.ToString(), Rectangle.Empty, 4, 40)
            };
            Pesant.LoadContent(pesantTexture, pesantAnimations);

            Texture2D knightTexture = content.Load<Texture2D>(@"Textures/Character");
            List<Animation> knightAnimations = new List<Animation>()
            {
                new Animation(AnimationStates.Walk.ToString() + AnimationDirections.Right.ToString(), Rectangle.Empty, 4, 40),
                new Animation(AnimationStates.Walk.ToString() + AnimationDirections.Left.ToString(), Rectangle.Empty, 4, 40)
            };
            Knight.LoadContent(knightTexture, knightAnimations);
        }

        private static void LoadLevel1(ContentManager content)
        {
            Rectangle playArea = new Rectangle(0, 0, 3000, 720);

            Texture2D layer1 = content.Load<Texture2D>(@"Textures/Level1/Level1Layer1");
            Texture2D layer2 = content.Load<Texture2D>(@"Textures/Level1/Level1Layer2");
            Texture2D layer3 = content.Load<Texture2D>(@"Textures/Level1/Level1Layer3");
            Texture2D layer4 = content.Load<Texture2D>(@"Textures/Level1/Level1Layer4");
            Texture2D layer5 = content.Load<Texture2D>(@"Textures/Level1/Level1Layer5");

            List<ParallaxElement> backgrounds = new List<ParallaxElement>()
            {
                new ParallaxElement(layer1, Vector2.Zero, playArea, new Vector2(1f / 5f, 1f), Color.White),
                new ParallaxElement(layer2, Vector2.Zero, playArea, new Vector2(1f / 4f, 1f), Color.White),
                new ParallaxElement(layer3, Vector2.Zero, playArea, new Vector2(1f / 3f, 1f), Color.White),
                new ParallaxElement(layer4, Vector2.Zero, playArea, new Vector2(1f / 2f, 1f), Color.White),
                new ParallaxElement(layer5, Vector2.Zero, playArea, new Vector2(1f / 1.5f, 1f), Color.White),
            };


            Texture2D boxTexture = content.Load<Texture2D>(@"Textures/Box");
            Texture2D barrelTexture = content.Load<Texture2D>(@"Textures/Barrel");

            Texture2D groundTexture = content.Load<Texture2D>(@"Textures/Ground");
            TerrainElement ground = new TerrainElement(new CollidableObject(groundTexture, new Vector2(playArea.Left, playArea.Bottom - groundTexture.Height), new Rectangle(0, 0, playArea.Width, groundTexture.Height), 0.0f));
            int groundStartHeight = playArea.Bottom - groundTexture.Height;

            List<TerrainElement> terrain = new List<TerrainElement>()
            {
                new TerrainElement(new CollidableObject(boxTexture, new Vector2(80, groundStartHeight - boxTexture.Height + 8)))
            };

            List<Enemy> enemies = new List<Enemy>()
            {
                new Pesant(new Vector2(300, groundStartHeight))
            };

            Vector2 playerStartPosition = new Vector2(100, groundStartHeight - 100);

            _levels.Add(new Level(backgrounds, playerStartPosition, terrain, ground, enemies, playArea));
        } // TODO

        private static void LoadLevel2(ContentManager content)
        {
            Rectangle playArea = new Rectangle(0, 0, 3000, 720);

            // Parallax background textures
            Texture2D layer1 = content.Load<Texture2D>(@"Textures/Level2/Level2Layer1");
            Texture2D layer2 = content.Load<Texture2D>(@"Textures/Level2/Level2Layer2");
            Texture2D layer3 = content.Load<Texture2D>(@"Textures/Level2/Level2Layer3");
            Texture2D layer4 = content.Load<Texture2D>(@"Textures/Level2/Level2Layer4");
            Texture2D layer5 = content.Load<Texture2D>(@"Textures/Level2/Level2Layer5");

            List<ParallaxElement> backgrounds = new List<ParallaxElement>()
            {
                new ParallaxElement(layer1, Vector2.Zero, playArea, Vector2.One / 5f, Color.White),
                new ParallaxElement(layer2, Vector2.Zero, playArea, Vector2.One / 4f, Color.White),
                new ParallaxElement(layer3, Vector2.Zero, playArea, Vector2.One / 3f, Color.White),
                new ParallaxElement(layer4, Vector2.Zero, playArea, Vector2.One / 2f, Color.White),
                new ParallaxElement(layer5, Vector2.Zero, playArea, Vector2.One / 1.8f, Color.White),
            };

            Vector2 playerStartPosition = new Vector2(playArea.Center.X, 1000);


            List<TerrainElement> terrain = new List<TerrainElement>()
            {
            };

            Texture2D groundTexture = content.Load<Texture2D>(@"Textures/Ground");
            TerrainElement ground = new TerrainElement(new CollidableObject(groundTexture, new Vector2(playArea.Left, playArea.Bottom - groundTexture.Height), new Rectangle(0, 0, playArea.Width, groundTexture.Height), 0.0f));
            int groundStartHeight = playArea.Bottom - groundTexture.Height;

            Texture2D pesantTexture = content.Load<Texture2D>(@"Textures/Character");


            List<Enemy> enemies = new List<Enemy>()
            {
                new Pesant(new Vector2(100, groundStartHeight))
            };

            _levels.Add(new Level(backgrounds, playerStartPosition, terrain, ground, enemies, playArea));
        } // TODO

        private static void LoadLevel3(ContentManager content)
        {
            Rectangle playArea = new Rectangle(0, 0, 3000, 720);

            // Parallax background textures
            Texture2D layer1 = content.Load<Texture2D>(@"Textures/Level3/Layer1");
            Texture2D layer2 = content.Load<Texture2D>(@"Textures/Level3/Layer2");
            Texture2D layer3 = content.Load<Texture2D>(@"Textures/Level3/Layer3");
            Texture2D layer4 = content.Load<Texture2D>(@"Textures/Level3/Layer4");
            Texture2D layer5 = content.Load<Texture2D>(@"Textures/Level3/Layer5");

            List<ParallaxElement> backgrounds = new List<ParallaxElement>()
            {
                new ParallaxElement(layer1, Vector2.Zero, playArea, Vector2.One / 5f, Color.White),
                new ParallaxElement(layer2, Vector2.Zero, playArea, Vector2.One / 4f, Color.White),
                new ParallaxElement(layer3, Vector2.Zero, playArea, Vector2.One / 3f, Color.White),
                new ParallaxElement(layer4, Vector2.Zero, playArea, Vector2.One / 2f, Color.White),
                new ParallaxElement(layer5, Vector2.Zero, playArea, Vector2.One / 1.8f, Color.White),
            };

            Vector2 playerStartPosition = new Vector2(playArea.Center.X, 1000);


            List<TerrainElement> terrain = new List<TerrainElement>()
            {
            };

            Texture2D groundTexture = content.Load<Texture2D>(@"Textures/Ground");
            TerrainElement ground = new TerrainElement(new CollidableObject(groundTexture, new Vector2(playArea.Left, playArea.Bottom - groundTexture.Height), new Rectangle(0, 0, playArea.Width, groundTexture.Height), 0.0f));
            int groundStartHeight = playArea.Bottom - groundTexture.Height;

            Texture2D pesantTexture = content.Load<Texture2D>(@"Textures/Character");


            List<Enemy> enemies = new List<Enemy>()
            {
                new Pesant(new Vector2(100, groundStartHeight))
            };

            _levels.Add(new Level(backgrounds, playerStartPosition, terrain, ground, enemies, playArea));
        } // TODO



        /// <summary>
        /// Loads level data into InGame
        /// </summary>
        /// <param name="levelNumber"></param>
        public static void StartLevel(int levelNumber)
        {
            // Stop music
            MediaPlayer.Stop();
            _levels[levelNumber].StartLevel(ref _playArea, ref TerrainElements, ref Ground, ref Enemies, ref _parallaxBackgrounds, ref _player.CollidableObject.Position);
            _camera.Limits = PlayArea;
            // Start new music
        }

        /// <summary>
        /// Update game logic, such as player, camera...
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime)
        {
            // Update player
            _player.Update(gameTime);
            foreach (TerrainElement terrainElement in TerrainElements)
            {
                _player.Collision(terrainElement.CollidableObject.Rectangle);
            }
            _player.Collision(Ground.CollidableObject.Rectangle);
            // Update Camera
            _camera.LookAt(_player.CollidableObject.Position);
        }



        public static void Draw(SpriteBatch spriteBatch)
        {
            // Draw background
            foreach (ParallaxElement background in _parallaxBackgrounds )
            {
                // Draw each background with a parallax scaled view matrix
                spriteBatch.Begin(SpriteSortMode.BackToFront, null, SamplerState.LinearWrap, null, null, null, _camera.GetViewMatrix(background.Parallax));
                background.Draw(spriteBatch);
                spriteBatch.End();
            }


            // Start drawing foreground
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _camera.GetViewMatrix());
            // Draw terrain
            foreach (TerrainElement terrainElement in TerrainElements)
            {
                terrainElement.Draw(spriteBatch);
            }
            spriteBatch.End();

            // Draw Ground
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null, _camera.GetViewMatrix());
            Ground.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _camera.GetViewMatrix());
            // Draw enemies
            foreach (Enemy enemy in Enemies)
            {
                enemy.Draw(spriteBatch);
            }
            // Draw player
            _player.Draw(spriteBatch);

            spriteBatch.End();


            // Draw UI
            spriteBatch.Begin();
#if DEBUG

            spriteBatch.DrawString(Game1.DebugFont, $"campos:{_camera.GetViewMatrix().Translation}\nplaypos:{_player.CollidableObject.Position}\njumvel:{_player.velocity}\njumtim:{_player.jumpTime}\njumcom:{_player.jumpComplete}\ngrnd:{_player.onGround}", Vector2.Zero, Color.White);
#endif
            spriteBatch.End();
        }
    }
}
