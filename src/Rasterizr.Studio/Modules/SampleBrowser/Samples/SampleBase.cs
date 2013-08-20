﻿using System.Windows.Controls;

namespace Rasterizr.Studio.Modules.SampleBrowser.Samples
{
	public abstract class SampleBase
	{
		public abstract string Name { get; }

		/// <summary>
		/// In a derived class, implements logic to initialize the sample.
		/// </summary>
		public abstract void Initialize(Image image);

		public virtual void BeginRun()
		{
		}

		/// <summary>
		/// In a derived class, implements logic to update any relevant sample state.
		/// </summary>
        public virtual void Update(float time)
		{
		}

		/// <summary>
		///   In a derived class, implements logic that should occur before all
		///   other rendering.
		/// </summary>
		public virtual void BeginDraw()
		{
		}

		/// <summary>
		/// In a derived class, implements logic to render the sample.
		/// </summary>
        public virtual void Draw(float time)
		{
		}

		/// <summary>
		///   In a derived class, implements logic that should occur after all
		///   other rendering.
		/// </summary>
		public virtual void EndDraw()
		{
		}

		public virtual void EndRun()
		{
		}
	} 
}