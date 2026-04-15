# Farm Game — Implementation Log: Phase 0 & Phase 1

> Cập nhật lần cuối: Phase 1 hoàn thành  
> Unity 2D, uGUI, Canvas 1080×1920 portrait, TextMeshPro

---

## Tổng quan

Tài liệu này ghi lại những gì đã được xây dựng trong Phase 0 và Phase 1, cách các thành phần hoạt động, và hướng dẫn sử dụng khi cần chỉnh sửa hoặc mở rộng.

---

## Phase 0 — Layout Skeleton

### Mục tiêu
Dựng toàn bộ khung UI tĩnh: Canvas, FarmWorld, TopHUD, SideButtons, BottomToolbar, ModeBar, SeedTray, BottomSheet, PopupConfirm — đúng kích thước và z-order theo spec.

### Cách chạy
```
Unity Editor → Menu: Farm > Setup Phase 0 Scene
```
Script sẽ xóa Canvas cũ (nếu có) và tạo lại toàn bộ từ đầu.

### File
`_Project/Assets/Scripts/Editor/FarmSceneSetup.cs`

### Canvas Settings
| Property | Giá trị |
|----------|---------|
| Render Mode | Screen Space — Overlay |
| UI Scale Mode | Scale With Screen Size |
| Reference Resolution | 1080 × 1920 |
| Screen Match Mode | Match Width Or Height |
| Match | **1.0 (Height)** |

> **Lưu ý quan trọng:** Match = 1.0 (Height) là đúng cho portrait mobile game. Match = 0.5 sẽ gây layout sai khi Game view không đúng resolution. Game view phải được set về **1080×1920 Portrait** (Android platform).

### Hierarchy sau Phase 0
```
Canvas
├── FarmWorld          ← full-screen, anchor stretch, z=0
├── TopHUD             ← width 1032, height 92, top Y 44
├── SideButtons_Right  ← 4 buttons 72×72, right margin 24, start Y 250
├── BottomToolbar      ← width 1032, height 120, bottom offset 34, 7 slots
├── ModeBar            ← width 560, height 64, bottom 170 — hidden by default
├── SeedTray           ← width 1032, height 180, bottom 250 — hidden by default
├── BottomSheet        ← full-width, height 260 collapsed — hidden by default
└── PopupConfirm       ← 760×320, centered — hidden by default
```

### Z-order (sibling index)
| Index | Object |
|-------|--------|
| 0 | FarmWorld |
| 1 | TopHUD |
| 2 | SideButtons_Right |
| 3 | BottomToolbar |
| 4 | ModeBar |
| 5 | SeedTray |
| 6 | BottomSheet |
| 7 | PopupConfirm |

### Sprites dùng
Tất cả sprites nằm ở `Assets/Textures/Shapes/` — white sprites, tint bằng `Image.color`.

| Sprite | 9-Slice Border | Dùng cho |
|--------|---------------|----------|
| `Rectangle.png` | 70px | HUD, Toolbar, SeedTray, BottomSheet, Buttons |
| `Squircle.png` | 128px | SideButtons, ToolSlots, Capsules |
| `Rectangle-Outline.png` | 80px | Tile highlight (Phase 2+) |
| `Circle.png` | — | Profile avatar |
| `Square.png` | — | Grab handle |

---

## Phase 1 — Tile States + Bottom Sheet

### Mục tiêu
- Implement data model cho tile grid (TileState, TileData, TileManager)
- Hiển thị 5×5 tile grid với màu placeholder theo state
- Tap tile → mở BottomSheet với content đúng theo state
- BottomSheet 2-level (collapsed/expanded) với grab handle
- UILayerManager enforce mutual exclusion rules

### Cách chạy
```
Unity Editor → Menu: Farm > Setup Phase 1 — Tiles + BottomSheet
```
> Phải chạy Phase 0 trước. Phase 1 setup tìm các object từ Phase 0 và bổ sung thêm.

---

## Kiến trúc Data Layer

### TileState (enum)
`_Project/Assets/Scripts/Core/TileState.cs`

