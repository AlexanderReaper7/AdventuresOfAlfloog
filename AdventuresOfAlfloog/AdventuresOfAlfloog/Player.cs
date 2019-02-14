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
        private float walkSpeed = .2f;
        private CollidableObject _collidableObject;
        private Vector2 _oldPosition;
        private AnimationSet _animationSet;

        private const int maxJumpTime = 90;
        public int jumpTime;
        public bool jumpComplete;
        public bool onGround;
        public float jumpVelocity;
        private const float jumpPower = 0.4f;
        private const float gravity = 0.04f;


        public Player(Texture2D texture, Vector2 spawnPosition, List<Animation> animations)
        {
            _animationSet = new AnimationSet(animations, AnimationStates.Idle, AnimationDirections.Right);

            Rectangle initialSourceRectangle = Rectangle.Empty;
            // Set initialSourceRectangle to the first frame in the first animation
            animations[0].SetToFrame(ref initialSourceRectangle, 0);
            CollidableObject = new CollidableObject(texture, spawnPosition, initialSourceRectangle, 0f);
            _oldPosition = CollidableObject.Position;
        }

        public void Update(GameTime gameTime)
        {
            UpdateMovement(gameTime);
            UpdateActions(gameTime);
            _animationSet.UpdateAnimation(ref CollidableObject.SourceRectangle, ref CollidableObject.Origin, gameTime);
            CollisionToTerrain();

#if DEBUG
            if (Keyboard.GetState().IsKeyDown(Keys.V))
            {
                CollidableObject.Position -= Vector2.One;
                
            }
#endif
        }
        
        private void UpdateMovement(GameTime gameTime)
        {
            // Update keyboard state
            _currentKeyboardState = Keyboard.GetState();
            // Reset displacement
            Vector2 displacement = Vector2.Zero;

            // Get input
            if (_currentKeyboardState.IsKeyDown(Keys.A))
            {
                displacement.X -= walkSpeed * gameTime.ElapsedGameTime.Milliseconds;
                _animationSet.AnimationDirection = AnimationDirections.Left;
            }

            if (_currentKeyboardState.IsKeyDown(Keys.D))
            {
                displacement.X += walkSpeed * gameTime.ElapsedGameTime.Milliseconds;
                _animationSet.AnimationDirection = AnimationDirections.Right;
            }

            displacement += UpdateJump(gameTime);

            // Add displacement to position
            AddToPosition(displacement);

            // if no movement was made set _animationState to Idle and return.
            if (displacement.X == Vector2.Zero.X)
            {
                _animationSet.AnimationState = AnimationStates.Idle;
                return;
            }
            // if something happened set _animationState to Walk
            _animationSet.AnimationState = AnimationStates.Walk;
        }

        private Vector2 UpdateJump(GameTime gameTime)
        {
            Vector2 displacement = Vector2.Zero;

            // If W or Up arrow key is pressed down And jump is not complete TODO: Fix jumping
            if (_currentKeyboardState.IsKeyDown(Keys.W) && !jumpComplete)
            {
                // Continue jump
                // Jump has already started
                if (jumpTime > 0)
                {
                    Jump(gameTime);
                    return displacement;
                }

                // Start jump
                // If jumpTime is reset and is on ground
                if (jumpTime == 0 && onGround)
                {
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

                displacement.Y -= jumpVelocity * gameTime.ElapsedGameTime.Milliseconds;
            if (!onGround)
            {
                jumpVelocity -= gravity * gameTime.ElapsedGameTime.Milliseconds;
            }
            else
            {
                jumpVelocity = 0f;
            }

            return displacement;
        }


        private void Jump(GameTime gameTime)
        {
            // Add elapsed time to timer
            jumpTime += gameTime.ElapsedGameTime.Milliseconds;
            // if timer has not expired
            if (jumpTime < maxJumpTime)
            {
                // set velocity to jump
                jumpVelocity = jumpPower * gameTime.ElapsedGameTime.Milliseconds;
            }
            // Else timer has expired
            else
            {
                // Complete jump
                jumpComplete = true;
            }
        }

        private void CollisionToTerrain()
        {
            foreach (TerrainElement terrainElement in InGame.TerrainElements)
            {
                // If player intersects with the terrain element set player position to closest position to terrain element from old position
                if (CollidableObject.BoundingRectangle.Intersects(terrainElement.CollidableObject.BoundingRectangle))
                {
                    // If its colliding on x axis set X to old Position
                    if (terrainElement.CollidableObject.BoundingRectangle.Contains(new Point((int)CollidableObject.Position.X, (int)_oldPosition.Y)))
                    {
                        CollidableObject.Position.X = _oldPosition.X;
                    }
                    // If its colliding on y axis set Y to old position
                    if (terrainElement.CollidableObject.BoundingRectangle.Contains(new Point((int)_oldPosition.X, (int)CollidableObject.Position.Y)))
                    {
                        CollidableObject.Position.Y = _oldPosition.Y;
                    }
                    onGround = true;
                    return;
                }
            }
            // collision to ground
            if (CollidableObject.BoundingRectangle.Intersects(InGame.Ground.CollidableObject.BoundingRectangle))
            {
                onGround = true;
                // Set position to old position
                CollidableObject.Position = new Vector2(CollidableObject.Position.X, InGame.Ground.CollidableObject.BoundingRectangle.Top - CollidableObject.Origin.Y); ; // TODO: to closest position to terrain
                return;
            }

            onGround = false;
        }

        private static Rectangle? WallCollision(Rectangle rectA, Rectangle oldRectA, Rectangle rectB)
        {
            // Create a rectangle from intersection
            Rectangle intersection = Rectangle.Intersect(rectA, rectB);

            // If no intersection was found return null
            if (intersection == Rectangle.Empty) { return null;}

            Rectangle output = rectA;

            // If A is fully inside B calculate delta direction
            if (intersection == rectA)
            {
                // Calculate entry direction
                Point deltaA = new Point(rectA.Center.X / oldRectA.Center.X, rectA.Center.Y / oldRectA.Center.Y);
                float direction = (float) Math.Atan2(deltaA.Y, deltaA.X);

            }


            Vector2 movePriority = new Vector2((float) intersection.Width / (float) rectA.Width,
                                               (float) intersection.Height / (float) rectA.Height);

            if (movePriority.X > movePriority.Y)
            {
                //output += intersection.Width
            }

            return output;
        }

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
                new Vector2(InGame.PlayArea.Left + CollidableObject.Origin.X, InGame.PlayArea.Top + CollidableObject.Origin.Y),
                new Vector2(InGame.PlayArea.Right - CollidableObject.Origin.X, InGame.PlayArea.Bottom - CollidableObject.Origin.Y));
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

        /// <summary>
        /// Adds gravity to velocity if not onGround
        /// </summary>
        /// <param name="gameTime"></param>
        private void Fall(GameTime gameTime)
        {
            if (!onGround)
            {
                // Add gravity
                jumpVelocity += gravity;
            }
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
        }
    }
}
