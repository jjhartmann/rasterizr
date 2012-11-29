﻿namespace Rasterizr.Pipeline.Rasterizer.Primitives
{
	internal struct BarycentricCoordinates
	{
		public float Alpha;
		public float Beta;
		public float Gamma;

		public bool IsOutsideTriangle
		{
			get { return (Alpha < 0 || Alpha > 1 || Beta < 0 || Beta > 1 || Gamma < 0 || Gamma > 1); }
		}
	}
}