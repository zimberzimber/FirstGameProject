using System;

namespace GameProject
{
	class EnemyFollower : Enemy
	{
		public static double[] RADIUS_RANGE = new double[2] { 5, 10 };
		public static double[] SPEED_RANGE = new double[2] { 3, 4 };

		protected static string SPRITE_PATH = "ms-appx:///Assets/enemy";
		protected static int FRAME_COUNT = 8;

		public EnemyFollower(GameBoard board) : base(board)
		{
			BuildSpriteCache(FRAME_COUNT, SPRITE_PATH);
			
			Random rnd = new Random();
			double radius = rnd.Next((int)(RADIUS_RANGE[0] * 100), (int)(RADIUS_RANGE[1] * 100));
			double speed = rnd.Next((int)(SPEED_RANGE[0] * 100), (int)(SPEED_RANGE[1] * 100));

			Radius = radius *= 0.01;
			Speed = speed *= 0.01;
		}

		public EnemyFollower(GameBoard board, Random rnd) : base(board)
		{
			BuildSpriteCache(FRAME_COUNT, SPRITE_PATH);

			double radius = rnd.Next((int)(RADIUS_RANGE[0] * 100), (int)(RADIUS_RANGE[1] * 100));
			double speed = rnd.Next((int)(SPEED_RANGE[0] * 100), (int)(SPEED_RANGE[1] * 100));

			Radius = radius *= 0.01;
			Speed = speed *= 0.01;
		}

		public override void UpdateFrame()
		{
			if (FRAME_COUNT > 1)
			{
				_frame = (_frame + 1) % FRAME_COUNT;
				_sprite.Source = _spriteCache[_frame];
			}
		}
	}
}
