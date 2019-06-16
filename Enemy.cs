using System;
using Windows.UI.Xaml.Media.Imaging;

namespace GameProject
{
	class Enemy : Entity
	{
		 protected static string SPRITE_PATH = "ms-appx:///Assets/enemy";
		protected static int FRAME_COUNT = 1;

		protected BitmapImage[] _spriteCache;

		// Planned on having another enemy type, but ended up not making it

		public Enemy(GameBoard board) : base(board)
		{
			BuildSpriteCache(FRAME_COUNT, SPRITE_PATH);
		}

		public virtual void Think()
		{
			double distanceToPlayer = GameBoard.GetDistance(_x, _y, _game.Player.X, _game.Player.Y);
			MoveTowards(_game.Player.X, _game.Player.Y, distanceToPlayer);
		}

		public void MoveTowards(double targetX, double targetY, double maxDistance)
		{
			double distance = (maxDistance < _speed) ? maxDistance : _speed;

			double dirX = targetX - _x;
			double dirY = targetY - _y;
			double third = Math.Sqrt(dirX * dirX + dirY * dirY);
			dirX = dirX / third * distance;
			dirY = dirY / third * distance;

			TranslatePosition(dirX, dirY);
		}

		public override void UpdateFrame()
		{
			if (FRAME_COUNT > 1)
			{
				_frame = (_frame + 1) % FRAME_COUNT;
				_sprite.Source = _spriteCache[_frame];
			}
		}

		public override void Die()
		{
			base.Die();
			_game.SoundHandler.PlaySound("enemyDeath");
		}
		
		protected void BuildSpriteCache(int frames, string sprite)
		{
			// Cache sprites - Prevents flickering and re-generating the image through Uri. processing vs memory...
			_spriteCache = new BitmapImage[frames];
			for (int i = 0; i < frames; i++)
				_spriteCache[i] = new BitmapImage(new Uri(sprite + i + ".png"));

			_sprite.Source = _spriteCache[0];
		}
	}
}
