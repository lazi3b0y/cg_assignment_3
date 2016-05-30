float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldInverseTranspose;
 
float4 TintColor = float4(1, 1, 1, 1);
float3 CameraPosition;
 
Texture ReflectionTexture; 
samplerCUBE ReflectionSampler = sampler_state 
{ 
   texture = <ReflectionTexture>; 
   magfilter = LINEAR; 
   minfilter = LINEAR; 
   mipfilter = LINEAR; 
   AddressU = Mirror;
   AddressV = Mirror; 
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
};
 
struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 Normal : NORMAL0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;
 
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
 
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 vertexPosition = mul(input.Position, World);
    float3 viewDirection = CameraPosition - vertexPosition;
 
    float3 normal = normalize(mul(input.Normal, WorldInverseTranspose));

    float3 reflection = reflect(-normalize(viewDirection), normalize(normal));

    return TintColor * texCUBE(ReflectionSampler, normalize(reflection));
}

technique Reflection
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 VertexShaderFunction();
        PixelShader = compile ps_4_0 PixelShaderFunction();
    }
}