using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using STRAFTAT_CC;

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
    public bool enableHealthESP = true;
    public bool enableDistanceESP = true;
    public bool enableNameESP = true;
    public bool showHealthBar = true;
    public bool showHealthNumber = true;

    // Exploits
    public bool InfiniteAmmo = false;
    public bool RapidFire = false;
    public bool InstaKill = false;
    public bool NoSpread = false;
    public bool WeaponSpeed = false;

    public bool Aimbot = false;
    public bool AutoShoot = false;
    public bool LegitAimbot = false;
    public bool LegitAutoShoot = false;
    public bool FlyMode = false;
    public bool GodMode = false;
    public bool MagicBullet = false;
    public bool FreezeEnemy = false;
    
    public bool TestEntityEnabled = false;
    public float TestEntityHeight = 1.7f;
    
    public float speedValue = 6f;
    public float jumpSpeedValue = 8f;
    public float jumpHeightValue = 2f;
    public float crouchSpeedValue = 3f;
    public float gravityValue = 20f;
    
    public bool enableSpeedHack = false;
    public bool infiniteJumpEnabled = false;
    
    public float legitSmoothing = 1.0f;
    public float legitFOV = 5.0f;
    public bool drawLegitFOV = false;
    
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
    private GUIStyle _panelStyle;
    private GUIStyle _tabStyle;
    private GUIStyle _activeTabStyle;
    private GUIStyle _toggleButtonStyle;
    private GUIStyle _toggleButtonActiveStyle;
    private GUIStyle _customToggleStyle;
    private GUIStyle _customToggleActiveStyle;
    private GUIStyle _warningStyle;

    public KeyCode boundKey_aimbot = KeyCode.E;
    public KeyCode boundKey_legitbot = KeyCode.Q;
    public KeyCode boundKey_createTestEntity = KeyCode.T;
    public bool waitingForKey = false;

    private Color _accentColor = new Color(0.2f, 0.6f, 1f);
    private Color _backgroundColor = new Color(0.12f, 0.12f, 0.12f, 0.95f);
    private Color _headerColor = new Color(0.15f, 0.15f, 0.15f, 1f);
    private Color _buttonColor = new Color(0.2f, 0.2f, 0.2f, 1f);
    private Color _buttonHoverColor = new Color(0.25f, 0.25f, 0.25f, 1f);
    private Color _toggleActiveColor = new Color(0.2f, 0.6f, 1f, 1f);
    private Color _warningColor = new Color(1f, 0.5f, 0f, 1f);

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
        _windowStyle = new GUIStyle(GUI.skin.window);
        _windowStyle.normal.background = MakeTexture(1, 1, _backgroundColor);
        _windowStyle.onNormal.background = MakeTexture(1, 1, _backgroundColor);
        _windowStyle.hover.background = MakeTexture(1, 1, _backgroundColor);
        _windowStyle.active.background = MakeTexture(1, 1, _backgroundColor);
        _windowStyle.focused.background = MakeTexture(1, 1, _backgroundColor);
        _windowStyle.onFocused.background = MakeTexture(1, 1, _backgroundColor);
        _windowStyle.normal.textColor = Color.white;
        _windowStyle.fontSize = 14;
        _windowStyle.fontStyle = FontStyle.Bold;
        _windowStyle.padding = new RectOffset(10, 10, 10, 10);

        _headerStyle = new GUIStyle(GUI.skin.label);
        _headerStyle.normal.background = MakeTexture(2, 2, _headerColor);
        _headerStyle.normal.textColor = Color.white;
        _headerStyle.fontSize = 16;
        _headerStyle.fontStyle = FontStyle.Bold;
        _headerStyle.alignment = TextAnchor.MiddleLeft;
        _headerStyle.padding = new RectOffset(10, 10, 5, 5);

        _buttonStyle = new GUIStyle(GUI.skin.button);
        _buttonStyle.normal.background = MakeTexture(2, 2, _buttonColor);
        _buttonStyle.hover.background = MakeTexture(2, 2, _buttonHoverColor);
        _buttonStyle.normal.textColor = Color.white;
        _buttonStyle.fontSize = 12;
        _buttonStyle.padding = new RectOffset(10, 10, 5, 5);

        _toggleStyle = new GUIStyle(GUI.skin.toggle);
        _toggleStyle.normal.textColor = Color.white;
        _toggleStyle.hover.textColor = Color.white;
        _toggleStyle.fontSize = 12;
        _toggleStyle.padding = new RectOffset(5, 5, 5, 5);

        _logStyle = new GUIStyle(GUI.skin.label);
        _logStyle.normal.textColor = Color.white;
        _logStyle.fontSize = 11;
        _logStyle.wordWrap = true;
        _logStyle.padding = new RectOffset(5, 5, 2, 2);

        _panelStyle = new GUIStyle(GUI.skin.box);
        _panelStyle.normal.background = MakeTexture(2, 2, _backgroundColor);
        _panelStyle.padding = new RectOffset(10, 10, 10, 10);

        _tabStyle = new GUIStyle(GUI.skin.button);
        _tabStyle.normal.background = MakeTexture(2, 2, _buttonColor);
        _tabStyle.hover.background = MakeTexture(2, 2, _buttonHoverColor);
        _tabStyle.normal.textColor = Color.white;
        _tabStyle.fontSize = 12;
        _tabStyle.padding = new RectOffset(10, 10, 5, 5);
        _tabStyle.margin = new RectOffset(2, 2, 2, 2);

        _activeTabStyle = new GUIStyle(_tabStyle);
        _activeTabStyle.normal.background = MakeTexture(2, 2, _toggleActiveColor);
        _activeTabStyle.hover.background = MakeTexture(2, 2, _toggleActiveColor);
        _activeTabStyle.normal.textColor = Color.white;

        _toggleButtonStyle = new GUIStyle(GUI.skin.button);
        _toggleButtonStyle.normal.background = MakeTexture(2, 2, _buttonColor);
        _toggleButtonStyle.hover.background = MakeTexture(2, 2, _buttonHoverColor);
        _toggleButtonStyle.normal.textColor = Color.white;
        _toggleButtonStyle.fontSize = 12;
        _toggleButtonStyle.padding = new RectOffset(10, 10, 5, 5);
        _toggleButtonStyle.margin = new RectOffset(2, 2, 2, 2);

        _toggleButtonActiveStyle = new GUIStyle(_toggleButtonStyle);
        _toggleButtonActiveStyle.normal.background = MakeTexture(2, 2, _toggleActiveColor);
        _toggleButtonActiveStyle.hover.background = MakeTexture(2, 2, _toggleActiveColor);

        _customToggleStyle = new GUIStyle(GUI.skin.button);
        _customToggleStyle.normal.background = MakeTexture(2, 2, _buttonColor);
        _customToggleStyle.hover.background = MakeTexture(2, 2, _buttonHoverColor);
        _customToggleStyle.active.background = MakeTexture(2, 2, _buttonHoverColor);
        _customToggleStyle.normal.textColor = Color.white;
        _customToggleStyle.fontSize = 12;
        _customToggleStyle.padding = new RectOffset(10, 10, 5, 5);
        _customToggleStyle.margin = new RectOffset(2, 2, 2, 2);

        _customToggleActiveStyle = new GUIStyle(_customToggleStyle);
        _customToggleActiveStyle.normal.background = MakeTexture(2, 2, _toggleActiveColor);
        _customToggleActiveStyle.hover.background = MakeTexture(2, 2, _toggleActiveColor);
        _customToggleActiveStyle.active.background = MakeTexture(2, 2, _toggleActiveColor);
        _customToggleActiveStyle.normal.textColor = Color.white;

        _warningStyle = new GUIStyle(GUI.skin.label);
        _warningStyle.normal.textColor = _warningColor;
        _warningStyle.fontSize = 12;
        _warningStyle.fontStyle = FontStyle.Bold;
        _warningStyle.padding = new RectOffset(5, 5, 0, 0);
    }

    private string currentMenu = "Ragebot";

    public void Draw()
    {
        // Force reinitialize styles if any are null
        if (_windowStyle == null || _headerStyle == null || _buttonStyle == null || 
            _toggleStyle == null || _logStyle == null || _panelStyle == null || 
            _tabStyle == null || _activeTabStyle == null || _toggleButtonStyle == null || 
            _toggleButtonActiveStyle == null || _customToggleStyle == null || 
            _customToggleActiveStyle == null || _warningStyle == null)
        {
            InitializeStyles();
        }

        int menuWidth = 200;
        int contentWidth = 400;

        GUILayout.BeginHorizontal(_windowStyle);

        // Left sidebar with tabs
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(menuWidth));
        DrawTab("Ragebot", "Ragebot");
        DrawTab("Legitbot", "Legitbot");
        DrawTab("Visuals", "Visuals");
        DrawTab("Exploits", "Exploits");
        DrawTab("Misc", "Misc");
        DrawTab("AntiAim", "AntiAim");
        DrawTab("Testing", "Testing");
        GUILayout.EndVertical();

        // Main content area
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(contentWidth));
        GUILayout.Label(currentMenu, _headerStyle);
        GUILayout.Space(10);
        DrawMenuContent();
        GUILayout.EndVertical();

        // Debug panel
        GUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(contentWidth));
        GUILayout.Label("Debug Log", _headerStyle);
        _debugScrollPosition = GUILayout.BeginScrollView(_debugScrollPosition);
        foreach (string log in _debugLogs)
        {
            GUILayout.Label(log, _logStyle);
        }
        GUILayout.EndScrollView();
        if (GUILayout.Button("Clear Logs", _toggleButtonStyle))
        {
            _debugLogs.Clear();
        }
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }

    private void DrawTab(string name, string menu)
    {
        GUIStyle style = currentMenu == menu ? _activeTabStyle : _tabStyle;
        if (GUILayout.Button(name, style))
        {
            currentMenu = menu;
        }
    }

    private void DrawToggleButton(string label, ref bool value)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label($"{label}:", _logStyle);
        GUIStyle style = value ? _customToggleActiveStyle : _customToggleStyle;
        if (GUILayout.Button(value ? "✓" : "", style, GUILayout.Width(20), GUILayout.Height(20)))
        {
            value = !value;
            AddDebugLog($"{label}: {value}");
        }
        GUILayout.EndHorizontal();
    }

    private void DrawMenuContent()
    {
        switch (currentMenu)
        {
            case "Ragebot":
                DrawRagebotContent();
                break;
            case "Legitbot":
                DrawLegitbotContent();
                break;
            case "Visuals":
                DrawVisualsContent();
                break;
            case "Exploits":
                DrawExploitsContent();
                break;
            case "Misc":
                DrawMiscContent();
                break;
            case "AntiAim":
                DrawAntiAimContent();
                break;
            case "Testing":
                DrawTestingContent();
                break;
        }
    }

    private void DrawRagebotContent()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        DrawToggleButton("Aimbot", ref Aimbot);
        DrawToggleButton("Auto Shoot", ref AutoShoot);
        
        GUILayout.Space(10);
        GUILayout.Label("Bound Key: " + boundKey_aimbot, _logStyle);
        
        if (GUILayout.Button("Change Key", _toggleButtonStyle))
        {
            waitingForKey = true;
        }
        
        if (waitingForKey)
        {
            GUILayout.Label("Waiting for key press...", _logStyle);
            Event e = Event.current;
            if (e.isKey)
            {
                boundKey_aimbot = e.keyCode;
                waitingForKey = false;
                AddDebugLog($"Set Rage Key to: {boundKey_aimbot}");
            }
        }
        GUILayout.EndVertical();
    }

    private void DrawLegitbotContent()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        DrawToggleButton("Legit Aimbot", ref LegitAimbot);
        DrawToggleButton("Legit Auto Shoot", ref LegitAutoShoot);
        DrawToggleButton("Draw FOV", ref drawLegitFOV);
        
        GUILayout.Space(10);
        GUILayout.Label("Smoothing: " + legitSmoothing.ToString("F1"), _logStyle);
        legitSmoothing = GUILayout.HorizontalSlider(legitSmoothing, 0.1f, 10f);
        
        GUILayout.Label("FOV: " + legitFOV.ToString("F1"), _logStyle);
        legitFOV = GUILayout.HorizontalSlider(legitFOV, 1f, 20f);
        
        GUILayout.Space(10);
        GUILayout.Label("Bound Key: " + boundKey_legitbot, _logStyle);
        
        if (GUILayout.Button("Change Key", _toggleButtonStyle))
        {
            waitingForKey = true;
        }
        
        if (waitingForKey)
        {
            GUILayout.Label("Waiting for input...", _logStyle);
            Event e = Event.current;
            if (e.isKey)
            {
                boundKey_legitbot = e.keyCode;
                waitingForKey = false;
                AddDebugLog($"Set Legit Key to: {boundKey_legitbot}");
            }
        }
        GUILayout.EndVertical();
    }

    private void DrawExploitsContent()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        DrawToggleButton("Rapid Fire", ref RapidFire);
        DrawToggleButton("No Spread", ref NoSpread);
        DrawToggleButton("Insta Kill", ref InstaKill);
        DrawToggleButton("Infinite Ammo", ref InfiniteAmmo);
        DrawToggleButton("Magic Bullet", ref MagicBullet);
        DrawToggleButton("God Mode [HOST]", ref GodMode);
        DrawToggleButton("Freeze Enemy [HOST]", ref FreezeEnemy);
        GUILayout.EndVertical();
    }

    private void DrawAntiAimContent()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.BeginHorizontal();
        GUILayout.Label("not working", _warningStyle);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        DrawToggleButton("Enable Slide", ref enableIsSlide);
        DrawToggleButton("Enable Grounded", ref enableIsGrounded);
        DrawToggleButton("Enable Sprinting", ref enableIsSprinting);
        DrawToggleButton("Enable Scope Aiming", ref enableIsScopeAiming);
        DrawToggleButton("Enable Leaning", ref enableIsLeaning);
        GUILayout.EndVertical();
    }

    private void DrawVisualsContent()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        DrawToggleButton("ESP", ref enableESP);
        
        if (enableESP)
        {
            GUILayout.Space(10);
            DrawToggleButton("3D Box ESP", ref enable3DBoxESP);
            DrawToggleButton("2D Box ESP", ref enable2DBoxESP);
            DrawToggleButton("Health ESP", ref enableHealthESP);
            DrawToggleButton("Distance ESP", ref enableDistanceESP);
            DrawToggleButton("Name ESP", ref enableNameESP);
            
            GUILayout.Space(10);
            DrawToggleButton("Show Health Bar", ref showHealthBar);
            DrawToggleButton("Show Health Number", ref showHealthNumber);
        }
        GUILayout.EndVertical();
    }

    private void DrawWeaponModsContent()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        DrawToggleButton("Weapon Speed", ref WeaponSpeed);
        GUILayout.EndVertical();
    }

    private void DrawMiscContent()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        DrawToggleButton("Speed Hack", ref enableSpeedHack);
        if (enableSpeedHack)
        {
            speedValue = GUILayout.HorizontalSlider(speedValue, 6f, 50f);
        }
        else
        {
            speedValue = 6f;
        }
        DrawToggleButton("Weapon Speed", ref WeaponSpeed);

        GUILayout.Label("Fly [Q: Down / E: Up]", _logStyle);
        DrawToggleButton("Fly Mode", ref FlyMode);

        GUILayout.Label("Teleport to Enemy:", _logStyle);
        if (GUILayout.Button("Teleport [Z]", _toggleButtonStyle))
        {
            AddDebugLog("Teleported");
        }
        GUILayout.EndVertical();
    }

    private void DrawTestingContent()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        DrawToggleButton("Test Entity", ref TestEntityEnabled);
        
        GUILayout.Space(10);
        if (GUILayout.Button("Create Test Entity", _toggleButtonStyle))
        {
            Cheat.Instance.Cache.TestEntity.CreateTestEntity();
        }
        
        GUILayout.Space(10);
        if (GUILayout.Button("Reload Menu Styles", _toggleButtonStyle))
        {
            InitializeStyles();
            AddDebugLog("Menu styles reloaded");
        }
        GUILayout.EndVertical();
    }

    public void AddDebugLog(string message)
    {
        _debugLogs.Add($"[{System.DateTime.Now.ToString("HH:mm:ss")}] {message}");
        if (_debugLogs.Count > MAX_LOGS)
            _debugLogs.RemoveAt(0);
    }
}
