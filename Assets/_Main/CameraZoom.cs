using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    public float targetZoom = 5f; // Nuevo ortho size deseado
    public float zoomSpeed = 2f;  // Qué tan rápido se interpola
    public Transform offsetObject;

    private CinemachineVirtualCamera vcam;
    //private bool playerInside = false;

    // Start is called before the first frame update
    void Start()
    {
        vcam = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //playerInside = true;
            StopAllCoroutines();
            StartCoroutine(ZoomIn());
            vcam.Follow = offsetObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //playerInside = false;
            StopAllCoroutines();
            StartCoroutine(ZoomOut());
            vcam.Follow = other.transform;
        }
    }

    IEnumerator ZoomIn()
    {
        while (Mathf.Abs(vcam.m_Lens.OrthographicSize - targetZoom) > 0.05f)
        {
            vcam.m_Lens.OrthographicSize = Mathf.Lerp(vcam.m_Lens.OrthographicSize, targetZoom, Time.deltaTime * zoomSpeed);
            yield return null;
        }
    }

    IEnumerator ZoomOut()
    {
        float defaultZoom = 7f; // tamaño original
        while (Mathf.Abs(vcam.m_Lens.OrthographicSize - defaultZoom) > 0.05f)
        {
            vcam.m_Lens.OrthographicSize = Mathf.Lerp(vcam.m_Lens.OrthographicSize, defaultZoom, Time.deltaTime * zoomSpeed);
            yield return null;
        }
    }
}
