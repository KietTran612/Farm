# FARM SCREEN UI/UX SPEC v1.1
## Farming-first vertical slice for mobile portrait

**Canvas chuẩn:** `1080 x 1920`
**Orientation:** `Portrait only`
**Mục tiêu bản này:** hoàn thiện **hệ trồng trọt + UI trồng trọt** trước, các hệ khác chỉ chừa điểm nối để mở rộng về sau.

---

# 1. Product intent

## 1.1. Product goal
Xây dựng màn hình Farm cho game mobile dọc, lấy cảm hứng từ game nông trại cổ điển nhưng theo hướng hiện đại hơn:
- gọn hơn
- dễ đọc hơn trên mobile
- thao tác nhanh hơn bằng tap và drag
- vẫn giữ cảm giác lao động nông trại: dọn đất, cuốc, gieo, chăm sóc, thu hoạch

## 1.2. Experience goal
Người chơi phải cảm thấy:
- farm là trung tâm màn hình
- thao tác nhanh, trực tiếp, ít popup nặng
- nhìn tổng quan là hiểu farm đang có chuyện gì
- **tap để xem hoặc xử lý chính xác 1 ô**
- **drag để xử lý nhanh nhiều ô**
- khi đã chọn tool thì luôn biết rõ mình đang ở mode nào

## 1.3. First-release scope
Bản này chỉ tập trung hoàn thiện hệ trồng trọt gồm:
- dọn cỏ hoang
- cuốc đất
- gieo hạt
- cây phát triển qua nhiều phase
- tưới nước
- nhổ cỏ
- bắt sâu
- thu hoạch
- cây chết vì sâu nếu bỏ quá lâu
- dọn xác cây chết

Chưa cần hoàn thiện:
- social online thật
- quest hoàn chỉnh
- level progression hoàn chỉnh
- shop đầy đủ ngoài tab hạt giống
- backpack đầy đủ
- decoration system
- animal, factory, building loop

---

# 2. Design principles

## 2.1. Interaction principles
- **Tap** = thao tác chính xác lên 1 ô hoặc mở thông tin của ô.
- **Drag** = thao tác nhanh trên nhiều ô.
- **Tap ô đất/cây** = mở bottom sheet theo ngữ cảnh.
- **Nhấc tay** = chỉ dừng drag chain, **không tự thoát tool mode**.
- **Tool mode** giữ tới khi:
  - người chơi bấm lại tool đó
  - người chơi bấm nút `X` trên mode bar
  - tool không còn điều kiện sử dụng, ví dụ hết hạt

## 2.2. Camera principles
- **1 ngón**: dùng tool hoặc chọn ô
- **2 ngón**: pan camera
- **pinch 2 ngón**: zoom
- không cho 1 ngón vừa pan vừa dùng tool

## 2.3. Information principles
- farm view phải sạch, không spam text
- world feedback là lớp đọc nhanh
- bottom sheet là lớp đọc kỹ
- timer không hiển thị đại trà trên tất cả ô
- mode đang chọn phải được báo hiệu bằng nhiều lớp rõ ràng

## 2.4. UI system principles
- farm phải là full-screen visual center
- bottom toolbar luôn hiện
- bottom sheet dùng chung một khung, nội dung đổi theo state
- seed tray là thanh chọn nhanh, không phải shop thu nhỏ
- problem state phải đọc được bằng cả **art trong world** lẫn **icon nhỏ**

---

# 3. Screen geometry

## 3.1. Base canvas
- Width: `1080`
- Height: `1920`
- Portrait only

## 3.2. Safe area
- Top safe: `44 px`
- Left safe: `24 px`
- Right safe: `24 px`
- Bottom safe: `34 px`

## 3.3. Major vertical zones
### Top HUD zone
- Start Y: `44`
- Height: `92`
- End Y: `136`

### Bottom toolbar zone
- Height: `120`
- Bottom offset: `34`
- Toolbar top Y: `1766`
- Toolbar bottom Y: `1886`

### Mode bar zone
- Height: `64`
- Gap above toolbar: `16`
- Mode bar top Y: `1686`
- Mode bar bottom Y: `1750`

