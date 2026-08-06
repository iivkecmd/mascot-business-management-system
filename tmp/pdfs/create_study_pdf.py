from pathlib import Path
import html
import re

from reportlab.lib import colors
from reportlab.lib.enums import TA_CENTER
from reportlab.lib.pagesizes import A4
from reportlab.lib.styles import ParagraphStyle, getSampleStyleSheet
from reportlab.lib.units import mm
from reportlab.pdfbase import pdfmetrics
from reportlab.pdfbase.ttfonts import TTFont
from reportlab.platypus import (
    BaseDocTemplate,
    Frame,
    KeepTogether,
    ListFlowable,
    ListItem,
    PageTemplate,
    Paragraph,
    Spacer,
    Table,
    TableStyle,
)

ROOT = Path(__file__).resolve().parents[2]
SOURCE = ROOT / "SKRIPTA_06_08_2026.md"
OUTPUT = ROOT / "output" / "pdf" / "Skripta_06_08_2026.pdf"

pdfmetrics.registerFont(TTFont("Arial", r"C:\Windows\Fonts\arial.ttf"))
pdfmetrics.registerFont(TTFont("Arial-Bold", r"C:\Windows\Fonts\arialbd.ttf"))
pdfmetrics.registerFont(TTFont("Consolas", r"C:\Windows\Fonts\consola.ttf"))

PAGE_W, PAGE_H = A4
NAVY = colors.HexColor("#19324A")
BLUE = colors.HexColor("#2D6A8A")
PALE = colors.HexColor("#EDF5F8")
INK = colors.HexColor("#24313A")
MUTED = colors.HexColor("#60717D")


def inline(text: str) -> str:
    value = html.escape(text)
    value = re.sub(r"`([^`]+)`", r'<font name="Consolas" color="#245B78">\1</font>', value)
    value = re.sub(r"\*\*([^*]+)\*\*", r"<b>\1</b>", value)
    return value


styles = getSampleStyleSheet()
styles.add(ParagraphStyle(
    name="CoverTitle", fontName="Arial-Bold", fontSize=25, leading=31,
    textColor=NAVY, alignment=TA_CENTER, spaceAfter=10,
))
styles.add(ParagraphStyle(
    name="CoverSub", fontName="Arial", fontSize=12, leading=18,
    textColor=MUTED, alignment=TA_CENTER,
))
styles.add(ParagraphStyle(
    name="H1x", fontName="Arial-Bold", fontSize=16, leading=21,
    textColor=NAVY, spaceBefore=12, spaceAfter=7, keepWithNext=True,
))
styles.add(ParagraphStyle(
    name="H2x", fontName="Arial-Bold", fontSize=12.5, leading=16,
    textColor=BLUE, spaceBefore=9, spaceAfter=5, keepWithNext=True,
))
styles.add(ParagraphStyle(
    name="Bodyx", fontName="Arial", fontSize=9.5, leading=14,
    textColor=INK, spaceAfter=5,
))
styles.add(ParagraphStyle(
    name="Codex", fontName="Consolas", fontSize=8.2, leading=12,
    textColor=INK, leftIndent=5, rightIndent=5,
))
styles.add(ParagraphStyle(
    name="Smallx", fontName="Arial", fontSize=8, leading=11, textColor=MUTED,
))


def footer(canvas, doc):
    canvas.saveState()
    canvas.setStrokeColor(colors.HexColor("#D7E2E8"))
    canvas.line(20 * mm, 14 * mm, PAGE_W - 20 * mm, 14 * mm)
    canvas.setFont("Arial", 8)
    canvas.setFillColor(MUTED)
    canvas.drawString(20 * mm, 9.5 * mm, "Družbalica - skripta za učenje")
    canvas.drawRightString(PAGE_W - 20 * mm, 9.5 * mm, f"Strana {doc.page}")
    canvas.restoreState()


doc = BaseDocTemplate(
    str(OUTPUT), pagesize=A4,
    leftMargin=20 * mm, rightMargin=20 * mm,
    topMargin=18 * mm, bottomMargin=20 * mm,
    title="Skripta - šta smo prešli 6. avgusta 2026.",
    author="Družbalica",
)
frame = Frame(doc.leftMargin, doc.bottomMargin, doc.width, doc.height, id="main")
doc.addPageTemplates(PageTemplate(id="study", frames=[frame], onPage=footer))

