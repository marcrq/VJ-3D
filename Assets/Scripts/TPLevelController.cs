using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class TPLevelController : MonoBehaviour
{
    public GameObject Text;
    public GameObject Camera;
    private GameObject instanciateText;
    MovePlayer movePlayer;
    GameObject player;
    public GameObject cercle;
    bool isInTp;

    void Start()
    {
        player = GameObject.Find("Player");
        if (player != null)
        {
            movePlayer = player.GetComponent<MovePlayer>();
        }

        isInTp = false;
    }

    void Update()
    {
        if (isInTp && Input.GetKeyDown(KeyCode.Z)) {
            isInTp = false;
            ++movePlayer.level;
            StartCoroutine(TeleportPlayer());
        }

        if (Input.GetKeyDown(KeyCode.B) && movePlayer.level == 1) {
            isInTp = false;
            movePlayer.level = 5;
            StartCoroutine(TeleportToBoss());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            instanciateText = Instantiate(Text, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.Euler(0f, Camera.transform.rotation.eulerAngles.y - 90f, 0f));
            cercle.SetActive(true);
        }
    }

    IEnumerator TeleportPlayer()
    {

        player.GetComponent<Collider>().enabled = false;

        yield return new WaitForFixedUpdate();
        if (movePlayer.level == 5) {
            Animator animator = player.GetComponent<Animator>();
            animator.SetTrigger("BossTrigger");
            player.transform.Translate(Vector3.up * 42f);
            Camera.transform.Translate(Vector3.up * 42f);
            Vector3 cameraRotation = Camera.transform.rotation.eulerAngles;
            cameraRotation.x += 20;
            Camera.transform.rotation = Quaternion.Euler(cameraRotation);
        }
        else {
            player.transform.Translate(Vector3.up * 42f);
            Camera.transform.Translate(Vector3.up * 40f);
        }
        
        player.GetComponent<Collider>().enabled = true;
        Destroy(instanciateText);
        cercle.SetActive(false);
    }

    IEnumerator TeleportToBoss()
    {

        player.GetComponent<Collider>().enabled = false;

        yield return new WaitForFixedUpdate();

        Animator animator = player.GetComponent<Animator>();
        animator.SetTrigger("BossTrigger");
        Vector3 movement = new Vector3(0.0f, 162f, 0.0f);
        player.transform.Translate(movement);
        Camera.transform.Translate(movement);
        Vector3 cameraRotation = Camera.transform.rotation.eulerAngles;
        cameraRotation.x += 20;
        Camera.transform.rotation = Quaternion.Euler(cameraRotation);
        
        player.GetComponent<Collider>().enabled = true;
        cercle.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTp = false;
            cercle.SetActive(false);
            if (instanciateText != null)
                Destroy(instanciateText);
        }
    }
}