### Seed tray zone
- Height: `180`
- Suggested bottom offset: `250`
- Seed tray top Y: `1490`
- Seed tray bottom Y: `1670`

### Bottom sheet zone
Collapsed:
- Height: `260`
- Top Y: `1660`

Expanded:
- Height: `540`
- Top Y: `1380`

## 3.4. Overlay conflict rule
Do seed tray và bottom sheet có vùng chồng nhau ở cả 2 mức, bắt buộc áp dụng:
- **Seed tray và bottom sheet (bất kỳ mức nào) không được mở cùng lúc**
- Khi vào seed mode, bottom sheet phải đóng hoàn toàn
- Khi bottom sheet đang mở (collapsed hoặc expanded), seed tray phải ẩn

---

# 4. Screen layer hierarchy

## 4.1. Layer order from back to front
1. Farm world
2. Top HUD
3. Side utility buttons
4. Bottom toolbar
5. Mode bar
6. Seed tray
7. Bottom sheet
8. Popup confirm

## 4.2. Visibility priority rules
- Popup confirm luôn nằm trên cùng
- Bottom sheet cao hơn seed tray
- Seed tray cao hơn toolbar và mode bar
- Mode bar luôn nằm trên toolbar
- Top HUD và side buttons không được che farm quá nhiều

---

# 5. Layout spec

## 5.1. Farm world
Farm world là lớp visual chính, hiển thị toàn màn hình dưới UI overlay.

### Rule
- không đặt panel nền cố định chiếm giữa màn hình
- không chia màn thành 2 nửa UI và farm
- world phải nhìn được rộng theo trục chéo isometric
- UI chỉ là overlay nổi quanh mép

### Visible farm preference
- vùng đọc chính: từ Y `136` tới khoảng Y `1766`
- vẫn cho world kéo dài xuống dưới overlay để tạo cảm giác full-screen

---

## 5.2. Top HUD

### Container
- Anchor: `Top Stretch`
- X: `24`
- Y: `44`
- Width: `1032`
- Height: `92`
- Radius: `24`
- Background: panel mờ tối, alpha nhẹ
- Internal padding: `16 horizontal`, `14 vertical`

### Internal layout
Từ trái sang phải:

#### A. Profile button
- Size: `64 x 64`
- Avatar/icon inner size: `56 x 56`

#### B. Level badge
- Size: `72 x 40`
- Gap from profile: `12`

#### C. Currency group
2 capsule chính:
- Gold
- Premium gem

Mỗi capsule:
- Height: `44`
- Min width: `112`
- Icon: `24 x 24`
- Text size: `18`
- Horizontal padding: `12`
- Gap giữa capsule: `10`

#### D. Flexible spacer
Co giãn để đẩy utility buttons sang phải.

#### E. Utility buttons
Ví dụ:
- Settings
- Inbox/Notice placeholder

Mỗi button:
- Size: `48 x 48`
- Icon: `22 x 22`
- Gap: `12`

### Rule
- top HUD phải nhìn mỏng, không nặng nề
- text không được quá dày
- không thêm quá nhiều badge, counter hoặc panel con

---

## 5.3. Side utility buttons

### Right side main stack
- Right margin: `24`
- Suggested start Y: `250`
- Vertical gap: `16`

Mỗi button:
- Size: `72 x 72`
- Radius: `22`
- Icon: `32 x 32`

Suggested order:
1. Shop
2. Backpack
3. Quest placeholder
4. Social placeholder

### Left side buttons
Chỉ dùng nếu thật cần.

Mỗi button:
- Size: `72 x 72`
- Radius: `22`
- Icon: `32 x 32`

Suggested use:
- event
- task shortcut

### Rule
- cạnh phải là cột utility chính
- cạnh trái không được dày đặc
- không dùng quá 2 nút bên trái ở bản đầu

---

## 5.4. Bottom toolbar

### Container
- Anchor: `Bottom Center`
- Width: `1032`
- Height: `120`
- Bottom offset: `34`
- Radius: `30`
- Background: panel nổi, đậm hơn top HUD
- Internal horizontal padding: `18`

