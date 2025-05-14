using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
[ExecuteAlways]
[RequireComponent(typeof(Renderer))]
public class AutoTileByScale : MonoBehaviour
{
    public Vector2 tilingMultiplier = Vector2.one;

    private Vector3 _lastScale;

    private void OnEnable()
    {
        _lastScale = transform.lossyScale;
        UpdateTiling();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying && transform.lossyScale != _lastScale)
        {
            _lastScale = transform.lossyScale;
            UpdateTiling();
        }
#endif
    }

    private void UpdateTiling()
    {
        if (TryGetComponent<Renderer>(out Renderer renderer) && renderer.sharedMaterial != null)
        {
            Vector3 scale = transform.lossyScale;

            Vector2 tiling = new Vector2(
                scale.x * tilingMultiplier.x,
                scale.z * tilingMultiplier.y
            );

            renderer.sharedMaterial.mainTextureScale = tiling;

#if UNITY_EDITOR
            UnityEditor.SceneView.RepaintAll(); // Forzar actualización visual
#endif
        }
    }
}
