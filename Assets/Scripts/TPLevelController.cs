using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class TPLevelController : MonoBehaviour
{
    public GameObject Text;
    public GameObject Camera;
    private GameObject instanciateText;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            instanciateText = Instantiate(Text, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.Euler(0f, Camera.transform.rotation.eulerAngles.y - 90f, 0f));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.Z))
        {
            other.transform.Translate(Vector3.up * 40f);
            Camera.transform.Translate(Vector3.up * 40f);
            Destroy(instanciateText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (instanciateText != null)
                Destroy(instanciateText);
        }
    }
}