### Slot structure
Có `7 slot`:
1. Cuốc đất
2. Hạt giống
3. Tưới nước
4. Thu hoạch
5. Nhổ cỏ
6. Bắt sâu
7. More

### Per-slot size
- Cell width: khoảng `136`
- Visual button: `92 x 92`
- Icon: `42 x 42`
- Label: `16`
- Gap between slots: `8`

### Button states
#### Default
- nền trung tính
- icon ở trạng thái bình thường

#### Active
- sáng hơn rõ rệt
- có glow hoặc fill nổi bật
- scale `1.06`
- tạo cảm giác “đang cầm tool”

#### Disabled
- opacity `50%`

### Rule
- toolbar luôn hiện
- không ẩn toolbar khi đang chơi farm
- More chỉ là slot mở rộng, không thay thế tool chính

---

## 5.5. Mode bar

### Purpose
Báo hiệu tool mode hiện tại thật rõ.

### Container
- Anchor: `Bottom Center`
- Width: `560`
- Height: `64`
- Position: cách top toolbar `16`
- Top Y: `1686`
- Bottom Y: `1750`
- Radius: `22`

### Content
- Icon tool bên trái
- Text mode ở giữa
- Nút `X` bên phải

### Element sizes
- Tool icon: `28 x 28`
- Text: `20`
- Close button: `40 x 40`
- Close icon: `18 x 18`

### Example text
- Đang dùng: Cuốc đất
- Đang dùng: Hạt giống Cà rốt
- Đang dùng: Tưới nước
- Đang dùng: Thu hoạch

### Visibility rule
- chỉ hiện khi có tool mode active
- không active thì ẩn
- khi bottom sheet đang mở (bất kỳ mức nào), mode bar ẩn tạm thời
- khi bottom sheet đóng, mode bar hiện lại nếu vẫn còn tool mode active

### Exit rule
- bấm `X` thoát tool mode
- bấm lại tool đang active cũng thoát tool mode

---

## 5.6. Seed tray

### Purpose
Thanh chọn hạt nhanh. Không phải catalog toàn bộ hạt, không phải shop.

### Open conditions
- bấm tool `Hạt giống`
- hoặc từ action `Gieo hạt` trên bottom sheet

### Container
- Anchor: `Bottom Center`
- Width: `1032`
- Height: `180`
- Bottom offset: `250`
- Radius: `28`
- Top Y: `1490`
- Bottom Y: `1670`

### Content rules
- hiển thị dạng list ngang
- chỉ hiện **hạt đang sở hữu hoặc đang dùng được**
- sort mặc định: **thời gian trồng ngắn đến dài**
- có shortcut mở `Shop > Seed tab`

### Seed card
- Width: `160`
- Height: `132`
- Radius: `22`
- Horizontal gap: `12`

Card layout:
- Seed/crop icon: `56 x 56`
- Name: `16–18`
- Grow time: `14–16`

### Card states
#### Default
- nền trung tính

#### Selected
- viền sáng rõ
- nền sáng hơn
- scale `1.04`

#### Out of stock
- opacity giảm
- số lượng = 0
- không vào planting mode

### Shop shortcut
- Button: `72 x 72`
- Icon: `32 x 32`
- Action: mở thẳng `Shop > Seed tab`

### Hết hạt rule
Khi đang drag gieo mà hết hạt:
- dừng gieo loại đó ngay
- thoát drag chain
- seed tray hiện lại bình thường
- không tự nhảy vào shop
- không tự đóng tool Hạt giống nếu còn hạt khác có thể chọn

---

## 5.7. Bottom sheet

### Purpose
Tap vào ô để xem trạng thái và action đúng ngữ cảnh.

### Shared shell
Bottom sheet phải giữ một khung chung, nhưng nội dung bên trong đổi theo state.

### Container
- Anchor: `Bottom Stretch`
- Width: `1080`
- Radius top-left/top-right: `32`

### Heights
#### Collapsed
- Height: `260`
- Top Y: `1660`

#### Expanded
- Height: `540`
- Top Y: `1380`

### Internal structure
#### Grab handle
- Size: `84 x 8`
- Margin top: `14`

