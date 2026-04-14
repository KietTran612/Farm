# FARM SCREEN SPEC PACK v1.1 — OVERVIEW

**Canvas:** `1080 x 1920`
**Orientation:** `Portrait only`

## Mục tiêu
Xây dựng màn Farm cho game mobile dọc, isometric nhẹ, modern casual, ưu tiên hoàn thiện hệ trồng trọt trước.

## Phạm vi bản này
- dọn cỏ hoang
- cuốc đất
- gieo hạt
- cây lớn theo nhiều phase
- tưới nước
- nhổ cỏ
- bắt sâu
- thu hoạch
- cây chết vì sâu
- dọn xác cây chết

## Triết lý tương tác
- **Tap** = thao tác chính xác 1 ô hoặc mở thông tin
- **Drag** = xử lý nhanh nhiều ô
- **Tap ô** = mở bottom sheet
- **Nhấc tay** = dừng drag chain, không thoát tool mode
- **Tool mode** chỉ thoát khi:
  - bấm lại tool
  - bấm `X` trên mode bar
  - tool không còn điều kiện dùng

## Camera
- 1 ngón: thao tác tool / chọn ô
- 2 ngón: pan camera
- pinch 2 ngón: zoom

## Các lớp UI chính
1. Farm world
2. Top HUD
3. Side utility buttons
4. Bottom toolbar
5. Mode bar
6. Seed tray
7. Bottom sheet
8. Popup confirm

## Hard rules
- Farm phải full-screen
- Toolbar luôn hiện
- Có đúng 6 tool chính + More
- Có mode bar phía trên toolbar
- Seed tray là thanh chọn hạt ngang nổi từ dưới lên
- Seed tray chỉ hiện hạt đang có
- Bottom sheet là loại 2 mức
- Timer không hiện đại trà
- Problem state phải có world feedback + icon nhỏ
- Seed tray và bottom sheet (bất kỳ mức nào) không mở cùng lúc

## Bộ file
- `farm-screen-uiux-spec-v1.1.md`: bản master đầy đủ
- `farm-layout-spec-v1.1.md`: bố cục và kích thước UI
- `farm-interaction-spec-v1.1.md`: luật thao tác và hành vi từng tool
- `farm-state-machine-spec-v1.1.md`: vòng đời state của ô đất, cây và tool mode
- `farm-agent-checklist-v1.1.md`: checklist nghiệm thu + hard rules ngắn
