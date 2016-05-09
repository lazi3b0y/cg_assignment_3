float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldInverseTranspose;

float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.1;

float3 DiffuseLightDirection = float3(1, 0, 0);
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 1.0;

float Shininess = 200;
float4 SpecularColor = float4(1, 1, 1, 1);    
float SpecularIntensity = 1.0;
float3 ViewVector = float3(1, 0, 0);
float3 CameraPos;

texture ModelTexture;
sampler2D textureSampler = sampler_state {
    Texture = (ModelTexture);
    MinFilter = Linear;
	Mipfilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};

float BumpConstant = 1;
texture NormalMap;
sampler2D bumpSampler = sampler_state {
    Texture = (NormalMap);
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

float EnvironmentMapIntensity;
texture ReflectiveModelTexture;
samplerCUBE ReflectiveSampler = sampler_state {
	texture = (ReflectiveModelTexture); 
	Magfilter = Linear; 
	Minfilter = Linear; 
	Mipfilter = Linear; 
	AddressU = Mirror; 
	AddressV = Mirror;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float3 Tangent : TANGENT0;
    float3 Binormal : BINORMAL0;
    float2 TextureCoordinate : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float2 TextureCoordinate : TEXCOORD0;
    float3 Normal : TEXCOORD1;
    float3 Tangent : TEXCOORD2;
    float3 Binormal : TEXCOORD3;
	float3 Reflection : TEXCOORD4;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	
	float3 VertexPosition = mul(input.Position, World);
    float3 ViewDirection = CameraPos - VertexPosition;
	float4 Normal = normalize(mul(input.Normal, WorldInverseTranspose));
    
	output.Reflection = reflect(-normalize(ViewDirection), normalize(Normal));	
    output.Normal = normalize(mul(input.Normal, WorldInverseTranspose));
    output.Tangent = normalize(mul(input.Tangent, WorldInverseTranspose));
    output.Binormal = normalize(mul(input.Binormal, WorldInverseTranspose));

    output.TextureCoordinate = input.TextureCoordinate;
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float3 bump = BumpConstant * (tex2D(bumpSampler, input.TextureCoordinate) - (0.5, 0.5, 0.5));
    float3 bumpNormal = input.Normal + (bump.x * input.Tangent + bump.y * input.Binormal);
    bumpNormal = normalize(bumpNormal);

    float diffuseIntensity = dot(normalize(DiffuseLightDirection), bumpNormal);
    if(diffuseIntensity < 0)
        diffuseIntensity = 0;

    float3 light = normalize(DiffuseLightDirection);
    float3 r = normalize(2 * dot(light, bumpNormal) * bumpNormal - light);
    float3 v = normalize(mul(normalize(ViewVector), World));
    float dotProduct = dot(r, v);

    float4 specular = SpecularIntensity * SpecularColor * max(pow(abs(dotProduct), Shininess), 0) * diffuseIntensity;

    float4 textureColor = tex2D(textureSampler, input.TextureCoordinate);
    textureColor.a = 1;

    return (saturate(textureColor * (diffuseIntensity) + AmbientColor * AmbientIntensity + specular)) * texCUBE(ReflectiveSampler, normalize(input.Reflection));
}

technique BumpRefMapped
{
    pass Pass1
    {
        VertexShader = compile vs_4_0 VertexShaderFunction();
        PixelShader = compile ps_4_0 PixelShaderFunction();
    }
}