#### Header row
- Leading icon: `56 x 56`
- Title: `24`
- Subtitle/state: `18`
- Optional badge: small state pill

#### Info row
Hiển thị:
- thời gian
- phase
- cảnh báo hiện tại nếu có

#### Action row
- 2–3 action phù hợp nhất

Action button:
- Height: `56`
- Min width: `160`
- Radius: `18`
- Icon: `22 x 22`
- Label: `18`

#### Expanded detail block
- phase hiện tại
- % tiến độ
- thời gian còn lại
- trạng thái chăm sóc nếu có

### Behavior rules
- tap ô khi không ở drag action thì có thể mở bottom sheet
- bottom sheet không được tự bật khi người chơi đang drag tool
- bottom sheet không được làm ngắt drag chain
- seed tray và bottom sheet (bất kỳ mức nào) không mở cùng lúc

---

# 6. Tile and crop system

## 6.1. Tile states
Danh sách state chuẩn cho phiên bản này:
1. `WildGrass`
2. `NormalSoil`
3. `TilledSoil`
4. `PlantedGrowing`
5. `PlantedReady`
6. `PlantedDead`

## 6.2. Tile geometry
### Visual reference size
- Width: `160`
- Height: `96`

### Hit area
Do isometric khó chạm trên mobile, hitbox phải lớn hơn phần nhìn thấy:
- Hit width: `176`
- Hit height: `116`

## 6.3. Highlight system
### Valid tile
- tint rất nhẹ
- không làm lòe art nền

### Current pointed tile
- viền sáng rõ hơn valid tint
- có glow ngắn khi action được chấp nhận

### Invalid tile
- tint xám hoặc đỏ nhạt
- không dùng đỏ gắt

### Rule
Khi đang ở tool mode, người chơi phải nhìn ra ngay:
- ô nào có thể tương tác
- ô nào đang được trỏ tới
- ô nào không hợp lệ

---

# 7. Bottom sheet content by tile state

## 7.1. WildGrass
### Collapsed
- Title: `Cỏ hoang`
- Time: không cần
- Actions:
  - `Cuốc đất`

### Expanded
- mô tả ngắn: ô đất cần được cải tạo trước khi trồng trọt

## 7.2. NormalSoil
### Collapsed
- Title: `Đất thường`
- Actions:
  - `Cuốc đất`

### Expanded
- mô tả: ô đất phải được cuốc lại trước khi gieo hạt

## 7.3. TilledSoil
### Collapsed
- Title: `Đất đã cuốc`
- Actions:
  - `Gieo hạt`
  - `Mở thanh hạt giống`

### Expanded
- mô tả: ô đất đã sẵn sàng gieo trồng

## 7.4. PlantedGrowing
### Collapsed
- Title: tên cây
- Status: `Đang phát triển`
- Time: thời gian còn lại
- Actions tùy tình huống (chỉ hiện khi điều kiện thỏa mãn):
  - `Tưới nước` → chỉ hiện khi `!isWatered`
  - `Nhổ cỏ` → chỉ hiện khi `hasWeeds`
  - `Bắt sâu` → chỉ hiện khi `hasPests`
  - Nếu không có vấn đề nào, không hiện action chăm sóc

### Expanded
- phase hiện tại
- % tiến độ
- thời gian còn lại
- trạng thái chăm sóc
- cảnh báo nếu có khô nước, cỏ, sâu

## 7.5. PlantedReady
### Collapsed
- Title: tên cây
- Status: `Sẵn sàng thu hoạch`
- Actions:
  - `Thu hoạch`

### Expanded
- phase cuối
- xác nhận cây đã hoàn tất thời gian sinh trưởng

## 7.6. PlantedDead
### Collapsed
- Title: tên cây
- Status: `Đã chết`
- Actions:
  - `Dọn xác cây`

### Expanded
- nguyên nhân: chết vì sâu
- reward khi dọn: `cỏ`

---

# 8. Crop growth and care system

## 8.1. Growth phases
Mỗi loại cây có:
- tối thiểu `3 phase`
- tối đa `5 phase`

