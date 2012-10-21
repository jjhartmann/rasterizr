using SlimShader.IO;
using SlimShader.Util;

namespace SlimShader.Tokens
{
	/// <summary>
	/// Hull Shader Declaration Phase: Tessellator Partitioning
	///
	/// OpcodeToken0:
	///
	/// [10:00] D3D11_SB_OPCODE_DCL_TESS_PARTITIONING
	/// [13:11] Partitioning
	/// [23:14] Ignored, 0
	/// [30:24] Instruction length in DWORDs including the opcode token. == 1
	/// [31]    0 normally. 1 if extended operand definition, meaning next DWORD
	///         contains extended operand description.  This dcl is currently not
	///         extended.
	/// </summary>
	public class TessellatorPartitioningDeclarationToken : DeclarationToken
	{
		public TessellatorPartitioning Partioning { get; private set; }

		public static TessellatorPartitioningDeclarationToken Parse(BytecodeReader reader)
		{
			var token0 = reader.ReadUInt32();
			return new TessellatorPartitioningDeclarationToken
			{
				Partioning = token0.DecodeValue<TessellatorPartitioning>(11, 13)
			};
		}
	}
}