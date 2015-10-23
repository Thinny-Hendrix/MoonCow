// this is the texture we are trying to render
uniform extern texture ScreenTexture;  
sampler screen = sampler_state 
{
	// get the texture we are trying to render.
    Texture = <ScreenTexture>;
};

// this is the alpha mapp texture, we set this from the C# code.
uniform extern texture MaskTexture;  
sampler mask = sampler_state
{
    Texture = <MaskTexture>;
};

// here we do the real work. 
float4 PixelShaderFunction(float2 inCoord: TEXCOORD0) : COLOR
{
	// we retrieve the color in the original texture at 
	// the current coordinate remember that this function 
	// is run on every pixel in our texture.
    float4 color = tex2D(screen, inCoord);

	// Since we are using a black and white mask the black 
	// area will have a value of 0 and the white areas will 
	// have a value of 255. Hence the black areas will subtract
	// nothing from our original color, and the white areas of
	// our mask will subtract all color from the color.
	color.rgba = color.rgba - tex2D(mask, inCoord).r;

	// return the new color of the pixel.
    return color;
}
 
technique
{
    pass P0
    {
		// The xbox can only run pixel shader 2.0 and for this 
		// purpose that is plenty for us too.
        PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
    }
}