### Design rule
- cây ngắn ngày: có thể dùng `3 phase`
- cây trung bình: nên dùng `4 phase`
- cây dài ngày: nên dùng `5 phase` hoặc số lần đổi hình dạng đủ để tránh chán

## 8.2. Growth readability
- thay đổi hình dạng cây là tín hiệu chính
- progress bằng số chỉ hiện khi focus hoặc mở bottom sheet

## 8.3. Phase presentation rule
### World view
- ưu tiên đọc bằng art và silhouette của cây

### Focused view
- hiển thị phase hiện tại
- hiển thị % tiến độ
- hiển thị thời gian còn lại

---

# 9. Problem state visual language

## 9.1. Dry / thiếu nước
### World feedback
- đất nhạt hơn
- có thể có vết nứt nhẹ

### Icon
- icon nhỏ giọt nước/cảnh báo
- size icon vấn đề: `20 x 20`

## 9.2. Weeds / cỏ
### World feedback
- cỏ mọc chen trên ô

### Icon
- icon cỏ nhỏ `20 x 20`

## 9.3. Pest / sâu
### World feedback
- sâu bám trực tiếp trên cây

### Icon
- icon sâu nhỏ `20 x 20`

## 9.4. Mandatory rule
Problem state bắt buộc phải dùng **kết hợp**:
- world feedback
- icon nhỏ

Mục tiêu là:
- nhìn toàn cảnh là nhận ra
- quét nhanh cũng phân biệt được

---

# 10. Timer and info density

## 10.1. Timer rule
Không hiển thị timer trên tất cả ô đang trồng.

## 10.2. Timer chỉ hiện khi
- ô đang được chọn
- ô gần hoàn thành

## 10.3. Timer display style
- badge nhỏ hoặc line text ngắn
- không che art chính
- nếu dùng icon đồng hồ nhỏ: `18 x 18`

## 10.4. Density rule
- world view phải sạch
- bottom sheet mới là nơi đọc kỹ
- không spam số liệu trên farm

---

# 11. Tool system overview

## 11.1. Primary toolbar tools
1. Cuốc đất
2. Hạt giống
3. Tưới nước
4. Thu hoạch
5. Nhổ cỏ
6. Bắt sâu
7. More

## 11.2. Shared tool rules
- chọn tool thì vào tool mode
- tool mode giữ cho tới khi người chơi tự tắt hoặc tool không còn dùng được
- nhấc tay không thoát tool mode
- tap = xử lý 1 ô chính xác
- drag = xử lý nhiều ô nhanh
- mode active phải có đủ 3 tín hiệu:
  1. nút tool active
  2. ô hợp lệ được highlight
  3. mode bar hiển thị trạng thái hiện tại

---

# 12. Interaction spec by tool

## 12.1. Cuốc đất

### Entry
- bấm tool `Cuốc đất` trên toolbar
- hoặc action `Cuốc đất` trong bottom sheet

### Valid targets
- `WildGrass`
- `NormalSoil`

### Basic behavior
- tap từng ô để cuốc
- drag qua nhiều ô để cuốc hàng loạt

### Drag processing rule
- kéo tới ô hợp lệ nào thì xử lý ngay ô đó
- không chờ đến khi thả tay mới xử lý toàn bộ
- nếu đi qua ô xung đột:
  - ô xung đột chưa xử lý
  - ghi nhận lại trong danh sách xung đột
- drag xong mới hiện popup xác nhận cho phần xung đột

### Conflict examples
- ô đang có cây
- ô thuộc trạng thái không nên tác động trực tiếp
- ô cần xác nhận trước khi phá

### Mandatory rules
- không popup giữa chừng khi đang drag
- không ngắt drag chain
- không phá cây tự động trong drag hàng loạt

---

## 12.2. Hạt giống

### Entry paths
Có 2 đường vào:
1. bấm tool `Hạt giống`, mở seed tray, chọn hạt
2. tap `TilledSoil`, bottom sheet gợi ý `Gieo hạt`, mở seed tray

### Valid targets
- `TilledSoil`

### Basic behavior
Khi đã chọn hạt:
- vào planting mode
- tap để gieo 1 ô
- drag để gieo nhiều ô

### Invalid target rule
- gặp ô không hợp lệ thì bỏ qua
- không popup
- không tự đổi mode

