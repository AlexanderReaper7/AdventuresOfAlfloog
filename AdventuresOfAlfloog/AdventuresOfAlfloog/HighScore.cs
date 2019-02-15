using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework.Input;

namespace AdventuresOfAlfloog
{
    [Serializable]
    public struct HighScoreEntry
    {
        public string name;
        public int score;

        public HighScoreEntry(string name, int score)
        {
            this.name = name;
            this.score = score;
        }

        public static HighScoreEntry GenerateDefaultHighScoreEntry()
        {
            return new HighScoreEntry("Default", 0);
        }
    }

    [Serializable]
    public struct SaveData
    {
        public List<HighScoreEntry> ScoreEntries;

        public SaveData(List<HighScoreEntry> scoreEntries)
        {
            ScoreEntries = scoreEntries;
        }

        public static SaveData GenerateDefaultSaveData()
        {
            return new SaveData(
                new List<HighScoreEntry>(5) {HighScoreEntry.GenerateDefaultHighScoreEntry()});
        }
    }

    
    public static class HighScore
    {
        private enum letters
        {
            A, B, C,
            D, E, F,
            G, H, I,
            J, K, L,
            M, N, O,
            P, Q, R,
            S, T, U,
            V, W, X,
            Y, Z, Å,
            Ä, Ö,
        }

        private static int letter1;
        private static int letter2;
        private static int letter3;
        public static int Letters1 { get { return letter1; } set { letter1 = value % 29; } }
        public static int Letters2 { get { return letter2; } set { letter2 = value % 29; } }
        public static int Letters3 { get { return letter3; } set { letter3 = value % 29; } }

        private static Menu _menu;
        private static Viewport _viewport;
        private static SpriteFont _scoreFont;

        private const string FileName = "save.dat";

        private static SaveData currentData;

        public static void Initilize()
        {
            if (!File.Exists(FileName))
            {
                SaveData data = SaveData.GenerateDefaultSaveData();
                currentData = data;
                DoSave(data, FileName);
            }
            else
            {
                currentData = LoadData(FileName);
            }
        }

        public static void LoadContent(ContentManager content, Viewport viewport)
        {
            Texture2D buttonRelease = content.Load<Texture2D>(@"Textures/Menu/backButton");
            Texture2D buttonPressed = content.Load<Texture2D>(@"Textures/Menu/backButtonPressed");

            // Create a new menu
            _menu = new Menu(content.Load<Texture2D>(@"Textures/Menu/MainMenuBackground"),
                new Button[]
                {
                    // Back button
                    new Button(new Point(0,0),
                        new Rectangle(100,100,200,50),
                        content.Load<Texture2D>(@"Textures/Menu/backButton"),
                        content.Load<Texture2D>(@"Textures/Menu/backButtonPressed"),
                        () => Game1.GameState = Game1.GameStates.MainMenu),
                    // Letter 1
                    new Button(new Point(1,0),
                        new Rectangle(150,150,50,50),
                        buttonRelease,
                        buttonPressed,
                        () => Letters1++),
                    new Button(new Point(1,1),
                        new Rectangle(150,150,50,50),
                        buttonRelease,
                        buttonPressed,
                        () => Letters1--),
                    // Letter2
                    new Button(new Point(2,0),
                        new Rectangle(150,150,50,50),
                        buttonRelease,
                        buttonPressed,
                        () => Letters2++),
                    new Button(new Point(2,1),
                        new Rectangle(150,150,50,50),
                        buttonRelease,
                        buttonPressed,
                        () => Letters2--),
                    // Letter3
                    new Button(new Point(3,0),
                        new Rectangle(150,150,50,50),
                        buttonRelease,
                        buttonPressed,
                        () => Letters3++),
                    new Button(new Point(3,1),
                        new Rectangle(150,150,50,50),
                        buttonRelease,
                        buttonPressed,
                        () => Letters3--),
                },
                new MenuControlScheme(Keys.W, Keys.S, Keys.A, Keys.D, Keys.Enter),
                viewport);
            _viewport = viewport;
            _scoreFont = content.Load<SpriteFont>(@"Fonts/Debug"); // TODO: Add high score font
        }

        public static void Update()
        {
            _menu.Update();
        }

        /// <summary>
        /// Opens file and saves data
        /// </summary>
        /// <param name="data">data to write</param>
        /// <param name="filename">name of file to write to</param>
        private static void DoSave(SaveData data, string filename)
        {
            // Open or create file
            FileStream stream = File.Open(filename, FileMode.OpenOrCreate);
            try
            {
                // Make to XML and try to open filestream
                XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                // Close file
                stream.Close();
            }
        }

        private static SaveData LoadData(string fileName)
        {
            SaveData data;

            FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
                data = (SaveData)serializer.Deserialize(stream);
            }
            finally
            {
                stream.Close();
            }

            return data;
        }

        public static void SaveHighScore(HighScoreEntry newScore)
        {
            // If there are already 10 scores
            if (currentData.ScoreEntries.Count >= 5)
            {
                // And newScore is smaller than the smallest score
                if (newScore.score < currentData.ScoreEntries[4].score)
                {
                    // Do not add score
                    return;
                }
            }
            // Add newScore
            currentData.ScoreEntries.Add(newScore);
            // sort scores
            for (int write = 0; write < currentData.ScoreEntries.Count; write++)
            {
                for (int sort = 0; sort < currentData.ScoreEntries.Count - 1; sort++)
                {
                    if (currentData.ScoreEntries[sort].score > currentData.ScoreEntries[sort + 1].score)
                    {
                        HighScoreEntry temp = currentData.ScoreEntries[sort + 1];
                        currentData.ScoreEntries[sort + 1] = currentData.ScoreEntries[sort];
                        currentData.ScoreEntries[sort] = temp;
                    }
                }
            }

            currentData.ScoreEntries.Reverse();
            DoSave(currentData, FileName);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            _menu.Draw(spriteBatch);

            spriteBatch.Begin();

            spriteBatch.DrawString(_scoreFont, "High Scores", new Vector2(_viewport.Width / 2f, _viewport.Height / 10f), Color.Black);

            // Draw score
            for (int i = 0; i < currentData.ScoreEntries.Count; i++)
            {
                // Draw Name
                spriteBatch.DrawString(_scoreFont, currentData.ScoreEntries[i].name, new Vector2(_viewport.Width / 2f - 100, i * 60 + 180), Color.Black);
                // Draw score
                spriteBatch.DrawString(_scoreFont, currentData.ScoreEntries[i].score.ToString(), new Vector2(_viewport.Width / 2f + 100, i * 60 + 180), Color.Black);
            }
            spriteBatch.End();
        }
    }
}

