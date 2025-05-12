import json

# Đọc dữ liệu từ file JSON
with open('cate.json', 'r', encoding='utf-8') as f:
    categories = json.load(f)



# Hàm tạo câu lệnh migration cho Categories
def generate_category_migration_code(categories):
    code = "modelBuilder.Entity<Category>().HasData(\n"
    for i, category in enumerate(categories):
        code += "    new Category\n    {\n"
        code += f"        Id = \"{category.get('Id', '')}\",\n"
        code += f"        Name = \"{category.get('Name', '')}\",\n"
        code += f"        Description = \"{category.get('Description', '')}\"\n"
        code += "    }"
        code += ",\n" if i < len(categories) - 1 else "\n"
    code += ");"

    return code


# Tạo mã migration từ dữ liệu Categories
migration_code = generate_category_migration_code(categories)

# Lưu mã migration vào file
with open('category_migration_code.txt', 'w', encoding='utf-8') as f:
    f.write(migration_code)

print("Câu lệnh migration cho Categories đã được lưu vào 'category_migration_code.txt'")
