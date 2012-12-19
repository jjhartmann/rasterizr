﻿using System;
using Rasterizr.Resources;

namespace Rasterizr.Pipeline.OutputMerger
{
	public class DepthStencilView : ResourceView
	{
		private readonly DepthStencilViewDescription _description;
		private readonly Format _actualFormat;

		public DepthStencilViewDescription Description
		{
			get { return _description; }
		}

		internal DepthStencilView(Device device, Resource resource, DepthStencilViewDescription? description)
			: base(device, resource)
		{
			switch (resource.ResourceType)
			{
				case ResourceType.Buffer:
					throw new ArgumentException("Invalid resource type for depth stencil view: " + resource.ResourceType);
			}

			if (description == null)
				description = new DepthStencilViewDescription
				{
					Format = Format.Unknown,
					Dimension = DepthStencilViewDimension.Unknown
				};

			_description = description.Value;
			_actualFormat = ResourceViewUtility.GetActualFormat(_description.Format, resource);
		}

		internal float GetDepth(int x, int y, int sampleIndex)
		{
			float result;
			Resource.GetData(out result, Resource.CalculateByteOffset(x, y, 0), sizeof(float));
			return result;
		}

		internal void SetDepth(int x, int y, int sampleIndex, float depth)
		{
			Resource.SetData(ref depth, Resource.CalculateByteOffset(x, y, 0));
		}

		internal void Clear(DepthStencilClearFlags clearFlags, float depth, byte stencil)
		{
			if (clearFlags.HasFlag(DepthStencilClearFlags.Depth))
			{
				switch (_actualFormat)
				{
					case Format.D32_Float_S8X24_UInt :
						Resource.Fill(ref depth);
						break;
					default :
						throw new ArgumentException();
				}
			}
		}
	}
}