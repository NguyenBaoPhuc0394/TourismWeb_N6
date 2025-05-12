import json

# Giả sử bạn lưu file là image.json
with open('image_data.json', 'r', encoding='utf-8') as f:
    images = json.load(f)

def generate_image_code(images):
    code = "modelBuilder.Entity<Image>().HasData(\n"
    for i, img in enumerate(images):
        code += f"    new Image\n    {{\n"
        code += f"        Id = \"{img['Id']}\",\n"
        code += f"        Tour_Id = \"{img['Tour_Id']}\",\n"
        code += f"        Url = \"{img['Url']}\"\n"
        code += f"    }}"
        if i != len(images) - 1:
            code += ",\n"
        else:
            code += "\n"
    code += ");"
    return code

# Ghi ra file
image_code = generate_image_code(images)
with open('image_migration.txt', 'w', encoding='utf-8') as f:
    f.write(image_code)

