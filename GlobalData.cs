using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
	class GlobalData
	{
		// Differing between different timer tick types
		public const int TIMER_TICK_NORMAL = 0;
		public const int TIMER_TICK_ENEMY = 1;
		public const int TIMER_TICK_GAMEOVER = 2;

		// Timer tick intervals
		public const int TIMER_INTERVAL_MS = 10;
		public const int TIMER_ENEMY_INTERVAL_MS = 10;

		// Game over states
		public const int GAME_RUNNING = 0;
		public const int GAME_LOST = 1;
		public const int GAME_WON = 2;
	}
}
