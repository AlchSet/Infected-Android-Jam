using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugCommands : MonoBehaviour
{
    public Damageable player;

    public Toggle toggle;

    // Start is called before the first frame update
    void Start()
    {
        player.immortal = toggle.isOn;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }

    public void TogglePlayerImmunity()
    {
        player.immortal = toggle.isOn;
    }


    public void TeleportPlayerLv0()
    {
        Debug.Log("TEL");
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = new Vector3(4.5f,0.14f, -19.4f);
        player.GetComponent<CharacterController>().enabled = true;
    }

    public void TeleportPlayerLv1()
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = new Vector3(-60.31f, 0.14f, -11.3f);
        player.GetComponent<CharacterController>().enabled = true;
    }
    public void TeleportPlayerLv2()
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = new Vector3(-99.4f,0.14f, -13.4f);
        player.GetComponent<CharacterController>().enabled = true;
    }
    public void TeleportPlayerLv3()
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = new Vector3(-152.3f, 5.7f, -13.4f);
        player.GetComponent<CharacterController>().enabled = true;
    }
    public void TeleportPlayerLv4()
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = new Vector3(-191.73f,0.75f, -13.12f);
        player.GetComponent<CharacterController>().enabled = true;
    }

    public void TeleportPlayerLv5()
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = new Vector3(-250.5f, 0.75f, -13.12f);
        player.GetComponent<CharacterController>().enabled = true;
    }

}
