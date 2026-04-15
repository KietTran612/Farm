using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Farm Game — TileVisualController
/// Attached to each tile GameObject in the world.
/// Listens to TileManager.OnTileChanged and updates visuals accordingly.
/// Uses solid-colour placeholders until real art is available.
/// </summary>
[RequireComponent(typeof(Image))]
public class TileVisualController : MonoBehaviour
{
    // ── Placeholder colours per state ─────────────────────────────────────────
    private static readonly Color ColWildGrass     = new Color(0.24f, 0.55f, 0.18f, 1f); // dark green
    private static readonly Color ColNormalSoil    = new Color(0.60f, 0.42f, 0.22f, 1f); // brown
    private static readonly Color ColTilledSoil    = new Color(0.45f, 0.28f, 0.12f, 1f); // dark brown
    private static readonly Color ColPlantedGrowing= new Color(0.30f, 0.65f, 0.25f, 1f); // mid green
    private static readonly Color ColPlantedReady  = new Color(0.95f, 0.80f, 0.10f, 1f); // golden yellow
    private static readonly Color ColPlantedDead   = new Color(0.40f, 0.35f, 0.30f, 1f); // grey-brown

    // ── State label colours ───────────────────────────────────────────────────
    private static readonly Color ColLabelDark  = new Color(0f, 0f, 0f, 0.7f);
    private static readonly Color ColLabelLight = new Color(1f, 1f, 1f, 0.85f);

    // ── References ────────────────────────────────────────────────────────────
    [SerializeField] private TextMeshProUGUI _stateLabel;   // optional debug label
    [SerializeField] private Image           _problemIcon;  // 20×20 problem overlay

    private Image      _bg;
    private Vector2Int _coord;

    // ─────────────────────────────────────────────────────────────────────────
    private void Awake()
    {
        _bg = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (TileManager.Instance != null)
            TileManager.Instance.OnTileChanged += OnTileChanged;
    }

    private void OnDisable()
    {
        if (TileManager.Instance != null)
            TileManager.Instance.OnTileChanged -= OnTileChanged;
    }

    // ── Public init ───────────────────────────────────────────────────────────
    public void Init(Vector2Int coord)
    {
        _coord = coord;
        var tile = TileManager.Instance?.GetTile(coord);
        if (tile != null) Refresh(tile);
    }

    // ── Internal ──────────────────────────────────────────────────────────────
    private void OnTileChanged(TileData tile)
    {
        if (tile.coord == _coord) Refresh(tile);
    }

    private void Refresh(TileData tile)
    {
        // Background colour
        _bg.color = tile.state switch
        {
            TileState.WildGrass      => ColWildGrass,
            TileState.NormalSoil     => ColNormalSoil,
            TileState.TilledSoil     => ColTilledSoil,
            TileState.PlantedGrowing => ColPlantedGrowing,
            TileState.PlantedReady   => ColPlantedReady,
            TileState.PlantedDead    => ColPlantedDead,
            _                        => Color.white
        };

        // Debug state label
        if (_stateLabel != null)
        {
            _stateLabel.text = tile.state switch
            {
                TileState.WildGrass      => "G",   // Grass
                TileState.NormalSoil     => "N",   // Normal
                TileState.TilledSoil     => "T",   // Tilled
                TileState.PlantedGrowing => "~",   // Growing
                TileState.PlantedReady   => "R",   // Ready
                TileState.PlantedDead    => "X",   // Dead
                _                        => "?"
            };
            // Use dark label on light backgrounds
            bool useDark = tile.state == TileState.PlantedReady;
            _stateLabel.color = useDark ? ColLabelDark : ColLabelLight;
        }

        // Problem icon (Phase 4 will populate this properly)
        if (_problemIcon != null)
        {
            bool hasProblem = tile.state == TileState.PlantedGrowing
                           && (tile.hasWeeds || tile.hasPests || !tile.isWatered);
            _problemIcon.gameObject.SetActive(hasProblem);
        }
    }
}