### Release rule
- nhấc tay chỉ dừng drag
- không tự thoát planting mode

### Exit rule
- bấm lại tool `Hạt giống`
- bấm `X` trên mode bar
- hoặc tool không còn dùng được

### Out-of-seed rule
Khi hết hạt trong lúc drag:
- dừng gieo loại đó ngay
- thoát drag chain
- hiện lại seed tray bình thường
- không tự mở shop

---

## 12.3. Tưới nước

### Entry
- bấm tool `Tưới nước`
- hoặc chọn action `Tưới nước` từ bottom sheet

### Valid targets
- `PlantedGrowing`

### Basic behavior
- tap hoặc drag để tưới
- tác dụng tức thì
- cây nhận trạng thái đủ nước ngay

### Already-watered rule
Nếu cây đã đủ nước rồi:
- vẫn cho thao tác xảy ra
- không cộng thêm tác dụng

### Reason
Giữ drag chain mượt, không gây khựng tay.

---

## 12.4. Nhổ cỏ

### Entry
- bấm tool `Nhổ cỏ`
- hoặc action `Nhổ cỏ` từ bottom sheet

### Valid targets
- ô đang có cỏ

### Behavior
- tap hoặc drag
- kéo tới đâu xử lý ngay tới đó

### Rule
- ô không có cỏ thì bỏ qua
- không popup
- không tự thoát mode

---

## 12.5. Bắt sâu

### Entry
- bấm tool `Bắt sâu`
- hoặc action `Bắt sâu` từ bottom sheet

### Valid targets
- ô đang có sâu

### Behavior
- tap hoặc drag
- kéo tới đâu xử lý ngay tới đó

### Rule
- ô không có sâu thì bỏ qua
- không popup
- không tự thoát mode

---

## 12.6. Thu hoạch

### Entry
- bấm tool `Thu hoạch`
- hoặc action `Thu hoạch` từ bottom sheet

### Valid targets
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

---

## 12.7. Dọn xác cây chết

### Entry
- action `Dọn xác cây`
- hoặc tool mở rộng nếu về sau đưa vào More

### Valid targets
- `PlantedDead`

### Behavior
- tap từng ô
- có thể hỗ trợ drag để đồng nhất input

### After cleanup
- nhận item `cỏ`
- ô trở về `NormalSoil`

---

# 13. Crop event and failure logic

## 13.1. Random event generation
Các vấn đề phát sinh theo xác suất tại từng phase:
- khô nước
- cỏ
- sâu

## 13.2. Effect rules
- khô nước: tăng trưởng chậm hơn hoặc giảm sản lượng
- cỏ: tăng trưởng chậm hơn hoặc giảm sản lượng
- sâu: tăng trưởng chậm hơn hoặc giảm sản lượng; nếu để quá lâu thì cây chết

## 13.3. Death outcome
Khi cây chết vì sâu:
- để lại cây héo trên ô
- phải dọn xác mới dùng lại được
- dọn xác nhận item `cỏ`

---

# 14. State machine spec

## 14.1. Tile lifecycle
### Core lifecycle
`WildGrass -> TilledSoil -> PlantedGrowing -> PlantedReady -> Harvest -> NormalSoil`

Để khớp với quyết định hiện tại, khuyến nghị logic state như sau:
- `WildGrass` --Cuốc đất--> `TilledSoil`
- `NormalSoil` --Cuốc đất--> `TilledSoil`
- `TilledSoil` --Gieo hạt--> `PlantedGrowing`
- `PlantedGrowing` --Đủ thời gian--> `PlantedReady`
- `PlantedReady` --Thu hoạch--> `NormalSoil`
- `PlantedGrowing` --Sâu quá lâu--> `PlantedDead`
- `PlantedDead` --Dọn xác--> `NormalSoil`

## 14.2. Care sub-states for PlantedGrowing
PlantedGrowing có thể có các flag kèm theo:
- `isWatered`
- `hasWeeds`
- `hasPests`
- `isNearReady`

### Suggested representation
Không tách thành state machine lớn độc lập cho từng vấn đề. Dùng:
- base state = `PlantedGrowing`
- overlays/flags = khô nước, cỏ, sâu

