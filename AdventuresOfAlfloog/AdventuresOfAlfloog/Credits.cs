using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;

namespace AdventuresOfAlfloog
{
   public static class Credits
   {
       private static Menu _menu;

       private static SpriteFont _creditsFont;
       private static Vector2 _creditsPos1 = new Vector2(50, 725);
       private static Vector2 _creditsPos2 = new Vector2(50, 805);
       private static Vector2 _creditsPos3 = new Vector2(50, 885);
       private static Vector2 _creditsPos4 = new Vector2(50, 965);
       private static Vector2 _creditsPos5 = new Vector2(50, 1045);
       private static Vector2 _creditsPos6 = new Vector2(50, 1125);
       private static Vector2 _creditsPos7 = new Vector2(50, 1205);
       private static Vector2 _creditsPos8 = new Vector2(50, 1285);
       private static Vector2 _creditsPos9 = new Vector2(50, 1265);
       private static Vector2 _creditsPos10 = new Vector2(50, 2045);
       private static Vector2 _creditsPos11 = new Vector2(50, 2045);
       private static Vector2 _creditsPos12 = new Vector2(50, 2045);
       private static Vector2 _creditsPos13 = new Vector2(50, 2045);
       private static string _creditsLine1;
       private static string _creditsLine2;
       private static string _creditsLine3;
       private static string _creditsLine4;
       private static string _creditsLine5;
       private static string _creditsLine6;
       private static string _creditsLine7;
       private static string _creditsLine8;
       private static string _creditsLine9;
       private static string _creditsLine10;
       private static string _creditsLine11;
       private static string _creditsLine12;
       private static string _creditsLine13;
       private static Vector2 _creditsSpeed = new Vector2(0, 100);
       private static Color _color;



        public static void LoadContent(ContentManager content, Viewport viewport)
        {
            
           _creditsFont = content.Load<SpriteFont>(@"Fonts/Debug");

            _menu = new Menu(content.Load<Texture2D>(@"Textures/Menu/MainMenuBackground"),
                new Button[]
                {
                    // Back button
                    new Button(new Point(0,0),
                        new Rectangle(1000, 600, 200, 50), 
                        content.Load<Texture2D>(@"Textures/Menu/backButton"),
                        content.Load<Texture2D>(@"Textures/Menu/backButtonPressed"),
                        () => Game1.GameState = Game1.GameStates.MainMenu),
                },
                new MenuControlScheme(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Enter),
                viewport);
                    
                    

            _creditsLine1 = "Producer";
            _creditsLine2 = "Albin Forsell";
            _creditsLine3 = "Artist Director";
            _creditsLine4 = "Leia Nordenmark";
            _creditsLine5 = "Artists";
            _creditsLine6 = "Robin Gunnarsson";
            _creditsLine7 = "Hjalmar Nilsson";
            _creditsLine8 = "Emil Carlsson Andersson";
            _creditsLine9 = "Dino Tomic";
            _creditsLine10 = "Lead Programmer";
            _creditsLine11 = "Alexander Oberg";
            _creditsLine12 = "Programmers";
            _creditsLine13 = "Andreas Augustsson";

        }
        
        public static void Update(GameTime gameTime)
        {
            _menu.Update();
            AnimateText(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            _menu.Draw(spriteBatch);
            spriteBatch.Begin();
            spriteBatch.DrawString(_creditsFont, _creditsLine1, _creditsPos1, Color.White);
            
                spriteBatch.DrawString(_creditsFont, _creditsLine2, _creditsPos2, Color.Black);
                
                spriteBatch.DrawString(_creditsFont, _creditsLine3, _creditsPos3, Color.Black);
            
                spriteBatch.DrawString(_creditsFont, _creditsLine4, _creditsPos4, Color.Black);
            
                spriteBatch.DrawString(_creditsFont, _creditsLine5, _creditsPos5, Color.Black);
            
                spriteBatch.DrawString(_creditsFont, _creditsLine6, _creditsPos6, Color.Black);
            
                spriteBatch.DrawString(_creditsFont, _creditsLine7, _creditsPos7, Color.Black);
            
                spriteBatch.DrawString(_creditsFont, _creditsLine8, _creditsPos8, Color.Black);
            
                spriteBatch.DrawString(_creditsFont, _creditsLine9, _creditsPos9, Color.Black);
            
                spriteBatch.DrawString(_creditsFont, _creditsLine10, _creditsPos10, Color.Black);
            spriteBatch.DrawString(_creditsFont, _creditsLine11, _creditsPos11, Color.Black);
            spriteBatch.DrawString(_creditsFont, _creditsLine12, _creditsPos12, Color.Black);
            spriteBatch.DrawString(_creditsFont, _creditsLine13, _creditsPos13, Color.Black);

            spriteBatch.End();
        }

        private static void AnimateText(GameTime gameTime)
        {
            // Moves the text up
            _creditsPos1 += _creditsSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _creditsPos2 += _creditsSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _creditsPos3 += _creditsSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _creditsPos4 += _creditsSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _creditsPos5 += _creditsSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _creditsPos6 += _creditsSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _creditsPos7 += _creditsSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _creditsPos8 += _creditsSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _creditsPos9 += _creditsSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _creditsPos10 += _creditsSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _creditsPos11 += _creditsSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _creditsPos12 += _creditsSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