```
WildGrass      → Đất hoang, chưa khai phá
NormalSoil     → Đất thường, đã dọn cỏ
TilledSoil     → Đất đã cuốc, sẵn sàng trồng
PlantedGrowing → Đang trồng, cây đang lớn
PlantedReady   → Cây đã chín, sẵn sàng thu hoạch
PlantedDead    → Cây đã chết (do sâu bệnh)
```

### TileData (class)
`_Project/Assets/Scripts/Data/TileData.cs`

Mỗi tile có một `TileData` instance chứa:

| Field | Type | Mô tả |
|-------|------|-------|
| `state` | TileState | State hiện tại |
| `cropType` | string | Loại cây (khi PlantedGrowing+) |
| `growthPhase` | int | Phase tăng trưởng (0-based) |
| `growthProgress` | float | Tiến độ 0.0→1.0 |
| `isWatered` | bool | Đã tưới nước chưa |
| `hasWeeds` | bool | Có cỏ dại không |
| `hasPests` | bool | Có sâu bệnh không |
| `isNearReady` | bool | Gần chín (progress > 0.85) |
| `pestDuration` | float | Thời gian sâu bệnh tồn tại (cho death timeout) |
| `coord` | Vector2Int | Tọa độ trong grid |

**`ClearCropData()`** — reset tất cả crop fields khi tile trở về đất trống.

### TileManager (singleton MonoBehaviour)
`_Project/Assets/Scripts/Core/TileManager.cs`

**Trách nhiệm:** Nguồn dữ liệu duy nhất (single source of truth) cho toàn bộ tile grid.

**API công khai:**
```csharp
TileManager.Instance.GetTile(Vector2Int coord)         // → TileData hoặc null
TileManager.Instance.SetTileState(coord, TileState)    // thay đổi state + fire event
TileManager.Instance.NotifyTileChanged(coord)          // fire event thủ công
TileManager.Instance.GetAllTiles()                     // IEnumerable<TileData>
TileManager.Instance.IsValidCoord(coord)               // kiểm tra bounds
```

**Event:**
```csharp
TileManager.Instance.OnTileChanged += (TileData tile) => { ... };
```
Fired mỗi khi `SetTileState()` hoặc `NotifyTileChanged()` được gọi.

**Inspector:**
- `_gridWidth` = 5 (default)
- `_gridHeight` = 5 (default)

---

## Kiến trúc Visual Layer

### TileVisualController
`_Project/Assets/Scripts/Core/TileVisualController.cs`

Gắn trên mỗi tile GameObject. Subscribe `TileManager.OnTileChanged`, chỉ xử lý event của tile có coord khớp.

**Màu placeholder:**
| State | Màu |
|-------|-----|
| WildGrass | Xanh lá đậm `#3D8C2E` |
| NormalSoil | Nâu `#996B38` |
| TilledSoil | Nâu đậm `#73481F` |
| PlantedGrowing | Xanh lá vừa `#4DA63F` |
| PlantedReady | Vàng `#F2CC1A` |
| PlantedDead | Xám nâu `#665A4D` |

**Label debug** (optional `_stateLabel`): hiển thị ký tự đơn G/N/T/~/R/X theo state.

**`Init(Vector2Int coord)`** — phải gọi sau khi component được tạo để bind coord.

### TileGridBuilder
`_Project/Assets/Scripts/Core/TileGridBuilder.cs`

Chạy lúc runtime (`Start()`). Walk qua các child `Tile_x_y` trong `_farmWorldRect` và gọi `Init(coord)` cho `TileVisualController` và `TileTapHandler`.

> Tile GameObjects được tạo sẵn ở editor time bởi `Phase1SceneSetup`. `TileGridBuilder` chỉ init coords, không tạo mới objects.

---

## Kiến trúc Input Layer

### TileTapHandler
`_Project/Assets/Scripts/Core/TileTapHandler.cs`

Gắn trên mỗi tile. Implement `IPointerDownHandler`, `IPointerUpHandler`, `IDragHandler`.

