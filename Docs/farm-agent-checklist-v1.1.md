# FARM AGENT CHECKLIST v1.1

## Hard rules
- [ ] Canvas đúng `1080 x 1920 portrait`
- [ ] Farm full-screen
- [ ] Bottom toolbar luôn hiện
- [ ] Có đúng 6 tool chính + More
- [ ] Có mode bar phía trên toolbar
- [ ] Seed tray là thanh ngang nổi từ dưới lên
- [ ] Seed tray chỉ hiện hạt đang có
- [ ] Shortcut trong seed tray mở `Shop > Seed tab`
- [ ] Bottom sheet là loại 2 mức
- [ ] Tool mode không tự thoát khi nhấc tay
- [ ] Problem state có world feedback + icon nhỏ
- [ ] Timer không hiển thị đại trà
- [ ] Seed tray và bottom sheet (bất kỳ mức nào) không mở cùng lúc
- [ ] Popup confirm không bật giữa lúc drag

## Forbidden changes
- [ ] Không chia đôi màn hình farm + panel
- [ ] Không biến farm thành khung nhỏ ở giữa
- [ ] Không đổi bottom sheet thành popup mini
- [ ] Không biến seed tray thành shop đầy đủ
- [ ] Không cho tool tự tắt sau mỗi thao tác
- [ ] Không cho thả tay là thoát mode
- [ ] Không cho timer hiện trên toàn bộ cây
- [ ] Không cho drag 1 ngón vừa pan camera vừa thao tác tool
- [ ] Không bật popup confirm ngay giữa drag chain

## Layout checklist
- [ ] Top HUD mỏng
- [ ] Side buttons gọn
- [ ] Bottom toolbar đủ 7 slot
- [ ] Mode bar đúng vị trí
- [ ] Seed tray không đè sai logic
- [ ] Bottom sheet có 2 mức

## Interaction checklist
- [ ] Tap mở bottom sheet
- [ ] Drag xử lý nhiều ô
- [ ] Bấm lại tool thoát mode
- [ ] Bấm X thoát mode
- [ ] 1 ngón thao tác, 2 ngón pan

## Farming state checklist
- [ ] WildGrass -> Cuốc đất
- [ ] NormalSoil -> Cuốc đất
- [ ] TilledSoil -> Gieo hạt
- [ ] Growing -> chăm sóc
- [ ] Ready -> thu hoạch
- [ ] Dead -> dọn xác

## Tool behavior checklist
- [ ] Cuốc đất drag xử lý ngay ô hợp lệ
- [ ] Ô xung đột chỉ confirm sau khi drag xong
- [ ] Gieo hạt drag được
- [ ] Hết hạt quay về seed tray
- [ ] Tưới thừa không cộng thêm tác dụng
- [ ] Thu hoạch bỏ qua ô chưa chín
- [ ] Thu xong về NormalSoil

## Visual readability checklist
- [ ] Phase đọc bằng hình dạng cây
- [ ] Valid tile đủ rõ
- [ ] Pointed tile có viền sáng hơn
- [ ] Problem state có world feedback + icon
- [ ] Timer không spam
