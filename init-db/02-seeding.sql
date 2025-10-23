USE green_wheel_db;
GO

-- ============================================
-- ROLES
-- ============================================
INSERT INTO roles (name, description)
VALUES
  ('Admin', 'System administrator'),
  ('Staff', 'Station staff'),
  ('Customer', 'Vehicle customer');
GO

-- ============================================
-- STATIONS
-- ============================================
INSERT INTO stations (name, address)
VALUES
  (N'Trạm A', N'123 Quận 3, TP.HCM'),
  (N'Trạm B', N'456 Quận 6, TP.HCM');
GO

-- ============================================
-- USERS (2 admins, 1 staff, 1 customer)
-- ============================================
DECLARE @adminRole UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM roles WHERE name='Admin');
DECLARE @staffRole UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM roles WHERE name='Staff');
DECLARE @customerRole UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM roles WHERE name='Customer');

INSERT INTO users (first_name, last_name, email, password, phone, sex, role_id)
VALUES
  (N'Nguyễn', N'AdminA', 'adminA@greenwheel.vn', '$2a$12$CZ2ikjkipa7p8kDYJN6o7.90TIjpIsswYSMr3iGYJBQQyj8/cgU06', '0901111111', 0, @adminRole),
  (N'Phạm', N'AdminB', 'adminB@greenwheel.vn', '$2a$12$CZ2ikjkipa7p8kDYJN6o7.90TIjpIsswYSMr3iGYJBQQyj8/cgU06', '0902222222', 1, @adminRole),
  (N'Trần', N'Staff', 'staff@greenwheel.vn', '$2a$12$UnyAq2ckOtLYgpDQbNTTje5IPx9cbdTRPw5MB.sDg12OYjygBWJFa', '0902345678', 1, @staffRole),
  (N'Lê', N'Customer', 'customer@greenwheel.vn', '$2a$12$EF0KCPRK/mIt16yJtjCL1u/R5K0NXE7Mu9Q0s1WLX.iNOVrNEtXYe', '0909999999', 0, @customerRole);
GO

-- ============================================
-- STAFFS
-- ============================================
DECLARE @adminA UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM users WHERE email='adminA@greenwheel.vn');
DECLARE @adminB UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM users WHERE email='adminB@greenwheel.vn');
DECLARE @staffUser UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM users WHERE email='staff@greenwheel.vn');
DECLARE @stationA UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM stations WHERE name LIKE N'%Trạm A%');
DECLARE @stationB UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM stations WHERE name LIKE N'%Trạm B%');

INSERT INTO staffs (user_id, station_id)
VALUES
  (@adminA, @stationA),
  (@adminB, @stationB),
  (@staffUser, @stationA);
GO

-- ============================================
-- BRANDS
-- ============================================
INSERT INTO brands (name, description, country, founded_year)
VALUES
  (N'VinFast', N'Thương hiệu xe điện Việt Nam', N'Việt Nam', 2017);
GO

-- ============================================
-- VEHICLE SEGMENTS
-- ============================================
INSERT INTO vehicle_segments (name, description)
VALUES
  (N'Compact', N'Xe nhỏ gọn cho đô thị'),
  (N'SUV', N'Xe gầm cao thể thao đa dụng');
GO

-- ============================================
-- VEHICLE MODELS
-- ============================================
DECLARE @brandVinfast UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM brands WHERE name='VinFast');
DECLARE @segmentSUV UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM vehicle_segments WHERE name='SUV');
DECLARE @segmentCompact UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM vehicle_segments WHERE name='Compact');

INSERT INTO vehicle_models (name, description, cost_per_day, deposit_fee, reservation_fee, seating_capacity, number_of_airbags, motor_power, battery_capacity, eco_range_km, sport_range_km, brand_id, segment_id)
VALUES
  (N'VinFast VF e34', N'SUV điện hạng C', 12000, 10000, 10000, 5, 6, 110.0, 42.0, 285.0, 250.0, @brandVinfast, @segmentSUV),
  (N'VinFast VF 5', N'Compact SUV điện hạng A', 11000, 8000, 10000, 5, 4, 70.0, 37.0, 300.0, 260.0, @brandVinfast, @segmentCompact);
