using ObjectPool;
using UnityEngine;

namespace RayCastSystem.Highlight.Outline
{
    public class OutlineView : MonoBehaviour, IPoolable
    {
        private const string ThicknessPropertyName = "thickness";

        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private MeshFilter _meshFilter;
        
        private readonly int _thicknessPropertyId = Shader.PropertyToID(ThicknessPropertyName);

        public void ApplyTo(Transform target, OutlineData data)
        {
            transform.SetParent(target, false);
            transform.localPosition = Vector3.zero;
            
            _meshFilter.mesh = data.Mesh;
            _meshRenderer.material.SetFloat(_thicknessPropertyId, data.Thickness);
        }

        void IPoolable.ResetDefaults()
        {
            _meshFilter.mesh = null;
            _meshRenderer.material.SetFloat(_thicknessPropertyId, 0);
        }
    }
}