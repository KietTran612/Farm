using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Farm Game — TileGridBuilder
/// At runtime: finds existing tile GameObjects in FarmWorld and initialises
/// their TileVisualController + TileTapHandler with correct grid coords.
/// The visual GameObjects are pre-built by Phase1SceneSetup at editor time.
/// </summary>
public class TileGridBuilder : MonoBehaviour
{
    [SerializeField] private RectTransform _farmWorldRect;
    [SerializeField] private int _gridWidth  = 5;
    [SerializeField] private int _gridHeight = 5;

    private void Start()
    {
        InitTileComponents();
    }

    /// <summary>
    /// Walk existing Tile_x_y children and call Init(coord) on their components.
    /// </summary>
    public void InitTileComponents()
    {
        if (_farmWorldRect == null)
        {
            Debug.LogError("[TileGridBuilder] _farmWorldRect not assigned.");
            return;
        }

        for (int x = 0; x < _gridWidth; x++)
        for (int y = 0; y < _gridHeight; y++)
        {
            var coord  = new Vector2Int(x, y);
            var tileTF = _farmWorldRect.Find($"Tile_{x}_{y}");
            if (tileTF == null) continue;

            tileTF.GetComponent<TileVisualController>()?.Init(coord);
            tileTF.GetComponent<TileTapHandler>()?.Init(coord);
        }
    }
}
