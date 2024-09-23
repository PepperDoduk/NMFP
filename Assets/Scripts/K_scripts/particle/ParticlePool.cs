using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : MonoBehaviour
{
    public static ParticlePool Instance; 
    public GameObject particlePrefab; 
    public int poolSize = 10; 

    private List<GameObject> particlePool; 

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
        particlePool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject particle = Instantiate(particlePrefab);
            particle.SetActive(false);
            particlePool.Add(particle);
        }
    }


    public GameObject GetParticle()
    {
        foreach (GameObject particle in particlePool)
        {
            if (!particle.activeInHierarchy)
            {
                return particle; 
            }
        }

       
        GameObject newParticle = Instantiate(particlePrefab);
        newParticle.SetActive(false);
        particlePool.Add(newParticle);
        return newParticle;
    }
}
