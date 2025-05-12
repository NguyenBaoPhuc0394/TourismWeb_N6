import json

# Đọc dữ liệu từ file JSON
with open('loc.json', 'r', encoding='utf-8') as f:
    locations = json.load(f)

# Hàm tạo câu lệnh migration cho Locations
def generate_location_migration_code(locations):
    code = "modelBuilder.Entity<Location>().HasData(\n"
    for i, location in enumerate(locations):
        code += "    new Location\n    {\n"
        code += f"        Id = \"{location.get('Id', '')}\",\n"
        code += f"        Name = \"{location.get('Name', '')}\"\n"
        code += "    }"
        code += ",\n" if i < len(locations) - 1 else "\n"
    code += ");"

    return code


# Tạo mã migration từ dữ liệu Locations
migration_code = generate_location_migration_code(locations)

# Lưu mã migration vào file
with open('location_migration_code.txt', 'w', encoding='utf-8') as f:
    f.write(migration_code)

print("Câu lệnh migration cho Locations đã được lưu vào 'location_migration_code.txt'")
