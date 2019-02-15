using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdventuresOfAlfloog
{
    public class Pesant : Enemy
    {
        private static Texture2D _texture;
        private static List<Animation> _animations;
        private const float walkSpeed = 0.1f;

        public Pesant(Vector2 position) : base(new CollidableObject(_texture, position), new AnimationSet(_animations, AnimationStates.Idle, AnimationDirections.Right), walkSpeed)
        {

        }

        public static void LoadContent(Texture2D texture, List<Animation> animations)
        {
            _texture = texture;
            _animations = animations;
        }
    }

    public class Knight : Enemy
    {
        private static Texture2D _texture;
        private static List<Animation> _animations;
        private const float walkSpeed = 0.1f;

        public Knight(Vector2 position) : base(new CollidableObject(_texture, position), new AnimationSet(_animations, AnimationStates.Idle, AnimationDirections.Right), walkSpeed)
        {

        }

        public static void LoadContent(Texture2D texture, List<Animation> animations)
        {
            _texture = texture;
            _animations = animations;
        }
    }
}
