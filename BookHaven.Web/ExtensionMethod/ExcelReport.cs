using ClosedXML.Excel;

namespace BookHaven.Web.ExtensionMethod
{
	public static class ExcelReport
	{
		public static void AddHeader(this IXLWorksheet sheet, string[] Header)
		{
			for (int i = 0; i < Header.Length; i++)
			{
				sheet.Cell(1, i + 1).SetValue(Header[i]);
			}
		}
		public static void AddFormatBody(this IXLWorksheet sheet)
		{
			sheet.ColumnsUsed().AdjustToContents();//علي كد اطول كلمه
			sheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;//سنتر الكلام
			sheet.CellsUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
			sheet.CellsUsed().Style.Border.OutsideBorderColor = XLColor.Black;
		}
		public static void AddFormatHeader(this IXLWorksheet sheet)
		{
			var header = sheet.Range("A1", "G1");//(1,1,1,8)(r,c,r,c)
			header.Style.Fill.BackgroundColor = XLColor.DarkGray;
			header.Style.Font.FontColor = XLColor.White;
			header.Style.Font.SetBold();
		}
		public static void AddStyleTable(this IXLWorksheet sheet, int NumOfRow, int NumOfCol)
		{
			var rang = sheet.Range(1, 1, NumOfRow, NumOfCol);
			var table = rang.CreateTable();
			table.Theme = XLTableTheme.TableStyleMedium13;
			table.ShowAutoFilter = false;
		}
	}
}
