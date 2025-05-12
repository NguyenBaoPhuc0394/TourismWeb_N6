import json
from datetime import datetime

def format_datetime(dt_str):
    try:
        dt = datetime.strptime(dt_str, "%Y-%m-%d %H:%M:%S")
        return f'new DateTime({dt.year}, {dt.month}, {dt.day}, {dt.hour}, {dt.minute}, {dt.second})'
    except:
        return "null"

def escape_multiline_string(s):
    # Escape " and make it compatible with @"" C# string
    return s.replace('"', '""')



def generate_hasdata_code(tours):

    code = "modelBuilder.Entity<Tour>().HasData(\n"
    for i, tour in enumerate(tours):
        detail = escape_multiline_string(tour['Detail_description'])
        schedule = escape_multiline_string(tour['Schedule_description'])
        code += "    new Tour\n    {\n"
        code += f"        Id = \"{tour['Id']}\",\n"
        code += f"        Name = \"{tour['Name']}\",\n"
        code += f"        Short_description = \"{tour['Short_description']}\",\n"
        code += f"        Detail_description = @\"{detail}\",\n"
        code += f"        Schedule_description = @\"{schedule}\",\n"
        code += f"        Category_Id = \"{tour['Category_Id']}\",\n"
        code += f"        Duration = \"{tour['Duration']}\",\n"
        code += f"        Price = {tour['Price']}m,\n"
        code += f"        Max_capacity = {tour['Max_capacity']},\n"
        code += f"        Location_Id = \"{tour['Location_id']}\",\n"
        code += f"        Create_at = {format_datetime(tour['Create_at'])},\n"
        code += f"        Update_at = {format_datetime(tour['Update_at'])}\n"
        code += "    }"
        code += ",\n" if i < len(tours) - 1 else "\n"
    code += ");"
    return code

# Load JSON data
with open("tour.json", "r", encoding="utf-8") as file:
    tours = json.load(file)

# Generate and print the migration code
migration_code = generate_hasdata_code(tours)
with open('tour_migration_code.txt', 'w', encoding='utf-8') as f:
    f.write(migration_code)

print("Câu lệnh migration đã được lưu vào 'migration_code.txt'")