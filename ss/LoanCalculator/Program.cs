﻿/* ====================================================================
   Licensed to the Apache Software Foundation (ASF) under one or more
   contributor license agreements.  See the NOTICE file distributed with
   this work for additional information regarding copyright ownership.
   The ASF licenses this file to You under the Apache License, Version 2.0
   (the "License"); you may not use this file except in compliance with
   the License.  You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
==================================================================== */

/* ================================================================
 * Author: Tony Qu 
 * Author's email: tonyqus (at) gmail.com 
 * NPOI Examples: https://github.com/nissl-lab/npoi-examples
 * Contributors:
 * 
 * ==============================================================*/

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;

namespace LoanCalculator
{
    class Program
    {
        static IWorkbook workbook;

        static void Main(string[] args)
        {
            InitializeWorkbook(args);
            Dictionary<String, ICellStyle> styles = CreateStyles(workbook);
            ISheet sheet = workbook.CreateSheet("Loan Calculator");
            sheet.IsPrintGridlines = (false);
            sheet.DisplayGridlines = (false);

            IPrintSetup printSetup = sheet.PrintSetup;
            printSetup.Landscape = (true);
            sheet.FitToPage = (true);
            sheet.HorizontallyCenter = (true);

            sheet.SetColumnWidth(0, 3 * 256);
            sheet.SetColumnWidth(1, 3 * 256);
            sheet.SetColumnWidth(2, 11 * 256);
            sheet.SetColumnWidth(3, 14 * 256);
            sheet.SetColumnWidth(4, 14 * 256);
            sheet.SetColumnWidth(5, 14 * 256);
            sheet.SetColumnWidth(6, 14 * 256);

            CreateNames(workbook);

            IRow titleRow = sheet.CreateRow(0);
            titleRow.HeightInPoints = (35);
            for (int i = 1; i <= 7; i++)
            {
                titleRow.CreateCell(i).CellStyle = styles["title"];
            }
            ICell titleCell = titleRow.GetCell(2);
            titleCell.SetCellValue("Simple Loan Calculator");
            sheet.AddMergedRegion(CellRangeAddress.ValueOf("$C$1:$H$1"));

            IRow row = sheet.CreateRow(2);
            ICell cell = row.CreateCell(4);
            cell.SetCellValue("Enter values");
            cell.CellStyle = styles["item_right"];

            row = sheet.CreateRow(3);
            cell = row.CreateCell(2);
            cell.SetCellValue("Loan amount");
            cell.CellStyle = styles["item_left"];
            cell = row.CreateCell(4);
            cell.CellStyle = styles["input_$"];
            cell.SetAsActiveCell();

            row = sheet.CreateRow(4);
            cell = row.CreateCell(2);
            cell.SetCellValue("Annual interest rate");
            cell.CellStyle = styles["item_left"];
            cell = row.CreateCell(4);
            cell.CellStyle = styles["input_%"];

            row = sheet.CreateRow(5);
            cell = row.CreateCell(2);
            cell.SetCellValue("Loan period in years");
            cell.CellStyle = styles["item_left"];
            cell = row.CreateCell(4);
            cell.CellStyle = styles["input_i"];

            row = sheet.CreateRow(6);
            cell = row.CreateCell(2);
            cell.SetCellValue("Start date of loan");
            cell.CellStyle = styles["item_left"];
            cell = row.CreateCell(4);
            cell.CellStyle = styles["input_d"];

            row = sheet.CreateRow(8);
            cell = row.CreateCell(2);
            cell.SetCellValue("Monthly payment");
            cell.CellStyle = styles["item_left"];
            cell = row.CreateCell(4);
            cell.CellFormula = ("IF(Values_Entered,Monthly_Payment,\"\")");
            cell.CellStyle = styles["formula_$"];

            row = sheet.CreateRow(9);
            cell = row.CreateCell(2);
            cell.SetCellValue("Number of payments");
            cell.CellStyle = styles["item_left"];
            cell = row.CreateCell(4);
            cell.CellFormula = ("IF(Values_Entered,Loan_Years*12,\"\")");
            cell.CellStyle = styles["formula_i"];

            row = sheet.CreateRow(10);
            cell = row.CreateCell(2);
            cell.SetCellValue("Total interest");
            cell.CellStyle = styles["item_left"];
            cell = row.CreateCell(4);
            cell.CellFormula = ("IF(Values_Entered,Total_Cost-Loan_Amount,\"\")");
            cell.CellStyle = styles["formula_$"];

            row = sheet.CreateRow(11);
            cell = row.CreateCell(2);
            cell.SetCellValue("Total cost of loan");
            cell.CellStyle = styles["item_left"];
            cell = row.CreateCell(4);
            cell.CellFormula = ("IF(Values_Entered,Monthly_Payment*Number_of_Payments,\"\")");
            cell.CellStyle = styles["formula_$"];

            WriteToFile();
        }