Điều này giúp UI đơn giản hơn và tránh số lượng state nổ quá lớn.

## 14.3. Tool mode state machine
### Tool mode states
- `None`
- `HoeMode`
- `SeedSelectMode`
- `PlantingMode(seedType)`
- `WaterMode`
- `HarvestMode`
- `WeedMode`
- `PestMode`

### Transitions
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

### Mandatory rules
- entering another tool replaces current tool mode
- releasing finger does not exit tool mode
- camera gestures do not exit tool mode

---

# 15. Agent implementation phases

## Phase 0 — Layout skeleton
Agent phải dựng:
- farm full-screen
- top HUD
- side utility buttons
- bottom toolbar
- mode bar placeholder
- seed tray placeholder
- bottom sheet placeholder

## Phase 1 — Tile state shell + bottom sheet
Agent phải dựng:
- 6 tile states
- bottom sheet 2 mức
- nội dung đổi theo state

## Phase 2 — Tool mode framework
Agent phải dựng:
- toolbar active state
- mode bar
- valid tile highlight
- invalid tile feedback
- exit rules

## Phase 3 — Planting flow
Agent phải dựng:
- seed tray
- select seed
- planting mode
- tap plant
- drag plant
- hết hạt quay về seed tray

## Phase 4 — Care flow
Agent phải dựng:
- water flow
- weed flow
- pest flow
- world feedback + problem icons

## Phase 5 — Harvest and dead plant flow
Agent phải dựng:
- ready harvest flow
- skip unready tiles
- dead crop display
- cleanup flow
- reward grass item

## Phase 6 — Polish
Agent mới được làm:
- animation nhẹ
- readability polish
- visual refinement
- easing, glow, transitions

---

# 16. Size system

## 16.1. Icon sizes
- Top utility icon: `22 x 22`
- Currency icon: `24 x 24`
- Side floating icon: `32 x 32`
- Main tool icon: `42 x 42`
- Mode bar icon: `28 x 28`
- Seed card icon: `56 x 56`
- Bottom sheet action icon: `22 x 22`
- Close icon: `18 x 18`
- Tile problem icon: `20 x 20`
- Timer badge icon: `18 x 18`

## 16.2. Typography scale
- Large title: `24`
- Main body/action label: `18`
- Secondary text: `16`
- Caption/timer phụ: `14`
- Emphasis number: `20`

## 16.3. Spacing scale
- Micro gap: `8`
- Small gap: `12`
- Medium gap: `16`
- Large gap: `24`
- Section gap: `32`

## 16.4. Radius scale
- Small control: `18`
- Medium card: `22`
- Large panel: `28–32`

---

# 17. Motion and feedback rules

## 17.1. Button feedback
- Tool active scale: `1.06`
- Press scale: `0.96–0.98`

## 17.2. Seed card feedback
- Selected scale: `1.04`
- Selected card phải có glow hoặc viền rõ

## 17.3. Bottom sheet motion
- trượt lên xuống mềm
- không nảy quá mạnh

## 17.4. Tile action feedback
- action thành công nên có flash hoặc glow ngắn
- pointed tile nên có viền sáng rõ hơn valid tile

---

# 18. Popup confirm spec

## 18.1. Usage
Chỉ dùng cho hành động có rủi ro hoặc xung đột, ví dụ:
- drag cuốc đất qua vùng có ô đang trồng

## 18.2. Container
- Width: `760`
- Height: `320`
- Radius: `28`

## 18.3. Structure
- Title
- short description
- two buttons row

## 18.4. Buttons
- `Hủy`
- `Xác nhận` hoặc `Bỏ qua ô xung đột`

Mỗi button:
- Height: `56`
- Min width: `180`

## 18.5. Content rule
Popup phải nói rõ:
- có bao nhiêu ô xung đột
- phần nào đã xử lý rồi
- phần nào đang chờ xác nhận

---

# 19. Hard rules for AI agents

