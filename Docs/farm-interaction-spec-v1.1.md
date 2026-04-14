# FARM INTERACTION SPEC v1.1

## Core interaction rules
- Tap = thao tác chính xác 1 ô hoặc mở thông tin
- Drag = xử lý nhanh nhiều ô
- Tap ô = mở bottom sheet
- Nhấc tay = dừng drag chain, không thoát tool mode
- Tool mode giữ đến khi:
  - bấm lại tool
  - bấm X trên mode bar
  - tool không còn điều kiện dùng

## Camera rules
- 1 ngón = dùng tool hoặc chọn ô
- 2 ngón = pan camera
- pinch 2 ngón = zoom
- không cho 1 ngón vừa pan vừa thao tác tool

## Shared tool rules
- chọn tool thì vào tool mode
- entering another tool thay current mode
- mode active phải có đủ 3 tín hiệu:
  1. nút tool active
  2. ô hợp lệ highlight
  3. mode bar hiển thị trạng thái
- releasing finger does not exit tool mode
- camera gestures do not exit tool mode

## Bottom sheet behavior
- tap ô khi không drag action thì có thể mở bottom sheet
- bottom sheet không tự bật khi người chơi đang drag tool
- bottom sheet không làm ngắt drag chain
- seed tray và bottom sheet (bất kỳ mức nào) không mở cùng lúc
- khi bottom sheet mở, mode bar ẩn tạm thời; khi đóng thì hiện lại nếu vẫn còn tool mode active

## Tool: Cuốc đất
### Valid targets
- `WildGrass`
- `NormalSoil`

### Behavior
- tap từng ô để cuốc
- drag qua nhiều ô để cuốc

### Drag rule
- ô hợp lệ: xử lý ngay
- ô xung đột: chưa xử lý, ghi nhận lại
- drag xong mới hiện popup xác nhận cho ô xung đột

### Mandatory
- không popup giữa chừng
- không ngắt drag chain
- không phá cây tự động trong drag hàng loạt

## Tool: Hạt giống
### Entry paths
1. bấm tool Hạt giống -> mở seed tray -> chọn hạt
2. tap `TilledSoil` -> bottom sheet -> Gieo hạt -> mở seed tray

### Valid targets
- `TilledSoil`

### Behavior
- sau khi chọn hạt -> vào `PlantingMode(seedType)`
- tap = gieo 1 ô
- drag = gieo nhiều ô

### Invalid target
- bỏ qua
- không popup
- không tự đổi mode

### Exit
- bấm lại tool Hạt giống
- bấm X trên mode bar
- hoặc tool không còn dùng được

### Out-of-seed
- dừng gieo loại đó ngay
- thoát drag chain
- hiện lại seed tray
- không tự mở shop

## Tool: Tưới nước
### Valid targets
- `PlantedGrowing`

### Behavior
- tap hoặc drag
- tác dụng tức thì
- cây nhận trạng thái đủ nước ngay

### Already-watered
- vẫn cho thao tác xảy ra
- không cộng thêm tác dụng

## Tool: Nhổ cỏ
### Valid target
- ô có cỏ

### Behavior
- tap hoặc drag
- kéo tới đâu xử lý ngay tới đó
- ô không có cỏ thì bỏ qua
- không popup

## Tool: Bắt sâu
### Valid target
- ô có sâu

### Behavior
- tap hoặc drag
- kéo tới đâu xử lý ngay tới đó
- ô không có sâu thì bỏ qua
- không popup

## Tool: Thu hoạch
### Valid target
- `PlantedReady`

### Behavior
- tap để thu 1 ô
- drag để thu nhiều ô

### Unready rule
- ô chưa chín: bỏ qua hoàn toàn
- không popup
- không cảnh báo nặng ở bản đầu

### After harvest
- ô trở về `NormalSoil`
- muốn trồng tiếp phải cuốc lại

## Tool: Dọn xác cây chết
### Valid target
- `PlantedDead`

### Behavior
- tap từng ô
- có thể hỗ trợ drag

### After cleanup
- nhận item `cỏ`
- ô trở về `NormalSoil`

## Problem state visuals
### Thiếu nước
- đất nhạt hơn, có thể nứt nhẹ
- icon giọt nước/cảnh báo

### Cỏ
- cỏ mọc chen trên ô
- icon cỏ nhỏ

### Sâu
- sâu bám trên cây
- icon sâu nhỏ

### Mandatory
- luôn dùng world feedback + icon nhỏ

## Timer rules
- không hiển thị timer trên tất cả ô
- chỉ hiện khi:
  - ô đang được chọn
  - ô gần hoàn thành

## Popup confirm
### Usage
- chỉ dùng cho hành động rủi ro hoặc xung đột
- ví dụ drag cuốc đất qua vùng có ô đang trồng

### Content
- nói rõ bao nhiêu ô xung đột
- phần nào đã xử lý
- phần nào đang chờ xác nhận

### Container
- Width `760`
- Height `320`
- Radius `28`

### Buttons
- Hủy
- Xác nhận / Bỏ qua ô xung đột
- Height `56`
- Min width `180`
