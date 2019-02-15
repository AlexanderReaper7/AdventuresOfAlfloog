using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AdventuresOfAlfloog
{
    public abstract class Enemy
    {
        public CollidableObject CollidableObject
        {
            get { return _collidableObject; }
            set { _collidableObject = value; }
        }

        public AnimationSet AnimationSet
        {
            get { return _animationSet; }
            set { _animationSet = value; }
        }

        private CollidableObject _collidableObject;
        private AnimationSet _animationSet;

        /// <summary>
        /// movement speed in pixels per millisecond
        /// </summary>
        private float _walkSpeed;

        public Enemy(CollidableObject collidableObject, AnimationSet animationSet, float walkSpeed)
        {
            CollidableObject = collidableObject;
            AnimationSet = animationSet;
            //animations[0].SetToFrame(ref CollidableObject.SourceRectangle, 0);
            _walkSpeed = walkSpeed;
        }


        public virtual void Update(GameTime gameTime)
        {
            MovementAi(gameTime);
        }

        private void MovementAi(GameTime gameTime)
        {
            // Reset displacement
            Vector2 displacement = Vector2.Zero;

            

            AddToPosition(displacement);
        }

        /// <summary>
        /// adds velocity to position while clamping to PlayArea
        /// </summary>
        private void AddToPosition(Vector2 valueToAdd)
        {
            CollidableObject.Position = Vector2.Clamp(
                CollidableObject.Position + valueToAdd,
                new Vector2(InGame.PlayArea.Left, InGame.PlayArea.Top),
                new Vector2(InGame.PlayArea.Right, InGame.PlayArea.Bottom));
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw Enemy
            spriteBatch.Draw(CollidableObject.Texture,
                CollidableObject.Position,
                CollidableObject.SourceRectangle,
                Color.White,
                CollidableObject.Rotation,
                CollidableObject.Origin,
                1,
                SpriteEffects.None,
                0);

        }
    }
}
