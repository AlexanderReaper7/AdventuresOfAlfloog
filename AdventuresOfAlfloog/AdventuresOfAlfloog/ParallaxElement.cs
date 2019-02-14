using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventuresOfAlfloog
{
    class ParallaxElement // TODO: come up with a new name
    {
        private Texture2D _texture;
        private Color _color;
        private Rectangle _source;

        public Vector2 Parallax { get; set; }
        public Vector2 Position { get; set; }

        public ParallaxElement(Texture2D texture, Vector2 position, Rectangle source, Vector2 parallax, Color color)
        {
            _texture = texture;
            _color = color;
            Parallax = parallax;
            Position = position;
            _source = source;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, _source,  _color);
        }
    }
}
