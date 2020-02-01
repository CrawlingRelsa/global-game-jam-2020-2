using UnityEngine;

public class PostProc : MonoBehaviour
{
	public Material material;
    public bool enabled;

    // Postprocess the image
    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit (source, destination, material);
    }

}
