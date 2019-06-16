using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.Storage;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace GameProject
{
	public sealed partial class MainPage : Page
	{
		DispatcherTimer timer = new DispatcherTimer();
		GameBoard game;

		bool leftHeld, rightHeld, upHeld, downHeld;
		bool systemPause = false;
		int enemyMovementTimer;

		public MainPage()
		{
			this.InitializeComponent();
			ApplicationView.PreferredLaunchViewSize = new Size(GameCanvas.Width, GameCanvas.Height + 50); //Adding the command bars height to canvas height
			ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

			Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
			Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;

			enemyMovementTimer = GlobalData.TIMER_ENEMY_INTERVAL_MS;
			timer.Tick += TimerTick;
			timer.Interval = new TimeSpan(0, 0, 0, 0, GlobalData.TIMER_INTERVAL_MS);
			timer.Start();

			game = new GameBoard(GameCanvas, MusicPlayer);
		}

		void TimerTick(object sender, object e)
		{
			OverlayText.Text = game.Title;
			OverlaySubtext.Text = game.Subtitle;

			if (game.GameState != GlobalData.GAME_RUNNING)
			{
				game.Update(GlobalData.TIMER_TICK_GAMEOVER);
				if (game.GameState == GlobalData.GAME_WON)
					LevelDisplay.Text = "";
				else
					LevelDisplay.Text = "Level: " + (game.Level + 1);
			}
			else
			{
				LevelDisplay.Text = "Level: " + (game.Level + 1);

				enemyMovementTimer -= GlobalData.TIMER_INTERVAL_MS;
				bool[] movementArgs = new bool[4] { upHeld, downHeld, leftHeld, rightHeld };

				if (enemyMovementTimer <= 0)
				{
					game.Update(movementArgs, GlobalData.TIMER_TICK_ENEMY);
					enemyMovementTimer = GlobalData.TIMER_INTERVAL_MS;
				}
				else
					game.Update(movementArgs);
			}
		}

		private void RestartButton_click(object sender, RoutedEventArgs e)
		{
			enemyMovementTimer = GlobalData.TIMER_INTERVAL_MS;
			game.ResetGame();
		}

		private void PauseButton_click(object sender, RoutedEventArgs e)
		{
			if (systemPause) return;

			if (timer.IsEnabled)
			{
				OverlayText.Text = "PAUSED";
				OverlaySubtext.Text = "Press 'P' to unpause";
				timer.Stop();
			}
			else
			{
				OverlayText.Text = game.Title;
				OverlaySubtext.Text = game.Subtitle;
				timer.Start();
			}
		}

		private async void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			if (systemPause || game.GameState == GlobalData.GAME_LOST) return;

			bool timerRunning = timer.IsEnabled;
			systemPause = true;
			timer.Stop();

			// Writing directly in installation folder because there's no permission for anything else
			string save = game.SaveGame();
			StorageFolder root = ApplicationData.Current.LocalFolder;
			StorageFile file = await root.CreateFileAsync("save.txt", CreationCollisionOption.OpenIfExists);
			await FileIO.WriteTextAsync(file, save);

			Title.Text = "Game saved";
			systemPause = false;
			if (timerRunning)
				timer.Start();
		}

		private async void LoadButton_Click(object sender, RoutedEventArgs e)
		{
			if (systemPause) return;

			bool timerRunning = timer.IsEnabled;
			systemPause = true;
			timer.Stop();

			string root = ApplicationData.Current.LocalFolder.Path;
			StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(root);
			try
			{
				StorageFile save = await storageFolder.GetFileAsync("save.txt");
				string data = await FileIO.ReadTextAsync(save);
				Player player = null;
				List<EnemyFollower> enemies = new List<EnemyFollower>();
				int level = 0;

				if (data.Contains("level"))
				{
					int length = data.IndexOf("\n");
					length -= "level".Length;
					string value = data.Substring("level".Length, length);
					level = int.Parse(value);
				}

				if (data.Contains("player"))
				{
					int start = data.IndexOf("player");
					int bracketsStart = data.IndexOf("{", start + "player".Length) + 1;
					int bracketsEnd = data.IndexOf("}", bracketsStart);
					string contents = data.Substring(bracketsStart, bracketsEnd - bracketsStart);

					int period = contents.IndexOf(",") + 1;
					string xPosStr = contents.Substring(0, period - 1);
					string yPosStr = contents.Substring(period, contents.Length - period);

					double x = double.Parse(xPosStr);
					double y = double.Parse(yPosStr);

					player = new Player(game);
					player.SetPosition(x, y);
				}

				if (data.Contains("enemies"))
				{
					int start = data.IndexOf("enemies");
					int bracketsStart = data.IndexOf("{", start + "enemies".Length) + 1;
					int bracketsEnd = data.IndexOf("}", bracketsStart);
					string contents = data.Substring(bracketsStart, bracketsEnd - bracketsStart);

					int enemyCount = int.Parse(contents);
					int lastStringIndex = bracketsEnd;

					for (int i = 0; i < enemyCount; i++)
					{
						int enemyStart = data.IndexOf("{", lastStringIndex) + 1;
						int enemyEnd = data.IndexOf("}", enemyStart);
						string enemyData = data.Substring(enemyStart, enemyEnd - enemyStart);

						int positionDataBracketStart = enemyData.IndexOf("[") + 1;
						int positionDataBracketEnd = enemyData.IndexOf("]", positionDataBracketStart);
						string position = enemyData.Substring(positionDataBracketStart, positionDataBracketEnd - 1);

						int periodPos = position.IndexOf(",");
						double x = double.Parse(position.Substring(0, periodPos));
						double y = double.Parse(position.Substring(periodPos + 1));

						int radiusPeriod = enemyData.IndexOf(",", positionDataBracketEnd) + 1;
						int speedPeriod = enemyData.IndexOf(",", radiusPeriod) + 1;

						int radiusLength = speedPeriod - 1 - radiusPeriod;
						int speedLength = enemyData.Length - speedPeriod;

						double radius = double.Parse(enemyData.Substring(radiusPeriod, radiusLength));
						double speed = double.Parse(enemyData.Substring(speedPeriod, speedLength));

						enemies.Add(new EnemyFollower(game));
						enemies[enemies.Count - 1].SetPosition(x, y);
						enemies[enemies.Count - 1].Radius = radius;
						enemies[enemies.Count - 1].Speed = speed;

						lastStringIndex = enemyEnd;
					}
				}

				if (player != null && enemies.Count > 1)
				{
					Title.Text = "Game loaded";
					game.Level = level;
					game.LoadGame(player, enemies);
				}
				else
					Title.Text = "Faulty save file";
			}
			catch (System.IO.FileNotFoundException ex)
			{
				Title.Text = "No saved game found";
			}

			systemPause = false;
			if (timerRunning)
				timer.Start();
		}

		private void CloseButton_click(object sender, RoutedEventArgs e)
		{ Application.Current.Exit(); }

		private void MuteButton_Click(object sender, RoutedEventArgs e)
		{ game.SoundHandler.Mute(); }

		private void MediaEnded(object sender, RoutedEventArgs e)
		{ game.SoundHandler.MusicEnded(); }

		void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
		{
			switch (args.VirtualKey)
			{
				case VirtualKey.A:
					leftHeld = true;
					break;
				case VirtualKey.D:
					rightHeld = true;
					break;
				case VirtualKey.W:
					upHeld = true;
					break;
				case VirtualKey.S:
					downHeld = true;
					break;
			}
		}

		void CoreWindow_KeyUp(CoreWindow sender, KeyEventArgs args)
		{
			switch (args.VirtualKey)
			{
				case VirtualKey.A:
					leftHeld = false;
					break;
				case VirtualKey.D:
					rightHeld = false;
					break;
				case VirtualKey.W:
					upHeld = false;
					break;
				case VirtualKey.S:
					downHeld = false;
					break;
				case VirtualKey.R:
					RestartButton_click(null, null);
					break;
				case VirtualKey.P:
					PauseButton_click(null, null);
					break;
				case VirtualKey.Escape:
					CloseButton_click(null, null);
					break;
			}
		}
	}
}
