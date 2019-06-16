using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace GameProject
{
	class BackgroundAnimator
	{
		private const string BACKGROUND_IMAGE_PATH = "ms-appx:///Assets/background.png";
		private readonly int[] BACKGROUND_IMAGE_DIMENSIONS = new int[2] { 128, 128 };
		private const double BACKGROUND_BACK_SPEED = 1;
		private const double BACKGROUND_FRONT_SPEED = 1.5;
		private const double BACKGROUND_BACK_OPACITY = 0.25;
		private const double BACKGROUND_FRONT_OPACITY = 0.50;
		private readonly double[] BACKGROUND_BACK_INITIAL_OFFSET = new double[2] { 50, 0 };

		private Image[,] _backgroundBackTiles;
		private Image[,] _backgroundFrontTiles;
		private Canvas _canvas;
		private readonly int _tilesOnX;
		private readonly int _tilesOnY;

		
		public BackgroundAnimator(Canvas canvas)
		{
			_canvas = canvas;

			_tilesOnX = (int)((float)_canvas.Width / BACKGROUND_IMAGE_DIMENSIONS[0] + 2);
			_tilesOnY = (int)((float)_canvas.Height / BACKGROUND_IMAGE_DIMENSIONS[1] + 2);

			_backgroundBackTiles = new Image[_tilesOnX, _tilesOnY];
			_backgroundFrontTiles = new Image[_tilesOnX, _tilesOnY];

			for (int i = 0; i < _tilesOnX; i++)
			{
				for (int j = 0; j < _tilesOnY; j++)
				{
					// Back lines
					_backgroundBackTiles[i, j] = new Image
					{
						Opacity = BACKGROUND_BACK_OPACITY,
						Source = new BitmapImage(new Uri(BACKGROUND_IMAGE_PATH))
					};
					Canvas.SetLeft(_backgroundBackTiles[i, j], i * BACKGROUND_IMAGE_DIMENSIONS[0] - BACKGROUND_BACK_INITIAL_OFFSET[0]);
					Canvas.SetTop(_backgroundBackTiles[i, j], j * BACKGROUND_IMAGE_DIMENSIONS[1] - BACKGROUND_BACK_INITIAL_OFFSET[1]);
					_canvas.Children.Add(_backgroundBackTiles[i, j]);

					// Front lines
					_backgroundFrontTiles[i, j] = new Image
					{
						Opacity = BACKGROUND_FRONT_OPACITY,
						Source = new BitmapImage(new Uri(BACKGROUND_IMAGE_PATH))
					};
					Canvas.SetLeft(_backgroundFrontTiles[i, j], i * BACKGROUND_IMAGE_DIMENSIONS[0]);
					Canvas.SetTop(_backgroundFrontTiles[i, j], j * BACKGROUND_IMAGE_DIMENSIONS[1]);
					_canvas.Children.Add(_backgroundFrontTiles[i, j]);
				}
			}
		}

		public void ReaddChildren()
		{
			for (int i = 0; i < _tilesOnX; i++)
			{
				for (int j = 0; j < _tilesOnY; j++)
				{
					_canvas.Children.Add(_backgroundBackTiles[i, j]);
					_canvas.Children.Add(_backgroundFrontTiles[i, j]);
				}
			}
		}

		public void Update(double speedMult)
		{
			double xBoundryBack = -BACKGROUND_IMAGE_DIMENSIONS[0];
			double yBoundryBack = -BACKGROUND_IMAGE_DIMENSIONS[1];
			double xBoundryFront = (_backgroundFrontTiles.GetLength(0) - 1) * BACKGROUND_IMAGE_DIMENSIONS[0];
			double yBoundryFront = (_backgroundFrontTiles.GetLength(1) - 1) * BACKGROUND_IMAGE_DIMENSIONS[1];

			for (int i = 0; i < _tilesOnX; i++)
			{
				for (int j = 0; j < _tilesOnY; j++)
				{
					// Back lines
					double newLeftBack = Canvas.GetLeft(_backgroundBackTiles[i, j]) - BACKGROUND_BACK_SPEED * speedMult;
					double newTopBack = Canvas.GetTop(_backgroundBackTiles[i, j]) - BACKGROUND_BACK_SPEED * speedMult;

					if (newLeftBack <= xBoundryBack)
						newLeftBack = (_backgroundBackTiles.GetLength(0) - 1) * BACKGROUND_IMAGE_DIMENSIONS[0];

					if (newTopBack <= yBoundryBack)
						newTopBack = (_backgroundBackTiles.GetLength(1) - 1) * BACKGROUND_IMAGE_DIMENSIONS[1];

					Canvas.SetLeft(_backgroundBackTiles[i, j], newLeftBack);
					Canvas.SetTop(_backgroundBackTiles[i, j], newTopBack);

					// Front lines
					double newLeftFront = Canvas.GetLeft(_backgroundFrontTiles[i, j]) + BACKGROUND_FRONT_SPEED * speedMult;
					double newTopFront = Canvas.GetTop(_backgroundFrontTiles[i, j]) + BACKGROUND_FRONT_SPEED * speedMult;

					if (newLeftFront >= xBoundryFront)
						newLeftFront = (newLeftFront % xBoundryFront) - BACKGROUND_IMAGE_DIMENSIONS[0];

					if (newTopFront >= yBoundryFront)
						newTopFront = (newTopFront % yBoundryFront) - BACKGROUND_IMAGE_DIMENSIONS[1];

					Canvas.SetLeft(_backgroundFrontTiles[i, j], newLeftFront);
					Canvas.SetTop(_backgroundFrontTiles[i, j], newTopFront);
				}
			}
		}
	}
}
