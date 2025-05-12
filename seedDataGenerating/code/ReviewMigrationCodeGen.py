import json
from datetime import datetime

with open("review.json", "r", encoding="utf-8") as f:
    reviews = json.load(f)

def format_datetime(dt_string):
    dt = datetime.strptime(dt_string, "%Y-%m-%d %H:%M:%S")
    return f"new DateTime({dt.year}, {dt.month}, {dt.day}, {dt.hour}, {dt.minute}, {dt.second})"

# Tạo mã EF Core
lines = []
lines.append("modelBuilder.Entity<Review>().HasData(")
for i, review in enumerate(reviews):
    comma = "," if i < len(reviews) - 1 else ""
    lines.append(f"""    new Review
    {{
        Id = "{review['Id']}",
        Tour_Id = "{review['Tour_Id']}",
        Customer_Id = "{"CUS001"}",
        Rating = {review['Rating']},
        Comment = @"{review['Comment']}",
        Create_at = {format_datetime(review['Create_at'])}
    }}{comma}""")
lines.append(");")

# Ghi ra file .txt
with open("review_migration_code.txt", "w", encoding="utf-8") as f:
    f.write("\n".join(lines))