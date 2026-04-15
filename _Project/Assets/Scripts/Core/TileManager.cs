using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Farm Game — TileManager
/// Owns the grid of TileData. Single source of truth for all tile states.
/// Fires OnTileChanged when any tile state changes.
/// </summary>
public class TileManager : MonoBehaviour
{
    // ── Inspector ─────────────────────────────────────────────────────────────
    [SerializeField] private int _gridWidth  = 5;
    [SerializeField] private int _gridHeight = 5;

    // ── Events ────────────────────────────────────────────────────────────────
    /// <summary>Fired whenever a tile's state changes. Passes the updated TileData.</summary>
    public event Action<TileData> OnTileChanged;

    // ── Internal grid ─────────────────────────────────────────────────────────
    private Dictionary<Vector2Int, TileData> _grid = new();

    // ── Singleton ─────────────────────────────────────────────────────────────
    public static TileManager Instance { get; private set; }

    // ─────────────────────────────────────────────────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        InitGrid();
    }

    // ─────────────────────────────────────────────────────────────────────────
    private void InitGrid()
    {
        _grid.Clear();
        for (int x = 0; x < _gridWidth; x++)
        for (int y = 0; y < _gridHeight; y++)
        {
            var coord = new Vector2Int(x, y);
            _grid[coord] = new TileData(coord, TileState.WildGrass);
        }
    }

    // ── Public API ────────────────────────────────────────────────────────────

    /// <summary>Returns TileData for the given coord, or null if out of bounds.</summary>
    public TileData GetTile(Vector2Int coord)
    {
        _grid.TryGetValue(coord, out var tile);
        return tile;
    }

    /// <summary>
    /// Transitions a tile to a new state.
    /// Clears crop data when returning to soil states.
    /// Fires OnTileChanged.
    /// </summary>
    public void SetTileState(Vector2Int coord, TileState newState)
    {
        var tile = GetTile(coord);
        if (tile == null)
        {
            Debug.LogWarning($"[TileManager] SetTileState: coord {coord} out of bounds.");
            return;
        }

        // Clear crop data when returning to bare soil
        if (newState == TileState.NormalSoil || newState == TileState.TilledSoil)
            tile.ClearCropData();

        tile.state = newState;
        OnTileChanged?.Invoke(tile);
    }

    /// <summary>Update a tile's data directly and fire change event.</summary>
    public void NotifyTileChanged(Vector2Int coord)
    {
        var tile = GetTile(coord);
        if (tile != null) OnTileChanged?.Invoke(tile);
    }

    /// <summary>Returns all tiles in the grid.</summary>
    public IEnumerable<TileData> GetAllTiles() => _grid.Values;

    /// <summary>Grid dimensions.</summary>
    public int GridWidth  => _gridWidth;
    public int GridHeight => _gridHeight;

    /// <summary>Returns true if coord is within grid bounds.</summary>
    public bool IsValidCoord(Vector2Int coord) => _grid.ContainsKey(coord);
}