Agent **phải tuân thủ** các điều sau:
- màn hình là `1080 x 1920 portrait`
- farm là full-screen
- toolbar nằm dưới và luôn hiện
- có đúng 6 tool chính + More
- có mode bar phía trên toolbar
- seed tray là thanh ngang nổi từ dưới lên
- seed tray chỉ hiện hạt đang có
- bấm shop trong seed tray mở `Shop > Seed tab`
- bottom sheet là loại 2 mức
- tap để xem/chính xác, drag để thao tác nhanh
- tool mode không tự thoát khi nhấc tay
- problem state phải có world feedback + icon nhỏ
- timer không hiển thị đại trà
- seed tray và bottom sheet (bất kỳ mức nào) không mở cùng lúc
- popup confirm không được bật giữa lúc drag

---

# 20. Forbidden changes for AI agents

Agent **không được**:
- đổi layout thành chia đôi farm + side panel
- biến farm thành khung nhỏ ở giữa màn hình
- đổi bottom sheet thành popup mini nổi trên ô
- biến seed tray thành shop đầy đủ
- cho tool tự tắt sau mỗi thao tác
- cho thả tay là thoát mode
- cho timer hiện trên toàn bộ cây
- cho drag 1 ngón vừa pan camera vừa thao tác tool
- làm popup xác nhận bật ngay giữa drag chain

---

# 21. Acceptance checklist

## Layout
- [ ] Đúng canvas 1080 x 1920
- [ ] Farm full-screen
- [ ] Top HUD mỏng
- [ ] Side buttons gọn
- [ ] Bottom toolbar đủ 7 slot
- [ ] Mode bar đúng vị trí
- [ ] Seed tray không đè sai logic
- [ ] Bottom sheet có 2 mức

## Interaction
- [ ] Tap mở bottom sheet
- [ ] Drag xử lý nhiều ô
- [ ] Nhấc tay không thoát mode
- [ ] Bấm lại tool thoát mode
- [ ] Bấm X thoát mode
- [ ] 1 ngón thao tác, 2 ngón pan

## Farming states
- [ ] WildGrass -> Cuốc đất
- [ ] NormalSoil -> Cuốc đất
- [ ] TilledSoil -> Gieo hạt
- [ ] Growing -> chăm sóc
- [ ] Ready -> thu hoạch
- [ ] Dead -> dọn xác

## Tool behavior
- [ ] Cuốc đất drag xử lý ngay ô hợp lệ
- [ ] Ô xung đột chỉ confirm sau khi drag xong
- [ ] Gieo hạt drag được
- [ ] Hết hạt quay về seed tray
- [ ] Tưới thừa không cộng thêm tác dụng
- [ ] Thu hoạch bỏ qua ô chưa chín
- [ ] Thu xong về NormalSoil

## Visual readability
- [ ] Phase đọc bằng hình dạng cây
- [ ] Valid tile đủ rõ
- [ ] Pointed tile có viền sáng hơn
- [ ] Problem state có world feedback + icon
- [ ] Timer không spam

---

# 22. Short prompt for downstream agents

> Thiết kế màn Farm cho game mobile dọc 1080x1920. Farm phải là full-screen isometric nhẹ, UI overlay hiện đại, gọn, ưu tiên thao tác 1 tay. Bắt buộc có: Top HUD, side utility buttons, bottom toolbar gồm Cuốc đất, Hạt giống, Tưới nước, Thu hoạch, Nhổ cỏ, Bắt sâu, More; mode bar phía trên toolbar; seed tray ngang nổi từ dưới lên; bottom sheet 2 mức. Input rule bắt buộc: tap để xem hoặc thao tác 1 ô chính xác, drag để xử lý nhanh nhiều ô; tool mode giữ đến khi người chơi tự tắt hoặc tool không còn dùng được; nhấc tay không tự thoát mode. Seed tray chỉ hiện hạt đang sở hữu, sort theo thời gian trồng ngắn đến dài, có shortcut mở Shop > Seed tab. Timer không hiển thị đại trà. Problem state phải có dấu hiệu trực quan trong world và icon nhỏ để đọc nhanh. Không được tự ý đổi layout tổng thể, không biến bottom sheet thành popup mini, không biến seed tray thành shop, không để popup confirm bật giữa lúc drag.
