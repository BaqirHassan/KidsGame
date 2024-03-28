using UnityEngine;

[ExecuteInEditMode]
public class ChangeSortingOrder : MonoBehaviour
{
    [SerializeField] private Renderer m_renderer;
    [SerializeField] private string m_sortingLayer;
    void Awake()
    {
        if (m_renderer == null)
        {
            m_renderer = GetComponent<Renderer>();
            m_renderer.sortingLayerName = m_sortingLayer;
            m_renderer.sortingOrder = 2;
        }
    }

    void Update()
    {
        //Debug.Log("Editor causes this Update");
        m_renderer.sortingLayerName = m_sortingLayer;
    }

    public void SortingOrderChange()
    {
        m_renderer.sortingLayerName = m_sortingLayer;
    }
}
