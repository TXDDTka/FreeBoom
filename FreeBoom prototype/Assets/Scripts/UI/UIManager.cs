using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        MenuPanel,
    }

    public PhotonTeams.Team team = PhotonTeams.Team.None;

    public CurrentPanel currentPanel = CurrentPanel.ChooseTeamPanel;

    public float timer = 0f;
    [SerializeField] private float _respawnTimer = 0f;
    private Coroutine _timerCoroutine;
    [SerializeField] private Camera _mainCamera = new Camera();
    [SerializeField] private GameObject _monitoringPanel = null;
    [SerializeField] private Image _redRespawnBar = null;
    [SerializeField] private Image _redLoadingBar = null;

    [SerializeField] private Image _blueRespawnBar = null;
    [SerializeField] private Image _blueLoadingBar = null;

    [SerializeField] private Text _redRespawnText = null;
    [SerializeField] private Text _blueRespawnText = null;

    public Button leaveGameBtn;
    public Button exitGameBtn;

    public bool respawn = false;
    public bool changing = false;

    [Serializable]
    public class PanelsList
    {
        public CurrentPanel panelName;
        public GameObject[] panelObjects;
        public Button[] panelButtons;
    //    public string[] buttonValue;
    //    public PhotonTeams.Team photonTeams;
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
         timer = _respawnTimer;
    }

    public void ChangePanel()
    {
        switch (currentPanel)
        {

            case CurrentPanel.ChooseTeamPanel:
                {
                    if (timer < _respawnTimer && timer > 0)
                    {
                        RespawnPanelOff();
                    }
                    foreach (var panel in panelsLists)
                    {
                        if (panel.panelName == currentPanel)
                        {
                            panel.panelObjects[0].SetActive(true);
                        }

                    }

                    break;
                }
            case CurrentPanel.ChooseCharacterPanel:
                {
                    if (_monitoringPanel.activeSelf == true)
                        _monitoringPanel.SetActive(false);

                    if (timer < _respawnTimer && timer > 0)
                    {
                        RespawnPanelOff();
                    }

                    foreach (var panel in panelsLists)
                    {
                        if (panel.panelName == currentPanel)
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
                        if (panel.panelName == currentPanel)
                        {
                            panel.panelObjects[0].SetActive(false);
                            panel.panelObjects[1].SetActive(false);
                            panel.panelObjects[2].SetActive(true);
                            _mainCamera.cullingMask = -1;
                        }
                    }

                    break;
                }
            case CurrentPanel.MenuPanel:
                {
                    foreach (var panel in panelsLists)
                    {
                        if (panel.panelName == currentPanel)
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
                            _mainCamera.cullingMask = 0;
                        }
                    }

                    break;
                }
        }
    }

   private void RespawnPanelOff()
    {
        respawn = false;
        StopCoroutine(_timerCoroutine);
        timer = _respawnTimer;
        RespawnStatus(false);

    }

    public void RespawnPanelOn()
    {
        respawn = true;
        currentPanel = CurrentPanel.MenuPanel;
        ChangePanel();
        RespawnStatus(true);
        _timerCoroutine = StartCoroutine(RespawnTimer());
    }



    public IEnumerator RespawnTimer()
    {

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            if (team == PhotonTeams.Team.Red)
            {
                _redRespawnText.text = string.Format("{0:0}", timer);
                _redLoadingBar.fillAmount = timer/ _respawnTimer;
            }
            else if (team == PhotonTeams.Team.Blue)
            {
                _blueRespawnText.text = string.Format("{0:0}", timer);
                _blueLoadingBar.fillAmount = timer / _respawnTimer;
            }


            yield return null;
        }

        if (timer < 0)
        {
            _mainCamera.cullingMask = -1;

            RespawnStatus(false);
            currentPanel = CurrentPanel.GamePanel;
            ChangePanel();
            timer = _respawnTimer;
        }
    }

    private void RespawnStatus(bool active)
    {
        if (team == PhotonTeams.Team.Red)
        {
            _redRespawnBar.gameObject.SetActive(active);
            _redLoadingBar.gameObject.SetActive(active);
            _redRespawnText.gameObject.SetActive(active);
        }
        else if (team == PhotonTeams.Team.Blue)
        {
            _blueRespawnBar.gameObject.SetActive(active);
            _blueLoadingBar.gameObject.SetActive(active);
            _blueRespawnText.gameObject.SetActive(active);
        }
    }

}