GO

-- ============================================
-- VEHICLE COMPONENTS
-- ============================================
INSERT INTO vehicle_components (name, description, damage_fee)
VALUES
  (N'Động cơ điện', N'Bộ phận tạo công suất vận hành', 10000),
  (N'Pin', N'Nguồn năng lượng cho xe', 10000),
  (N'Hệ thống phanh', N'Tăng độ an toàn khi di chuyển', 10000),
  (N'Nội thất', N'Ghế ngồi, màn hình, tiện ích nội thất', 10000);
GO

-- ============================================
-- MODEL COMPONENTS
-- ============================================
INSERT INTO model_components (model_id, component_id)
SELECT m.id, c.id
FROM vehicle_models m
CROSS JOIN vehicle_components c;
GO

-- ============================================
-- VEHICLES
-- ============================================
DECLARE @modelE34 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM vehicle_models WHERE name=N'VinFast VF e34');
DECLARE @modelVf5 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM vehicle_models WHERE name=N'VinFast VF 5');
DECLARE @stationA2 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM stations WHERE name LIKE N'%Trạm A%');
DECLARE @stationB2 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM stations WHERE name LIKE N'%Trạm B%');

INSERT INTO vehicles (license_plate, status, model_id, station_id)
VALUES
(N'51A-123.01', 0, @modelE34, @stationA2),
(N'51A-123.02', 0, @modelVf5, @stationA2),
(N'51B-456.01', 0, @modelE34, @stationB2),
(N'51B-456.02', 0, @modelVf5, @stationB2);
GO

-- ============================================
-- RENTAL CONTRACTS (3 hợp đồng bị huỷ - status = 5)
-- ============================================
DECLARE @customerUser UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM users WHERE email='customer@greenwheel.vn');
DECLARE @handoverStaff UNIQUEIDENTIFIER = (SELECT TOP 1 user_id FROM staffs WHERE user_id = (SELECT id FROM users WHERE email='staff@greenwheel.vn'));
DECLARE @adminA2 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM users WHERE email='adminA@greenwheel.vn');
DECLARE @adminB2 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM users WHERE email='adminB@greenwheel.vn');
DECLARE @stationA3 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM stations WHERE name LIKE N'%Trạm A%');
DECLARE @stationB3 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM stations WHERE name LIKE N'%Trạm B%');
DECLARE @vehA UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM vehicles WHERE license_plate=N'51A-123.01');
DECLARE @vehB UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM vehicles WHERE license_plate=N'51B-456.01');
DECLARE @vehC UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM vehicles WHERE license_plate=N'51A-123.02');

INSERT INTO rental_contracts
(description, notes, start_date, end_date, status, is_signed_by_staff, is_signed_by_customer,
 vehicle_id, customer_id, handover_staff_id, return_staff_id, station_id)
VALUES
-- ❌ Hợp đồng bị huỷ 1
(N'Hợp đồng A1', N'Huỷ do thay đổi kế hoạch',
 DATEADD(DAY, -5, SYSDATETIMEOFFSET()), DATEADD(DAY, -3, SYSDATETIMEOFFSET()),
 5, 0, 0, @vehA, @customerUser, @handoverStaff, @adminA2, @stationA3),

-- ❌ Hợp đồng bị huỷ 2
(N'Hợp đồng B1', N'Huỷ vì không cung cấp đủ giấy tờ',
 DATEADD(DAY, -4, SYSDATETIMEOFFSET()), DATEADD(DAY, -2, SYSDATETIMEOFFSET()),
 5, 0, 0, @vehB, @customerUser, @handoverStaff, @adminB2, @stationB3),

-- ❌ Hợp đồng bị huỷ 3
(N'Hợp đồng A2', N'Huỷ do không thanh toán tiền cọc',
 DATEADD(DAY, -2, SYSDATETIMEOFFSET()), DATEADD(DAY, -1, SYSDATETIMEOFFSET()),
 5, 0, 0, @vehC, @customerUser, @handoverStaff, @adminA2, @stationA3);
GO
