using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

namespace SolutionWeb.Common
{
    public class ExcelExtHelper
    {
        public ExcelExtHelper()
        {
        }

        #region 案件模板导出
        /// <summary>
        /// 
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="mbPath"></param>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public static bool ToSingleMbExcel(string savePath, string mbPath, List<SingleMb> dataList, List<CaseRowInfo> listCaseRow)
        {
            try
            {
                IWorkbook workbook;
                try
                {
                    using (FileStream file = new FileStream(mbPath, FileMode.Open, FileAccess.Read))
                    {
                        workbook = new HSSFWorkbook(file);//创建对应文件EXCEL2003
                    }
                }
                catch (Exception)
                {
                    using (FileStream file = new FileStream(mbPath, FileMode.Open, FileAccess.Read))
                    {
                        workbook = new XSSFWorkbook(file);//创建对应文件EXCEL2007
                    }
                }


                #region 画表格
                #region 样式
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.WrapText = true;//换行
                //设置单元格上下左右边框线  
                cellStyle.BorderTop = BorderStyle.Thin;
                cellStyle.BorderBottom = BorderStyle.Thin;
                cellStyle.BorderLeft = BorderStyle.Thin;
                cellStyle.BorderRight = BorderStyle.Thin;
                //文字水平和垂直对齐方式  
                cellStyle.Alignment = HorizontalAlignment.Center;
                cellStyle.VerticalAlignment = VerticalAlignment.Center;

                //第二种样式
                ICellStyle leftCellStyle = workbook.CreateCellStyle();
                leftCellStyle.CloneStyleFrom(cellStyle);
                leftCellStyle.Alignment = HorizontalAlignment.Left; //水平居左
                #endregion


                ISheet sheet = workbook.GetSheetAt(0);
                for (int i = 5; i <= listCaseRow.Count + 5; i++)//从第6行开始加行
                {
                    IRow row = sheet.CreateRow(i);
                    if (i != listCaseRow.Count + 5)//普通行
                    {
                        row.HeightInPoints = 30;//行高
                        for (int j = 0; j <= 5; j++)//每行加6单元格
                        {
                            ICell cell = row.GetCell(j, MissingCellPolicy.CREATE_NULL_AS_BLANK); //在行中创建单元格
                            if (1 < j && j < 5)
                            {
                                cell.CellStyle = leftCellStyle;//水平居左样式
                            }
                            else
                            {
                                cell.CellStyle = cellStyle;//居中样式
                            }
                        }
                        sheet.AddMergedRegion(new CellRangeAddress(i, i, 2, 4));//合并2-4列
                    }
                    else//备注行
                    {
                        row.HeightInPoints = 120;//行高
                        for (int j = 0; j <= 5; j++)//每行加6单元格
                        {
                            ICell cell = row.GetCell(j, MissingCellPolicy.CREATE_NULL_AS_BLANK); //在行中创建单元格
                            if (0 < j)
                            {
                                cell.CellStyle = leftCellStyle;//水平居左样式
                            }
                            else
                            {
                                cell.CellStyle = cellStyle;//居中样式
                            }
                        }
                        sheet.AddMergedRegion(new CellRangeAddress(i, i, 1, 5));//合并2-4列
                    }
                }
                #endregion




                //再填空
                foreach (SingleMb item in dataList)
                {
                    IRow rowhead = sheet.GetRow(item.rowIndex - 1);
                    ICell cellhead = rowhead.GetCell(item.cellIndex - 1);
                    cellhead.SetCellValue(item.value);//写入

                }
                //循环数据
                string hbcell = "";
                int m = 5;
                for (int i = 5; i <= listCaseRow.Count + 5; i++)//从第6行开始填写
                {
                    if (i < listCaseRow.Count + 5)
                    {
                        if (hbcell != "")
                        {
                            if (hbcell != listCaseRow[i - 5].cellOne)
                            {
                                //CellRangeAddress四个参数为：起始行，结束行，起始列，结束列
                                sheet.AddMergedRegion(new CellRangeAddress(m, i - 1, 0, 0));//合并
                                hbcell = listCaseRow[i - 5].cellOne;
                                m = i;
                            }
                        }
                        else
                        {
                            hbcell = listCaseRow[i - 5].cellOne;
                            m = i;
                        }

                        IRow rowhead = sheet.GetRow(i);

                        if(listCaseRow[i - 5].cellThree.Length > 58)
                        {
                            rowhead.HeightInPoints = 45;//行高
                        }

                        ICell cellhead = rowhead.GetCell(0);
                        cellhead.SetCellValue(listCaseRow[i - 5].cellOne.Replace("\n","").Replace("\r", ""));//写入

                        cellhead = rowhead.GetCell(1);
                        cellhead.SetCellValue(listCaseRow[i - 5].cellTwo);//写入

                        cellhead = rowhead.GetCell(2);
                        cellhead.SetCellValue(listCaseRow[i - 5].cellThree.Replace("\n", "").Replace("\r", ""));//写入

                        cellhead = rowhead.GetCell(5);
                        cellhead.SetCellValue(listCaseRow[i - 5].cellFour);//写入

                    }
                    else//合并最后一列
                    {
                        sheet.AddMergedRegion(new CellRangeAddress(m, i - 1, 0, 0));//合并
                    }
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                    {
                        workbook.Write(fs);
                    }
                }

            }
            catch (Exception ex)
            {
                RecordLog.RecordError(ex.ToString());
                return false;
            }
            return true;
        }
        
        #endregion


    }

    public class SingleMb{

        public int rowIndex { get; set; }
        public int cellIndex { get; set; }
        public string value { get; set; }
    }

    public class CaseRowInfo
    {
        public string cellOne { get; set; }
        public string cellTwo { get; set; }
        public string cellThree { get; set; }
        public string cellFour { get; set; }
    }
}