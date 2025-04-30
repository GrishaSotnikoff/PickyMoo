// Source/System/ScreenFadeSystem.cs
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PickyMoo.ESC;

namespace PickyMoo.Source.System
{
    public class ScreenFadeSystem : ISystem
    {
        private readonly SpriteBatch _batch;
        private readonly Texture2D _pixel;

        public float Opacity { get; private set; } = 0f;
        private float _fadeSpeed = 2f;
        private bool _fadingIn = false;
        private bool _fadingOut = false;
        private bool _triggeredSwitch = false;
        private string _nextLocation = null;
        private readonly LocationManager _locationManager;

        public ScreenFadeSystem(SpriteBatch batch, Texture2D pixel, LocationManager locationManager)
        {
            _batch = batch;
            _pixel = pixel;
            _locationManager = locationManager;
        }

        public void FadeTo(string nextLocation)
        {
            if (_fadingOut || _fadingIn) return;
            _fadingOut = true;
            _nextLocation = nextLocation;
        }

        public void Update(List<IComponent> comps, GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_fadingOut)
            {
                Opacity += _fadeSpeed * delta;
                if (Opacity >= 1f)
                {
                    Opacity = 1f;
                    _fadingOut = false;
                    _triggeredSwitch = true;
                }
            }
            else if (_triggeredSwitch)
            {
                _locationManager.SwitchTo(_nextLocation);
                _triggeredSwitch = false;
                _fadingIn = true;
            }
            else if (_fadingIn)
            {
                Opacity -= _fadeSpeed * delta;
                if (Opacity <= 0f)
                {
                    Opacity = 0f;
                    _fadingIn = false;
                }
            }

            if (Opacity > 0f)
            {
                _batch.Begin();
                _batch.Draw(_pixel, new Rectangle(0, 0, 9999, 9999), new Color((byte)0, (byte)0, (byte)0, (byte)(Opacity * 255)));
                _batch.End();
            }
        }
    }
}