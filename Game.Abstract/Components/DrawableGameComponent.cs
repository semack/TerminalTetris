﻿using System.Threading;
using System.Threading.Tasks;
using GameFramework.EventArgs;
using GameFramework.IO;

namespace GameFramework.Components
{
    public abstract class DrawableGameComponent : GameComponent
    {
        private int _drawOrder;
        private bool _visible;

        protected Display Display => Game.Display;

        protected DrawableGameComponent(Game game) : base(game)
        {
        }

        public bool Visible
        {
            get => _visible;
            set
            {
                if (value == _visible) return;
                _visible = value;
                Task.Run(async () => { await VisibleChangedAsync(this, new System.EventArgs()); }).Start();
            }
        }

        public virtual int DrawOrder
        {
            get => _drawOrder;
            set
            {
                if (value != _drawOrder)
                {
                    _drawOrder = value;
                    Task.Run(async () => { await DrawOrderChangedAsync(this, new System.EventArgs()); }).Start();
                }
            }
        }

        protected virtual async Task VisibleChangedAsync(object sender, System.EventArgs eventArgs,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        protected virtual async Task DrawOrderChangedAsync(object sender, System.EventArgs eventArgs,
            CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
        }

        public abstract Task DrawAsync(object sender, GameUpdateEventArgs args,
            CancellationToken cancellationToken = default);
    }
}