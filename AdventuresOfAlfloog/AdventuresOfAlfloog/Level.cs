using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventuresOfAlfloog
{
    /// <summary>
    /// Contains data for starting a level
    /// </summary>
    class Level
    {
        private List<ParallaxElement> _backgrounds;
        private Vector2 _playerStartPosition;
        private List<TerrainElement> _terrainElements;
        private TerrainElement _ground;
        private List<Enemy> _enemies;
        /// <summary>
        /// Size of level
        /// </summary>
        private Rectangle _playArea;

        /// <summary>
        /// Sets InGame to this level
        /// </summary>
        /// <param name="playArea"></param>
        /// <param name="ground"></param>
        public void StartLevel(ref Rectangle playArea, ref List<TerrainElement> terrainElements, ref TerrainElement ground, ref List<Enemy> enemies, ref List<ParallaxElement> backgrounds, ref Vector2 playerPosition)
        {
            playArea = _playArea;
            terrainElements = _terrainElements;
            ground = _ground;
            enemies = _enemies;
            backgrounds = _backgrounds;
            playerPosition = _playerStartPosition;
        }

        public Level(List<ParallaxElement> backgrounds, Vector2 playerStartPosition, List<TerrainElement> terrainElements, TerrainElement ground, List<Enemy> enemies, Rectangle playArea)
        {
            _backgrounds = backgrounds;
            _playerStartPosition = playerStartPosition;
            _terrainElements = terrainElements;
            _ground = ground;
            _enemies = enemies;
            _playArea = playArea;
        }
    }
}
