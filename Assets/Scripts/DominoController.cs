using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject dominoes;
    [SerializeField] private GameObject dominoesPrefab;
    private Vector3 position;
    private Quaternion rotation;
    void Start()
    {
        position = dominoes.transform.position;
        rotation = dominoes.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetDominoes()
    {
        Destroy(dominoes);
        dominoes = Instantiate(dominoesPrefab,position,rotation);
    }
}
