using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPRingController : MonoBehaviour
{
    public GameObject Text;
    public GameObject Camera;
    public Vector3 movement;
    private GameObject instanciateText;
    MovePlayer movePlayer;
    GameObject player;
    public GameObject cercle;
    bool isInTpRing;
    public bool isIn;
    Collider myCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        if (player != null)
        {
            movePlayer = player.GetComponent<MovePlayer>();
        }

        isInTpRing = false;
        myCollider = GetComponent<Collider>();
    }

    IEnumerator TeleportPlayer()
    {
        isInTpRing = false;
        player.GetComponent<Collider>().enabled = false;
        myCollider.enabled = false;
        yield return new WaitForFixedUpdate();
        player.transform.position = player.transform.position + movement;
        cercle.SetActive(false);
        myCollider.enabled = true;
        if (instanciateText != null)
                Destroy(instanciateText);
        player.GetComponent<Collider>().enabled = true;
    }

    void Update()
    {
        if (isIn && isInTpRing && Input.GetKeyDown(KeyCode.S)) {
            StartCoroutine(TeleportPlayer());
        }
        else if (!isIn && isInTpRing && Input.GetKeyDown(KeyCode.W)) {
            StartCoroutine(TeleportPlayer());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTpRing = false;
            instanciateText = Instantiate(Text, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.Euler(0f, Camera.transform.rotation.eulerAngles.y - 90f, 0f));
            cercle.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTpRing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cercle.SetActive(false);
            isInTpRing = false;
            if (instanciateText != null)
                Destroy(instanciateText);
        }
    }
}
