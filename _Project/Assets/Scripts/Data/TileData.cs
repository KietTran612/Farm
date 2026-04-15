using UnityEngine;

/// <summary>
/// Farm Game — Per-tile data container.
/// Holds all state for a single tile on the farm grid.
/// </summary>
[System.Serializable]
public class TileData
{
    // ── Core state ────────────────────────────────────────────────────────────
    public TileState state = TileState.WildGrass;

    // ── Crop info (valid when PlantedGrowing / PlantedReady / PlantedDead) ───
    public string cropType = "";        // e.g. "Carrot", "Tomato"
    public int    growthPhase = 0;      // 0-based phase index
    public float  growthProgress = 0f; // 0.0 → 1.0 within current phase

    // ── Problem flags (valid when PlantedGrowing) ────────────────────────────
    public bool isWatered  = false;
    public bool hasWeeds   = false;
    public bool hasPests   = false;

    // ── Near-ready flag ───────────────────────────────────────────────────────
    /// <summary>True when overall progress > 0.85 — triggers timer display.</summary>
    public bool isNearReady = false;

    // ── Pest duration tracker (for death timeout) ────────────────────────────
    public float pestDuration = 0f;

    // ── Grid coordinate (set by TileManager on creation) ─────────────────────
    public Vector2Int coord;

    // ─────────────────────────────────────────────────────────────────────────
    public TileData(Vector2Int coord, TileState initialState = TileState.WildGrass)
    {
        this.coord = coord;
        this.state = initialState;
    }

    /// <summary>Reset all crop-related fields when tile returns to soil.</summary>
    public void ClearCropData()
    {
        cropType      = "";
        growthPhase   = 0;
        growthProgress= 0f;
        isWatered     = false;
        hasWeeds      = false;
        hasPests      = false;
        isNearReady   = false;
        pestDuration  = 0f;
    }
}
