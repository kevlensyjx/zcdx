/**
 * EasyUI DataGrid根据字段动态合并单元格
 * 参数 tableID 要合并table的id
 * 参数 colList 要合并的列,用逗号分隔(例如："name,department,office");
 */
function mergeCellsByField(tableID, colList, mainColIndex) {
    var ColArray = colList.split(",");
    var tTable = $("#" + tableID);
    var TableRowCnts = tTable.datagrid("getRows").length;
    var tmpA;
    var tmpB;
    var PerTxt = "";
    var CurTxt = "";
    var alertStr = "";

    for (j = ColArray.length - 1; j >= 0; j--) {
        PerTxt = "";
        tmpA = 1;
        tmpB = 0;

        for (i = 0; i <= TableRowCnts; i++) {
            if (i == TableRowCnts) {
                CurTxt = "";
            }
            else {
                CurTxt = tTable.datagrid("getRows")[i][ColArray[j]];
            }
            if (PerTxt == CurTxt) {
                tmpA += 1;
            }
            else {
                tmpB += tmpA;

                tTable.datagrid("mergeCells", {
                    index: i - tmpA,
                    field: ColArray[j],　　//合并字段
                    rowspan: tmpA,
                    colspan: null
                });


                tmpA = 1;
            }
            PerTxt = CurTxt;
        }
    }
    tmpA = 0;
    PerTxt = "";
    CurTxt = "";
    for (var i = 0; i <= TableRowCnts ; i++) {

        if (i == TableRowCnts) {
            CurTxt = "";
        }
        else {
            CurTxt = tTable.datagrid("getRows")[i][ColArray[mainColIndex]];
        }
        if (PerTxt == CurTxt) {
            tmpA += 1;
        }
        else {
            if (tmpA != 0) {
                if (tTable.datagrid("getRows")[i - tmpA][ColArray[0]] == tTable.datagrid("getRows")[i - tmpA][ColArray[1]]) {
                    tTable.datagrid('mergeCells', {
                        index: i - tmpA,
                        field: ColArray[0],
                        rowspan: tmpA,
                        colspan: 2
                    });
                }
            }
            tmpA = 1;
        }

        PerTxt = CurTxt;
    }

}

/**
  * EasyUI DataGrid根据字段动态合并单元格
  * 参数 tableID 要合并table的id
  * 参数 colList 要合并的列,用逗号分隔(例如："name,department,office");
  */
function mergeCellsByField_Special(tableID, colList, mainColIndex) {
    var ColArray = colList.split(",");
    var tTable = $("#" + tableID);
    var TableRowCnts = tTable.datagrid("getRows").length;
    var tmpA;
    var tmpB;
    var PerTxt = "";
    var CurTxt = "";
    var alertStr = "";

    for (j = ColArray.length - 1; j >= 0; j--) {
        PerTxt = "";
        tmpA = 1;
        tmpB = 0;

        for (i = 0; i <= TableRowCnts; i++) {
            if (i == TableRowCnts) {
                CurTxt = "";
            }
            else {
                CurTxt = tTable.datagrid("getRows")[i][ColArray[j]];
            }
            if (PerTxt == CurTxt) {
                tmpA += 1;
            }
            else {
                tmpB += tmpA;

                tTable.datagrid("mergeCells", {
                    index: i - tmpA,
                    field: ColArray[j],　　//合并字段
                    rowspan: tmpA,
                    colspan: null
                });


                tmpA = 1;
            }
            PerTxt = CurTxt;
        }
    }
    tmpA = 0;
    PerTxt = "";
    CurTxt = "";
    for (var i = 0; i <= TableRowCnts ; i++) {

        if (i == TableRowCnts) {
            CurTxt = "";
        }
        else {
            CurTxt = tTable.datagrid("getRows")[i][ColArray[mainColIndex]];
        }
        if (PerTxt == CurTxt) {
            tmpA += 1;
        }
        else {
            if (tmpA != 0) {
                if (tTable.datagrid("getRows")[i - tmpA][ColArray[0]] == tTable.datagrid("getRows")[i - tmpA][ColArray[1]]) {
                    tTable.datagrid('mergeCells', {
                        index: i - tmpA,
                        field: ColArray[0],
                        rowspan: tmpA,
                        colspan: 2
                    });
                }
            }
            tmpA = 1;
        }

        PerTxt = CurTxt;
    }

   

}


/**
  * EasyUI DataGrid根据字段动态合并单元格
  * 参数 tableID 要合并table的id
  * 参数 colList 要合并的列,用逗号分隔(例如："name,department,office");
  */
function mergeCellsByField_Specia2(tableID, colList, mainColIndex) {
    var ColArray = colList.split(",");
    var tTable = $("#" + tableID);
    var TableRowCnts = tTable.datagrid("getRows").length;
    var tmpA;
    var tmpB;
    var PerTxt = "";
    var CurTxt = "";
    var alertStr = "";

    for (j = ColArray.length - 1; j >= 0; j--) {
        PerTxt = "";
        tmpA = 1;
        tmpB = 0;

        for (i = 0; i <= TableRowCnts; i++) {
            if (i == TableRowCnts) {
                CurTxt = "";
            }
            else {
                CurTxt = tTable.datagrid("getRows")[i][ColArray[j]];
            }
            if (PerTxt == CurTxt) {
                tmpA += 1;
            }
            else {
                tmpB += tmpA;

                tTable.datagrid("mergeCells", {
                    index: i - tmpA,
                    field: ColArray[j],　　//合并字段
                    rowspan: tmpA,
                    colspan: null
                });


                tmpA = 1;
            }
            PerTxt = CurTxt;
        }
    }
    tmpA = 0;
    PerTxt = "";
    CurTxt = "";
    for (var i = 0; i <= TableRowCnts ; i++) {

        if (i == TableRowCnts) {
            CurTxt = "";
        }
        else {
            CurTxt = tTable.datagrid("getRows")[i][ColArray[mainColIndex]];
        }
        if (PerTxt == CurTxt) {
            tmpA += 1;
        }
        else {
            if (tmpA != 0) {
                if (tTable.datagrid("getRows")[i - tmpA][ColArray[0]] == tTable.datagrid("getRows")[i - tmpA][ColArray[1]]) {
                    tTable.datagrid('mergeCells', {
                        index: i - tmpA,
                        field: ColArray[0],
                        rowspan: tmpA,
                        colspan: 2
                    });
                }
            }
            tmpA = 1;
        }

        PerTxt = CurTxt;
    }

   

}