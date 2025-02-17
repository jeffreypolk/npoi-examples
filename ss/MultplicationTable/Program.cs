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

/*
 This sample shows you simple calculation via the cell formulas 
 */

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.IO;

namespace MultplicationTableInXls
{
    class Program
    {
        static IWorkbook workbook;

        static void Main(string[] args)
        {
            workbook = new HSSFWorkbook();

            //here, we must insert at least one sheet to the workbook. otherwise, Excel will say 'data lost in file'
            //So we insert three sheet just like what Excel does
            ISheet sheet1 = workbook.CreateSheet("Multiple Table");

            //create horizontal 1-9
            var row0 = sheet1.CreateRow(0);
            for (int i = 1; i <= 9; i++)
            {
                row0.CreateCell(i).SetCellValue(i);
            }
            //create vertical 1-9
            for (int i = 1; i <= 9; i++)
            {
                sheet1.CreateRow(i).CreateCell(0).SetCellValue(i);
            }
            //create the cell formula
            for (int iRow = 1; iRow <= 9; iRow++)
            {
                IRow row = sheet1.GetRow(iRow);
                for (int iCol = 1; iCol <= 9; iCol++)
                {
                    //the first cell of each row * the first cell of each column
                    string formula = GetCellPosition(iRow, 0) + "*" + GetCellPosition(0, iCol);
                    row.CreateCell(iCol).CellFormula = formula;
                }
            }

            WriteToFile();
        }

        static string GetCellPosition(int row, int col)
        {
            col = Convert.ToInt32('A') + col;
            row = row + 1;
            return ((char)col) + row.ToString();
        }

        static void WriteToFile()
        {
            //Write the stream data of workbook to the root directory
            FileStream file = new FileStream(@"test.xls", FileMode.Create);
            workbook.Write(file);
            file.Close();
        }
    }
}
