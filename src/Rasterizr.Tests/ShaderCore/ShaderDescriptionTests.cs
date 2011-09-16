﻿using NUnit.Framework;
using Nexus;
using Rasterizr.ShaderCore;

namespace Rasterizr.Tests.ShaderCore
{
	[TestFixture]
	public class ShaderDescriptionTests
	{
		private struct TestPixelShaderInput
		{
			[Semantic(Semantics.TexCoord)]
			public ColorF TexCoord;

			[Semantic(Semantics.Color)]
			public ColorF Color;
		}

		private struct TestPixelShaderOutput
		{
			[Semantic(Semantics.Color)]
			public ColorF Color;
		}

		[Shader(typeof(TestPixelShaderInput), typeof(TestPixelShaderOutput))]
		private class TestShader
		{
			
		}

		[Test]
		public void CanGetInputAndOutputParametersFromShader()
		{
			// Arrange.

			// Act.
			var shaderDescription = new ShaderDescription(typeof(TestShader));

			// Assert.
			Assert.That(shaderDescription.InputParameters, Has.Length.EqualTo(2));
			Assert.That(shaderDescription.OutputParameters, Has.Length.EqualTo(1));
		}
	}
}