        /**
 * cell styles used for formatting calendar sheets
 */
        private static Dictionary<String, ICellStyle> CreateStyles(IWorkbook wb)
        {
            Dictionary<String, ICellStyle> styles = new Dictionary<String, ICellStyle>();

            ICellStyle style = null;
            IFont titleFont = wb.CreateFont();
            titleFont.FontHeightInPoints = (short)14;
            titleFont.FontName = "Trebuchet MS";
            style = wb.CreateCellStyle();
            style.SetFont(titleFont);
            style.BorderBottom = BorderStyle.Dotted;
            style.BottomBorderColor = IndexedColors.Grey40Percent.Index;
            styles.Add("title", style);

            IFont itemFont = wb.CreateFont();
            itemFont.FontHeightInPoints = (short)9;
            itemFont.FontName = "Trebuchet MS";
            style = wb.CreateCellStyle();
            style.Alignment = (HorizontalAlignment.Left);
            style.SetFont(itemFont);
            styles.Add("item_left", style);

            style = wb.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Right;
            style.SetFont(itemFont);
            styles.Add("item_right", style);

            style = wb.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Right;
            style.SetFont(itemFont);
            style.BorderRight = BorderStyle.Dotted;
            style.RightBorderColor = IndexedColors.Grey40Percent.Index;
            style.BorderBottom = BorderStyle.Dotted;
            style.BottomBorderColor = IndexedColors.Grey40Percent.Index;
            style.BorderLeft = BorderStyle.Dotted;
            style.LeftBorderColor = IndexedColors.Grey40Percent.Index;
            style.BorderTop = BorderStyle.Dotted;
            style.TopBorderColor = IndexedColors.Grey40Percent.Index;
            style.DataFormat = (wb.CreateDataFormat().GetFormat("_($* #,##0.00_);_($* (#,##0.00);_($* \"-\"??_);_(@_)"));
            styles.Add("input_$", style);

            style = wb.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Right;
            style.SetFont(itemFont);
            style.BorderRight = BorderStyle.Dotted;
            style.RightBorderColor = IndexedColors.Grey40Percent.Index;
            style.BorderBottom = BorderStyle.Dotted;
            style.BottomBorderColor = IndexedColors.Grey40Percent.Index;
            style.BorderLeft = BorderStyle.Dotted;
            style.LeftBorderColor = IndexedColors.Grey40Percent.Index;
            style.BorderTop = BorderStyle.Dotted;
            style.TopBorderColor = IndexedColors.Grey40Percent.Index;
            style.DataFormat = (wb.CreateDataFormat().GetFormat("0.000%"));
            styles.Add("input_%", style);

            style = wb.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Right;
            style.SetFont(itemFont);
            style.BorderRight = BorderStyle.Dotted;
            style.RightBorderColor = IndexedColors.Grey40Percent.Index;
            style.BorderBottom = BorderStyle.Dotted;
            style.BottomBorderColor = IndexedColors.Grey40Percent.Index;
            style.BorderLeft = BorderStyle.Dotted;
            style.LeftBorderColor = IndexedColors.Grey40Percent.Index;
            style.BorderTop = BorderStyle.Dotted;
            style.TopBorderColor = IndexedColors.Grey40Percent.Index;
            style.DataFormat = wb.CreateDataFormat().GetFormat("0");
            styles.Add("input_i", style);

            style = wb.CreateCellStyle();
            style.Alignment = (HorizontalAlignment.Center);
            style.SetFont(itemFont);
            style.DataFormat = wb.CreateDataFormat().GetFormat("m/d/yy");
            styles.Add("input_d", style);

            style = wb.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Right;
            style.SetFont(itemFont);
            style.BorderRight = BorderStyle.Dotted;
            style.RightBorderColor = IndexedColors.Grey40Percent.Index;
            style.BorderBottom = BorderStyle.Dotted;
            style.BottomBorderColor = IndexedColors.Grey40Percent.Index;
            style.BorderLeft = BorderStyle.Dotted;
            style.LeftBorderColor = IndexedColors.Grey40Percent.Index;
            style.BorderTop = BorderStyle.Dotted;
            style.TopBorderColor = IndexedColors.Grey40Percent.Index;
            style.DataFormat = wb.CreateDataFormat().GetFormat("$##,##0.00");
            style.BorderBottom = BorderStyle.Dotted;
            style.BottomBorderColor = IndexedColors.Grey40Percent.Index;
            style.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            style.FillPattern = FillPattern.SolidForeground;
            styles.Add("formula_$", style);

            style = wb.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Right;
            style.SetFont(itemFont);
            style.BorderRight = BorderStyle.Dotted;
            style.RightBorderColor = IndexedColors.Grey40Percent.Index;
            style.BorderBottom = BorderStyle.Dotted;
            style.BottomBorderColor = IndexedColors.Grey40Percent.Index;
            style.BorderLeft = BorderStyle.Dotted;
            style.LeftBorderColor = IndexedColors.Grey40Percent.Index;
            style.BorderTop = BorderStyle.Dotted;
            style.TopBorderColor = IndexedColors.Grey40Percent.Index;
            style.DataFormat = wb.CreateDataFormat().GetFormat("0");
            style.BorderBottom = BorderStyle.Dotted;
            style.BottomBorderColor = IndexedColors.Grey40Percent.Index;
            style.FillForegroundColor = IndexedColors.Grey25Percent.Index;
            style.FillPattern = (FillPattern.SolidForeground);
            styles.Add("formula_i", style);

            return styles;
        }

