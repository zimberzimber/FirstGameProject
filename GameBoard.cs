using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace GameProject
{
	class GameBoard
	{
		// Game setup data
		private const int ENEMY_COUNT_BASE = 10;
		private const int ENEMY_COUNT_INCREASE_PER_LEVEL = 2;
		private const int MAX_LEVELS = 5;
		private const double MINIMUM_ENEMY_PLAYER_DISTANCE = 100;
		private const double MINIMUM_ENEMY_ENEMY_DISTANCE = 50;
		private const double GAME_SPEED_AT_SETUP = 0;
		private const double GAME_SPEED_INCREMENT_AFTER_SETUP = 0.01;
		private const double GAME_SPEED_NORMAL = 1;

		private List<EnemyFollower> _enemies;
		private Player _player;
		private Canvas _canvas;
		private int _gameState;
		private BackgroundAnimator _backgroundAnimator;
		private SoundHandler _soundHandler;
		private double _gameSpeedMultiplier = 1;
		private bool _speedingUpAfterSetup = false;
		private int _level;
		private string _title = "";
		private string _subtitle = "";

		public List<EnemyFollower> Enemies { get { return _enemies; } }
		public Player Player { get { return _player; } }
		public Canvas Canvas { get { return _canvas; } }
		public int GameState { get { return _gameState; } }
		public SoundHandler SoundHandler { get { return _soundHandler; } set { _soundHandler = value; } }
		public double GameSpeedMultiplier { get { return _gameSpeedMultiplier; } set { _gameSpeedMultiplier = value; } }
		public int Level { get { return _level; } set { _level = value; } }
		public string Title { get { return _title; } set { _title = value; } }
		public string Subtitle { get { return _subtitle; } set { _subtitle = value; } }

		//		CONSTRUCTOR
		public GameBoard(Canvas canvas, MediaElement musicPlayer)
		{
			_canvas = canvas;
			_enemies = new List<EnemyFollower>();
			_backgroundAnimator = new BackgroundAnimator(canvas);
			_soundHandler = new SoundHandler(musicPlayer);

			SetupGame();
		}


		//		STATIC METHODS
		/// <summary> Get the distance between two x y coordinates </summary>
		public static double GetDistance(double x1, double y1, double x2, double y2)
		{
			double xDist = Math.Pow(x2 - x1, 2);
			double yDist = Math.Pow(y2 - y1, 2);
			double distance = Math.Sqrt(xDist + yDist);
			distance = Math.Abs(distance);

			return distance;
		}


		//		INSTANCE METHODS

		private void SetupGame()
		{
			if (_level >= MAX_LEVELS)
				_level = 0;

			int enemies = ENEMY_COUNT_INCREASE_PER_LEVEL * _level + ENEMY_COUNT_BASE;
			Random rnd = new Random();
			double canvasWidth = _canvas.Width;
			double canvasHeight = _canvas.Height;
			double xPos = canvasWidth * rnd.NextDouble();
			double yPos = canvasHeight * rnd.NextDouble();

			_title = "";
			_subtitle = "";
			_gameState = GlobalData.GAME_RUNNING;
			_player = new Player(this);
			_player.SetPosition(xPos, yPos);
			// adding the player to the canvas later so its drawn over enemies and their "corpses"

			_enemies.Clear();
			for (int i = 0; i < enemies; i++)
			{
				bool validPlacement;

				do
				{
					validPlacement = true;

					xPos = canvasWidth * rnd.NextDouble();
					yPos = canvasHeight * rnd.NextDouble();

					double distanceToPlayer = GetDistance(xPos, yPos, _player.X, _player.Y);
					if (distanceToPlayer <= MINIMUM_ENEMY_PLAYER_DISTANCE)
					{
						validPlacement = false;
					}

					if (validPlacement)
					{
						foreach (Enemy enemy2 in _enemies)
						{
							if (enemy2 != null)
							{
								double distance = GetDistance(xPos, yPos, enemy2.X, enemy2.Y);
								if (distance <= MINIMUM_ENEMY_ENEMY_DISTANCE)
								{
									validPlacement = false;
									break;
								}
							}
						}
					}
				} while (!validPlacement);


				if (validPlacement)
				{
					_enemies.Add(new EnemyFollower(this, rnd));
					_enemies[i].SetPosition(xPos, yPos);
					_canvas.Children.Add(_enemies[i].Sprite);
				}
			}

			_canvas.Children.Add(_player.Sprite);

			_gameSpeedMultiplier = GAME_SPEED_AT_SETUP;
			_speedingUpAfterSetup = true;
		}

		public string SaveGame()
		{
			string save = "";
			string enemyData = "";
			int enemiesAlive = 0;

			save += "level" + _level;
			save += "\nplayer{" + _player.X + "," + _player.Y + "}";

			foreach (Enemy enemy in _enemies)
			{
				if (enemy.IsAlive)
				{
					enemiesAlive++;
					enemyData += "\n{[" + enemy.X + "," + enemy.Y + "]," + enemy.Radius + "," + enemy.Speed + "}";
				}
			}
			save += "\nenemies{" + enemiesAlive + "}";
			save += enemyData;

			return save;
		}

		public void LoadGame(Player player, List<EnemyFollower> enemies)
		{
			_gameState = GlobalData.GAME_RUNNING;
			_title = "";
			_subtitle = "";

			_canvas.Children.Clear();
			_backgroundAnimator.ReaddChildren();

			_enemies = enemies;
			foreach (Enemy enemy in _enemies)
			{
				_canvas.Children.Add(enemy.Sprite);
			}

			_player = player;
			_canvas.Children.Add(_player.Sprite);

			_gameSpeedMultiplier = GAME_SPEED_AT_SETUP;
			_speedingUpAfterSetup = true;
		}

		public void ResetGame()
		{
			_canvas.Children.Clear();
			_backgroundAnimator.ReaddChildren();
			_gameState = GlobalData.GAME_RUNNING;
			SetupGame();
		}

		/// <summary> No args |OR| timer tick arg only </summary>
		public void Update(int tickType = GlobalData.TIMER_TICK_NORMAL)
		{ Update(new bool[4] { false, false, false, false }, tickType); }

		/// <summary> Movement arg only </summary>
		public void Update(bool[] movementArgs)
		{ Update(movementArgs, GlobalData.TIMER_TICK_NORMAL); }

		/// <summary> Movement and tick args </summary>
		public void Update(bool[] movementArgs, int tickType)
		{
			_backgroundAnimator.Update(_gameSpeedMultiplier);
			UpdateFrames();

			if (tickType != GlobalData.TIMER_TICK_GAMEOVER)
			{
				_player.Move(movementArgs);

				if (tickType == GlobalData.TIMER_TICK_ENEMY)
					RunEnemyThinks();

				CheckPlayerCollision();

				if (_gameState == GlobalData.GAME_RUNNING && tickType == GlobalData.TIMER_TICK_ENEMY)
					CheckEnemyCollision();
			}

			if (_speedingUpAfterSetup)
			{
				_gameSpeedMultiplier += GAME_SPEED_INCREMENT_AFTER_SETUP;
				if (_gameSpeedMultiplier >= GAME_SPEED_NORMAL)
				{
					_gameSpeedMultiplier = GAME_SPEED_NORMAL;
					_speedingUpAfterSetup = false;
				}
			}
		}

		private void CheckPlayerCollision()
		{
			foreach (Enemy enemy in _enemies)
			{
				if (enemy.IsAlive)
				{
					double distanceToPlayer = GetDistance(enemy.X, enemy.Y, _player.X, _player.Y);
					if (distanceToPlayer <= enemy.Radius + _player.Radius)
					{
						_player.Die();
						GameOver();
						return;
					}
				}
			}
		}

		private void CheckEnemyCollision()
		{
			int iterationCounter = 0;
			foreach (Enemy enemy in _enemies)
			{
				if (enemy.IsAlive)
				{
					for (int i = iterationCounter + 1; i < _enemies.Count; i++)
					{
						if (_enemies[i].IsAlive)
						{
							double distance = GetDistance(enemy.X, enemy.Y, _enemies[i].X, _enemies[i].Y);
							if (distance <= enemy.Radius + _enemies[i].Radius)
								enemy.Die();
						}
					}
				}

				iterationCounter++;
			}

			int enemiesAlive = 0;
			foreach (Enemy enemy in _enemies)
			{
				if (enemy.IsAlive)
					enemiesAlive++;

				if (enemiesAlive > 1)
				{ return; }
			}

			NextLevel();
		}

		private void RunEnemyThinks()
		{
			foreach (Enemy enemy in _enemies)
			{
				if (enemy.IsAlive)
					enemy.Think();
			}
		}

		private void UpdateFrames()
		{
			foreach (Enemy enemy in _enemies)
			{
				if (enemy.IsAlive)
					enemy.UpdateFrame();
			}
		}

		private void NextLevel()
		{
			if (++_level < MAX_LEVELS)
				ResetGame();
			else
				GameOver(true);
		}

		public void GameOver(bool victory = false)
		{
			if (victory)
			{
				_gameState = GlobalData.GAME_WON;
				_title = "YOU WIN";
				_subtitle = "Hope you enjoyed!\nPress 'R' to start over";
			}
			else
			{
				_gameState = GlobalData.GAME_LOST;
				if (_level > 0)
				{
					_title = "GAME OVER";
					_subtitle = "Level reduced by 1\nPress 'R' to try again";
					_level--;
				}
				else
				{
					_title = "GAME OVER";
					_subtitle = "Press 'R' to try again";
				}
			}
		}
	}
}
