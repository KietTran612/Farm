# FARM STATE MACHINE SPEC v1.1

## Tile state list
1. `WildGrass`
2. `NormalSoil`
3. `TilledSoil`
4. `PlantedGrowing`
5. `PlantedReady`
6. `PlantedDead`

## Tile lifecycle
Recommended flow:
- `WildGrass` --Cuốc đất--> `TilledSoil`
- `NormalSoil` --Cuốc đất--> `TilledSoil`
- `TilledSoil` --Gieo hạt--> `PlantedGrowing`
- `PlantedGrowing` --Đủ thời gian--> `PlantedReady`
- `PlantedReady` --Thu hoạch--> `NormalSoil`
- `PlantedGrowing` --Sâu quá lâu--> `PlantedDead`
- `PlantedDead` --Dọn xác--> `NormalSoil`

## Notes
- `WildGrass` và `NormalSoil` đều là trạng thái chưa trồng được ngay
- chỉ `TilledSoil` mới là target hợp lệ cho gieo hạt
- sau thu hoạch không quay về `TilledSoil`, mà quay về `NormalSoil`

## PlantedGrowing overlays / flags
Không tách thêm state machine quá lớn cho từng vấn đề. Dùng:
- base state = `PlantedGrowing`
- overlay flags:
  - `isWatered`
  - `hasWeeds`
  - `hasPests`
  - `isNearReady`

## Random problem generation
Các vấn đề phát sinh theo xác suất ở từng phase:
- khô nước
- cỏ
- sâu

## Effect rules
- khô nước: tăng trưởng chậm hơn hoặc giảm sản lượng
- cỏ: tăng trưởng chậm hơn hoặc giảm sản lượng
- sâu: tăng trưởng chậm hơn hoặc giảm sản lượng; nếu để quá lâu thì cây chết

## Death outcome
Khi cây chết vì sâu:
- để lại cây héo trên ô
- phải dọn xác
- dọn xác nhận item `cỏ`
- ô trở về `NormalSoil`

## Growth phase rules
Mỗi loại cây có:
- tối thiểu `3 phase`
- tối đa `5 phase`

Suggested:
- cây ngắn ngày -> `3 phase`
- cây trung bình -> `4 phase`
- cây dài ngày -> `5 phase`

## Readability rules
### World view
- phase đọc bằng hình dạng cây

### Focused view
- phase hiện tại
- % tiến độ
- thời gian còn lại

## Tool mode states
- `None`
- `HoeMode`
- `SeedSelectMode`
- `PlantingMode(seedType)`
- `WaterMode`
- `HarvestMode`
- `WeedMode`
- `PestMode`

## Tool mode transitions
- Toolbar hoe -> `HoeMode`
- Toolbar seed -> `SeedSelectMode`
- Select seed card -> `PlantingMode(seedType)`
- Toolbar water -> `WaterMode`
- Toolbar harvest -> `HarvestMode`
- Toolbar weed -> `WeedMode`
- Toolbar pest -> `PestMode`
- Tap same tool again -> `None`
- Tap mode bar X -> `None`
- Out of selected seed while planting -> `SeedSelectMode`

## Mandatory tool mode rules
- entering another tool replaces current tool mode
- releasing finger does not exit tool mode
- camera gestures do not exit tool mode

## Bottom sheet by state
### `WildGrass`
- collapsed:
  - title `Cỏ hoang`
  - action `Cuốc đất`

### `NormalSoil`
- collapsed:
  - title `Đất thường`
  - action `Cuốc đất`

### `TilledSoil`
- collapsed:
  - title `Đất đã cuốc`
  - actions:
    - `Gieo hạt`
    - `Mở thanh hạt giống`

### `PlantedGrowing`
- collapsed:
  - title tên cây
  - status `Đang phát triển`
  - time còn lại
  - actions theo tình huống (chỉ hiện khi điều kiện thỏa mãn):
    - `Tưới nước` → chỉ hiện khi `!isWatered`
    - `Nhổ cỏ` → chỉ hiện khi `hasWeeds`
    - `Bắt sâu` → chỉ hiện khi `hasPests`
    - Nếu không có vấn đề nào, không hiện action chăm sóc

### `PlantedReady`
- collapsed:
  - title tên cây
  - status `Sẵn sàng thu hoạch`
  - action `Thu hoạch`

### `PlantedDead`
- collapsed:
  - title tên cây
  - status `Đã chết`
  - action `Dọn xác cây`
