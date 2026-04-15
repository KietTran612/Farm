using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;

/// <summary>
/// Farm Game — Phase 1 Scene Setup
/// Menu: Farm/Setup Phase 1 — Tiles + BottomSheet
/// 
/// Adds to the existing Phase 0 scene:
///   - Managers GameObject (TileManager, TileGridBuilder, UILayerManager)
///   - Tile grid inside FarmWorld
///   - Wires BottomSheet with full content UI
/// </summary>
public static class Phase1SceneSetup
{
    private static readonly Color ColText     = new Color(0.95f, 0.95f, 0.95f, 1f);
    private static readonly Color ColTextDim  = new Color(0.70f, 0.70f, 0.70f, 1f);
    private static readonly Color ColBtnBg    = new Color(0.20f, 0.55f, 0.25f, 1f); // green action btn
    private static readonly Color ColBtnText  = Color.white;
    private static readonly Color ColSheetBg  = new Color(0.13f, 0.13f, 0.13f, 0.97f);

    private const string SpritesRoot = "Assets/Textures/Shapes/";
    private const string SprRect     = SpritesRoot + "Rectangle.png";
    private const string SprSquircle = SpritesRoot + "Squircle.png";
    private const string SprSquare   = SpritesRoot + "Square.png";

    // ─────────────────────────────────────────────────────────────────────────
    [MenuItem("Farm/Setup Phase 1 — Tiles + BottomSheet")]
    public static void SetupPhase1()
    {
        var sprRect     = AssetDatabase.LoadAssetAtPath<Sprite>(SprRect);
        var sprSquircle = AssetDatabase.LoadAssetAtPath<Sprite>(SprSquircle);
        var sprSquare   = AssetDatabase.LoadAssetAtPath<Sprite>(SprSquare);

        // ── Find existing Phase 0 objects ─────────────────────────────────────
        var canvas      = GameObject.Find("Canvas");
        var farmWorld   = canvas?.transform.Find("FarmWorld")?.gameObject;
        var bottomSheet = canvas?.transform.Find("BottomSheet")?.gameObject;
        var modeBar     = canvas?.transform.Find("ModeBar")?.gameObject;
        var seedTray    = canvas?.transform.Find("SeedTray")?.gameObject;

        if (canvas == null || farmWorld == null || bottomSheet == null)
        {
            Debug.LogError("[Phase1Setup] Phase 0 objects not found. Run Farm/Setup Phase 0 Scene first.");
            return;
        }

        // ── 1. Managers GameObject ────────────────────────────────────────────
        SetupManagers(farmWorld, modeBar, seedTray);

        // ── 2. Wire BottomSheet with full content ─────────────────────────────
        SetupBottomSheet(bottomSheet, sprRect, sprSquircle, sprSquare);

        // ── 3. Save ───────────────────────────────────────────────────────────
        EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log("[Phase1Setup] Phase 1 setup complete.");
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Managers + Tile Grid (built at editor time)
    // ─────────────────────────────────────────────────────────────────────────
    static void SetupManagers(GameObject farmWorld, GameObject modeBar, GameObject seedTray)
    {
        // Remove old Managers if exists
        var existing = GameObject.Find("Managers");
        if (existing != null) Object.DestroyImmediate(existing);

        var managers = new GameObject("Managers");

        // TileManager (singleton, manages grid data at runtime)
        managers.AddComponent<TileManager>();

        // UILayerManager
        var ulm = managers.AddComponent<UILayerManager>();        var soUlm = new SerializedObject(ulm);
        if (seedTray != null)
            soUlm.FindProperty("_seedTrayRoot").objectReferenceValue = seedTray;
        if (modeBar != null)
            soUlm.FindProperty("_modeBarRoot").objectReferenceValue  = modeBar;
        soUlm.ApplyModifiedProperties();

        // Build tile grid directly in editor (no runtime dependency)
        BuildTileGridInEditor(farmWorld);

        Debug.Log("[Phase1Setup] Managers created.");
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Build tile grid as UI GameObjects inside FarmWorld at editor time
    // ─────────────────────────────────────────────────────────────────────────
    static void BuildTileGridInEditor(GameObject farmWorld)
    {
        // Remove old tiles
        var farmRT = farmWorld.GetComponent<RectTransform>();
        for (int i = farmRT.childCount - 1; i >= 0; i--)
            Object.DestroyImmediate(farmRT.GetChild(i).gameObject);

        const int   gridW    = 5;
        const int   gridH    = 5;
        const float tileSize = 160f;
        const float tileGap  = 8f;

        // Placeholder colours per state (WildGrass for all initially)
        Color colWildGrass = new Color(0.24f, 0.55f, 0.18f, 1f);

        float totalW = gridW * tileSize + (gridW - 1) * tileGap;
        float totalH = gridH * tileSize + (gridH - 1) * tileGap;
        float startX = -totalW * 0.5f + tileSize * 0.5f;
        float startY =  totalH * 0.5f - tileSize * 0.5f;

        for (int x = 0; x < gridW; x++)
        for (int y = 0; y < gridH; y++)
        {
            var tileGO = new GameObject($"Tile_{x}_{y}");
            tileGO.transform.SetParent(farmRT, false);

            var rt = tileGO.AddComponent<RectTransform>();
            rt.sizeDelta        = new Vector2(tileSize, tileSize);
            rt.anchorMin        = new Vector2(0.5f, 0.5f);
            rt.anchorMax        = new Vector2(0.5f, 0.5f);
            rt.pivot            = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = new Vector2(
                startX + x * (tileSize + tileGap),
                startY - y * (tileSize + tileGap)
            );

            // Background image — WildGrass colour
            var img = tileGO.AddComponent<Image>();
            img.color = colWildGrass;
            img.raycastTarget = true;

            // State emoji label
            var labelGO = new GameObject("StateLabel");
            labelGO.transform.SetParent(tileGO.transform, false);
            var labelRT = labelGO.AddComponent<RectTransform>();
            labelRT.anchorMin = Vector2.zero;
            labelRT.anchorMax = Vector2.one;
            labelRT.offsetMin = Vector2.zero;
            labelRT.offsetMax = Vector2.zero;
            var tmp = labelGO.AddComponent<TextMeshProUGUI>();
            tmp.text      = "G"; // WildGrass placeholder (no emoji — font fallback)
            tmp.fontSize  = 48f;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.raycastTarget = false;

            // Runtime components (active at play time)
            tileGO.AddComponent<TileVisualController>();
            var tap = tileGO.AddComponent<TileTapHandler>();
            // coord will be set at runtime via TileGridBuilder.Start()
        }

        // Keep TileGridBuilder on Managers for runtime init
        var managers = GameObject.Find("Managers");
        if (managers != null)
        {
            var tgb = managers.AddComponent<TileGridBuilder>();
            var so  = new SerializedObject(tgb);
            so.FindProperty("_farmWorldRect").objectReferenceValue = farmRT;
            so.ApplyModifiedProperties();
        }

        Debug.Log($"[Phase1Setup] Built {gridW}×{gridH} tile grid in editor.");
    }

    // ─────────────────────────────────────────────────────────────────────────
    // BottomSheet — rebuild with full content UI
    // ─────────────────────────────────────────────────────────────────────────
    static void SetupBottomSheet(GameObject sheet, Sprite sprRect, Sprite sprSquircle, Sprite sprSquare)
    {
        // Clear existing children except Handle
        var handle = sheet.transform.Find("Handle");
        for (int i = sheet.transform.childCount - 1; i >= 0; i--)
        {
            var child = sheet.transform.GetChild(i);
            if (child.name != "Handle") Object.DestroyImmediate(child.gameObject);
        }

        // Add SheetHandleDrag to handle
        if (handle != null)
        {
            if (handle.GetComponent<SheetHandleDrag>() == null)
                handle.gameObject.AddComponent<SheetHandleDrag>();
        }

        // ── Content container ─────────────────────────────────────────────────
        var content = Child(sheet, "Content");
        var cRt = content.GetComponent<RectTransform>();
        cRt.anchorMin = new Vector2(0f, 0f);
        cRt.anchorMax = new Vector2(1f, 1f);
        cRt.offsetMin = new Vector2(24f, 16f);
        cRt.offsetMax = new Vector2(-24f, -36f);

        var vlg = content.AddComponent<VerticalLayoutGroup>();
        vlg.spacing               = 12f;
        vlg.padding               = new RectOffset(0, 0, 8, 8);
        vlg.childForceExpandWidth = true;
        vlg.childForceExpandHeight= false;
        vlg.childAlignment        = TextAnchor.UpperCenter;

        // Title
        var titleGO = Child(content, "TitleText");
        LE(titleGO, 0, 36);
        var titleTmp = titleGO.AddComponent<TextMeshProUGUI>();
        titleTmp.text      = "Tile Title";
        titleTmp.fontSize  = 24f;
        titleTmp.fontStyle = FontStyles.Bold;
        titleTmp.color     = ColText;
        titleTmp.alignment = TextAlignmentOptions.Left;

        // Status
        var statusGO = Child(content, "StatusText");
        LE(statusGO, 0, 28);
        var statusTmp = statusGO.AddComponent<TextMeshProUGUI>();
        statusTmp.text      = "";
        statusTmp.fontSize  = 18f;
        statusTmp.color     = ColTextDim;
        statusTmp.alignment = TextAlignmentOptions.Left;
        statusGO.SetActive(false);

        // Timer
        var timerGO = Child(content, "TimerText");
        LE(timerGO, 0, 24);
        var timerTmp = timerGO.AddComponent<TextMeshProUGUI>();
        timerTmp.text      = "";
        timerTmp.fontSize  = 16f;
        timerTmp.color     = ColTextDim;
        timerTmp.alignment = TextAlignmentOptions.Left;
        timerGO.SetActive(false);

        // ── Action buttons ────────────────────────────────────────────────────
        var actionsRow = Child(content, "ActionsRow");
        LE(actionsRow, 0, 64);
        var hlg = actionsRow.AddComponent<HorizontalLayoutGroup>();
        hlg.spacing               = 12f;
        hlg.childForceExpandWidth = false;
        hlg.childForceExpandHeight= true;
        hlg.childAlignment        = TextAnchor.MiddleLeft;

        var btnHoe         = MakeActionBtn(actionsRow, "BtnHoe",         "Cuốc đất",              sprRect);
        var btnPlant       = MakeActionBtn(actionsRow, "BtnPlant",       "Gieo hạt",              sprRect);
        var btnOpenSeedTray= MakeActionBtn(actionsRow, "BtnOpenSeedTray","Mở thanh hạt giống",    sprRect);
        var btnWater       = MakeActionBtn(actionsRow, "BtnWater",       "Tưới nước",             sprRect);
        var btnWeed        = MakeActionBtn(actionsRow, "BtnWeed",        "Nhổ cỏ",                sprRect);
        var btnPest        = MakeActionBtn(actionsRow, "BtnPest",        "Bắt sâu",               sprRect);
        var btnHarvest     = MakeActionBtn(actionsRow, "BtnHarvest",     "Thu hoạch",             sprRect);
        var btnCleanup     = MakeActionBtn(actionsRow, "BtnCleanup",     "Dọn xác cây",           sprRect);

        // ── Wire BottomSheetController ────────────────────────────────────────
        var bsc = sheet.GetComponent<BottomSheetController>()
               ?? sheet.AddComponent<BottomSheetController>();

        var so = new SerializedObject(bsc);
        so.FindProperty("_sheetRect").objectReferenceValue   = sheet.GetComponent<RectTransform>();
        so.FindProperty("_sheetRoot").objectReferenceValue   = sheet;
        so.FindProperty("_titleText").objectReferenceValue   = titleTmp;
        so.FindProperty("_statusText").objectReferenceValue  = statusTmp;
        so.FindProperty("_timerText").objectReferenceValue   = timerTmp;
        so.FindProperty("_btnHoe").objectReferenceValue          = btnHoe;
        so.FindProperty("_btnPlant").objectReferenceValue        = btnPlant;
        so.FindProperty("_btnOpenSeedTray").objectReferenceValue = btnOpenSeedTray;
        so.FindProperty("_btnWater").objectReferenceValue        = btnWater;
        so.FindProperty("_btnWeed").objectReferenceValue         = btnWeed;
        so.FindProperty("_btnPest").objectReferenceValue         = btnPest;
        so.FindProperty("_btnHarvest").objectReferenceValue      = btnHarvest;
        so.FindProperty("_btnCleanup").objectReferenceValue      = btnCleanup;
        so.ApplyModifiedProperties();

        // Close button (X) at top-right of sheet
        var btnClose = Child(sheet, "BtnClose");
        var closeRt  = btnClose.GetComponent<RectTransform>();
        closeRt.anchorMin        = new Vector2(1f, 1f);
        closeRt.anchorMax        = new Vector2(1f, 1f);
        closeRt.pivot            = new Vector2(1f, 1f);
        closeRt.sizeDelta        = new Vector2(40f, 40f);
        closeRt.anchoredPosition = new Vector2(-16f, -8f);
        var closeImg = btnClose.AddComponent<Image>();
        if (sprSquircle != null) { closeImg.sprite = sprSquircle; closeImg.type = Image.Type.Sliced; }
        closeImg.color = new Color(0.8f, 0.2f, 0.2f, 0.9f);
        var closeTmp = new GameObject("Text");
        closeTmp.transform.SetParent(btnClose.transform, false);
        var cRt2 = closeTmp.AddComponent<RectTransform>();
        cRt2.anchorMin = Vector2.zero; cRt2.anchorMax = Vector2.one;
        cRt2.offsetMin = Vector2.zero; cRt2.offsetMax = Vector2.zero;
        var cTmp = closeTmp.AddComponent<TextMeshProUGUI>();
        cTmp.text = "✕"; cTmp.fontSize = 18f; cTmp.color = Color.white;
        cTmp.alignment = TextAlignmentOptions.Center;

        // Wire close button
        var closeBtn = btnClose.AddComponent<Button>();
        closeBtn.onClick.AddListener(() => BottomSheetController.Instance?.Close());

        Debug.Log("[Phase1Setup] BottomSheet wired.");
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Helpers
    // ─────────────────────────────────────────────────────────────────────────

    static GameObject MakeActionBtn(GameObject parent, string name, string label, Sprite sprRect)
    {
        var go = Child(parent, name);
        LE(go, 200f, 56f);

        var img = go.AddComponent<Image>();
        if (sprRect != null) { img.sprite = sprRect; img.type = Image.Type.Sliced; }
        img.color = ColBtnBg;

        var tmp = new GameObject("Text");
        tmp.transform.SetParent(go.transform, false);
        var rt = tmp.AddComponent<RectTransform>();
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.offsetMin = new Vector2(8, 0); rt.offsetMax = new Vector2(-8, 0);
        var t = tmp.AddComponent<TextMeshProUGUI>();
        t.text = label; t.fontSize = 16f; t.color = ColBtnText;
        t.alignment = TextAlignmentOptions.Center;
        t.textWrappingMode = TextWrappingModes.NoWrap;

        go.AddComponent<Button>();
        go.SetActive(false); // hidden by default; BottomSheetController shows as needed
        return go;
    }

    static GameObject Child(GameObject parent, string name)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        go.AddComponent<RectTransform>();
        return go;
    }

    static LayoutElement LE(GameObject go, float w, float h)
    {
        var le = go.AddComponent<LayoutElement>();
        if (w > 0) { le.preferredWidth = w; le.minWidth = w; }
        le.preferredHeight = h; le.minHeight = h;
        return le;
    }
}
