
Shader ".Rero/Particle/Lit Decals"
{
	Properties
	{
		[Enum(Alpha Blended,10,Additive,1,None,0)] _Blend2 ("Alpha Blend Mode", Float) = 10

		_MaxRange ("Coverage Range", Float) = 1
		_RangeHardness ("Range Edge Fade", Range(0,1)) = .5
		[Toggle(_)] _Decal ("Decal", Float) = 0
		[Toggle(_)] _Emiss ("Particle Color Emission", Float) = 0

        [HDR] _AmbColor ("Ambient Color", color) = (.5, .5, .5, 1.)
        [Space(10)]_Diffuse ("Diffuse", Range(0, 1.)) = 1
        [HDR] _DifColor ("Diffuse Color", color) = (.75, .75, .75, 1.)	
         [Space(10)]_Shininess ("Specular Shininess", Range(5, 100)) = 10
        [HDR] _SpecuColor ("Specular color", color) = (1., 1., 1., 1.)

		[NoScaleOffset]_FallbackCube ("Fallback Cubemap", Cube) = "black" {}
		_Roughness ("Roughness", Range(0, 1)) = 0.5
		_FakeLightDir ("Fake Light Direction", Vector) = (0.5,0.5,0.5)
		
		_MainTex ("Texture", 2D) = "white" {}
		_MaskAlpha ("Alpha Override", Float) = 1
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_Normal ("Normal", Float) = 1
		_Sharpness ("Triplanar Blend Sharpness", Float) = 4
		_MaskTex ("Alpha Mask", 2D) = "white" {}
		
		_NoiseTex ("Dissolve Texture", 2D) = "white" {}
		[KeywordEnum(Object, World)] _DissolveSpace ("Dissolve Space", Float) = 1
		[Toggle(_)] _ClipAlpha ("Clip Alpha", Float) = 0
		_ClipLevel ("Cutout Threshold", Range (0, 1.01)) = .5
		[HDR] _EdgeColor1 ("Outer Edge Color", Color) = (1.0, 1.0, 1.0, 1.0)
		[HDR] _EdgeColor2 ("Inner Edge Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Edges ("Edge Amount", Float) = .1

		[Toggle(_)] _DistortTex ("Distort Texture", Float) = 0
		_DistSpeed ("Distortion Speed", Float) = 0
		_DistSize ("Distortion Scale", Float) = 0
	}

	SubShader
	{	
	
		Tags {"Queue" = "Transparent-1" "IgnoreProjectors" = "True"}
		Blend One [_Blend2]
		ZTest Greater
		Zwrite Off
		Cull Front
		Lighting On
		
        CGINCLUDE
		#pragma target 3.0
		ENDCG
		
		Pass
		{
			Name "PDBase"
			Tags { "LightMode" = "ForwardBase" }
			
			CGPROGRAM
			
			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			
			#pragma multi_compile_fwdbase nolightmap
			
            #include "reroLitDecals.cginc"
			
			ENDCG
		}
		/* Pass
		{
			Name "PDAdd"
			Tags { "LightMode" = "ForwardAdd" }
			Blend One One
			ZWrite Off
			
			CGPROGRAM
			
			#pragma target 3.0
			
			#pragma multi_compile_fwdadd
			
			#pragma vertex vert
			#pragma fragment frag
			
            #include "LitProjectiveDissolveShaderParticle.cginc"
			
			ENDCG
			
		} */
	}
} 