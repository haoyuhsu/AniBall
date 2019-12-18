#if KRIPTO_FX_LWRP_RENDERING
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Experimental.Rendering.LightweightPipeline;
using UnityEngine.Rendering;


//[RequireComponent(typeof(PostProcessLayer))]
public class ME_LWRP_RenderDistortion: MonoBehaviour, IAfterTransparentPass
{
    public bool IsActive = true;
    private RFX4_CustomDistortionImpl customustomDistortionImpl;


    public ScriptableRenderPass GetPassToEnqueue(RenderTextureDescriptor baseDescriptor, RenderTargetHandle colorHandle, RenderTargetHandle depthHandle)
    {
        if (customustomDistortionImpl == null) customustomDistortionImpl = new RFX4_CustomDistortionImpl();
        customustomDistortionImpl._baseDescriptor = baseDescriptor;
        customustomDistortionImpl._colorHandle = colorHandle;
        customustomDistortionImpl._depthHandle = depthHandle;
        customustomDistortionImpl.IsActive = IsActive;
        return customustomDistortionImpl;
    }
}


public class RFX4_CustomDistortionImpl : ScriptableRenderPass
{
    public bool IsActive = true;
    const string transparentsTag = "Render Distorted Transparent";
    const string copyColorTag = "Copy Transparent Color";

    public RenderTextureDescriptor _baseDescriptor;
    public RenderTargetHandle _colorHandle;
    public RenderTargetHandle _depthHandle;

    private RenderTargetHandle _destination;

    public RFX4_CustomDistortionImpl()
    {
        RegisterShaderPassName("CustomDistortion");
        _destination.Init("_GrabTexture");
    }

    public override void Execute(ScriptableRenderer renderer, ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (!IsActive) return;
         //copy opaque + transparent color

        CommandBuffer cmd = CommandBufferPool.Get(copyColorTag);

        RenderTargetIdentifier colorRT = _colorHandle.Identifier();
        RenderTargetIdentifier opaqueColorRT = _destination.Identifier();
        RenderTextureDescriptor opaqueDesc = ScriptableRenderer.CreateRenderTextureDescriptor(ref renderingData.cameraData, 1);
   
        cmd.GetTemporaryRT(_destination.id, opaqueDesc);
        cmd.Blit(colorRT, opaqueColorRT);
       
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);


        //draw custom distortion with transparent 

        cmd = CommandBufferPool.Get(transparentsTag);
        using (new ProfilingSample(cmd, transparentsTag))
        {
            var loadOp = RenderBufferLoadAction.Load;
            var storeOp = RenderBufferStoreAction.Store;
            SetRenderTarget(cmd, _colorHandle.Identifier(), loadOp, storeOp,
                _depthHandle.Identifier(), loadOp, storeOp, ClearFlag.None, Color.black, _baseDescriptor.dimension);

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            Camera camera = renderingData.cameraData.camera;
#if UNITY_2019_1_OR_NEWER
            DrawingSettings drawingSettings = new DrawingSettings(new ShaderTagId("CustomDistortion"), new SortingSettings(camera));
            FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.transparent);
            context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings);
#else
            var drawSettings = CreateDrawRendererSettings(camera, SortFlags.CommonTransparent, RendererConfiguration.None, renderingData.supportsDynamicBatching);
            var transparentFilterSettings = new FilterRenderersSettings(true) { renderQueueRange = RenderQueueRange.transparent };
            context.DrawRenderers(renderingData.cullResults.visibleRenderers, ref drawSettings, transparentFilterSettings);
#endif
        }

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void FrameCleanup(CommandBuffer cmd)
    {
        base.FrameCleanup(cmd);

        cmd.ReleaseTemporaryRT(_destination.id);
    }

}
#endif