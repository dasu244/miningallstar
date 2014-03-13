Shader "Custom/SpriteMasking" {
Properties {
	[PerRendererData]_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	
	// Non-lightmapped
	Pass {
			//Blend SrcAlpha OneMinusSrcAlpha
			Alphatest greater [_Cutoff]
			SetTexture [_MainTex] {
				Combine texture
			} 
		}
	}

}