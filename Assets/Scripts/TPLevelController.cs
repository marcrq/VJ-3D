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

    void Start()
    {
        player = GameObject.Find("Player");
        if (player != null)
        {
            movePlayer = player.GetComponent<MovePlayer>();
        }
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

    IEnumerator TeleportPlayer(Collider other)
    {
        other.GetComponent<Collider>().enabled = false;
        yield return new WaitForFixedUpdate();
        if (movePlayer.level == 5) {
            Animator animator = player.GetComponent<Animator>();
            animator.SetTrigger("BossTrigger");
            other.GetComponent<Rigidbody>().MovePosition(other.transform.position + Vector3.up * 42f);
            Camera.transform.Translate(Vector3.up * 45f);
            Vector3 cameraRotation = Camera.transform.rotation.eulerAngles;
            cameraRotation.x += 20;
            Camera.transform.rotation = Quaternion.Euler(cameraRotation);
        }
        else {
            other.GetComponent<Rigidbody>().MovePosition(other.transform.position + Vector3.up * 42f);
            Camera.transform.Translate(Vector3.up * 40f);
        }
        other.GetComponent<Collider>().enabled = true;
        Destroy(instanciateText);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.Z))
        {
            ++movePlayer.level;
            StartCoroutine(TeleportPlayer(other));
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
