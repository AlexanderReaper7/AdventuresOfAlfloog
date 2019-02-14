using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventuresOfAlfloog
{
    /// <summary>
    /// A foreground element that casts shadows and are collisional
    /// </summary>
    public class TerrainElement
    {
        public CollidableObject CollidableObject { get; set; }

        public TerrainElement(CollidableObject collidableObject)
        {
            CollidableObject = collidableObject;
            CollidableObject.Origin = Vector2.Zero;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CollidableObject.Texture, CollidableObject.BoundingRectangle, CollidableObject.SourceRectangle, Color.White, CollidableObject.Rotation, CollidableObject.Origin, SpriteEffects.None, 0.0f);
        }
    }
}