**Logic phân biệt tap vs drag:**
```
PointerDown → lưu vị trí bắt đầu
OnDrag      → nếu di chuyển > 10px → đánh dấu isDragging = true
PointerUp   → nếu isDragging = false → đây là tap → gọi BottomSheetController.OpenForTile()
```

Threshold 10px đảm bảo tap ngắn không bị nhầm thành drag.

---

## Kiến trúc UI Layer

### BottomSheetController
`_Project/Assets/Scripts/UI/BottomSheetController.cs`

**Singleton.** Gắn trên GameObject `BottomSheet` trong Canvas.

**2 levels:**
| Level | Height |
|-------|--------|
| Collapsed | 260px |
| Expanded | 540px |

**Flow khi tap tile:**
```
TileTapHandler.OnPointerUp()
  → BottomSheetController.OpenForTile(coord)
    → TileManager.GetTile(coord)
    → PopulateContent(tile)        ← switch theo tile.state
    → SetSheetActive(true)
    → SetExpanded(false)           ← luôn mở collapsed trước
    → OnSheetOpened?.Invoke()      ← UILayerManager lắng nghe
```

**Content theo state:**
| State | Title | Status | Actions hiển thị |
|-------|-------|--------|-----------------|
| WildGrass | Cỏ hoang | — | Cuốc đất |
| NormalSoil | Đất thường | — | Cuốc đất |
| TilledSoil | Đất đã cuốc | — | Gieo hạt, Mở thanh hạt giống |
| PlantedGrowing | tên cây | Đang phát triển | Tưới nước\*, Nhổ cỏ\*, Bắt sâu\* |
| PlantedReady | tên cây | Sẵn sàng thu hoạch | Thu hoạch |
| PlantedDead | tên cây | Đã chết | Dọn xác cây |

\* Chỉ hiển thị khi điều kiện đúng: Tưới nước khi `!isWatered`, Nhổ cỏ khi `hasWeeds`, Bắt sâu khi `hasPests`.

**Events:**
```csharp
BottomSheetController.Instance.OnSheetOpened += () => { ... };
BottomSheetController.Instance.OnSheetClosed += () => { ... };
```

**API công khai:**
```csharp
BottomSheetController.Instance.OpenForTile(Vector2Int coord)
BottomSheetController.Instance.Close()
BottomSheetController.Instance.ToggleExpand()
bool isOpen     = BottomSheetController.Instance.IsOpen;
bool isExpanded = BottomSheetController.Instance.IsExpanded;
```

### SheetHandleDrag
`_Project/Assets/Scripts/UI/SheetHandleDrag.cs`

Gắn trên GameObject `Handle` bên trong BottomSheet. Route drag events đến `BottomSheetController`:
- `IBeginDragHandler` → `OnHandleDragBegin()`
- `IDragHandler` → `OnHandleDrag()` — resize sheet theo ngón tay
- `IEndDragHandler` → `OnHandleDragEnd()` — snap về collapsed hoặc expanded
- `IPointerClickHandler` → `ToggleExpand()` — tap handle = toggle

### UILayerManager
`_Project/Assets/Scripts/UI/UILayerManager.cs`

Gắn trên `Managers` GameObject. Enforce các conflict rules:

| Rule | Khi nào | Hành động |
|------|---------|-----------|
| REQ-2.3 | Sheet mở | Ẩn SeedTray |
| REQ-2.4 | Sheet mở | Ẩn ModeBar, ghi nhớ trạng thái |
| REQ-2.4 | Sheet đóng | Khôi phục ModeBar nếu trước đó đang active |
| REQ-2.2 | Vào SeedMode | Đóng Sheet trước, rồi hiện SeedTray |

**Inspector refs cần assign:**
- `_seedTrayRoot` → GameObject `SeedTray`
- `_modeBarRoot` → GameObject `ModeBar`

**API cho ToolModeManager (Phase 2) gọi:**
```csharp
UILayerManager.OnEnterSeedMode()   // đóng sheet + hiện seed tray
UILayerManager.OnExitSeedMode()    // ẩn seed tray
```

---

## Scene Hierarchy sau Phase 1

