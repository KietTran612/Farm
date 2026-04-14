using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;

/// <summary>
/// Farm Game — Phase 0 Scene Setup
/// Menu: Farm > Setup Phase 0 Scene
/// </summary>
public static class FarmSceneSetup
{
    // ── Sprite paths ──────────────────────────────────────────────────────────
    private const string SpritesRoot  = "Assets/Textures/Shapes/";
    private const string SprRect      = SpritesRoot + "Rectangle.png";
    private const string SprSquircle  = SpritesRoot + "Squircle.png";
    private const string SprRectOut   = SpritesRoot + "Rectangle-Outline.png";
    private const string SprCircle    = SpritesRoot + "Circle.png";
    private const string SprSquare    = SpritesRoot + "Square.png";

    // ── Colours ───────────────────────────────────────────────────────────────
    private static readonly Color ColFarmBg      = HexColor("#3A6B35");   // dark green farm
    private static readonly Color ColHudBg       = new Color(0.08f, 0.08f, 0.08f, 0.82f);
    private static readonly Color ColToolbarBg   = new Color(0.10f, 0.10f, 0.10f, 0.92f);
    private static readonly Color ColSlotDefault = new Color(1f, 1f, 1f, 0.08f);
    private static readonly Color ColModeBarBg   = new Color(0.10f, 0.10f, 0.10f, 0.90f);
    private static readonly Color ColSeedTrayBg  = new Color(0.10f, 0.10f, 0.10f, 0.92f);
    private static readonly Color ColSheetBg     = new Color(0.13f, 0.13f, 0.13f, 0.97f);
    private static readonly Color ColHandle      = new Color(0.5f,  0.5f,  0.5f,  0.6f);
    private static readonly Color ColText        = new Color(0.95f, 0.95f, 0.95f, 1f);
    private static readonly Color ColTextDim     = new Color(0.7f,  0.7f,  0.7f,  1f);
    private static readonly Color ColBtnClose    = HexColor("#FF4444CC");
    private static readonly Color ColSideBtnBg   = new Color(0.10f, 0.10f, 0.10f, 0.88f);

    // ─────────────────────────────────────────────────────────────────────────
    [MenuItem("Farm/Setup Phase 0 Scene")]
    public static void SetupPhase0Scene()
    {
        // Load sprites
        Sprite sprRect     = LoadSprite(SprRect);
        Sprite sprSquircle = LoadSprite(SprSquircle);
        Sprite sprRectOut  = LoadSprite(SprRectOut);
        Sprite sprCircle   = LoadSprite(SprCircle);
        Sprite sprSquare   = LoadSprite(SprSquare);

        // Set 9-slice borders
        SetSpriteBorder(SprRect,     70, 70, 70, 70);
        SetSpriteBorder(SprSquircle, 128,128,128,128);
        SetSpriteBorder(SprRectOut,  80, 80, 80, 80);

        // ── 0.1  Canvas ───────────────────────────────────────────────────────
        GameObject canvasGO = CreateCanvas();

        // ── 0.2  FarmWorld ────────────────────────────────────────────────────
        GameObject farmWorld = CreateFarmWorld(canvasGO);

        // ── 0.3  TopHUD ───────────────────────────────────────────────────────
        GameObject topHud = CreateTopHUD(canvasGO, sprRect, sprSquircle, sprCircle);

        // ── 0.4  SideButtons_Right ────────────────────────────────────────────
        GameObject sideButtons = CreateSideButtons(canvasGO, sprSquircle);

        // ── 0.5 + 0.6  BottomToolbar ─────────────────────────────────────────
        GameObject toolbar = CreateBottomToolbar(canvasGO, sprRect, sprSquircle);

        // ── 0.7  ModeBar (hidden) ─────────────────────────────────────────────
        GameObject modeBar = CreateModeBar(canvasGO, sprRect, sprSquircle);

        // ── 0.8  SeedTray (hidden) ────────────────────────────────────────────
        GameObject seedTray = CreateSeedTray(canvasGO, sprRect);

        // ── 0.9  BottomSheet (hidden) ─────────────────────────────────────────
        GameObject bottomSheet = CreateBottomSheet(canvasGO, sprRect, sprSquare);

        // ── Popup placeholder (hidden) ────────────────────────────────────────
        GameObject popup = CreatePopupPlaceholder(canvasGO, sprRect);

        // ── 0.10  Enforce sibling z-order ─────────────────────────────────────
        farmWorld.transform.SetSiblingIndex(0);
        topHud.transform.SetSiblingIndex(1);
        sideButtons.transform.SetSiblingIndex(2);
        toolbar.transform.SetSiblingIndex(3);
        modeBar.transform.SetSiblingIndex(4);
        seedTray.transform.SetSiblingIndex(5);
        bottomSheet.transform.SetSiblingIndex(6);
        popup.transform.SetSiblingIndex(7);

        // ── 0.11  Verify FarmWorld full-screen ────────────────────────────────
        var rt = farmWorld.GetComponent<RectTransform>();
        bool ok = rt.anchorMin == Vector2.zero && rt.anchorMax == Vector2.one
               && rt.offsetMin == Vector2.zero && rt.offsetMax == Vector2.zero;
        Debug.Log(ok
            ? "[FarmSetup] 0.11 PASS — FarmWorld is full-screen."
            : "[FarmSetup] 0.11 FAIL — FarmWorld not full-screen!");

        EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log("[FarmSetup] Phase 0 complete.");
    }

