-- SEED DATA
USE green_wheel_db;
GO

-- Roles
INSERT INTO roles (created_at, updated_at, name, description)
VALUES
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 'Admin', 'System administrator'),
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 'Staff', 'Station staff'),
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), 'Customer', 'Vehicle customer');

-- Stations
INSERT INTO stations (created_at, updated_at, name, address)
VALUES
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'Trạm A', N'123 Quận 3, TP.HCM'),
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'Trạm B', N'456 Quận 6, TP.HCM');

-- Users (1 admin, 1 staff, 1 customer)
DECLARE @adminRole UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM roles WHERE name='Admin');
DECLARE @staffRole UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM roles WHERE name='Staff');
DECLARE @customerRole UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM roles WHERE name='Customer');

INSERT INTO users (created_at, updated_at, first_name, last_name, email, password, phone, sex, role_id)
VALUES
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'Nguyễn', N'Admin', 'admin@greenwheel.vn', '$2a$12$CZ2ikjkipa7p8kDYJN6o7.90TIjpIsswYSMr3iGYJBQQyj8/cgU06', '0901234567', 0, @adminRole),
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'Trần', N'Staff', 'staff@greenwheel.vn', '$2a$12$UnyAq2ckOtLYgpDQbNTTje5IPx9cbdTRPw5MB.sDg12OYjygBWJFa', '0902345678', 1, @staffRole),
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'Lê', N'Customer', 'customer@greenwheel.vn', '$2a$12$EF0KCPRK/mIt16yJtjCL1u/R5K0NXE7Mu9Q0s1WLX.iNOVrNEtXYe', NULL, 0, @customerRole);

-- Staffs (gán staff user vào station Hà Nội)
DECLARE @staffUser UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM users WHERE email='staff@greenwheel.vn');
DECLARE @adminUser UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM users WHERE email='admin@greenwheel.vn');
DECLARE @stationHN UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM stations WHERE name LIKE N'%Trạm A%');

INSERT INTO staffs (user_id, station_id)
VALUES 
	(@staffUser, @stationHN),
	(@adminUser, @stationHN);

-- Brands (VinFast)
INSERT INTO brands (created_at, updated_at, name, description, country, founded_year)
VALUES
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'VinFast', N'Thương hiệu xe điện Việt Nam', N'Việt Nam', 2017);

-- Vehicle Segments
INSERT INTO vehicle_segments (created_at, updated_at, name, description)
VALUES
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'Compact', N'Xe nhỏ gọn cho đô thị'),
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'SUV', N'Xe gầm cao thể thao đa dụng'),
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'Sedan', N'Xe 4 chỗ hạng sang');

-- Vehicle Models (VinFast VF e34, VF 5, VF 8)
DECLARE @brandVinfast UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM brands WHERE name='VinFast');
DECLARE @segmentSUV UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM vehicle_segments WHERE name='SUV');
DECLARE @segmentCompact UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM vehicle_segments WHERE name='Compact');

INSERT INTO vehicle_models (created_at, updated_at, name, description, cost_per_day, deposit_fee, reservation_fee, seating_capacity, number_of_airbags, motor_power, battery_capacity, eco_range_km, sport_range_km, brand_id, segment_id)
VALUES
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'VinFast VF e34', N'SUV điện hạng C', 12000, 10000, 10000, 5, 6, 110.0, 42.0, 285.0, 250.0, @brandVinfast, @segmentSUV),
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'VinFast VF 5', N'Compact SUV điện hạng A', 11000, 8000, 10000, 5, 4, 70.0, 37.0, 300.0, 260.0, @brandVinfast, @segmentCompact),
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'VinFast VF 8', N'SUV điện hạng D', 20000, 15000, 10000, 7, 10, 300.0, 90.0, 420.0, 400.0, @brandVinfast, @segmentSUV);

-- Vehicle Components (các bộ phận cơ bản)
INSERT INTO vehicle_components (created_at, updated_at, name, description, damage_fee)
VALUES
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'Động cơ điện', N'Bộ phận tạo công suất vận hành', 10000),
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'Pin', N'Nguồn năng lượng cho xe', 10000),
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'Hệ thống phanh', N'Tăng độ an toàn khi di chuyển', 10000),
  (SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'Nội thất', N'Ghế ngồi, màn hình, tiện ích nội thất', 10000);

-- Model Components (gán các components vào tất cả models)
DECLARE @compEngine UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM vehicle_components WHERE name=N'Động cơ điện');
DECLARE @compBattery UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM vehicle_components WHERE name=N'Pin');
DECLARE @compBrake UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM vehicle_components WHERE name=N'Hệ thống phanh');
DECLARE @compInterior UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM vehicle_components WHERE name=N'Nội thất');

INSERT INTO model_components (created_at, updated_at, model_id, component_id)
SELECT SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), vm.id, c.id
FROM vehicle_models vm
CROSS JOIN (SELECT id FROM vehicle_components) c;


-- Vehicles
DECLARE @stationA UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM stations WHERE name=N'Trạm A');
DECLARE @stationB UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM stations WHERE name=N'Trạm B');

DECLARE @modelE34 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM vehicle_models WHERE name=N'VinFast VF e34');
DECLARE @modelVf5 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM vehicle_models WHERE name=N'VinFast VF 5');
DECLARE @modelVf8 UNIQUEIDENTIFIER = (SELECT TOP 1 id FROM vehicle_models WHERE name=N'VinFast VF 8');

INSERT INTO vehicles (created_at, updated_at, license_plate, status, model_id, station_id)
VALUES
-- Trạm A
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51A-123.01', 0, @modelE34, @stationA),
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51A-123.02', 1, @modelE34, @stationA),
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51A-123.03', 2, @modelE34, @stationA),
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51A-123.04', 0, @modelVf5, @stationA),
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51A-123.05', 0, @modelVf5, @stationA),
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51A-123.06', 1, @modelVf5, @stationA),
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51A-123.07', 2, @modelVf8, @stationA),
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51A-123.08', 0, @modelVf8, @stationA),
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51A-123.09', 1, @modelVf8, @stationA),
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51A-123.10', 0, @modelE34, @stationA),

-- Trạm B
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51B-456.01', 0, @modelE34, @stationB),
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51B-456.02', 1, @modelE34, @stationB),
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51B-456.03', 2, @modelE34, @stationB),
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51B-456.04', 0, @modelVf5, @stationB),
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51B-456.05', 0, @modelVf5, @stationB),
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51B-456.06', 1, @modelVf5, @stationB),
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51B-456.07', 2, @modelVf8, @stationB),
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51B-456.08', 0, @modelVf8, @stationB),
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51B-456.09', 1, @modelVf8, @stationB),
(SYSDATETIMEOFFSET(), SYSDATETIMEOFFSET(), N'51B-456.10', 0, @modelE34, @stationB);