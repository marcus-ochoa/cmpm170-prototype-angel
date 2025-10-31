using UnityEditor.ShaderGraph.Internal;
using UnityEngine;


[ExecuteInEditMode]
public class EnemyEditor : MonoBehaviour
{
    [SerializeField] private float rayLength = 10.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * rayLength);
    }
}
