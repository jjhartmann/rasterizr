﻿using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Rasterizr.Diagnostics;
using Rasterizr.Diagnostics.Logging;
using Rasterizr.Pipeline.OutputMerger;
using Rasterizr.Pipeline.Rasterizer;
using Rasterizr.Platform.Wpf;
using Rasterizr.Resources;
using Rasterizr.Toolkit.Effects;
using Rasterizr.Toolkit.Models;
using SharpDX;
using SlimShader;
using Viewport = Rasterizr.Pipeline.Rasterizer.Viewport;

namespace Rasterizr.SampleBrowser.Samples.ModelLoading
{
	[Export(typeof(SampleBase))]
	[ExportMetadata("SortOrder", 4)]
	public class ModelLoadingSample : SampleBase
	{
		private DeviceContext _deviceContext;
		private RenderTargetView _renderTargetView;
		private DepthStencilView _depthView;
		private WpfSwapChain _swapChain;

	    private BasicEffect _effect;
		private Model _model;

		public override string Name
		{
			get { return "Model Loading"; }
		}

		public override void Initialize(Image image)
		{
			const int width = 600;
			const int height = 400;

			// Create device and swap chain.
			var device = new Device();
			_swapChain = new WpfSwapChain(device, width, height);
			image.Source = _swapChain.Bitmap;
			_deviceContext = device.ImmediateContext;

			// Create RenderTargetView from the backbuffer.
			var backBuffer = Texture2D.FromSwapChain(_swapChain, 0);
			_renderTargetView = device.CreateRenderTargetView(backBuffer);

			// Create the depth buffer
			var depthBuffer = device.CreateTexture2D(new Texture2DDescription
			{
				ArraySize = 1,
				MipLevels = 1,
				Width = width,
				Height = height,
				BindFlags = BindFlags.DepthStencil
			});

			// Create the depth buffer view
			_depthView = device.CreateDepthStencilView(depthBuffer);

            // Load model.
		    var modelLoader = new ModelLoader(device, TextureLoader.CreateTextureFromStream);
            _model = modelLoader.Load("Samples/ModelLoading/Sponza/sponza.3ds");

		    _effect = new BasicEffect(device.ImmediateContext);
		    _effect.LightPosition = new Vector3(0, 2.5f, 0);

		    _model.SetInputLayout(device, _effect.VertexShader.Bytecode.InputSignature);

			var sampler = device.CreateSamplerState(new SamplerStateDescription
			{
				Filter = Filter.MinMagMipPoint,
				AddressU = TextureAddressMode.Wrap,
				AddressV = TextureAddressMode.Wrap,
				AddressW = TextureAddressMode.Wrap,
				BorderColor = new Number4(0, 0, 0, 1),
				ComparisonFunction = Comparison.Never,
				MaximumAnisotropy = 16,
				MipLodBias = 0,
				MinimumLod = 0,
				MaximumLod = 16,
			});

			// Prepare all the stages
			_deviceContext.PixelShader.SetSamplers(0, sampler);

			// Setup targets and viewport for rendering
			_deviceContext.Rasterizer.SetViewports(new Viewport(0, 0, width, height, 0.0f, 1.0f));
			_deviceContext.OutputMerger.SetTargets(_depthView, _renderTargetView);

            //_deviceContext.Rasterizer.State = device.CreateRasterizerState(new RasterizerStateDescription
            //{
            //    CullMode = CullMode.None
            //});

			// Prepare matrices
			_effect.Projection = Matrix.PerspectiveFovLH(MathUtil.PiOverFour, 
				width / (float) height, 0.1f, 100.0f);
		}

		public override void Draw(DemoTime time)
		{
			// Clear views
			_deviceContext.ClearDepthStencilView(_depthView, DepthStencilClearFlags.Depth, 1.0f, 0);
            _deviceContext.ClearRenderTargetView(_renderTargetView, new Number4(0, 0, 0, 1));

            // Rotate camera
            var cameraPosition = new Vector3(0, 3, 5.0f);
            var cameraLookAt = new Vector3(0, 2.0f, 0);
            Vector4 tempPos = Vector3.Transform(cameraPosition, Matrix.RotationY(0.2f * time.ElapsedTime));
            cameraPosition = new Vector3(tempPos.X, tempPos.Y, tempPos.Z);

            // Update matrices.
		    _effect.World = Matrix.Translation(0, -_model.AxisAlignedBoxCentre.Y / 2, 0);
            _effect.View = Matrix.LookAtLH(cameraPosition, cameraLookAt, Vector3.UnitY);

		    _effect.Apply();
		    _model.Draw(_deviceContext);

			// Present!
			_swapChain.Present();

			base.Draw(time);
		}
	}
}