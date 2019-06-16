using System;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace GameProject
{
	class SoundHandler
	{
		readonly string[,] MUSIC_FILES = new string[2, 2]{
								{"music_default.ogg", "default"},
								{"music_default.ogg", "boss"}
							};
		readonly string[,] SFX_FILES = new string[2, 2]{
								{"sfx_enemyDeath.ogg", "enemyDeath"},
								{"sfx_playerDeath.ogg", "playerDeath"}
							};

		private MediaElement _musicPlayer;
		private MediaElement _sfxPlayer;
		private IRandomAccessStream[] _musicStreams;
		private IRandomAccessStream[] _sfxStreams;
		private string[] _musicNames;
		private string[] _sfxNames;
		private int _currentTrackIndex;
		
		public SoundHandler(MediaElement musicPlayer)
		{
			_musicPlayer = musicPlayer;
			_sfxPlayer = new MediaElement();
			GenerateStreamArrays();
		}

		private async void GenerateStreamArrays()
		{
			_musicStreams = new IRandomAccessStream[MUSIC_FILES.GetLength(0)];
			_musicNames = new string[MUSIC_FILES.GetLength(0)];

			StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");

			for (int i = 0; i < MUSIC_FILES.GetLength(0); i++)
			{
				StorageFile file = await folder.GetFileAsync(MUSIC_FILES[i, 0]);
				_musicStreams[i] = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
				_musicNames[i] = MUSIC_FILES[i, 1];
			}

			_sfxStreams = new IRandomAccessStream[SFX_FILES.GetLength(0)];
			_sfxNames = new string[SFX_FILES.GetLength(0)];
			for (int i = 0; i < SFX_FILES.GetLength(0); i++)
			{
				StorageFile file = await folder.GetFileAsync(SFX_FILES[i, 0]);
				_sfxStreams[i] = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
				_sfxNames[i] = SFX_FILES[i, 1];
			}

			PlayMusic("default");
		}

		public void update()
		{

		}

		public void PlayMusic(int index)
		{
			_currentTrackIndex = index;
			_musicPlayer.SetSource(_musicStreams[index], "");
			_musicPlayer.Play();
		}

		public void PlayMusic(string music)
		{
			for (int i = 0; i < _musicNames.Length; i++)
			{
				if (music == _musicNames[i])
				{
					_musicPlayer.Position = TimeSpan.Zero;
					PlayMusic(i);
					return;
				}
			}
		}

		public void MusicEnded()
		{
			_musicPlayer.Position = TimeSpan.Zero;
			_musicPlayer.Play();
		}

		public void Mute()
		{ _musicPlayer.IsMuted = !_musicPlayer.IsMuted; }

		public void PlaySound(string sound)
		{
			if (_musicPlayer.IsMuted) return;

			for (int i = 0; i < _sfxNames.Length; i++)
			{
				if (_sfxNames[i] == sound)
				{
					MediaElement sfxPlayer = new MediaElement();
					sfxPlayer.SetSource(_sfxStreams[i], "");
					sfxPlayer.Play();
					return;
				}
			}
		}
	}
}
