Shader "Custom/skyshader"
{
    Properties
    {
         _Color1 ("Top Color", Color) = (1,1,1,1)
         _Color2 ("Mid Color", Color) = (1,1,1,1)
         _Color3 ("Bot Color", Color) = (1,1,1,1)
         _MainTex ("Albedo (RGB)", 2D) = "white" {}
    
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
       
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf nolight noambient noforwardadd nolightmap novertexlights noshadow


         sampler2D _MainTex;
         fixed4 _Color1;
         fixed4 _Color2;
         fixed4 _Color3;

        struct Input
        {
            float2 uv_MainTex;
        };

        
         
       
        void surf (Input IN, inout SurfaceOutput  o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D(_MainTex,IN.uv_MainTex);
            o.Emission=lerp(_Color1,_Color2,c.r);
            o.Emission=lerp(o.Emission,_Color3,c.b);
         
        }
        float4 Lightingnolight(SurfaceOutput s, float3 lightDir, float atten)
        {
            return float4(0,0,0,1);
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}
