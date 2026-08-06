from pathlib import Path

from PIL import Image, ImageDraw

source = Path(__file__).parent / "rendered-06"
files = sorted(source.glob("study-*.png"))
thumb_width = 280
margin = 16
label_height = 24
columns = 3
rows = (len(files) + columns - 1) // columns

with Image.open(files[0]) as sample:
    ratio = thumb_width / sample.width
    thumb_height = int(sample.height * ratio)

sheet = Image.new(
    "RGB",
    (
        columns * thumb_width + (columns + 1) * margin,
        rows * (thumb_height + label_height) + (rows + 1) * margin,
    ),
    "white",
)
draw = ImageDraw.Draw(sheet)

for index, file in enumerate(files):
    with Image.open(file) as page:
        thumb = page.convert("RGB").resize((thumb_width, thumb_height))
    column = index % columns
    row = index // columns
    x = margin + column * (thumb_width + margin)
    y = margin + row * (thumb_height + label_height + margin)
    sheet.paste(thumb, (x, y + label_height))
    draw.text((x, y), file.stem, fill="black")

sheet.save(source / "contact-sheet.png")
