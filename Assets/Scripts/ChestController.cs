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
    bool isInChest;

    void Start()
    {
        isOpened = false;
        isInChest = false;
    }

    void Update()
    {
        if (!isOpened && isInChest && Input.GetKeyDown(KeyCode.Z))
        {
            isOpened = true;
            isInChest = false;
            Destroy(instanciateText);
            instanciateObject = Instantiate(ChestObject, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.identity);
            instanciateObject.transform.parent = transform;
        }
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
            isInChest = false;
            return;
        }

        isInChest = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInChest = false;
            if (instanciateText != null)
                Destroy(instanciateText);

            if (instanciateObject != null)
                Destroy(instanciateObject);
        }
    }
}