```
Main (scene)
├── Main Camera
├── Canvas
│   ├── FarmWorld
│   │   ├── Tile_0_0 ... Tile_4_4   ← 25 tiles, mỗi tile có:
│   │   │                               Image (màu theo state)
│   │   │                               StateLabel (TMP, debug)
│   │   │                               TileVisualController
│   │   │                               TileTapHandler
│   ├── TopHUD
│   ├── SideButtons_Right
│   ├── BottomToolbar
│   ├── ModeBar                     ← hidden
│   ├── SeedTray                    ← hidden
│   ├── BottomSheet                 ← hidden, có BottomSheetController
│   │   ├── Handle                  ← có SheetHandleDrag
│   │   ├── Content
│   │   │   ├── TitleText
│   │   │   ├── StatusText
│   │   │   ├── TimerText
│   │   │   └── ActionsRow
│   │   │       ├── BtnHoe
│   │   │       ├── BtnPlant
│   │   │       ├── BtnOpenSeedTray
│   │   │       ├── BtnWater
│   │   │       ├── BtnWeed
│   │   │       ├── BtnPest
│   │   │       ├── BtnHarvest
│   │   │       └── BtnCleanup
│   │   └── BtnClose
│   └── PopupConfirm                ← hidden
└── Managers
    ├── TileManager
    ├── TileGridBuilder
    └── UILayerManager
```

---

## Luồng dữ liệu tổng thể (Phase 0 + 1)

```
[Player tap tile]
      ↓
TileTapHandler.OnPointerUp()
      ↓ (nếu không phải drag)
BottomSheetController.OpenForTile(coord)
      ↓
TileManager.GetTile(coord) → TileData
      ↓
PopulateContent(tile) → hiển thị title/status/actions đúng state
      ↓
OnSheetOpened event
      ↓
UILayerManager.HandleSheetOpened()
      → ẩn SeedTray
      → ẩn ModeBar (ghi nhớ state)

[Player đóng sheet]
      ↓
BottomSheetController.Close()
      ↓
OnSheetClosed event
      ↓
UILayerManager.HandleSheetClosed()
      → khôi phục ModeBar nếu cần

[State thay đổi — Phase 2+]
      ↓
TileManager.SetTileState(coord, newState)
      ↓
OnTileChanged event → TileVisualController.Refresh()
      → cập nhật màu tile
```

---

## Những gì chưa làm (Phase 2+)

| Phase | Nội dung |
|-------|---------|
| Phase 2 | ToolModeManager, InputRouter, ModeBar active, tile highlight |
| Phase 3 | SeedInventoryManager, SeedTray UI, PlantingMode |
| Phase 4 | CropGrowthManager, Water/Weed/Pest tools, problem icons |
| Phase 5 | HarvestMode, PlantedDead cleanup, PopupConfirm |
| Phase 6 | Animations, transitions, polish |

---

## Lưu ý khi chỉnh sửa

### Thêm tile state mới
1. Thêm value vào `TileState` enum
2. Thêm case trong `TileVisualController.Refresh()` (màu + label)
3. Thêm case trong `BottomSheetController.PopulateContent()` (content)
4. Thêm transition trong `TileManager.SetTileState()` nếu cần clear data

### Thay đổi grid size
Sửa `_gridWidth` / `_gridHeight` trong Inspector của `TileManager` và `TileGridBuilder`, sau đó chạy lại `Farm > Setup Phase 1`.

### Thêm action button mới vào BottomSheet
1. Thêm `[SerializeField] private GameObject _btnXxx` trong `BottomSheetController`
2. Thêm `Hide(_btnXxx)` vào `SetAllActionsHidden()`
3. Thêm `Show(_btnXxx)` vào đúng case trong `PopulateContent()`
4. Tạo button GO trong `Phase1SceneSetup.SetupBottomSheet()` và wire qua `SerializedObject`

### Debug tile state
Trong Play mode, gọi từ bất kỳ script nào:
```csharp
TileManager.Instance.SetTileState(new Vector2Int(2, 2), TileState.PlantedReady);
```
Tile sẽ đổi màu ngay lập tức và BottomSheet sẽ hiển thị đúng content khi tap.
