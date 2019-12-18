My email is "kripto289@gmail.com"
You can contact me for any questions.

My English is not very good, and if there are any translation errors, you can let me know :)


The package includes prefabs of mesh effects and demo scenes for pc/mobiles with characters and environment.

------------------------------------------------------------------------------------------------------------------------------------------

NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE NOTE !!!!


	1) Unity does NOT supported multiple submeshes with multiple materials to each submesh. 1 submesh = 1 material!
	For example, character model with 1 mesh (head, hands, body, etc) but with multiple materials (for head, for hands, for body).
	You can not add new material (for example lightning material) to all submeshes! 
	So, if your model have more then 2 submeshes, then material will be added only for last submesh. (for example only to head but not for hands and body)
	
	You must use splitted meshes. There is no performance or draw calls difference. But then you can use additional materials for all meshes.



------------------------------------------------------------------------------------------------------------------------------------------

NOTE for PC:

	If you want to use posteffect for PC like in the demo:

	1) Download unity free posteffects 
	https://assetstore.unity.com/packages/essentials/post-processing-stack-83912
	2) Add "PostProcessingBehaviour.cs" on main Camera.
	3) Set the "PostEffectsProfile" (the path "\Assets\KriptoFX\MeshEffect\ImagePostEffects\PostEffectsProfile.asset")
	4) You should turn on "HDR" on main camera for correct posteffects. 
	If you have forward rendering path (by default in Unity), you need disable antialiasing "edit->project settings->quality->antialiasing"
	or turn of "MSAA" on main camera, because HDR does not works with msaa. If you want to use HDR and MSAA then use "MSAA of post effect". 
	It's faster then default MSAA and have the same quality.



------------------------------------------------------------------------------------------------------------------------------------------
NOTE for MOBILES:

	For correct work on mobiles in your project scene you need:

	1) Add script "ME_MobileBloom.cs" on main camera and disable HDR on main camera. That all :)
	You need disable HDR on main camera for avoid rendering bug on unity 2018+ (maybe later it will be fixed by unity).

	This script will render scene to renderTexture with HDR format and use it for postprocessing. 
	It's faster then default any posteffects, because it's avoid "OnRenderImage" and overhead on cpu readback. 
	(a source https://forum.unity.com/threads/post-process-mobile-performance-alternatives-to-graphics-blit-onrenderimage.414399/#post-2759255)
	Also, I use RenderTextureFormat.RGB111110Float for mobile rendering and it have the same overhead like a default texture (RGBA32)

------------------------------------------------------------------------------------------------------------------------------------------

Effect USING:

In editor mode:

	1) Just drag & drop prefab to your object (a prefab of effect should be as child to your mesh). 
	2) Set this object to the property "Mesh Object" of script "PSMeshRendererUpdater".
	3) Click "Update Mesh Renderer".
	Particles and materials will be added to your object automatically. But if you want, you can add material and particles manually. 


In runtime mode:

	var currentInstance = Instantiate(Effect) as GameObject; 
	var psUpdater = currentInstance.GetComponent<PSMeshRendererUpdater>();
	psUpdater.UpdateMeshEffect(MeshObject);


For SCALING just change transform scale of mesh or use "StartScaleMultiplier" of script "PSMeshRendererUpdater"

------------------------------------------------------------------------------------------------------------------------------------------
Supported platforms:

	PC/Consoles/VR/Mobiles with directx9/11, opengles 2.0/3.0 and gamma/linear color space
	Supported SRP rendering. LightWeight render pipeline (LWRP) and HighDefinition render pipeline (HDRP)
	All effects tested on Oculus Rift CV1 with single and dual mode rendering and works perfect. 

------------------------------------------------------------------------------------------------------------------------------------------


If you have some questions, you can write me to email "kripto289@gmail.com" 