import json
from datetime import datetime

# Load dữ liệu từ file JSON
with open('schedule.json', 'r', encoding='utf-8') as f:
    schedules = json.load(f)

# Hàm sinh EF code
def generate_schedule_code(schedules):
    code = "modelBuilder.Entity<Schedule>().HasData(\n"
    for i, sche in enumerate(schedules):
        # Tách ngày giờ
        start_date = datetime.strptime(sche['Start_date'], "%Y-%m-%d")
        create_at = datetime.strptime(sche['Create_at'], "%Y-%m-%d %H:%M:%S")

        code += f"    new Schedule\n    {{\n"
        code += f"        Id = \"{sche['Id']}\",\n"
        code += f"        Start_date = new DateOnly({start_date.year}, {start_date.month}, {start_date.day}),\n"
        code += f"        Available = {sche['Available']},\n"
        code += f"        Status = {sche['Status']},\n"
        code += f"        Adult_price = {sche['Adult_price']}m,\n"
        code += f"        Children_price = {sche['Children_price']}m,\n"
        code += f"        Discount = {int(sche['Discount'] * 100)},\n"
        code += f"        Create_at = new DateTime({create_at.year}, {create_at.month}, {create_at.day}, {create_at.hour}, {create_at.minute}, {create_at.second}),\n"
        code += f"        Tour_Id = \"{sche['Tour_Id']}\"\n"
        code += f"    }}"

        if i != len(schedules) - 1:
            code += ",\n"
        else:
            code += "\n"
    code += ");"
    return code

# Ghi ra file
ef_code = generate_schedule_code(schedules)
with open('schedule_migration.txt', 'w', encoding='utf-8') as f:
    f.write(ef_code)

