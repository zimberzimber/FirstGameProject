using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace GameProject
{
	class Entity
	{
		protected static string SPRITE_PATH = "ms-appx:///Assets/dead";
		protected static string SPRITE_DEAD_PATH = "ms-appx:///Assets/dead";
		protected static int FRAME_COUNT = 1;

		protected double _x, _y, _radius, _speed;
		protected bool _isAlive = true;
		protected GameBoard _game;

		protected Image _sprite;
		protected int _frame;

		public double X { get { return _x; } set { _x = value; } }
		public double Y { get { return _y; } set { _y = value; } }
		public double Speed { get { return _speed; } set { _speed = value; } }
		public double Radius
		{
			get { return _radius; }
			set
			{
				_radius = value;
				_sprite.Width = value * 2;
				_sprite.Height = value * 2;
			}
		}

		public bool IsAlive { get { return _isAlive; } set { _isAlive = value; } }
		public GameBoard Board { get { return _game; } }
		
		public Image Sprite { get { return _sprite; } }
		public int Frame { get { return _frame; } set { _frame = value; } }

		public Entity(GameBoard board)
		{
			_game = board;
			_sprite = new Image();
			SetPosition(0, 0);
		}

		public virtual void UpdateFrame() { }

		public void SetPosition(double x, double y)
		{
			_x = Math.Max(0 + _radius, Math.Min(x, _game.Canvas.Width - _radius));
			_y = Math.Max(0 + _radius, Math.Min(y, _game.Canvas.Height - _radius));

			Canvas.SetLeft(_sprite, _x - _radius);
			Canvas.SetTop(_sprite, _y - _radius);
		}

		public void TranslatePosition(double x, double y)
		{
			SetPosition(_x + (x * _game.GameSpeedMultiplier), _y + (y * _game.GameSpeedMultiplier));
		}

		public virtual void Die()
		{
			_isAlive = false;
			_sprite.Source = new BitmapImage(new Uri(SPRITE_DEAD_PATH + "0.png"));
			_sprite.Opacity = 0.5;
		}
	}
}
