using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public GameObject ChestObject;
    public GameObject Text;
    public GameObject Camera;
    private bool isOpened;
    private GameObject instanciateText;
    private GameObject instanciateObject;

    void Start()
    {
        isOpened = false;
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isOpened && other.CompareTag("Player"))
        {
            instanciateText = Instantiate(Text, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.Euler(0f, Camera.transform.rotation.eulerAngles.y - 90f, 0f));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isOpened || !other.CompareTag("Player"))
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            isOpened = true;
            Destroy(instanciateText);
            instanciateObject = Instantiate(ChestObject, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.identity);
            instanciateObject.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (instanciateText != null)
                Destroy(instanciateText);

            if (instanciateObject != null)
                Destroy(instanciateObject);
        }
    }
}
