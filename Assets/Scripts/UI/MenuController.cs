using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour     // This controls all menus/UI
{
    [SerializeField] private GameObject _winMenu;

    public GridPlayer gridPlayer;
    public GameObject _resetText;

    private bool won = false;

    public bool Won { get => won; set => won = value; }

    [SerializeField] private GameObject _pauseMenu;
    public GameObject _hudMenu;
    public GameObject _controlsText;

    public bool IsPaused;
    //public bool InSettings;

    public LookAt lookAt;
    public GameObject _startMessage;

    public bool inControls = false;


    public bool tutorial = false;
    public GameObject WASD;
    public GameObject SPRINT;
    public GameObject JUMP;
    public GameObject KICK;
    public GameObject RESET;
    public GameObject REWIND;
    public GameObject SWITCHGRID;
    public GameObject GRIDWASD;
    public GameObject SWITCHFREE;
    public GameObject GOAL;
    public GameObject BOXBUTTON;
    public bool text1 = false;
    public bool text2 = false;
    public bool text3 = false;
    public bool text4 = false;
    public bool text5 = false;
    public bool text6 = false;
    public bool text7 = false;     // These handle which tutorial text is on screen
    public bool text8 = false;
    public bool text9 = false;
    public bool text10 = false;
    public bool text11 = false;
    public BoxGridSnap box;     // This is only necessary for the tutorial level and can be assigned to a random box at the beginning

    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        PauseMode(false);

        _controlsText.SetActive(false);

        text1 = true;       // For tutorial beginning
    }

    void Update()
    {
        OnWin();
        CheckIfPaused();
        HudControl();
        StartMessage();
        ResetRewindMessage();

        if (inControls)
        {
            if (Input.anyKeyDown)
            {
                Resume();
                _controlsText.SetActive(false);
                inControls = false;
            }
        }

        if (tutorial)
        {
            TutorialHandler();
        }
    }

    private void TutorialHandler()
    {
        if (text1)      // Walking
        {
            WASD.SetActive(true);

            if (Input.GetKeyUp(KeyCode.D))        // I chose D since if theyre turning then they've probably moved a bit already
            {
                text2 = true;
                    
            }
        }

        if (text2)      // Sprint
        {
            text1 = false;
            WASD.SetActive(false);

            SPRINT.SetActive(true);

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {           // GetKeyUp so it won't disappear the moment they press it
                text3 = true;
                
            }
        }

        if (text3)      // Jump
        {
            text1 = false;

            text2 = false;
            SPRINT.SetActive(false);

            JUMP.SetActive(true);

            if (Input.GetKeyUp(KeyCode.Space))
            {           // GetKeyUp so it won't disappear the moment they press it
                text4 = true;
            }
        }

        if (text4)      // Kick
        {
            text1 = false;
            text3 = false;
            JUMP.SetActive(false);

            KICK.SetActive(true);

            if (Input.GetKeyUp(KeyCode.F))
            {           // GetKeyUp so it won't disappear the moment they press it
                text5 = true;
            }
        }

        if (text5)      // Reset
        {
            text1 = false;
            text4 = false;
            KICK.SetActive(false);

            RESET.SetActive(true);

            if (Input.GetKeyUp(KeyCode.R))
            {           // GetKeyUp so it won't disappear the moment they press it
                text6 = true;
            }
        }

        if (text6)      // Rewind
        {
            text1 = false;
            text5 = false;
            RESET.SetActive(false);

            REWIND.SetActive(true);

            if (Input.GetKeyUp(KeyCode.Return))
            {           // GetKeyUp so it won't disappear the moment they press it
                text7 = true;
            }
        }

        if (text7)      // Switch to Grid
        {
            text1 = false;
            text6 = false;
            REWIND.SetActive(false);


            SWITCHGRID.SetActive(true);

            if (Input.GetKeyUp(KeyCode.Mouse1))
            {           // GetKeyUp so it won't disappear the moment they press it
                text8 = true;
            }
        }

        if (text8)      // Grid Walking
        {
            text1 = false;
            text7 = false;
            SWITCHGRID.SetActive(false);

            GRIDWASD.SetActive(true);

            if (Input.GetKeyUp(KeyCode.D))
            {           // GetKeyUp so it won't disappear the moment they press it
                text9 = true;
            }
        }

        if (text9)      // Switch to Free
        {
            text1 = false;
            text8 = false;
            GRIDWASD.SetActive(false);

            SWITCHFREE.SetActive(true);

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {           // GetKeyUp so it won't disappear the moment they press it
                text10 = true;
            }
        }

        if (text10)      // Goal
        {
            text1 = false;
            text9 = false;
            SWITCHFREE.SetActive(false);

            GOAL.SetActive(true);

            if (box.gridSnapping)   // If the box is in the grid, move on
            {           
                text11 = true;
            }
        }

        if (text11)      // Box button
        {
            text1 = false;
            text10 = false;
            GOAL.SetActive(false);

            BOXBUTTON.SetActive(true);
        }
    }

    private void OnWin()
    {
        if (Won == true)                // Retrieve win status from the goal script "Victory"
        {
            _hudMenu.SetActive(false);
            Cursor.visible = true;
            _winMenu.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;

            
        }

        else
        {
            _hudMenu.SetActive(true);
            //Cursor.visible = false;
            _winMenu.SetActive(false);
            //Time.timeScale = 1;                           // Uncommenting the rest of these breaks the pause menu
            //Cursor.lockState = CursorLockMode.Locked;     // If it breaks again then dont bother with mouse, just use text
            // saying 'Enter to progress' and add the KeyDown input
        }

    }

    public void NextLevel()
    {
       // Debug.Log("Next Level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Next level is the current +1
    }

    private void CheckIfPaused()
    {
        if (Input.GetKeyDown(KeyCode.Escape))   // Esc to pause
        {
            TogglePause();
            PauseMode(IsPaused);
        }
    }

    public void TogglePause()
    {
        if (IsPaused)
        {
            IsPaused = false;
        }
        else
        {
            IsPaused = true;
        }
    }

    private void ChangeCursorMode(bool unlocked)
    {
        if (unlocked)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void PauseMode(bool paused)
    {
        IsPaused = paused;
        OnPause(paused);
        ChangeCursorMode(paused);
        // Comment Following If There's No Settings Menu
        //if (!paused)
        //{
        //    SetSettingsMode(false);
        //}
    }

    private void OnPause(bool paused)       // Pausing activates pause menu and stops game time
    {
        if (paused)
        {
            _pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            _pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    void HudControl()                   // If paused, no HUD
    {
        if (IsPaused)
        {
            _hudMenu.SetActive(false);
        }
        else
        {
            _hudMenu.SetActive(true);
        }
    }

    void StartMessage()
    {
        if (lookAt.introSequence == true)       // Retrieves the intro status from camera script "LookAt"
        {
            _startMessage.SetActive(true);      // Sets intro message active
            _hudMenu.SetActive(false);          // and HUD inactive
        }

        else if (lookAt.introSequence == false) // When in-game, set intro message inactive and activate HUD
        {
            _startMessage.SetActive(false);
            _hudMenu.SetActive(true);

            
        }
    }

    void ResetRewindMessage()
    {
        if (gridPlayer.Lifted)
        {
            _resetText.SetActive(true);
        }

        else if (!gridPlayer.Lifted)
        {
            _resetText.SetActive(false);
        }
    }
    /*
    public void ToggleSettings()
    {
        if (InSettings)
        {
            _pauseMenu.SetActive(true);
            SetSettingsMode(false);
        }
        else
        {
            _pauseMenu.SetActive(false);
            SetSettingsMode(true);
        }
    }

    private void SetSettingsMode(bool status)
    {
        InSettings = status;
        _settingsMenu.SetActive(status);
    }
    */

    public void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;                // On resume time moves again and the other UI elements are set inactive
        _pauseMenu.SetActive(false);
        _controlsText.SetActive(false);
    }

    public void ControlsText()
    {
        _pauseMenu.SetActive(false);
        _controlsText.SetActive(true);

        inControls = true;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        //Application.Quit();
        Debug.Log("Changing scene to main menu");
        SceneManager.LoadScene(0);        // On exit send them back to the main menu
#endif
    }
}