    // ─────────────────────────────────────────────────────────────────────────
    // 0.1  Canvas — 1080×1920, ScaleWithScreenSize, match 0.5
    // ─────────────────────────────────────────────────────────────────────────
    static GameObject CreateCanvas()
    {
        var existing = GameObject.Find("Canvas");
        if (existing != null) Object.DestroyImmediate(existing);

        var go = new GameObject("Canvas");
        var canvas = go.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var scaler = go.AddComponent<CanvasScaler>();
        scaler.uiScaleMode            = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution    = new Vector2(1080, 1920);
        scaler.screenMatchMode        = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight     = 0.5f;

        go.AddComponent<GraphicRaycaster>();
        return go;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // 0.2  FarmWorld — full-screen solid colour, no sprite (avoids stretch)
    // ─────────────────────────────────────────────────────────────────────────
    static GameObject CreateFarmWorld(GameObject canvas)
    {
        var go = Child(canvas, "FarmWorld");
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        // Plain Image — no sprite, just a solid colour fill
        var img = go.AddComponent<Image>();
        img.color = ColFarmBg;
        img.raycastTarget = false;
        return go;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // 0.3  TopHUD — width 1032, height 92, top Y 44
    // ─────────────────────────────────────────────────────────────────────────
    static GameObject CreateTopHUD(GameObject canvas, Sprite sprRect, Sprite sprSquircle, Sprite sprCircle)
    {
        var go = Child(canvas, "TopHUD");
        var rt = go.GetComponent<RectTransform>();
        // Anchor top-stretch, pivot top-centre
        // anchoredPosition.y = -(44 + 92/2) = -90  (centre of panel from top)
        rt.anchorMin        = new Vector2(0.5f, 1f);
        rt.anchorMax        = new Vector2(0.5f, 1f);
        rt.pivot            = new Vector2(0.5f, 1f);
        rt.sizeDelta        = new Vector2(1032f, 92f);
        rt.anchoredPosition = new Vector2(0f, -44f);

        SlicedImage(go, sprRect, ColHudBg);

        // ── Horizontal layout ─────────────────────────────────────────────────
        var hlg = go.AddComponent<HorizontalLayoutGroup>();
        hlg.padding               = new RectOffset(16, 16, 14, 14);
        hlg.spacing               = 12f;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight= true;
        hlg.childControlWidth     = true;
        hlg.childControlHeight    = true;
        hlg.childAlignment        = TextAnchor.MiddleLeft;

        // A. Profile avatar (Circle, 64×64)
        var profile = Child(go, "ProfileBtn");
        LE(profile, 64, 64);
        CircleImage(profile, sprCircle, HexColor("#FFFFFF40"));

        // B. Level badge (Rectangle sliced, 72×40)
        var lvl = Child(go, "LevelBadge");
        LE(lvl, 72, 40);
        SlicedImage(lvl, sprRect, HexColor("#FFFFFF20"));
        TMP(lvl, "Lv.1", 16, ColText, TextAlignmentOptions.Center);

        // C. Gold capsule
        var gold = CurrencyCapsule(go, "CapsuleGold", sprRect, "999 G");

        // D. Gem capsule
        var gem  = CurrencyCapsule(go, "CapsuleGem",  sprRect, "99 Gem");
        // E. Flexible spacer
        var spacer = Child(go, "Spacer");
        var spacerLE = spacer.AddComponent<LayoutElement>();
        spacerLE.flexibleWidth = 1f;
        spacerLE.minWidth      = 0f;

        // F. Settings button (48×48)
        var settings = Child(go, "BtnSettings");
        LE(settings, 48, 48);
        SlicedImage(settings, sprSquircle, HexColor("#FFFFFF15"));
        TMP(settings, "SET", 13, ColText, TextAlignmentOptions.Center);

        // G. Inbox button (48×48)
        var inbox = Child(go, "BtnInbox");
        LE(inbox, 48, 48);
        SlicedImage(inbox, sprSquircle, HexColor("#FFFFFF15"));
        TMP(inbox, "MSG", 13, ColText, TextAlignmentOptions.Center);

        return go;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // 0.4  SideButtons_Right — 4 buttons, 72×72, right margin 24, start Y 250
    // ─────────────────────────────────────────────────────────────────────────
    static GameObject CreateSideButtons(GameObject canvas, Sprite sprSquircle)
    {
        var go = Child(canvas, "SideButtons_Right");
        var rt = go.GetComponent<RectTransform>();
        // Anchor top-right, pivot top-right
        rt.anchorMin        = new Vector2(1f, 1f);
        rt.anchorMax        = new Vector2(1f, 1f);
        rt.pivot            = new Vector2(1f, 1f);
        // 4 buttons × 72 + 3 gaps × 16 = 336
        rt.sizeDelta        = new Vector2(72f, 4 * 72f + 3 * 16f);
        rt.anchoredPosition = new Vector2(-24f, -250f);

        var vlg = go.AddComponent<VerticalLayoutGroup>();
        vlg.spacing               = 16f;
        vlg.childForceExpandWidth = false;
        vlg.childForceExpandHeight= false;
        vlg.childAlignment        = TextAnchor.UpperCenter;

        string[] names  = { "SideBtn_Shop", "SideBtn_Backpack", "SideBtn_Quest", "SideBtn_Social" };
        string[] icons  = { "Shop", "Bag", "Quest", "Social" };

        foreach (var (n, icon) in System.Linq.Enumerable.Zip(names, icons, (a,b)=>(a,b)))
        {
            var btn = Child(go, n);
            LE(btn, 72, 72);
            SlicedImage(btn, sprSquircle, ColSideBtnBg);
            TMP(btn, icon, 28, ColText, TextAlignmentOptions.Center);
        }
        return go;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // 0.5+0.6  BottomToolbar — width 1032, height 120, bottom offset 34
    // ─────────────────────────────────────────────────────────────────────────
    static GameObject CreateBottomToolbar(GameObject canvas, Sprite sprRect, Sprite sprSquircle)
    {
        var go = Child(canvas, "BottomToolbar");
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin        = new Vector2(0.5f, 0f);
        rt.anchorMax        = new Vector2(0.5f, 0f);
        rt.pivot            = new Vector2(0.5f, 0f);
        rt.sizeDelta        = new Vector2(1032f, 120f);
        rt.anchoredPosition = new Vector2(0f, 34f);

        SlicedImage(go, sprRect, ColToolbarBg);

        var hlg = go.AddComponent<HorizontalLayoutGroup>();
        hlg.padding               = new RectOffset(18, 18, 14, 14);
        hlg.spacing               = 6f;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight= false;
        hlg.childControlWidth     = false;
        hlg.childControlHeight    = false;
        hlg.childAlignment        = TextAnchor.MiddleCenter;

        string[] slotNames  = { "Slot_Hoe","Slot_Seed","Slot_Water","Slot_Harvest","Slot_Weed","Slot_Pest","Slot_More" };
        string[] slotIcons  = { "HOE", "SEED", "H2O", "HARV", "WEED", "PEST", "..." };
        string[] slotLabels = { "Cuoc dat","Hat giong","Tuoi nuoc","Thu hoach","Nho co","Bat sau","More" };

        for (int i = 0; i < slotNames.Length; i++)
        {
            var slot = Child(go, slotNames[i]);
            // 1032 - 36 padding - 6*6 gaps = 960 / 7 = 137 → use 132
            LE(slot, 132f, 92f);

            // Slot background (squircle)
            SlicedImage(slot, sprSquircle, ColSlotDefault);

            // Vertical layout inside slot
            var vlg = slot.AddComponent<VerticalLayoutGroup>();
            vlg.padding               = new RectOffset(4, 4, 6, 6);
            vlg.spacing               = 2f;
            vlg.childForceExpandWidth = true;
            vlg.childForceExpandHeight= false;
            vlg.childAlignment        = TextAnchor.MiddleCenter;

            // Icon text
            var iconGO = Child(slot, "Icon");
            LE(iconGO, 128f, 48f);
            var iconTmp = iconGO.AddComponent<TextMeshProUGUI>();
            iconTmp.text      = slotIcons[i];
            iconTmp.fontSize  = 32f;
            iconTmp.color     = ColText;
            iconTmp.alignment = TextAlignmentOptions.Center;

            // Label text
            var lblGO = Child(slot, "Label");
            LE(lblGO, 128f, 22f);
            var lblTmp = lblGO.AddComponent<TextMeshProUGUI>();
            lblTmp.text      = slotLabels[i];
            lblTmp.fontSize  = 13f;
            lblTmp.color     = ColTextDim;
            lblTmp.alignment = TextAlignmentOptions.Center;
            lblTmp.textWrappingMode = TextWrappingModes.NoWrap;
        }
        return go;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // 0.7  ModeBar — width 560, height 64, bottom Y = 170 from bottom
    //      (top Y 1686 → from bottom = 1920 - 1750 = 170)
    // ─────────────────────────────────────────────────────────────────────────
    static GameObject CreateModeBar(GameObject canvas, Sprite sprRect, Sprite sprSquircle)
    {
        var go = Child(canvas, "ModeBar");
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin        = new Vector2(0.5f, 0f);
        rt.anchorMax        = new Vector2(0.5f, 0f);
        rt.pivot            = new Vector2(0.5f, 0f);
        rt.sizeDelta        = new Vector2(560f, 64f);
        rt.anchoredPosition = new Vector2(0f, 170f);

        SlicedImage(go, sprRect, ColModeBarBg);

        var hlg = go.AddComponent<HorizontalLayoutGroup>();
        hlg.padding               = new RectOffset(16, 12, 8, 8);
        hlg.spacing               = 10f;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight= true;
        hlg.childAlignment        = TextAnchor.MiddleLeft;

        // Mode icon placeholder
        var modeIcon = Child(go, "ModeIcon");
        LE(modeIcon, 28, 28);
        SlicedImage(modeIcon, sprSquircle, HexColor("#FFFFFF30"));

        // Mode name (flexible)
        var modeName = Child(go, "ModeName");
        var mnLE = modeName.AddComponent<LayoutElement>();
        mnLE.flexibleWidth  = 1f;
        mnLE.preferredHeight= 48f;
        var mnTmp = modeName.AddComponent<TextMeshProUGUI>();
        mnTmp.text      = "Đang dùng: Cuốc đất";
        mnTmp.fontSize  = 20f;
        mnTmp.color     = ColText;
        mnTmp.alignment = TextAlignmentOptions.MidlineLeft;

        // Close button
        var btnClose = Child(go, "BtnClose");
        LE(btnClose, 40, 40);
        SlicedImage(btnClose, sprSquircle, ColBtnClose);
        TMP(btnClose, "✕", 18, Color.white, TextAlignmentOptions.Center);

        go.SetActive(false);
        return go;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // 0.8  SeedTray — width 1032, height 180, bottom offset 250
    // ─────────────────────────────────────────────────────────────────────────
    static GameObject CreateSeedTray(GameObject canvas, Sprite sprRect)
    {
        var go = Child(canvas, "SeedTray");
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin        = new Vector2(0.5f, 0f);
        rt.anchorMax        = new Vector2(0.5f, 0f);
        rt.pivot            = new Vector2(0.5f, 0f);
        rt.sizeDelta        = new Vector2(1032f, 180f);
        rt.anchoredPosition = new Vector2(0f, 250f);

        SlicedImage(go, sprRect, ColSeedTrayBg);
        TMP(go, "SEED TRAY — placeholder", 20, ColTextDim, TextAlignmentOptions.Center);

        go.SetActive(false);
        return go;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // 0.9  BottomSheet — full-width, collapsed 260, anchored bottom
    // ─────────────────────────────────────────────────────────────────────────
    static GameObject CreateBottomSheet(GameObject canvas, Sprite sprRect, Sprite sprSquare)
    {
        var go = Child(canvas, "BottomSheet");
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin        = new Vector2(0f, 0f);
        rt.anchorMax        = new Vector2(1f, 0f);
        rt.pivot            = new Vector2(0.5f, 0f);
        rt.sizeDelta        = new Vector2(0f, 260f);
        rt.anchoredPosition = new Vector2(0f, 0f);

        SlicedImage(go, sprRect, ColSheetBg);

        // Grab handle
        var handle = Child(go, "Handle");
        var hRt = handle.GetComponent<RectTransform>();
        hRt.anchorMin        = new Vector2(0.5f, 1f);
        hRt.anchorMax        = new Vector2(0.5f, 1f);
        hRt.pivot            = new Vector2(0.5f, 1f);
        hRt.sizeDelta        = new Vector2(84f, 8f);
        hRt.anchoredPosition = new Vector2(0f, -14f);
        var hImg = handle.AddComponent<Image>();
        hImg.sprite = sprSquare;
        hImg.color  = ColHandle;
        hImg.type   = Image.Type.Simple;

        // Content area
        var content = Child(go, "Content");
        var cRt = content.GetComponent<RectTransform>();
        cRt.anchorMin = new Vector2(0f, 0f);
        cRt.anchorMax = new Vector2(1f, 1f);
        cRt.offsetMin = new Vector2(24f, 0f);
        cRt.offsetMax = new Vector2(-24f, -30f);
        TMP(content, "BOTTOM SHEET — placeholder", 20, ColTextDim, TextAlignmentOptions.Center);

        go.SetActive(false);
        return go;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // PopupConfirm placeholder
    // ─────────────────────────────────────────────────────────────────────────
    static GameObject CreatePopupPlaceholder(GameObject canvas, Sprite sprRect)
    {
        var go = Child(canvas, "PopupConfirm");
        var rt = go.GetComponent<RectTransform>();
        rt.anchorMin        = new Vector2(0.5f, 0.5f);
        rt.anchorMax        = new Vector2(0.5f, 0.5f);
        rt.pivot            = new Vector2(0.5f, 0.5f);
        rt.sizeDelta        = new Vector2(760f, 320f);
        rt.anchoredPosition = Vector2.zero;
        SlicedImage(go, sprRect, ColSheetBg);
        TMP(go, "POPUP CONFIRM — placeholder", 20, ColTextDim, TextAlignmentOptions.Center);
        go.SetActive(false);
        return go;
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────────────────────────────────

    static GameObject Child(GameObject parent, string name)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        go.AddComponent<RectTransform>();
        return go;
    }

    // Sliced Image using 9-slice sprite
    static Image SlicedImage(GameObject go, Sprite sprite, Color color)
    {
        var img = go.GetComponent<Image>() ?? go.AddComponent<Image>();
        img.sprite = sprite;
        img.color  = color;
        img.type   = (sprite != null) ? Image.Type.Sliced : Image.Type.Simple;
        img.raycastTarget = true;
        return img;
    }

    // Circle image (Simple)
    static Image CircleImage(GameObject go, Sprite sprite, Color color)
    {
        var img = go.GetComponent<Image>() ?? go.AddComponent<Image>();
        img.sprite = sprite;
        img.color  = color;
        img.type   = Image.Type.Simple;
        return img;
    }

    // LayoutElement with preferred size
    static LayoutElement LE(GameObject go, float w, float h)
    {
        var le = go.AddComponent<LayoutElement>();
        le.preferredWidth  = w;
        le.preferredHeight = h;
        le.minWidth        = w;
        le.minHeight       = h;
        return le;
    }

    // TMP text child (fills parent)
    static TextMeshProUGUI TMP(GameObject parent, string text, float size, Color color, TextAlignmentOptions align)
    {
        // Check if TMP already exists directly on this GO
        var existing = parent.GetComponent<TextMeshProUGUI>();
        if (existing != null)
        {
            existing.text      = text;
            existing.fontSize  = size;
            existing.color     = color;
            existing.alignment = align;
            return existing;
        }

        var go = new GameObject("Text");
        go.transform.SetParent(parent.transform, false);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        var tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text            = text;
        tmp.fontSize        = size;
        tmp.color           = color;
        tmp.alignment       = align;
        tmp.textWrappingMode= TextWrappingModes.NoWrap;
        tmp.overflowMode    = TextOverflowModes.Ellipsis;
        return tmp;
    }

    // Currency capsule helper
    static GameObject CurrencyCapsule(GameObject parent, string name, Sprite sprRect, string label)
    {
        var go = Child(parent, name);
        LE(go, 120f, 44f);
        SlicedImage(go, sprRect, HexColor("#FFFFFF18"));

        var hlg = go.AddComponent<HorizontalLayoutGroup>();
        hlg.padding               = new RectOffset(10, 10, 0, 0);
        hlg.spacing               = 6f;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight= true;
        hlg.childAlignment        = TextAnchor.MiddleCenter;

        var lbl = Child(go, "Label");
        var le  = lbl.AddComponent<LayoutElement>();
        le.flexibleWidth = 1f;
        var tmp = lbl.AddComponent<TextMeshProUGUI>();
        tmp.text            = label;
        tmp.fontSize        = 18f;
        tmp.color           = ColText;
        tmp.alignment       = TextAlignmentOptions.Center;
        tmp.textWrappingMode= TextWrappingModes.NoWrap;
        return go;
    }

    // Set 9-slice border on a sprite asset
    static void SetSpriteBorder(string path, float l, float b, float r, float t)
    {
        var ti = AssetImporter.GetAtPath(path) as TextureImporter;
        if (ti == null) return;
        ti.spriteBorder = new Vector4(l, b, r, t);
        ti.SaveAndReimport();
    }

    static Sprite LoadSprite(string path)
    {
        var s = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (s == null) Debug.LogWarning($"[FarmSetup] Sprite not found: {path}");
        return s;
    }

    static Color HexColor(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color c);
        return c;
    }
}
