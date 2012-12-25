using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Gravitation.Screens
{
    class BaseScreen
    {
        protected Matrix _view;
        protected int screenHeight;
        protected int screenWidth;

        private Vector2 _cameraPosition;
        private Vector2 _screenCenter;
        private Vector3 _cameraZoom;



        public BaseScreen(int screenHeight, int screenWidth)
        {
            _cameraZoom = new Vector3(1f, 1f, 1f);

            _view = Matrix.Identity *
                        Matrix.CreateScale(_cameraZoom);
            _cameraPosition = Vector2.Zero;
            _screenCenter = new Vector2(screenWidth / 2f,
                                                screenHeight / 2f);

            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
        }

        public virtual void windowCloseing()
        {

        }

        protected void fire<A>(EventHandler<A> evnt, A args) where A : EventArgs
        {
            if (evnt != null)
                evnt.Invoke(this, args);
        }
    }
}
