using Unity.Mathematics;

public readonly struct SmallXXHash4
{
	const uint primeB = 0b10000101111010111100101001110111;
	const uint primeC = 0b11000010101100101010111000111101;
	const uint primeD = 0b00100111110101001110101100101111;
	const uint primeE = 0b00010110010101100110011110110001;

	public static implicit operator uint4(SmallXXHash4 hash)
	{
		uint4 avalanche = hash.accumulator;
		avalanche ^= avalanche >> 15;
		avalanche *= primeB;
		avalanche ^= avalanche >> 13;
		avalanche *= primeC;
		avalanche ^= avalanche >> 16;
		return avalanche;
	}

	public static implicit operator SmallXXHash4(uint4 accumulator) => new SmallXXHash4(accumulator);

	public static implicit operator SmallXXHash4(SmallXXHash hash) => new SmallXXHash4(hash.accumulator);

	public static SmallXXHash4 operator +(SmallXXHash4 h, int v) => h.accumulator + (uint)v;

	public static SmallXXHash4 Seed(int4 seed) => (uint4)seed + primeE;

	public static SmallXXHash4 Select(SmallXXHash4 a, SmallXXHash4 b, bool4 c) => math.select(a.accumulator, b.accumulator, c);

	static uint4 RotateLeft(uint4 data, int steps)
	{
		return (data << steps) | (data >> 32 - steps);
	}

	public readonly uint4 accumulator;

	public SmallXXHash4(uint4 accumulator)
	{
		this.accumulator = accumulator;
	}

	public SmallXXHash4 Eat(int4 data)
	{
		return RotateLeft(accumulator + (uint4)data * primeC, 17) * primeD;
	}

	public uint4 BytesA => (uint4)this & 255;

	public uint4 BytesB => ((uint4)this >> 8) & 255;

	public uint4 BytesC => ((uint4)this >> 16) & 255;

	public uint4 BytesD => (uint4)this >> 24;

	public float4 Floats01A => (float4)BytesA * (1f / 255f);

	public float4 Floats01B => (float4)BytesB * (1f / 255f);

	public float4 Floats01C => (float4)BytesC * (1f / 255f);

	public float4 Floats01D => (float4)BytesD * (1f / 255f);

	public uint4 GetBits(int count, int shift) => ((uint4)this >> shift) & (uint)((1 << count) - 1);

	public float4 GetBitsAsFloats01(int count, int shift) => (float4)GetBits(count, shift) * (1f / ((1 << count) - 1));
}
