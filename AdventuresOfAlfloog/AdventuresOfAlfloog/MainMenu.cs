using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AdventuresOfAlfloog
{
    static class Main
    {
        private static Menu _menu;

        public static void LoadContent(ContentManager content, Viewport viewport)
        {
            _menu = new Menu(content.Load<Texture2D>(@"Textures/Menu/MainMenuBackground"),
                new Button[]
                {
                    new Button(new Point(0, 0),
                        new Rectangle(100, 200, 200, 50),
                        content.Load<Texture2D>(@"Textures/Menu/playButton"),
                        content.Load<Texture2D>(@"Textures/Menu/playButtonPressed"),
                        () =>
                        {
                            Game1.GameState = Game1.GameStates.InGame;
                            InGame.StartLevel(0);
                        }),

                    new Button(new Point(1, 0),
                        new Rectangle(400, 200, 200, 50),
                        content.Load<Texture2D>(@"Textures/Menu/highscoresButton"),
                        content.Load<Texture2D>(@"Textures/Menu/highscoresButtonPressed"),
                        () => Game1.GameState = Game1.GameStates.HighScore),

                    new Button(new Point(2, 0),
                        new Rectangle(700, 200, 200, 50),
                        content.Load<Texture2D>(@"Textures/Menu/creditsButton"),
                        content.Load<Texture2D>(@"Textures/Menu/creditsButtonPressed"),
                        () => Game1.GameState = Game1.GameStates.Credits),

                    new Button(new Point(3, 0),
                        new Rectangle(1000, 200, 200, 50),
                        content.Load<Texture2D>(@"Textures/Menu/exitButton"),
                        content.Load<Texture2D>(@"Textures/Menu/exitButtonPressed"),
                        () => Game1.GameState = Game1.GameStates.Exit),
                },
                new MenuControlScheme(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Enter),
                viewport);


        }
         
        public static void Update()
        {
            _menu.Update();
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            _menu.Draw(spriteBatch);
        }
    }
}
