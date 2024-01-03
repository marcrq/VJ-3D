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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            instanciateText = Instantiate(Text, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.Euler(0f, Camera.transform.rotation.eulerAngles.y - 90f, 0f));
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
            if (instanciateText != null)
                Destroy(instanciateText);
        }
    }
}
