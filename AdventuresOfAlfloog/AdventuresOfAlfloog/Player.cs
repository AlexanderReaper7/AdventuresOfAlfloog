using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AdventuresOfAlfloog
{

    public class Player
    {
        private static KeyboardState _currentKeyboardState;

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

        /// <summary>
        /// movement speed in pixels per millisecond
        /// </summary>dd
        private float walkSpeed = .3f;
        private CollidableObject _collidableObject;
        private Vector2 _oldPosition;
        public Vector2 velocity;
        private AnimationSet _animationSet;

        private const int maxJumpTime = 110;
        public int jumpTime;
        public bool jumpComplete;
        public bool onGround;
        private const float jumpPower = 0.08f;
        private const float gravity = 0.006f;


        public Player(Texture2D texture, Vector2 spawnPosition, List<Animation> animations)
        {
            _animationSet = new AnimationSet(animations, AnimationStates.Idle, AnimationDirections.Right);

            Rectangle initialSourceRectangle = Rectangle.Empty;
            // Set initialSourceRectangle to the first frame in the first animation
            animations[0].SetToFrame(ref initialSourceRectangle, 0);
            CollidableObject = new CollidableObject(texture, spawnPosition, initialSourceRectangle, 0f) {Origin = Vector2.Zero};
            _oldPosition = CollidableObject.Position;
        }

        public void Update(GameTime gameTime)
        {
            UpdateMovement(gameTime);
            UpdateActions(gameTime);
            _animationSet.UpdateAnimation(ref CollidableObject.SourceRectangle, gameTime);

#if DEBUG 
            if (Keyboard.GetState().IsKeyDown(Keys.V))
            {
                CollidableObject.Position -= Vector2.One;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.B))
            {
                CollidableObject.Position += Vector2.One- Vector2.UnitY;
            }
#endif
        }
        
        private void UpdateMovement(GameTime gameTime)
        {
            // Add displacement to position
            AddToPosition(velocity);

            // Update keyboard state
            _currentKeyboardState = Keyboard.GetState();

            Vector2 newVelocity = Vector2.Zero;

            // Get input
            if (_currentKeyboardState.IsKeyDown(Keys.A)) // Left
            {
                newVelocity.X += -walkSpeed * gameTime.ElapsedGameTime.Milliseconds;
                _animationSet.AnimationDirection = AnimationDirections.Left;
                _animationSet.AnimationState = AnimationStates.Walk;
            }
            if (_currentKeyboardState.IsKeyDown(Keys.D)) // Right
            {
                newVelocity.X += walkSpeed * gameTime.ElapsedGameTime.Milliseconds;
                _animationSet.AnimationDirection = AnimationDirections.Right;
                _animationSet.AnimationState = AnimationStates.Walk;
            }
            if ((_currentKeyboardState.IsKeyUp(Keys.A) && _currentKeyboardState.IsKeyUp(Keys.D)) || (_currentKeyboardState.IsKeyDown(Keys.A) && _currentKeyboardState.IsKeyDown(Keys.D))) // None
            {
                newVelocity.Y = 0.0f;
                _animationSet.AnimationState = AnimationStates.Idle;
            }

            velocity.X = newVelocity.X;
            velocity.Y = MathHelper.Min(velocity.Y, 2f);

            UpdateJump(gameTime);
        }

        private void UpdateJump(GameTime gameTime)
        {
            // If W or Up arrow key is pressed down And jump is not complete
            if (_currentKeyboardState.IsKeyDown(Keys.W) && !jumpComplete)
            {
                // Continue jump
                // Jump has already started
                if (jumpTime > 0)
                {
                    Jump(gameTime);
                    return;
                }

                // Start jump
                // If jumpTime is reset and is on ground
                if (jumpTime == 0 && onGround)
                {
                    CollidableObject.Position.Y -= 10;
                    Jump(gameTime);
                }
                else
                {
                    Fall(gameTime);
                }
            }
            else
            {
                // key was released, therefore set jump to complete
                if (jumpTime > 0)
                {
                    jumpComplete = true;
                }

                // if keys are up and player is on ground
                if (_currentKeyboardState.IsKeyUp(Keys.W) && onGround)
                {
                    // Reset jump
                    jumpTime = 0;
                    jumpComplete = false;
                }
                // Fall
                Fall(gameTime);
            }
        }


        private void Jump(GameTime gameTime)
        {
            // Add elapsed time to timer
            jumpTime += gameTime.ElapsedGameTime.Milliseconds;
            // if timer has not expired
            if (jumpTime < maxJumpTime)
            {
                // set velocity to jump
                velocity.Y = -jumpPower * gameTime.ElapsedGameTime.Milliseconds;
            }
            // Else timer has expired
            else
            {
                // Complete jump
                jumpComplete = true;
            }
        }

        /// <summary>
        /// Adds gravity to velocity if not onGround
        /// </summary>
        /// <param name="gameTime"></param>
        private void Fall(GameTime gameTime)
        {
            if (!onGround)
            {
                // Add gravity
                velocity.Y += gravity;
            }
        }

        public void Collision(Rectangle otherRectangle)
        {
            if (CollidableObject.Rectangle.TouchTopof(otherRectangle))
            {
                CollidableObject.Position.Y = otherRectangle.Y - CollidableObject.Rectangle.Height;
                velocity.Y = 0f;
                onGround = true;
            }
            else
            {
                onGround = false;
            }
            if (CollidableObject.Rectangle.TouchLeftOf(otherRectangle))
            {
                CollidableObject.Position.X = otherRectangle.X - CollidableObject.Rectangle.Width - 2;
            }

            if (CollidableObject.Rectangle.TouchRightOf(otherRectangle))
            {
                CollidableObject.Position.X = otherRectangle.X + otherRectangle.Width + 2;
            }

            if (CollidableObject.Rectangle.TouchBottomOf(otherRectangle))
            {
                velocity.Y = gravity;
            }
        }

        //private void CollisionToTerrain()
        //{
        //    // Ground collision
        //    // Create a new rectangle 
        //    Rectangle groundDetectionRectangle = CollidableObject.BoundingRectangle;
        //    groundDetectionRectangle.Y = CollidableObject.BoundingRectangle.Bottom ;
        //    groundDetectionRectangle.Height = 2;


        //    foreach (TerrainElement terrainElement in InGame.TerrainElements)
        //    {
        //        // Y axis aligns
        //        if (groundDetectionRectangle.Bottom == terrainElement.CollidableObject.BoundingRectangle.Top)
        //        {
        //            // X axis aligns
        //            if (groundDetectionRectangle.Left < terrainElement.CollidableObject.BoundingRectangle.Right &&
        //                groundDetectionRectangle.Right > terrainElement.CollidableObject.BoundingRectangle.Left)
        //            {
        //                onGround = true;
        //                velocity.Y = MathHelper.Max(velocity.Y, 0.0f);
        //            }
        //        }

        //        // Get new rectangle
        //        Rectangle? newRect = WallCollision(CollidableObject.BoundingRectangle,
        //            new Rectangle((int) _oldPosition.X, (int) _oldPosition.Y, CollidableObject.BoundingRectangle.Width, CollidableObject.BoundingRectangle.Height),
        //            velocity,
        //            CollidableObject.Origin,
        //            terrainElement.CollidableObject.BoundingRectangle);
        //        // If collision 
        //        if (newRect != null)
        //        {
        //            CollidableObject.Position = new Vector2(newRect.Value.X, newRect.Value.Y);
        //            velocity.Y = 0.0f;
        //            return;
        //        }
        //    }

        //    // collision to ground
        //    Rectangle? groundRect = WallCollision(CollidableObject.BoundingRectangle,
        //        new Rectangle((int)_oldPosition.X, (int)_oldPosition.Y, CollidableObject.BoundingRectangle.Width, CollidableObject.BoundingRectangle.Height),
        //        velocity,
        //        CollidableObject.Origin,
        //        InGame.Ground.CollidableObject.BoundingRectangle);

        //    // collision detected
        //    if (groundRect != null)
        //    {
        //        CollidableObject.Position.Y = groundRect.Value.Y;
        //        onGround = true;
        //        velocity.Y = 0;
        //        return;
        //    }

        //    // No collision to terrain
        //    onGround = false;
        //}

        //private static Rectangle? WallCollision(Rectangle rectA, Rectangle oldRectA, Vector2 velocityA, Vector2 originA, Rectangle rectB)
        //{
        //    // Create a rectangle from intersection
        //    Rectangle intersection = Rectangle.Intersect(rectA, rectB);

        //    // If intersection is empty
        //    if (intersection == Rectangle.Empty)
        //    {
        //        // Then no intersection was found
        //        return null;
        //    }


        //    // Calculate entry direction

        //    //float timeXCollision = (rectA.Left - rectB.Right) / -velocityA.X;
        //    //float timeYCollision = (rectB.Bottom - rectA.Top) / velocityA.Y;

        //    Point deltaA = new Point(rectA.Center.X - oldRectA.Center.X, rectA.Center.Y - oldRectA.Center.Y);
        //    float direction = (float) Math.Atan2(deltaA.Y, deltaA.X);


        //    bool top = false, right = false, bottom = false, left = false;
        //    if (direction >= 0 && direction < MathHelper.PiOver2)
        //    {
        //        top = true;
        //        right = true;
        //    } // Direction is top right

        //    if (direction >= MathHelper.PiOver2 && direction < MathHelper.Pi)
        //    {
        //        top = true;
        //        left = true;
        //    } // Direction is top left

        //    if (direction >= -MathHelper.Pi && direction < -MathHelper.PiOver2)
        //    {
        //        bottom = true;
        //        left = true;
        //    } // Direction is bottom left

        //    if (direction >= -MathHelper.PiOver2 && direction < 0)
        //    {
        //        bottom = true;
        //        right = true;
        //    } // Direction is bottom right


        //    // move the one which requires least displacement
        //    Vector2 movePriority = new Vector2((float) intersection.Width - (float) rectA.Width,
        //                                       (float) intersection.Height - (float) rectA.Height);
        //    Rectangle output = rectA;

        //    //if (movePriority.X < movePriority.Y) // prioritize Y if x is smaller than y
        //    //{
        //        if (top) output.Y -= intersection.Height - (int)originA.Y;
        //        if (bottom) output.Y += intersection.Height - (int)originA.Y;
        //    //}
        //    //else
        //    //{
        //        if (left) output.X -= intersection.Width - (int)originA.X;
        //        if (right) output.X += intersection.Width - (int)originA.X;
        //    //}


        //    return output;
        //}

        /// <summary>
        /// adds displacement to position while clamping to PlayArea
        /// </summary>
        private void AddToPosition(Vector2 displacement)
        {
            SetPosition(CollidableObject.Position + displacement);
        }

        private void SetPosition(Vector2 position)
        {
            // Update old position
            _oldPosition = CollidableObject.Position;
            CollidableObject.Position = Vector2.Clamp(
                position,
                new Vector2(InGame.PlayArea.Left - CollidableObject.Origin.X, InGame.PlayArea.Top - CollidableObject.Origin.Y),
                new Vector2(InGame.PlayArea.Right - CollidableObject.Texture.Width + CollidableObject.Origin.X, InGame.PlayArea.Bottom - CollidableObject.Texture.Height + CollidableObject.Origin.Y));
        }

        private void UpdateActions(GameTime gameTime)
        {

            if (_currentKeyboardState.IsKeyDown(Keys.Space))
            {
                Attack();
                // Overwrite the previous animation state (Walk) with Action1
                _animationSet.AnimationState = AnimationStates.Attack;
                // Return so the player does not make any other move
                return;
            }
            // If no action and no movement was made AnimationState will remain Idle
        }


        private void Attack()
        {

        }

        /// <summary>
        /// Draw Player
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CollidableObject.Texture,
                CollidableObject.Position,
                CollidableObject.SourceRectangle,
                Color.White,
                CollidableObject.Rotation,
                CollidableObject.Origin,
                1,
                SpriteEffects.None,
                0);

            spriteBatch.Draw(CollidableObject.Texture, CollidableObject.Rectangle, Color.Black);
        }
    }
}
