using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }


    //public PhotonPlayerNetwork photonPlayerNetwork;
    // [SerializeField]private GameObject[] panels = null;


    public enum CurrentPanel
    {
        ChooseTeamPanel,
        ChooseCharacterPanel,
        GamePanel,
        MenuPanel
    }

    public CurrentPanel currentPanel = CurrentPanel.ChooseTeamPanel;

    public float timer = 0f;
    [SerializeField] private float respawnTimer = 0f;
    private Coroutine timerCoroutine;
    [SerializeField] private new Camera camera = null;
    [SerializeField] private GameObject monitoringPanel = null;
    [SerializeField] private Image redRespawnBar = null;
    [SerializeField] private Image redLoadingBar = null;

    [SerializeField] private Image blueRespawnBar = null;
    [SerializeField] private Image blueLoadingBar = null;

    [SerializeField] private Text redRespawnText = null;
    [SerializeField] private Text blueRespawnText = null;

    public string team = null;

    public Button leaveGameBtn;
    public Button exitGameBtn;

    public bool respawn = false;
    public bool changing = false;

    [Serializable]
    public class PanelsList
    {
        public string panelName;
        public GameObject[] panelObjects;
        public Button[] panelButtons;
        public string[] buttonValue;
    }

    public List<PanelsList> panelsLists = new List<PanelsList>();

    private void InitializeSingleton()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    void Awake()
    {
        InitializeSingleton();
         timer = respawnTimer;
    }

    public void ChangePanel()
    {
        switch (currentPanel)
        {

            case CurrentPanel.ChooseTeamPanel:
                {
                    if (timer < respawnTimer && timer > 0)
                    {
                        RespawnPanelOff();
                    }
                    foreach (var panel in panelsLists)
                    {
                        if (panel.panelName == currentPanel.ToString())
                        {
                            panel.panelObjects[0].SetActive(true);
                        }

                    }

                    break;
                }
            case CurrentPanel.ChooseCharacterPanel:
                {
                    if (monitoringPanel.activeSelf == true)
                        monitoringPanel.SetActive(false);

                    if (timer < respawnTimer && timer > 0)
                    {
                        RespawnPanelOff();
                    }

                    foreach (var panel in panelsLists)
                    {
                        if (panel.panelName == currentPanel.ToString())
                        {
                            panel.panelObjects[0].SetActive(false);
                            panel.panelObjects[1].SetActive(true);

                        }
                    }

                    break;
                }
            case CurrentPanel.GamePanel:
                {
                    foreach (var panel in panelsLists)
                    {
                        if (panel.panelName == currentPanel.ToString())
                        {
                            panel.panelObjects[0].SetActive(false);
                            panel.panelObjects[1].SetActive(false);
                            panel.panelObjects[2].SetActive(true);
                            camera.cullingMask = -1;
                            //if(changing)
                            //{
                            //    panel.panelObjects[3].SetActive(true);
                            //    changing = false;
                            //}
                        }
                    }

                    break;
                }
            case CurrentPanel.MenuPanel:
                {
                    foreach (var panel in panelsLists)
                    {
                        if (panel.panelName == currentPanel.ToString())
                        {
                            if(respawn)
                            {
                                panel.panelButtons[2].gameObject.SetActive(false);
                            }
                            else
                                panel.panelButtons[2].gameObject.SetActive(true);
                            panel.panelObjects[0].SetActive(false);
                            panel.panelObjects[1].SetActive(true);
                            panel.panelObjects[2].SetActive(true);
                            camera.cullingMask = 0;
                        }
                    }

                    break;
                }
        }
    }
            
   private void RespawnPanelOff()
    {
        respawn = false;
        StopCoroutine(timerCoroutine);
        timer = respawnTimer;
        RespawnStatus(false);

    }

    public void RespawnPanelOn()
    {
        respawn = true;
        currentPanel = CurrentPanel.MenuPanel;
        ChangePanel();
        RespawnStatus(true);
        timerCoroutine = StartCoroutine(RespawnTimer());
    }



    public IEnumerator RespawnTimer()
    {

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (team == "Red")
            {
                redRespawnText.text = string.Format("{0:0}", timer);
                redLoadingBar.fillAmount = timer/ respawnTimer;
            }
            else
            {
                blueRespawnText.text = string.Format("{0:0}", timer);
                blueLoadingBar.fillAmount = timer / respawnTimer;
            }
            

            yield return null;
        }

        if (timer < 0)
        {
            Camera.main.cullingMask = -1;

            RespawnStatus(false);
            currentPanel = CurrentPanel.GamePanel;
            ChangePanel();
            timer = respawnTimer;
        }
    }

    private void RespawnStatus(bool active)
    {
        if (team == "Red")
        {
            redRespawnBar.gameObject.SetActive(active);
            redLoadingBar.gameObject.SetActive(active);
            redRespawnText.gameObject.SetActive(active);
        }
        else
        {
            blueRespawnBar.gameObject.SetActive(active);
            blueLoadingBar.gameObject.SetActive(active);
            blueRespawnText.gameObject.SetActive(active);
        }
    }

}


