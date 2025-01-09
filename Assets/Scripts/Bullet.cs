using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.ParticleSystemJobs;

namespace TaskMaster
{
    public class Bullet : MonoBehaviour
    {
    [SerializeField]private ParticleSystem ps;
    [SerializeField]private TrailRenderer tr;
        
        private void Awake(){
            Destroy(gameObject,5f);
            ps.gameObject.SetActive(true);
            tr.enabled = true;
        }

        private void OnCollisionEnter(Collision collision){
            ps.gameObject.SetActive(false);
            tr.enabled = false;
            Destroy(gameObject,3f);
        }
    }
}
