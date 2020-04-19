﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Game.Framework.IO;
using Terminal.Tetris.Definitions;
using Terminal.Tetris.Resources;
using Terminal.Tetris.Screens;

namespace Terminal.Tetris
{
    public class Tetris : Game.Framework.Game
    {
        public Tetris(Display display,
            Keyboard keyboard,
            TimeSpan targetElapsedTime)
            : base(display, keyboard, targetElapsedTime)
        {
        }

        public override async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            var (width, height) = await Display.GetWidthHeightAsync(cancellationToken);
            if (width < Constants.ScreenWidth || height < Constants.ScreenHeight)
                throw new ArgumentException(string.Format(Strings.ScreenResolutionError,
                    Constants.ScreenWidth, Constants.ScreenHeight));
        }

        public override async Task RunAsync(CancellationToken cancellationToken = default)
        {
            ThreadPool.QueueUserWorkItem(async state =>
            {
                var splashScreen = new SplashScreen(this);
                var mainScreen = new MainScreen(this);
                var scoresScreen = new ScoresScreen(this);

                await base.RunAsync(cancellationToken);

                var playAgain = true;
                while (playAgain)
                {
                    var playerLevel = await splashScreen.GetPlayerLevelAsync(cancellationToken);
                    var scores = await mainScreen.PlayGameAsync(playerLevel, cancellationToken);
                    playAgain = await scoresScreen.ShowLetterBoardAsync(scores, cancellationToken);
                }

                System.Terminal.GenerateBreakSignal(TerminalBreakSignal.Quit);
            }, cancellationToken);
            await base.RunAsync(cancellationToken);
        }
    }
}