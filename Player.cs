using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace GameProject
{
	class Player : Entity
	{
		public static double PLAYER_SPEED = 4.25;
		public static double PLAYER_RADIUS = 10;

		private const int MOVE_UP = 0;
		private const int MOVE_DOWN = 1;
		private const int MOVE_LEFT = 2;
		private const int MOVE_RIGHT = 3;

		private string _spritePath = "ms-appx:///Assets/player0.png";
		
		public Player(GameBoard board) : base(board)
		{
			_sprite.Source = new BitmapImage(new Uri(_spritePath));
			Radius = PLAYER_RADIUS;
			_speed = PLAYER_SPEED;
		}

		// Usually I'd add a function that returns a reference to the parent holding this class instance
		// But since there's no point in doing that in this project, I just pass the canvas reference to the function
		public void Move(bool[] movementArgs)
		{
			double dx = 0;
			double dy = 0;

			if (movementArgs[MOVE_UP] && !movementArgs[MOVE_DOWN])
			{
				dy -= _speed;
			}
			else if (!movementArgs[MOVE_UP] && movementArgs[MOVE_DOWN])
			{
				dy += _speed;
			}

			if (movementArgs[MOVE_LEFT] && !movementArgs[MOVE_RIGHT])
			{
				dx -= _speed;
			}
			else if (!movementArgs[MOVE_LEFT] && movementArgs[MOVE_RIGHT])
			{
				dx += _speed;
			}

			if (dx != 0 && dy != 0)
			{
				double third = Math.Sqrt(dx * dx + dy * dy);
				dx = dx / third * _speed;
				dy = dy / third * _speed;
			}

			TranslatePosition(dx, dy);

			double withinBorderPositionX = Math.Max(0 + _radius, Math.Min(_game.Canvas.Width - _radius, _x));
			double withinBorderPositionY = Math.Max(0 + _radius, Math.Min(_game.Canvas.Height - _radius, _y));
			SetPosition(withinBorderPositionX, withinBorderPositionY);
		}

		public override void Die()
		{
			base.Die();
			_game.SoundHandler.PlaySound("playerDeath");
		}
	}
}
