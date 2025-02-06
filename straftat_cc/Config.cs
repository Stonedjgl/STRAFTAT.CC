using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class Config
{
    public static Config Instance = new Config();


    private Vector2 _debugScrollPosition = Vector2.zero;
    private List<string> _debugLogs = new List<string>();
    private const int MAX_LOGS = 50;
    private bool _showDebugWindow = true;
    private bool _showCombat = true;
    private bool _showVisuals = true;
    private bool _showWeaponMods = true;
    private bool _showMisc = true;
    private bool _showDebug = true;


    // Visuals
    public bool enableESP = true;
    public bool enable3DBoxESP = true;
    public bool enable2DBoxESP = false;

    // Exploits
    public bool InfiniteAmmo = false;
    public bool RapidFire = false;
    public bool InstaKill = false;
    public bool NoSpread = false;
    public bool WeaponSpeed = false;

    public bool Aimbot = false;
    public bool AutoShoot = false;
    public bool FlyMode = false;
    public bool GodMode = false;
    public bool MagicBullet = false;
    public bool FreezeEnemy = false;

    // Player modifiable values
    public float speedValue = 6f;
    public float jumpSpeedValue = 8f;
    public float jumpHeightValue = 2f;
    public float crouchSpeedValue = 3f;
    public float gravityValue = 20f;

    // Flags to toggle hacks
    public bool enableSpeedHack = false;
    public bool infiniteJumpEnabled = false;

    // testing menu
    public bool enableIsSlide = false;
    public bool enableIsGrounded = false;
    public bool enableIsSprinting = false;
    public bool enableIsScopeAiming = false;
    public bool enableIsLeaning = false;

    private GUIStyle _windowStyle;
    private GUIStyle _headerStyle;
    private GUIStyle _buttonStyle;
    private GUIStyle _toggleStyle;
    private GUIStyle _logStyle;

    public KeyCode boundKey_aimbot = KeyCode.E;
    public bool waitingForKey = false;

    private Texture2D MakeTexture(int width, int height, Color color)
    {
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = color;

        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    private void InitializeStyles()
    {
        if (_windowStyle == null)
        {
            _windowStyle = new GUIStyle(GUI.skin.window);
            _windowStyle.normal.background = MakeTexture(2, 2, new Color(0.1f, 0.1f, 0.1f, 0.95f));
            _windowStyle.normal.textColor = Color.white;
            _windowStyle.fontSize = 14;
            _windowStyle.fontStyle = FontStyle.Bold;
            _headerStyle = new GUIStyle(GUI.skin.button);
            _headerStyle.normal.background = MakeTexture(2, 2, new Color(0.2f, 0.2f, 0.2f, 1));
            _headerStyle.hover.background = MakeTexture(2, 2, new Color(0.25f, 0.25f, 0.25f, 1));
            _headerStyle.normal.textColor = Color.cyan;
            _headerStyle.fontSize = 12;
            _headerStyle.fontStyle = FontStyle.Bold;
            _headerStyle.alignment = TextAnchor.MiddleLeft;
            _headerStyle.padding = new RectOffset(10, 10, 5, 5);
            _toggleStyle = new GUIStyle(GUI.skin.toggle);
            _toggleStyle.normal.textColor = Color.white;
            _toggleStyle.onNormal.textColor = Color.white;
            _toggleStyle.hover.textColor = Color.white;
            _toggleStyle.onHover.textColor = Color.white;
            _toggleStyle.fontSize = 12;
            _toggleStyle.padding = new RectOffset(20, 5, 5, 5);
            _toggleStyle.margin = new RectOffset(5, 5, 5, 5);
            _buttonStyle = new GUIStyle(GUI.skin.button);
            _buttonStyle.normal.background = MakeTexture(2, 2, new Color(0.2f, 0.2f, 0.2f, 1));
            _buttonStyle.hover.background = MakeTexture(2, 2, new Color(0.3f, 0.3f, 0.3f, 1));
            _buttonStyle.normal.textColor = Color.white;
            _buttonStyle.fontSize = 12;
            _logStyle = new GUIStyle(GUI.skin.label);
            _logStyle.normal.textColor = Color.white;
            _logStyle.fontSize = 11;
            _logStyle.wordWrap = true;
        }
    }

    private string currentMenu = "Aimbot";

    public void Draw()
    {
        InitializeStyles();
        int buttonWidth = 150;
        int buttonHeight = 50;
        int menuWidth = 200;

        // Left-side button panel
        GUI.Box(new Rect(10, 20, menuWidth, 550), "NIGGALOSE");

        if (GUI.Button(new Rect(30, 50, buttonWidth, buttonHeight), "Aimbot"))
            currentMenu = "Aimbot";

        if (GUI.Button(new Rect(30, 110, buttonWidth, buttonHeight), "Visuals"))
            currentMenu = "Visuals";

        if (GUI.Button(new Rect(30, 170, buttonWidth, buttonHeight), "Exploits"))
            currentMenu = "Exploits";

        if (GUI.Button(new Rect(30, 230, buttonWidth, buttonHeight), "Misc"))
            currentMenu = "Misc";
        if (GUI.Button(new Rect(30, 290, buttonWidth, buttonHeight), "AntiAim"))
            currentMenu = "AntiAim";

        // Right-side content panel
        GUI.Box(new Rect(menuWidth + 30, 20, 400, 550), "TAB: " + currentMenu);
        GUI.Label(new Rect(menuWidth + 50, 50, 280, 200), GetMenuContent());

        GUI.Box(new Rect(650, 20, 400, 550), "DEBUG LOG");

        // Set up a scrollable area inside the "DEBUG LOG" box
        _debugScrollPosition = GUI.BeginScrollView(new Rect(660, 50, 380, 450), _debugScrollPosition, new Rect(0, 0, 360, _debugLogs.Count * 20));

        for (int i = 0; i < _debugLogs.Count; i++)
        {
            GUI.Label(new Rect(10, i * 20, 360, 20), _debugLogs[i], _logStyle);
        }

        GUI.EndScrollView();

        // Clear Logs Button
        if (GUI.Button(new Rect(660, 510, 380, 30), "Clear Logs"))
        {
            _debugLogs.Clear();
        }
    }

    string GetMenuContent()
    {
        switch (currentMenu)
        {
            case "Aimbot":
                return AimbotContents();
            case "Visuals":
                return GetVisualsContent();
            case "Exploits":
                return GetExploitsContent();
            case "Misc":
                return GetMiscContent();
            default:
                return "Select a category.";
            case "AntiAim":
                return AntiAimMenu();
        }
    }

    string AimbotContents()
    {
        if (GUI.Button(new Rect(300, 50, 250, 40), $"Aimbot [{boundKey_aimbot}]: {Aimbot}"))
            Aimbot = !Aimbot;
        if (GUI.Button(new Rect(300, 100, 250, 40), $"Enable Auto Shoot: {AutoShoot}"))
            AutoShoot = !AutoShoot;
        // Display current keybind
        GUI.Label(new Rect(300, 150, 250, 40), "Bound Key: " + boundKey_aimbot);

        // Button to change keybind
        if (GUI.Button(new Rect(300, 200, 250, 40), waitingForKey ? "Press a Key..." : "Change Key"))
        {
            waitingForKey = true;  // Enter input mode
        }
        if (waitingForKey)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                boundKey_aimbot = e.keyCode;  // Assign new key
                waitingForKey = false; // Exit input mode
            }
        }
        return "";
    }

    string GetVisualsContent()
    {
        // Visual Toggles
        if (GUI.Button(new Rect(300, 50, 250, 40), $"Enable ESP: {enableESP}"))
            enableESP = !enableESP;

        if (GUI.Button(new Rect(300, 100, 250, 40), $"Enable 3D Box ESP: {enable3DBoxESP}"))
            enable3DBoxESP = !enable3DBoxESP;
        return "";  // No need to return any text now
    }

    string GetExploitsContent()
    {
        // Exploits Toggles
        if (GUI.Button(new Rect(300, 50, 250, 40), $"Rapid Fire: {RapidFire}"))
            RapidFire = !RapidFire;

        if (GUI.Button(new Rect(300, 100, 250, 40), $"No Spread: {NoSpread}"))
            NoSpread = !NoSpread;

        if (GUI.Button(new Rect(300, 150, 250, 40), $"Insta Kill: {InstaKill}"))
            InstaKill = !InstaKill;

        if (GUI.Button(new Rect(300, 200, 250, 40), $"Infinite Ammo: {InfiniteAmmo}"))
            InfiniteAmmo = !InfiniteAmmo;

        if (GUI.Button(new Rect(300, 250, 250, 40), $"WeaponSpeed: {WeaponSpeed}"))
            WeaponSpeed = !WeaponSpeed;

        return "";  // No need to return any text now
    }

    string GetMiscContent()
    {
        if (GUI.Button(new Rect(300, 50, 250, 20), $"Infinite Jump: {infiniteJumpEnabled}"))
            infiniteJumpEnabled = !infiniteJumpEnabled;

        GUI.Label(new Rect(300, 100, 250, 20), "Speed:");
        speedValue = GUI.HorizontalSlider(new Rect(300, 120, 250, 20), speedValue, 6f, 50f);
        if (GUI.Button(new Rect(300, 150, 250, 20), $"Enable Speed Hack: {enableSpeedHack}"))
            enableSpeedHack = !enableSpeedHack;

        GUI.Label(new Rect(300, 180, 250, 20), "Fly:");
        if (GUI.Button(new Rect(300, 200, 250, 20), $"Enable Fly Hack: {FlyMode}"))
            FlyMode = !FlyMode;

        GUI.Label(new Rect(300, 230, 250, 20), "Godmode:");
        if (GUI.Button(new Rect(300, 250, 250, 20), $"Enable Godmode: {GodMode}"))
            GodMode = !GodMode;

        GUI.Label(new Rect(300, 280, 250, 20), "Magic Bullet:");
        if (GUI.Button(new Rect(300, 300, 250, 20), $"Enable Magic Bullet: {MagicBullet}"))
            MagicBullet = !MagicBullet;

        GUI.Label(new Rect(300, 330, 250, 20), "Teleport to Enemy:");
        if (GUI.Button(new Rect(300, 350, 250, 20), $"Teleport"))
        {
        }

        return "";  // No need to return any text now
    }

    string AntiAimMenu()
    {
        if (GUI.Button(new Rect(300, 50, 250, 20), $"IsSlide: {enableIsSlide}"))
            enableIsSlide = !enableIsSlide;
        /*
        if (GUI.Button(new Rect(250, 100, 250, 20), $"IsGrounded: {enableIsGrounded}"))
            enableIsGrounded = !enableIsGrounded;
        if (GUI.Button(new Rect(250, 150, 250, 20), $"IsSprinting: {enableIsSprinting}"))
            enableIsSprinting = !enableIsSprinting;
        if (GUI.Button(new Rect(250, 200, 250, 20), $"IsScopeAiming: {enableIsScopeAiming}"))
            enableIsScopeAiming = !enableIsScopeAiming;
        if (GUI.Button(new Rect(250, 250, 250, 20), $"IsLeaning: {enableIsLeaning}"))
            enableIsLeaning = !enableIsLeaning;
        */
        return "";
    }
    public void AddDebugLog(string message)
    {
        _debugLogs.Insert(0, $"[{Time.time:F1}] {message}");
        if (_debugLogs.Count > MAX_LOGS)
            _debugLogs.RemoveAt(_debugLogs.Count - 1);
    }
}
