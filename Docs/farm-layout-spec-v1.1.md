# FARM LAYOUT SPEC v1.1

## Canvas và safe area
- Canvas: `1080 x 1920`
- Top safe: `44`
- Left safe: `24`
- Right safe: `24`
- Bottom safe: `34`

## Vùng dọc chính
### Top HUD
- X: `24`
- Y: `44`
- Width: `1032`
- Height: `92`

### Bottom toolbar
- Width: `1032`
- Height: `120`
- Bottom offset: `34`
- Top Y: `1766`
- Bottom Y: `1886`

### Mode bar
- Width: `560`
- Height: `64`
- Gap trên toolbar: `16`
- Top Y: `1686`
- Bottom Y: `1750`

### Seed tray
- Width: `1032`
- Height: `180`
- Bottom offset: `250`
- Top Y: `1490`
- Bottom Y: `1670`

### Bottom sheet
Collapsed:
- Height: `260`
- Top Y: `1660`

Expanded:
- Height: `540`
- Top Y: `1380`

## Rule chồng lớp
- Seed tray và bottom sheet (bất kỳ mức nào) không mở cùng lúc
- Khi vào seed mode, bottom sheet phải đóng hoàn toàn
- Khi bottom sheet đang mở (collapsed hoặc expanded), seed tray phải ẩn
- Khi bottom sheet đang mở, mode bar ẩn tạm thời; hiện lại khi bottom sheet đóng nếu vẫn còn tool mode active

## Farm world
- full-screen dưới toàn bộ UI overlay
- không chia đôi màn hình
- không đặt panel lớn cố định giữa màn hình
- vùng nhìn chính: từ Y `136` tới khoảng Y `1766`

## Top HUD
### Container
- Anchor: `Top Stretch`
- Width: `1032`
- Height: `92`
- Radius: `24`
- Padding: `16 horizontal`, `14 vertical`

### Thành phần
#### Profile button
- `64 x 64`
- avatar inner `56 x 56`

#### Level badge
- `72 x 40`
- gap `12`

#### Currency capsules
Mỗi capsule:
- Height `44`
- Min width `112`
- Icon `24 x 24`
- Text `18`
- Horizontal padding `12`
- Gap `10`

#### Utility buttons
Mỗi button:
- `48 x 48`
- icon `22 x 22`
- gap `12`

## Side utility buttons
### Cạnh phải
- Right margin `24`
- Start Y `250`
- Vertical gap `16`

Mỗi button:
- `72 x 72`
- radius `22`
- icon `32 x 32`

Order:
1. Shop
2. Backpack
3. Quest placeholder
4. Social placeholder

### Cạnh trái
- tối đa 1–2 nút
- mỗi button `72 x 72`
- icon `32 x 32`

## Bottom toolbar
### Container
- Width `1032`
- Height `120`
- Radius `30`
- Internal horizontal padding `18`

### Slots
1. Cuốc đất
2. Hạt giống
3. Tưới nước
4. Thu hoạch
5. Nhổ cỏ
6. Bắt sâu
7. More

### Kích thước mỗi slot
- Cell width khoảng `136`
- Visual button `92 x 92`
- Icon `42 x 42`
- Label `16`
- Gap `8`

### State
Default:
- nền trung tính

Active:
- sáng hơn rõ
- glow/fill nổi bật
- scale `1.06`

Disabled:
- opacity `50%`

## Mode bar
### Container
- Width `560`
- Height `64`
- Radius `22`

### Thành phần
- Tool icon `28 x 28`
- Text `20`
- Close button `40 x 40`
- Close icon `18 x 18`

## Seed tray
### Container
- Width `1032`
- Height `180`
- Radius `28`

### Seed card
- Width `160`
- Height `132`
- Radius `22`
- Gap `12`

Card layout:
- icon `56 x 56`
- name `16–18`
- grow time `14–16`

### Shop shortcut
- Button `72 x 72`
- Icon `32 x 32`

## Bottom sheet
### Container
- Width `1080`
- Radius top `32`

### Heights
- Collapsed `260`
- Expanded `540`

### Structure
- Grab handle `84 x 8`
- Header icon `56 x 56`
- Title `24`
- Subtitle `18`

### Action button
- Height `56`
- Min width `160`
- Radius `18`
- Icon `22 x 22`
- Label `18`

## Tile geometry
### Visual reference
- Width `160`
- Height `96`

### Hit area
- Width `176`
- Height `116`

## Highlight
Valid:
- tint rất nhẹ

Pointed:
- viền sáng hơn + glow ngắn

Invalid:
- tint xám hoặc đỏ nhạt

## Size system
### Icon
- Top utility `22 x 22`
- Currency `24 x 24`
- Side floating `32 x 32`
- Main tool `42 x 42`
- Mode bar `28 x 28`
- Seed card `56 x 56`
- Bottom sheet action `22 x 22`
- Close `18 x 18`
- Problem icon `20 x 20`
- Timer badge `18 x 18`

### Typography
- Large title `24`
- Main/action `18`
- Secondary `16`
- Caption/timer `14`
- Emphasis number `20`

### Spacing
- Micro `8`
- Small `12`
- Medium `16`
- Large `24`
- Section `32`

### Radius
- Small control `18`
- Medium card `22`
- Large panel `28–32`
