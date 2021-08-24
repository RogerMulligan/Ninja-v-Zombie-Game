using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CustomExtensions;

public class GameBehavior : MonoBehaviour, IManager
{
    private string _state;

    public string State
    {
        get
        {
            return _state;
        }
        set
        {
            _state = value;
        }

    }

    public string labelText = "Collect all 4 items and win your freedom!";

    public int maxItems = 4;
    public bool showWinScreen = false;
    public bool showLossScreen = false;

    public Stack<string> lootStack = new Stack<string>();

    private int _itemsCollected = 0;
    public int Items
    {
        get
        {
            return _itemsCollected;
        }
        set
        {
            _itemsCollected = value;
            Debug.LogFormat("Items: {0}", _itemsCollected);

            if (_itemsCollected >= maxItems)
            {
                labelText = "You have found all the items";
                showWinScreen = true;
                Time.timeScale = 0f;
            }
            else
            {
                labelText = "Item found, only" + (maxItems - _itemsCollected) + " more to go!";
            }
        }

    }

    private int _playerHP = 10;
    public int HP
    {
        get
        {
            return _playerHP;
        }
        set
        {
            _playerHP = value;
            if (_playerHP <= 0)
            {
                PlayerLost();
            }
            else
            {
                labelText = "Ouch...that has got to hurt. ";
            }
            Debug.LogFormat("Lives: {0}", _playerHP);
        }
    }

    public delegate void DebugDelegate(string newText);
    public DebugDelegate debug = Print;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        InventoryList<string> inventoryList = new InventoryList<string>();
        inventoryList.SetItem("Potion");
        Debug.Log(inventoryList.item);
    }

    // IManager and CustomExtensions
    public void Initialize()
    {
        _state = "Manager intialized...";
        _state.FancyDebug();
        debug(_state);
        LogWithDelegate(debug);

        GameObject player = GameObject.Find("Player");
        PlayerBehaviour playerBehaviour = player.GetComponent<PlayerBehaviour>();
        playerBehaviour.playerJump += HandlePlayerJump;
    }

    public void HandlePlayerJump()
    {
        debug("Player has jumped...");
    }

    public static void Print(string newText)
    {
        Debug.Log(newText);
    }

    public void LogWithDelegate(DebugDelegate del)
    {
        del("Delegating the debug task...");
    }
    //On Screen player health and items collected ( Top Left of screen)
    private void OnGUI()
    {
        GUI.Box(new Rect(20, 20, 150, 25), "Player Health: " + _playerHP);
        GUI.Box(new Rect(20, 50, 150, 25), "Items Collected: " + _itemsCollected);
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 50, 300, 50), labelText);

        // if player is wins:
        if (showWinScreen)
        {
            if (GUI.Button(new Rect(Screen.width/2 -100, Screen.height/2 - 50, 200, 100), "YOU WON!"))
            {
                Utilities.RestartLevel(0);
            }
        }
        // if player loses
        if (showLossScreen)
        {
            if (GUI.Button(new Rect(Screen.width / 2 -100, Screen.height / 2 -50, 200, 100), "You lose..."))
            {
                Utilities.RestartLevel(-1);
            }

            try
            {
                Utilities.RestartLevel(-1);
                debug("Level restarted successfully...");
            }
            catch(System.ArgumentException e)
            {
                Utilities.RestartLevel(0);
                debug("Reverting to scene: " + e.ToString());
            }
            finally
            {
                debug("Restart handled...");
            }
        }

        
    }
    

    void PlayerLost()
    {
        labelText = "You want another life with that?";
        showLossScreen = true;
        Time.timeScale = 0;
    }

  

    // Update is called once per frame
    void Update()
    {
        
    }

    void IManager.Initilize()
    {
        throw new System.NotImplementedException();
    }
}
