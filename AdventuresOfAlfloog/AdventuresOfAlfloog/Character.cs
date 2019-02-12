using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresOfAlfloog
{
    public abstract class Character
    {
        protected CollidableObject _collidableObject;

        protected AnimationSet _animationSet;

        public CollidableObject CollidableObject
        {
            get { return _collidableObject; }
            set { _collidableObject = value; }
        }

        public AnimationSet AnimationSet => _animationSet;

        protected Character(CollidableObject collidableObject, AnimationSet animationSet)
        {
            _collidableObject = collidableObject;
            _animationSet = animationSet;
        }

    }
}
