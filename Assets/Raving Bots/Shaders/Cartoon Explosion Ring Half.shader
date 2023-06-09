Shader "FX/Cartoon Explosion/Ring Half"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_Mask ("Mask", Range(0, 1)) = 0.5
		_Divider ("Divider", Range(-1, 1)) = 1
		
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex SpriteVert
			#pragma fragment MaskedFrag
			#pragma target 2.0
			#pragma multi_compile_instancing
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnitySprites.cginc"
			
			float _Mask;
			float _Divider;
			
			fixed SampleMask(float2 texcoord, fixed a)
			{
				texcoord = texcoord/_Mask - (0.5/_Mask - 0.5) * float2(1, 1);
				
				if (texcoord.x < 0 || texcoord.x > 1 || texcoord.y < 0 || texcoord.y > 1)
					return 0;
				
				return SampleSpriteTexture(texcoord).a * a;
			}
			
			fixed4 MaskedFrag(v2f IN) : SV_Target
			{
				if (sign(_Divider) * IN.texcoord.y > sign(_Divider) * 0.5)
					return fixed4(0, 0, 0, 0);
				
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
				
				c.a -= _Mask > 0 ? SampleMask(IN.texcoord, IN.color.a) : 0;
				
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
