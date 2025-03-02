using UnityEngine;

namespace ExtractionAgent
{
    public class mouseposs : MonoBehaviour
    {
        Camera cam;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        }

        // Update is called once per frame
        void Update()
        {
           Ray  ray = cam.ScreenPointToRay(Input.mousePosition);
           if(Physics.Raycast(ray, out RaycastHit raycastHit))
           {
                transform.position = raycastHit.point;
           }
        }
    }
}
