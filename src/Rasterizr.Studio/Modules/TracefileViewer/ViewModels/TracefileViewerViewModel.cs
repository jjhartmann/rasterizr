﻿using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using Caliburn.Micro;
using Gemini.Framework;
using Rasterizr.Diagnostics.Logging.ObjectModel;
using Rasterizr.Math;
using Rasterizr.Studio.Modules.GraphicsDebugging;
using Rasterizr.Studio.Modules.GraphicsDebugging.ViewModels;

namespace Rasterizr.Studio.Modules.TracefileViewer.ViewModels
{
	public class TracefileViewerViewModel : Document
	{
		private readonly string _fileName;
		private readonly TracefileViewModel _tracefile;

		[Import]
		private ISelectionService _selectionService;

		public override string DisplayName
		{
			get { return Path.GetFileName(_fileName); }
		}

		public IList<TracefileFrameViewModel> Frames
		{
			get { return _tracefile.Frames; }
		}

		private TracefileFrameViewModel _selectedFrame;
		public TracefileFrameViewModel SelectedFrame
		{
			get { return _selectedFrame; }
			set
			{
				_selectedFrame = value;
				_selectionService.SelectedFrame = value;
				NotifyOfPropertyChange(() => SelectedFrame);
			}
		}

		private Point _hoverPixel;
		internal Point HoverPixel
		{
			get { return _hoverPixel; }
			set
			{
				_hoverPixel = value;
				NotifyOfPropertyChange(() => HoverPixelDescription);
				HasHoverPixel = true;
			}
		}

		public string HoverPixelDescription
		{
			get
			{
				if (SelectedFrame.Image == null)
					return string.Empty;

				return string.Format("Hovered pixel X: {0} ({1:F3}) Y: {2} ({3:F3})",
					_hoverPixel.X, _hoverPixel.X / (float) SelectedFrame.Image.PixelWidth,
					_hoverPixel.Y, _hoverPixel.Y / (float) SelectedFrame.Image.PixelHeight);
			}
		}

		private bool _hasHoverPixel;
		public bool HasHoverPixel
		{
			get { return _hasHoverPixel; }
			set
			{
				_hasHoverPixel = value;
				NotifyOfPropertyChange(() => HasHoverPixel);
			}
		}

		private Point _selectedPixel;
		internal Point SelectedPixel
		{
			get { return _selectedPixel; }
			set
			{
				_selectedPixel = value;
				NotifyOfPropertyChange(() => SelectedPixelDescription);
				HasSelectedPixel = true;
			}
		}

		public string SelectedPixelDescription
		{
			get
			{
				if (SelectedFrame.Image == null)
					return string.Empty;

				return string.Format("Selected pixel X: {0} ({1:F3}) Y: {2} ({3:F3})",
					_selectedPixel.X, _selectedPixel.X / (float) SelectedFrame.Image.PixelWidth,
					_selectedPixel.Y, _selectedPixel.Y / (float) SelectedFrame.Image.PixelHeight);
			}
		}

		private bool _hasSelectedPixel;
		public bool HasSelectedPixel
		{
			get { return _hasSelectedPixel; }
			set
			{
				_hasSelectedPixel = value;
				NotifyOfPropertyChange(() => HasSelectedPixel);
			}
		}

		public TracefileViewerViewModel(string fileName, Tracefile tracefile)
		{
			_fileName = fileName;
			_tracefile = new TracefileViewModel(tracefile);
			_hasSelectedPixel = false;
		}
	}
}