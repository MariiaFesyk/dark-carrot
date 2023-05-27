using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class DrawLayerRenderFeature : ScriptableRendererFeature {
    [System.Serializable]
    public class DrawLayerSettings {
        public string renderPassTag = "DrawLayerFeature";
        public bool opaque = false;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        public string renderTextureName = "_MaskTex";
        public FilterMode filterMode = FilterMode.Point;
        public LayerMask layerMask;
    }
    public DrawLayerSettings settings = new DrawLayerSettings();
    private DrawLayerRenderPass renderPass;
    public override void Create(){
        renderPass = new DrawLayerRenderPass(settings);
    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData){
#if UNITY_EDITOR
        if(renderingData.cameraData.isSceneViewCamera) return;
#endif
        renderer.EnqueuePass(renderPass);
    }
    
    public class DrawLayerRenderPass : ScriptableRenderPass {
        private DrawLayerSettings settings;
        RenderTargetHandle temporaryTexture;
        RenderTargetIdentifier destination;
        FilteringSettings filteringSettings;
        RenderStateBlock renderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
        List<ShaderTagId> shaderTagIdList = new List<ShaderTagId>();
        private int destinationID = -1;
        public DrawLayerRenderPass(DrawLayerSettings settings){
            this.settings = settings;
            destinationID = Shader.PropertyToID(settings.renderTextureName);
            profilingSampler = new ProfilingSampler(settings.renderPassTag);
            filteringSettings = new FilteringSettings(null, settings.layerMask);
            shaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
            shaderTagIdList.Add(new ShaderTagId("UniversalForward"));
            shaderTagIdList.Add(new ShaderTagId("Universal2D"));
            shaderTagIdList.Add(new ShaderTagId("UniversalForwardOnly"));
        }
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData){
            RenderTextureDescriptor targetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            targetDescriptor.depthBufferBits = 0;
            targetDescriptor.colorFormat = RenderTextureFormat.ARGB32;
            ScriptableRenderer renderer = renderingData.cameraData.renderer;

            cmd.GetTemporaryRT(destinationID, targetDescriptor, settings.filterMode);
            destination = new RenderTargetIdentifier(destinationID);
            cmd.SetGlobalTexture(settings.renderTextureName, destination);
            ConfigureTarget(destination);
            ConfigureClear(ClearFlag.All, Color.clear);
        }
        public override void OnCameraCleanup(CommandBuffer cmd){
            if (destinationID != -1) cmd.ReleaseTemporaryRT(destinationID);
        }
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData){
            Camera camera = renderingData.cameraData.camera;

            CommandBuffer cmd = CommandBufferPool.Get(settings.renderPassTag);

            SortingCriteria sortingCriteria = settings.opaque
                ? renderingData.cameraData.defaultOpaqueSortFlags
                : SortingCriteria.CommonTransparent;
            var drawingSettings = CreateDrawingSettings(shaderTagIdList, ref renderingData, sortingCriteria);
            using (new ProfilingScope(cmd, profilingSampler)) {
                context.DrawRenderers(
                    renderingData.cullResults, ref drawingSettings, ref filteringSettings, ref renderStateBlock
                );
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
}