        //define named ranges for the inputs and formulas
        public static void CreateNames(IWorkbook wb)
        {
            IName name;

            name = wb.CreateName();
            name.NameName = ("Interest_Rate");
            name.RefersToFormula = ("'Loan Calculator'!$E$5");

            name = wb.CreateName();
            name.NameName = ("Loan_Amount");
            name.RefersToFormula = ("'Loan Calculator'!$E$4");

            name = wb.CreateName();
            name.NameName = ("Loan_Start");
            name.RefersToFormula = ("'Loan Calculator'!$E$7");

            name = wb.CreateName();
            name.NameName = ("Loan_Years");
            name.RefersToFormula = ("'Loan Calculator'!$E$6");

            name = wb.CreateName();
            name.NameName = ("Number_of_Payments");
            name.RefersToFormula = ("'Loan Calculator'!$E$10");

            name = wb.CreateName();
            name.NameName = ("Monthly_Payment");
            name.RefersToFormula = ("-PMT(Interest_Rate/12,Number_of_Payments,Loan_Amount)");

            name = wb.CreateName();
            name.NameName = ("Total_Cost");
            name.RefersToFormula = ("'Loan Calculator'!$E$12");

            name = wb.CreateName();
            name.NameName = ("Total_Interest");
            name.RefersToFormula = ("'Loan Calculator'!$E$11");

            name = wb.CreateName();
            name.NameName = ("Values_Entered");
            name.RefersToFormula = ("IF(ISBLANK(Loan_Start),0,IF(Loan_Amount*Interest_Rate*Loan_Years>0,1,0))");
        }

        static void WriteToFile()
        {
            string filename = "loan-calculator.xls";
            if (workbook is XSSFWorkbook) filename += "x";
            //Write the stream data of workbook to the root directory
            FileStream file = new FileStream(filename, FileMode.Create);
            workbook.Write(file);
            file.Close();
        }

        static void InitializeWorkbook(string[] args)
        {
            if (args.Length > 0 && args[0].Equals("-xls"))
                workbook = new HSSFWorkbook();
            else
                workbook = new XSSFWorkbook();
        }
    }
}
