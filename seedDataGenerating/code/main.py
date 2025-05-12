import json
from collections import Counter

# Đọc file schedule.json
with open("schedule.json", "r", encoding="utf-8") as f:
    schedules = json.load(f)

# Đếm số lần xuất hiện của mỗi Id
id_counts = Counter(schedule["Id"] for schedule in schedules)

# In ra các Id bị trùng
duplicates = {id_: count for id_, count in id_counts.items() if count > 1}

if duplicates:
    print("🔴 Các Id bị trùng trong dữ liệu Schedule:")
    for id_, count in duplicates.items():
        print(f" - Id: {id_} xuất hiện {count} lần")
else:
    print("✅ Không có Id nào bị trùng trong dữ liệu Schedule.")