lines = SOURCE.read_text(encoding="utf-8").splitlines()
story = [
    Spacer(1, 20 * mm),
    Paragraph("SKRIPTA ZA UČENJE", styles["CoverTitle"]),
    Paragraph("Družbalica - backend tok za rezervacije", styles["CoverSub"]),
    Spacer(1, 8 * mm),
    Table([[""]], colWidths=[55 * mm], rowHeights=[2.2 * mm], style=TableStyle([
        ("BACKGROUND", (0, 0), (-1, -1), BLUE),
    ])),
    Spacer(1, 8 * mm),
    Paragraph("ASP.NET Core · Entity Framework Core · SQL Server · REST API · DTO · HTTP", styles["CoverSub"]),
    Spacer(1, 12 * mm),
    Paragraph("6. avgust 2026.", styles["CoverSub"]),
]

i = 1  # Preskoči originalni naslov; naslovna strana ga zamenjuje.
while i < len(lines):
    raw = lines[i]
    stripped = raw.strip()
    if not stripped or stripped == "---":
        i += 1
        continue
    if stripped.startswith("```"):
        lang = stripped[3:].strip()
        code = []
        i += 1
        while i < len(lines) and not lines[i].strip().startswith("```"):
            code.append(lines[i])
            i += 1
        code_text = "<br/>".join(html.escape(x).replace(" ", "&nbsp;") for x in code)
        label = f'<font name="Arial-Bold" color="#60717D">{html.escape(lang.upper())}</font><br/>' if lang else ""
        box = Table([[Paragraph(label + code_text, styles["Codex"])]], colWidths=[doc.width], style=TableStyle([
            ("BACKGROUND", (0, 0), (-1, -1), PALE),
            ("BOX", (0, 0), (-1, -1), 0.5, colors.HexColor("#C9DCE5")),
            ("LEFTPADDING", (0, 0), (-1, -1), 8),
            ("RIGHTPADDING", (0, 0), (-1, -1), 8),
            ("TOPPADDING", (0, 0), (-1, -1), 7),
            ("BOTTOMPADDING", (0, 0), (-1, -1), 7),
        ]))
        story.extend([box, Spacer(1, 5)])
        i += 1
        continue
    if stripped.startswith("|"):
        rows = []
        while i < len(lines) and lines[i].strip().startswith("|"):
            cells = [c.strip() for c in lines[i].strip().strip("|").split("|")]
            if not all(re.fullmatch(r"[-:]+", c) for c in cells):
                rows.append([Paragraph(inline(c), styles["Smallx"]) for c in cells])
            i += 1
        widths = [doc.width / len(rows[0])] * len(rows[0])
        table = Table(rows, colWidths=widths, repeatRows=1)
        table.setStyle(TableStyle([
            ("BACKGROUND", (0, 0), (-1, 0), NAVY),
            ("TEXTCOLOR", (0, 0), (-1, 0), colors.white),
            ("FONTNAME", (0, 0), (-1, 0), "Arial-Bold"),
            ("GRID", (0, 0), (-1, -1), 0.4, colors.HexColor("#C8D5DC")),
            ("VALIGN", (0, 0), (-1, -1), "TOP"),
            ("LEFTPADDING", (0, 0), (-1, -1), 6),
            ("RIGHTPADDING", (0, 0), (-1, -1), 6),
            ("TOPPADDING", (0, 0), (-1, -1), 5),
            ("BOTTOMPADDING", (0, 0), (-1, -1), 5),
            ("ROWBACKGROUNDS", (0, 1), (-1, -1), [colors.white, PALE]),
        ]))
        story.extend([table, Spacer(1, 5)])
        continue
    if stripped.startswith("# "):
        story.append(Paragraph(inline(stripped[2:]), styles["H1x"]))
    elif stripped.startswith("## "):
        story.append(Paragraph(inline(stripped[3:]), styles["H1x"]))
    elif stripped.startswith("### "):
        story.append(Paragraph(inline(stripped[4:]), styles["H2x"]))
    elif re.match(r"^\d+\. ", stripped):
        text = re.sub(r"^\d+\. ", "", stripped)
        story.append(ListFlowable(
            [ListItem(Paragraph(inline(text), styles["Bodyx"]))],
            bulletType="1", start=stripped.split(".")[0], leftIndent=16,
        ))
    elif stripped.startswith("- "):
        items = []
        while i < len(lines) and lines[i].strip().startswith("- "):
            items.append(ListItem(Paragraph(inline(lines[i].strip()[2:]), styles["Bodyx"])))
            i += 1
        story.append(ListFlowable(items, bulletType="bullet", bulletFontName="Arial", leftIndent=16))
        continue
    else:
        story.append(Paragraph(inline(stripped), styles["Bodyx"]))
    i += 1

OUTPUT.parent.mkdir(parents=True, exist_ok=True)
doc.build(story)
print(OUTPUT)
