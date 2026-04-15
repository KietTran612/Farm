/// <summary>
/// Farm Game — Tile State Enum
/// Each tile on the farm grid has exactly one state at any time.
/// </summary>
public enum TileState
{
    WildGrass,      // Untouched land — needs hoeing
    NormalSoil,     // Cleared soil — needs hoeing to till
    TilledSoil,     // Ready for planting
    PlantedGrowing, // Seed planted, crop growing
    PlantedReady,   // Crop fully grown, ready to harvest
    PlantedDead     // Crop died (pest timeout) — needs cleanup
}
