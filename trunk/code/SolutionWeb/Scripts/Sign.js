//IE浏览器至少是5.5，因为内部有个JS函数需要5.5的版本
/*
 * --2015年5月19日 17:20:09
 *   1.增加印章可拖动 同一页面上可签多个章 
 *   2.增加从key中导出印章图片activex控件接口  TZ_GetSealPic
     3.增加在线签章参数设置接口 TZ_SetESMServerParam (后已删除)
     4.增加时间戳    20151125
     5.支持用户指定显示签章的大小 TZ_SetShowSize(nWidth,nHeight),在TZ_AddSealInPosEx方法中调用   20151207
     6.签章完毕后自动保存 避免再次弹出pin码问题 验证结果发生变更时需要保存数据 会再次连接key可能会弹出pin码   
     7.添加 TZ_ForceDelete(sealName,DeleteAble)接口，可强制删除已经保存的章子 ，忽略TZSevedSignDeleteAble变量的作用   20151217
     8.添加海颐KEY的驱动 软证书 -1，中孚 0，海泰 2，渔翁 6或704，海颐 21， 其他。。。。    20160123
     9.修改配置参数TZSevedSignDeleteAble =1 保存后盖章本人可以撤销印章 以前是不允许删除的 20160411
     10.除原先透明gif外，支持白色背景gif图片格式解码显示 20160415
     11.添加对https协议支持  20160505
     12.添加TZ_SetHandWritingSignHeight(ULONG height)指定手写签批的高度  20161010
     
 * --2014年5月26日 14:53:15
 * 	 1.添加插入章函数,用来定位章的具体位置。
 * 	 2.由于ocx控件修改，从新修改批量签章错误提示。
 *   3.修正回显章错误。
 * 普通版本（没有定制）-对应普通ocx控件 V 1.0.3.0 
 * --2013年12月13日 09:45:41
 * 	 1.添加关于对话框
 *   2.添加一级签批和二级                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
 * 	 3.更正签章完成并提交数据禁止删除章。
*/
TZSevedSignDeleteAble = 0;   // 已保存的印章可否删除 0不可 1可删除
TZVen = 918; //厂商ID，可以咨询技术支持人员
TZOnline = 0;   // 是否在线签章 1在线 0离线
TZszServer = "http://59.49.39.73:9200/SealServer1/"; //服务器地址  备注：若使用公网对内网的映射 此处改写为映射的URL地址 ZHANGJI
TZESMServer = "http://59.49.39.73:9200/BZESM/services/r/WebSign/";           // 在线签章ESM服务器地址
TZLog = 0;          // 若在线签章，是否写日志
TZLockSeal = 1;     // 可拖动  ZHANGJI 20150520      0 可拖动  1固定不能动
/*  支持时间戳 
    在给定控件初始化参数后，调用调用TZ_InitSeal2方法
*/
//TZTimeStemServerURL = "http://zeitstempel.dfn.de";     // 时间戳服务器 若不需要时间戳 则TZTimeStemServerURL = "";
TZTimeStemServerURL = "";     // 时间戳服务器 若不需要时间戳 则TZTimeStemServerURL = "";
TZTimeStempUser ="";   // 用户名
TZTimeStempPwd  ="";    //密码
TZUseKeyOnPrint = 1;          // 打印前是否验证签章 1:验证  0:不验证  // 设置TZUseKeyOnPrint = 0可直接打印忽略签章有效性    20161207
/*
TZSaveWork = "http://192.168.96.1:8080/Demo/" //保存数据地址
*/

//   TZbSaveEnc = false; //保存时是否加密
TZszServerPub = "30818902818100A968E0D1D66ED175546202F07D16DA182749D02231128A0026FEFFB1C5B4F8FAE2A3735916BEEA611559C5DB8CCA9494583EA99F418B4DD2D0D0ED4037518735844205043A6A887F72A80F2358CC77D41A484B927F9E492BBEEA89B81A491847DA3BA37EBD17332DF3325E32B6D7177361BB690EE020E2660D03176715C59A610203010001";
TZTotalSealName = "";//内部使用,不要修改
TZShowSealName = "";//内部使用，不要修改
TZTotalPrintPic = ""; //内部使用，不要修改
TZTotalPrintDiv = ""; //内部使用，不要修改
TZPrintPath = "c:\\tzTmpPrint"; //打印时的临时路径，根据实际情况进行修改
TZbGetEnc = false; //加载时是否加密，由于获取代码修改，此选项取消，所有加载均不做加密处理

//添加签名验证
TZVerifyOnSign = false; //验证时是否需要验证签名证书
TZVerifyAddr = "http://192.168.2.166:8080/EsmManyPKeyIE6/GetCertValidate.action";//验证地址
TZRemoteSealPath = "";
TZLastSealID = "";	//页面中最后一个章ID，用于查看证书、撤销签批等功能演示。
//辅助类，用于生成UUID，
TZOneSignBatch="";			//一级签批	保留
TZTwoSignBatch="";			//二级签批	保留
TZAllSignSeal ="";			//所有签章	保留

TZSaveSignSealOther="";			//存放所有签章。
TZBatShowSeals = "";			//批处理调用回显的章。

TZTotalSealName="";					//页面没有上传的章的名字
TZSpecialSealID="";				//特殊章ID


function UUID() {
	this.id = this.createUUID();
}

// When asked what this Object is, lie and return it's value  
UUID.prototype.valueOf = function() {
	return this.id;
}
UUID.prototype.toString = function() {
	return this.id;
}

//  
// INSTANCE SPECIFIC METHODS  
//  

UUID.prototype.createUUID = function() {
	//   
	var dg = new Date(1582, 10, 15, 0, 0, 0, 0);
	var dc = new Date();
	var t = dc.getTime() - dg.getTime();
	var tl = UUID.getIntegerBits(t, 0, 31);
	var tm = UUID.getIntegerBits(t, 32, 47);
	var thv = UUID.getIntegerBits(t, 48, 59) + '1'; // version 1, security version is 2  
	var csar = UUID.getIntegerBits(UUID.rand(4095), 0, 7);
	var csl = UUID.getIntegerBits(UUID.rand(4095), 0, 7);

	// since detection of anything about the machine/browser is far to buggy,   
	// include some more random numbers here  
	// if NIC or an IP can be obtained reliably, that should be put in  
	// here instead.  
	var n = UUID.getIntegerBits(UUID.rand(8191), 0, 7)
			+ UUID.getIntegerBits(UUID.rand(8191), 8, 15)
			+ UUID.getIntegerBits(UUID.rand(8191), 0, 7)
			+ UUID.getIntegerBits(UUID.rand(8191), 8, 15)
			+ UUID.getIntegerBits(UUID.rand(8191), 0, 15); // this last number is two octets long  
	return tl + tm + thv + csar + csl + n;
}
 
// aren't enough bits to shift where it needs to.  
UUID.getIntegerBits = function(val, start, end) {
	var base16 = UUID.returnBase(val, 16);
	var quadArray = new Array();
	var quadString = '';
	var i = 0;
	for (i = 0; i < base16.length; i++) {
		quadArray.push(base16.substring(i, i + 1));
	}
	for (i = Math.floor(start / 4); i <= Math.floor(end / 4); i++) {
		if (!quadArray[i] || quadArray[i] == '')
			quadString += '0';
		else
			quadString += quadArray[i];
	}
	return quadString;
}

// Replaced from the original function to leverage the built in methods in  
// JavaScript. Thanks to Robert Kieffer for pointing this one out  
UUID.returnBase = function(number, base) {
	return (number).toString(base).toUpperCase();
}

// pick a random number within a range of numbers  
// int b rand(int a); where 0 <= b <= a  
UUID.rand = function(max) {
	return Math.floor(Math.random() * (max + 1));
}



/*
功能 : 
    在线签章或者key类型等参数设置
*/
function TZ_SetESMServerParam()
{
    if(SignControl== null)
    {
        alert("请等待控件包下载安装结束再设置"); 
        return;
    }
    
    SignControl.TZ_SetServerParam();
    
    return ;
}

/* 强制删除签章 
   sealName: 印章名称
   DeleteAble: 跟TZSevedSignDeleteAble表示含义一致 1可删除 0不删除
*/
function TZ_ForceDelete(sealName,DeleteAble)
{
   var Sealobj = document.getElementById(sealName);
   if(Sealobj == null) 
   {
        alert("印章不存在!");
         return;
   }
   var deleteAble = 1;     // 跟TZSevedSignDeleteAble表示含义一致  1可删除 0不删除
   Sealobj.TZ_SetDeleteAble(deleteAble); 
   // 然后删除印章就能删掉了   ZHANGJI  20160106
   Sealobj.TZ_DeleteSeal();
   location.reload();
   return ;
}

/*
	函数
	  	TZ_SignFromServerEx
	参数
			sealName
					章的名字
			sealID
					章的ID
	作用
			用户点击按钮，直接生成章。
*/
function TZ_SignFromServerEx(sealName,sealID)
{	
	 
	var obj = null;
	var divObj = null;
	try
	{
		obj = document.getElementById(sealName);
		if(null == obj)
		{
			alert("无法获取新插入对象");
			return false;
		}
		var ControlID = sealName + "Div";
		divObj = document.getElementById(ControlID);
		if(null == divObj)
		{
			alert("无法获取层对象");
			return false;
		}

		var f = obj.TZ_SignFromServer(sealID);
	
		if(f == 1)
		{  
			
			  obj.TZ_Destory();
			  var ParentEID = obj.TZ_GetElementID();
			  var ObjParent = document.getElementById(ParentEID);
  			  ObjParent.removeChild(divObj);
           
              alert("未成功签批或签章");
              return false;
  
		}
		
		// 以签章图片的大小设定章子大小 另控件重绘   
		obj.style.width= obj.TZ_GetImgWidth()+"px";
		obj.style.height= obj.TZ_GetImgHeight()+"px";
	
		return true;
	}
	catch(e)
	{
		var ParentEID = obj.TZ_GetElementID();
		obj.TZ_Destory();
		var ObjParent = document.getElementById(ParentEID);
		ObjParent.removeChild(divObj);
  			  
		alert(e);
		return false;
	}
	
}


/*
函数		AddSealInPosEx
参数
    workid
        业务对象id号  此id号对应mysql-seal-sealinfo表中workid(非关键字)字段，每一行标示1个签章。  若签章页面存在多个签章 则workid是相等的 
		szAreaIDs 
			  区域ＩＤ　元素ＩＤ　
		isSeal	
				1 签章 0 签批
		left
				章的x位置
		top
				章的y位置
				
		SealParentEID
				章的父元素ID
				
*/
function AddSealInPosEx(workid,szAreaIDs,isSeal,l,t,SealParentEID)
{
    
    //alert("测试"); alert("测试");
	   if((null == szAreaIDs) || (szAreaIDs == ""))
    {
        alert("请设置签名域的ID");
        return false;
    }
    
    var sealName = new UUID().id;
    if(sealName == "")
    {
        alert("生成sealID错误");
        return false;
    }

    //插入样章，并设置其位置 且保存业务表单id(签章立即保存必须的参数)  ZHANGJI    20160105
    TZ_AddSealInPosEx(workid,sealName,isSeal,l,t,SealParentEID);   

    //设置签名IDs  签名区域
    TZ_SetSealSignIDs(sealName, szAreaIDs);
    //服务器直接签章并立即保存    20151215  ZHANGJI
 	  var retFlag = TZ_SignFromServerEx(sealName,sealName);
 	  
 	  if(retFlag != true)    // 盖章失败
 	  {
 	  	return false;
 	  }
    if(TZTotalSealName == "")
    {
        TZTotalSealName = sealName;
    }
    else
    {
        TZTotalSealName = TZTotalSealName + "," + sealName;
    }
    
    //存放 没有保存章数据
    if(TZSaveSignSealOther == "")
    {
    	TZSaveSignSealOther = sealName;
    }
    else
    {
    	TZSaveSignSealOther = TZSaveSignSealOther + "," + sealName;
    }
    
    
    if(retFlag)   TZLastSealID = sealName;//存入最后一个章。
   
   
    TZSpecialSealID += sealName+",";   // 记录所有章ID 保存章时候使用  ZHANGJI

    return  sealName;    // 返回印章id
     
}

/*
函数		TZ_AddSealInPosEx
参数
                workid
                    业务对象id号  此id号对应mysql-seal-sealinfo表中workid(非关键字)字段，每一行标示1个签章。  若签章页面存在多个签章 则workid是相等的 
				SealName
					章的名称，全局变量ＵＵＩＤ值
				isSeal
				   1 签章 0 签批
				left
						章左侧坐标
				top
						章的右侧坐标
				SealParentEID
					  章的父元素ID	
			   
					  
		
*/
function TZ_AddSealInPosEx(workid,SealName,isSeal,l,t,SealParentEID)
{
  //alert("测试");  alert("测试");
	try
  {
	var SealOcx = document.getElementById(SealName);
	
	if(SealOcx != null)
	{
		alert("章已添加");
		return;
	}
	
	var ControlID = SealName + "Div";
	var divElement = document.createElement("div");  
	if("string" == typeof(ControlID))
	 {
		divElement.id=ControlID;  
	 }


   // divElement.style.position="absolute";//absolute   // 创建和回显的时候属性保持一致  ZHANGJI  20150818
	divElement.style.position="relative";//relative

	divElement.style.left = "0px";
	divElement.style.top =  "-150px";

	
	divElement.innerHTML="<OBJECT id="+SealName+"  classid='clsid:BD33C300-D056-4858-B2E5-F5E29FAE8B89' ></OBJECT>";    // ZHANGJI
	var ObjParent = document.getElementById(SealParentEID);
	
	if(ObjParent == null) 
	{
	    alert("没有找到ObjParent父元素, 无法加载印章");
	    return ;
	}
	
    ObjParent.appendChild(divElement);            // ZHANGJI


	SealOcx=document.getElementById(SealName);
	if(SealOcx == null)
	{
		alert("添加章失败");
		return;
	}
	
	SealOcx.TZ_SetStrYZName(SealName);   
	//添加设置Key厂商ID和设置是否为签章还是签批
	if(TZOnline == 1)
	SealOcx.TZ_InitSeal(TZVen,TZOnline,TZszServer,TZESMServer,TZszServerPub,isSeal,TZLog,TZSevedSignDeleteAble);  
	else  SealOcx.TZ_InitSeal(TZVen,TZOnline,TZszServer,"",TZszServerPub,isSeal,0,TZSevedSignDeleteAble);  
  
  // SealOcx.TZ_SetShowSize(100,100);     // 这里可指定显示章子的宽高  ZHANGJI 20151204
  SealOcx.TZ_SetHandWritingSignHeight(80);
  // 给定时间戳服务器参数  
  SealOcx.TZ_InitSeal2(TZTimeStemServerURL,TZTimeStempUser,TZTimeStempPwd);  
	/*
	ZHANGJI 把签章回显时要加载的元素ID保存。 
	*/
	SealOcx.TZ_SetElementID(SealParentEID);
	
	//   需要设置workid 作为保存印章数据表的行id  ZHANGJI  20160105
	SealOcx.TZ_SetWorkID(workid);    
	
	SealOcx.TZ_LockSeal(TZLockSeal);    // 可拖动  ZHANGJI 20150520      0 可拖动  1固定不能动
	
	
	 // 首次签章通知控件保存在父元素中的位置
  SealOcx.TZ_SetPosInWeb(l,t);  // 保存签章在父div层次中的位置		
 
	//添加结束
	//设置是否需要签名验证
	if(TZVerifyOnSign)
	{
	    if(TZVerifyAddr == "")
	    {
	    	TZ_SealDel(SealID);
	        alert("请设置验证签名证书地址");
	        return;
	    }
	    else
	    {
	     SealOcx.TZ_SetVerify(TZVerifyAddr);
	    }
	}
   
	//添加结束
	SealOcx.style.position="absolute";
	SealOcx.style.left = l +"px";
	SealOcx.style.top= t +"px";
	

  }
  catch(e)
  {
    alert(e);
  }
}

//设置签名的表单ID，id以逗号分隔,SealID为添加签章的名字，szSignIDs为id字符串
function TZ_SetSealSignIDs(SealID, szSignIDs) {
	try {
		var SealOcx = document.getElementById(SealID);
		if (SealOcx == null) {
			alert("章未添加");
			return;
		}

		SealOcx.TZ_SetSignIDs(szSignIDs);

	} catch (e) {
		alert(e);
	}
}


/*
			参数
					SealID		章的唯一标识
			作用
					删除创建的Div元素。
			备注：
				  此函数在右键菜单中删除签章时会被调用
*/
function TZ_SealDel(SealID) 
{
	//删除印章
	 
	/**/
	try
	 {

		var szSealID;
		if ("string" == typeof (SealID)) {
			szSealID = SealID;
		} else {
			szSealID = SealID.id;
		}

		var pp = document.getElementById(szSealID);
		if (pp == null)
		 {
			alert("删除时获取签章对象错误");
			return;
		}  
		var ParentID = pp.TZ_GetElementID();
			pp.TZ_Destory();
		 
		
		
		//删除Div元素
		var SealOcx = document.getElementById(szSealID + "Div");
		if (null == SealOcx) 
		{
			alert("object empty");
			return;
		}
		var ParentObj = document.getElementById(ParentID);
		if(ParentObj == null)
		{
			document.body.removeChild(SealOcx);
			return;
		}
		ParentObj.removeChild(SealOcx);
		
		
		//在TZTotalSealName中删除对应名字即可
		var tzNames = TZTotalSealName.split(",");
		
		for (i = 0; i < tzNames.length; i++) {
			if (szSealID == tzNames[i]) {
				tzNames.splice(i, 1);
				break;
			}
		}
		TZTotalSealName = tzNames.join(",");

		//删除回显中的印章
		tzNames = TZShowSealName.split(",");
		for (i = 0; i < tzNames.length; i++) {
			if (szSealID == tzNames[i]) {
				tzNames.splice(i, 1);
				break;
			}
		}
		TZShowSealName = tzNames.join(",");
	} catch (e) {

		alert(e);
	}
	
}


/*
    获取签名数据，SealID用于标示签章对象,此函数在右键菜单中点击签名以及认证时均会被调用
    该接口不可删除 因为OCX控件会自动使用
 */
function TZ_GetSignDataOne(SealID, idName) {

	var szSealID
	if ("string" == typeof (SealID)) //参数判断
	{
		szSealID = SealID;
	} else {
		szSealID = SealID.id;
	}

	if ((null == idName) || ("" == idName)) {
		SealOcx.TZ_SetSignData("");
		return;
	}
	try {
		//根据参数获取对象
		var SealOcx = document.getElementById(szSealID);
		if (SealOcx == null) {
			alert("没有章0");
			return;
		}

		var SignData = document.getElementById(idName).value;

		SealOcx.TZ_SetSignDataOne(SignData);
	} catch (e) {
		alert(e);
	}
}


 
 /* 
 ZHANGJI 该接口不要删除  回调接口用于拖动控件和回显时刻的签章位置定位
    在实际应用中,Web页面千变万化 控件无法提前获知准确的结构 因此将可能需要的几个参数使用回调接口
    的方式提供给用户, 签章定位的代码由用户的*.js代码完成 
    
   

     SealID:  章ID
     ParentID: 父DIV或Table层次等ID
     // 以下nLeft和nTop参数表示控件移动的目标位置,相对于IE浏览器的客户区(document.body)的坐标
     nLeftYS: 签章初始位置x坐标(相对于父元素)
     nTopYS: 签章初始位置y坐标(相对于父元素)
     nOffSetX: 章移动位置后的累计x方向偏移量
     nOffSetY:  章移动位置后的累计y方向偏移量
     nSealWitdh: 章的宽度
     
    当签章控件鼠标拖动位置时,被控件自动调用
    若签章锁定不可拖动时，则该接口不被调用

 */

function TZ_SealMoveExSink(SealID,ParentID,nLeftYS,nTopYS,nOffSetX,nOffSetY,nSealWitdh)
{

    var SealOcx = document.getElementById(SealID);
    var ParentDiv = document.getElementById(ParentID);
    if(SealOcx == null || ParentDiv == null)
    {
        alert("章或父DIV不存在!");
        return;
    }
    var ParentLeft = ParentDiv.getBoundingClientRect().left;    // 获得SealPos的DIV相对于body的x坐标
    // 由于控件把签章初始位置和移动后的累计偏移量回传了 通过简单计算就可定位签章
    SealOcx.style.top= nTopYS + nOffSetY +"px";                    
    SealOcx.style.left = nLeftYS + nOffSetX +"px";    // 章相对于SealPos的x坐标= 相对于body的x坐标 - SealPos相对于body的x坐标
     
   // 若移动后 则自动保存签章
   var sealName = SealOcx.TZ_GetStrYZName();
   var WorkId = SealOcx.TZ_GetWorkId();       // 这里返回的是签章时刻传入的workid  (TZ_SignFromServerEx(workid,sealName,sealName))
   TZ_SaveSeal(sealName,WorkId);
    return; 
     
     
}



// 关于对话框
function About()
{
     var SealID = "TZ_tmpSeal";
	    var SealOcx = document.getElementById(SealID);
	    if(SealOcx != null)
	    {
		    TZ_SealDel(SealID);
		    //return;
	    }
	    var ControlID = SealID + "Div";
	    var divElement = document.createElement("div");  
	    if("string" == typeof(ControlID))
	     {
		    divElement.id=ControlID;  
	     }
        
	    divElement.style.position="absolute";
	    divElement.style.left = 0 +"px";
	    divElement.style.top = 0 +"px";
        //divElement.style.display = "none";
	    divElement.innerHTML="<OBJECT id="+SealID+"    classid='clsid:BD33C300-D056-4858-B2E5-F5E29FAE8B89'></OBJECT>";
        document.body.appendChild(divElement);
         
	    SealOcx=document.getElementById(SealID);
	        if(SealOcx == null)
	        {
		        alert("添加章失败");
		        return;
	        }
	    SealOcx.AboutBox();
}


//函数说明：分个读取签章。回显时刻从数据库中获取workid 若获取不到重新设置
//用于显示多个签章
/*
	fun Name
					TZ_ShowSeal
	功能:  数据库中读取签名的章，回显章。
	参数:
		   workid	表单主关键字，一条记录的主关键字。
		   ParentID[可选] 为兼容早期版本(较早版本可能未保存ParentID),可指定印章显示的父元素DIV层, 以便准确定位
	
				  
*/
function TZ_ShowSeal(workid,ParentID)
{

  try
  {
  //先创建临时控件
    var SealID = "TZ_tmpSeal";
	var SealOcx = document.getElementById(SealID);
	if(SealOcx != null)
	{
		TZ_SealDel(SealID);
	}
	
	var ControlID = SealID + "Div";
	var divElement = document.createElement("div");  
	if("string" == typeof(ControlID))
	 {
		divElement.id=ControlID;  
	 }
	divElement.innerHTML="<OBJECT id="+SealID+"    classid='clsid:BD33C300-D056-4858-B2E5-F5E29FAE8B89'></OBJECT>";
    document.body.appendChild(divElement);
     
        
	SealOcx=document.getElementById(SealID);
	if(SealOcx == null)
	{
		alert("添加章失败");
		return;
	}
	
	SealOcx.TZ_SetStrYZName(SealID);
	
	SealOcx.TZ_SetServer(TZszServer,TZszServerPub);
	
	var pTZTotalSealName = SealOcx.TZ_ReadTableIDs(workid);   // 从服务器读取TABLE表单ID
	if(pTZTotalSealName == "")
	{
	    alert("获取tableID错误");
            SealOcx.TZ_Destory();
            document.body.removeChild(divElement);
	    return;
	}
	else if(pTZTotalSealName == "0")   //获取表单ID为0表示尚未盖过章的新建表单
	{
	   //alert("获得表单ID=0，表示该表单尚未签章");
	   // TZTotalSealName = "";
	}
	else
	{
	    
	    // 格式 99(workeID表示表单页面在数据库中表示该表单)   341,342;(2个表单ID说明签过2次章,每个章占用数据库的一行)
	    groups = pTZTotalSealName.split(',');
	    
	    // 分别查询所有签章
	    for(i = 0; i < groups.length;i++)
	    {
	        elements = groups[i];
	        var sealinfo = SealOcx.TZ_GetSingleSeal(elements);    // 从服务器数据库获得签章数据 xml格式 比如查询表单ID=341   ZHANGJI
	        infos = sealinfo.split('#');
	        if(infos.length != 2)
	        {
	            alert("获取签章失败");
	            continue;
	        }
	        sealid = infos[0];
	        sealxml = infos[1];
	        	var SealObj = document.getElementById(sealid);
					if (SealObj != null) {
						alert("章已添加");
					return;
						}
						var ControlID = sealid + "Div";
		var DivObj = document.createElement("div");
		if ("string" == typeof (ControlID)) {
			DivObj.id = ControlID;
		    }
		    
		    
               		    
//		DivObj.style.position = "relative";
		DivObj.style.position = "absolute";   // 与创建时候的属性保持一致  ZHANGJI
		
		DivObj.style.left = 0 +"px";
		DivObj.style.top = 0 +"px";
		DivObj.innerHTML = "<OBJECT id="
				+ sealid
				+ "  classid='clsid:BD33C300-D056-4858-B2E5-F5E29FAE8B89' ></OBJECT>"; //width=157 height=157
 		document.body.appendChild(DivObj); 
		SealObj = document.getElementById(sealid);
		if (SealObj == null) {
			alert("添加章失败");
			return;
		    }
		     
		SealObj.TZ_SetStrYZName(sealid);
		
		
		 if(SealObj.TZ_ImPortData(sealxml) ==false)
            {
                alert("签章数据导入失败，签章ID" + sealid);
                continue;
           }
        var workiddd = SealObj.TZ_GetWorkId();
        if(workiddd == "") 
           SealObj.TZ_SetWorkID(workid) ;     // 若没保存业务表单id则重新设置 目的兼容以前的印章。 ZHANGJI  20160105 
           
           
		SealObj.TZ_SetServer(TZszServer,TZszServerPub);  // 设置服务器地址 是必须的
		
		/* 
		ZHANGJI 20150920 
		在TZ_ImPortData方法调用完毕后, 控件解析XML文件并获取TZESMServer、TZOnline、TZLog、TZSevedSignDeleteAble 等参数
		若重新调用 TZ_InitSeal方法则覆盖以上参数的值
		SealOcx.TZ_InitSeal(TZVen,TZOnline,TZszServer,TZESMServer,TZszServerPub,1,TZLog,TZSevedSignDeleteAble);  
		*/
		
		
		var EleParentID = SealObj.TZ_GetElementID();
		document.body.removeChild(DivObj);
		
		//重新添加元素
		var ParentEle = document.getElementById(EleParentID);   // 此处为签章处面板
		if(ParentEle == null)
		{
		   ParentEle = document.getElementById(ParentID);  // 若未保存父元素ID则使用方法参数ParentID
		}
		if(ParentEle != null)
		ParentEle.appendChild(DivObj);    //ZHANGJI  加载到指定签章处面板上
		else document.body.appendChild(DivObj);

	    
		SealObj.style.position = "absolute";
		SealObj.style.left = SealObj.TZ_GetPosX() +"px";
		SealObj.style.top = SealObj.TZ_GetPosY() +"px";
		
		
		var showsizes = SealObj.TZ_GetShowSize();
    var showsize = showsizes.split(",");
     
 		SealObj.style.width=showsize[0]+"px";
 		SealObj.style.height=showsize[1]+"px";
   
		//验证数字签名
			if (TZVerifyOnSign) {
			if (TZVerifyAddr == "") {
				TZ_SealDel(sealid);
				alert("请设置验证签名证书地址");
				return;
			} else {
				SealObj.TZ_SetVerify(TZVerifyAddr);
			}                                      
		}
	 
			
			//回显中的章
  	    TZLastSealID = sealid;		//回显
  	    TZSpecialSealID += sealid+",";   // 记录所有章ID 保存章时候使用  ZHANGJI
  		if(TZTotalSealName == "")
            {
            	TZTotalSealName = sealid;
            }
            else
            {
            	TZTotalSealName = TZTotalSealName + "," + sealid;
            }
			
			
            if(TZShowSealName == "")
            {
            	TZShowSealName = sealid;
            }
            else
            {
            	TZShowSealName = TZShowSealName + "," + sealid;
            }
      

	    }
       
	}
	// 临时控件删除
       SealOcx.TZ_Destory(); 
	   document.body.removeChild(divElement);
	return;
	
  }
  catch(e)
  {
    SealOcx.TZ_Destory();
    document.body.removeChild(divElement);
    alert(e);
  }
  
}



//函数说明：提交签章。传入签章名称以及业务ID
// 保存绝对位置 ZHANGJI 20150426
/*
sealName: 章ID  workid:表单ID

返回值:
 -1 签章删除了
 0  保存成功
 1  其他原因保存失败
 2  非本人无法保存 失败
 
*/
function TZ_SaveSeal(sealName,workid)
{

    obj = document.getElementById(sealName);
    if(null == obj)
    {
       // 签章删除了 
        return -1;
    }
//    var left = obj.getBoundingClientRect().left;      
//    var top= obj.getBoundingClientRect().top;
   
    var left = parseInt(obj.style.left);      
    var top  = parseInt(obj.style.top);
    obj.TZ_SetPosInWeb(left,top);  // 保存签章在父div层次中的位置

   
    obj.TZ_SetServer(TZszServer,TZszServerPub);
    
    var rtn = obj.TZ_SaveSeal(workid,false);
    if(rtn == 2) 
    {
        // 非签章者本人 无法保存
        return rtn;
    }
    if(rtn == 1) // 保存失败
    {
        return rtn;
    }
    
    
    return rtn ;
}







var ParentDivIndex = 1;

//开始打印时需要调用
function  TZ_prePrint()
{
   try
   {
     var PrintMode= 0;    // 打印模式  默认gif图片在正文下方打印
	 var tzNames = TZTotalSealName.split(",");
	 //先重新验证一遍 
	 for(i = 0; i < tzNames.length;i++)
	   {
    	 szSealID = tzNames[i];
         var obj = document.getElementById(szSealID);
         if(null == obj)
         {
             continue;	 
         }

         // 验证有效性
         if(TZUseKeyOnPrint == 1)
         {
	         var Rtn = obj.TZ_Verify();
	         if(Rtn  != 0)  
	         {
	            alert("存在验证不成功的签章, 禁止打印!");
	            return false;
	         }
         }
     }    

     for(i = 0; i < tzNames.length;i++)
     {
    	 szSealID = tzNames[i];
    
         var obj = document.getElementById(szSealID);
         if(null == obj)
         {
        
             continue;	 
         }  
         var oleft = obj.offsetLeft;
         var otop = obj.offsetTop;

//       var oleft = obj.getBoundingClientRect().left;  相对于document的绝对位置 打印位置会发生改变
//       var otop = obj.getBoundingClientRect().top;
          
         var savePic = obj.TZ_ExportEx(TZszServer);
         var printMode2 = obj.TZ_GetPrintMode();    // 获得打印方式
       
            
         if(savePic=="NOKEY")
         {
        	 return false; //没有key直接返回
         }
         if(savePic == "")
         {
        	 continue;
         }
         var showsizes = obj.TZ_GetShowSize();
         var showsize = showsizes.split(",");
         
         var HomeID = "HomeDiv";
         var HomeDiv = document.getElementById(HomeID);
         if(HomeDiv == null)
         {
            alert("找不到HomeDiv区域!");
            return ;
         }
         
         var ParentID = obj.TZ_GetElementID();
         var ParentEle = document.getElementById(ParentID);    // 获得签章控件定位div
         if(ParentEle == null)
         {
            alert("签章控件定位区域不存在!");
            return ;
         }
         ParentDivIndex = ParentEle.style.zIndex;
         
         
         var PrintID = "PrintDIV" ;
         var PrintDiv = document.getElementById(PrintID);
         if(PrintDiv == null)     // 创建GIF图片加载的图床DIV, 大小属性与ParentEle一致, 使得GIF图片位置与控件相同
         {
       
             PrintDiv = document.createElement("div");         // 创建GIF图片加载的图床DIV, 大小属性与ParentEle一致, 使得GIF图片位置与控件相同
             
             PrintDiv.id = PrintID;  //  "PrintDIV"
             PrintDiv.style.left =ParentEle.style.left;
             PrintDiv.style.top =ParentEle.style.top;
             PrintDiv.style.position="relative";
             PrintDiv.style.width= ParentEle.style.width;
             PrintDiv.style.align= "center";
             
             HomeDiv.appendChild(PrintDiv);  
             
         }
           
 
         obj.style.display = "none";    // 控件隐藏
         var ControlID = szSealID + "PrintDiv";
	     var divElement = document.createElement("div");  
	     if("string" == typeof(ControlID))
	     {
		     divElement.id=ControlID;  
	     }
         if(TZTotalPrintDiv == "")
         {
        	  TZTotalPrintDiv = ControlID;
         }
         else
         {
        	  TZTotalPrintDiv = TZTotalPrintDiv + "," + ControlID;
         } 
         var ImgContrl = szSealID+"Img";
        

        
//       divElement.style.position="relative";
         divElement.style.position="absolute";     // 创建和回显的时候属性保持一致  ZHANGJI  20150818
	     divElement.style.left =0 +"px"; 
	     divElement.style.top = 0+"px"; 
	 

         divElement.innerHTML="<img src='" + savePic +"' id ="+ImgContrl+ " width=" +showsize[0] +" height=" +showsize[1]+ "/>";  //  GIF图片显示
         PrintDiv.appendChild(divElement);
         
         var Parentheight = parseInt(ParentEle.getBoundingClientRect().bottom) - parseInt(ParentEle.getBoundingClientRect().top);
         var ImgE = document.getElementById(ImgContrl);
         ImgE.style.position="absolute";
	     ImgE.style.left = oleft+"px";
	     ImgE.style.top = parseInt(otop) -Parentheight +"px";
//         ImgE.style.top = parseInt(otop) ;

	     if(PrintMode == 1) 
	     {
	      ParentEle.style.zIndex =  0;     // 将文档正文次序放到上层
	      PrintDiv.style.zIndex = 1;   // gif图片要在文本之下 不能遮住文字
	      continue;  // 在上方打印就不修改状态了
	     }
	  
	     
	     if(printMode2 == 1)      // 打印图片在文档上方避免印章被遮盖
	     {
	      ParentEle.style.zIndex =  0;     // 将文档正文次序放到上层
	      PrintDiv.style.zIndex = 1;   // gif图片要在文本之下 不能遮住文字
	     
	      PrintMode = 1;  // gif图片在文本上方打印  修改PrintMode
	     }
	     else //  printMode2 ==0
	     {
	      PrintDiv.style.zIndex = 0;   // gif图片要在文本之下 不能遮住文字
	      ParentEle.style.zIndex =  1;     // 将文档正文次序放到上层
	     }
	  
       
      }
     
     
     tzNames = TZTotalSealName.split(",");
     for(i = 0; i < tzNames.length;i++)
     {
    	 szSealID = tzNames[i];
    
         var obj = document.getElementById(szSealID);
         if(null == obj)
         {
             continue;	 
         }
         obj.style.display = "none";
      }
      
   
    }  
	catch(e)
	{
		alert(e);
		return true;
	}
	
}

//调用打印结束后调用
function TZ_finPrint()
{

	try
	{
	
		// 删除GIF图片
		var picPrintNames = TZTotalPrintDiv.split(",");
		for(i = 0; i < picPrintNames.length; i++)
		{
			
			var bbb = document.getElementById(picPrintNames[i]);
		    if(bbb)
		    {
		        var PrintID = "PrintDIV";
                var PrintDiv = document.getElementById(PrintID);     // 打印时创建的GIF图床 ZHANGJI
                if(PrintDiv)
			    PrintDiv.removeChild(bbb);
			}
		}
		TZTotalPrintDiv = "";
		//  删除临时生成的PrintDiv 
		 var HomeID = "HomeDiv";
         var HomeDiv = document.getElementById(HomeID);
         if(HomeDiv == null)
         {
            alert("找不到HomeDiv区域!");
            return ;
         }
		var PrintID = "PrintDIV";
        var PrintDiv = document.getElementById(PrintID);     // 打印时创建的GIF图床 ZHANGJI
        if(PrintDiv)  HomeDiv.removeChild(PrintDiv);
        
	    // 恢复显示控件        
         tzNames = TZTotalSealName.split(",");
         for(i = 0; i < tzNames.length;i++)
         {
    	     szSealID = tzNames[i];
        
             var obj = document.getElementById(szSealID);
             if(null == obj)
             {
                 continue;	 
             }
             obj.style.display = "block";
             var ParentID = obj.TZ_GetElementID();
             var ParentEle = document.getElementById(ParentID);    // 获得签章控件定位div
             if(ParentEle == null)
             {
                alert("签章控件定位区域不存在!");
                return ;
             }
             ParentEle.style.zIndex =  ParentDivIndex;     // 将文档正文次序恢复到上层显示
          }
            return;
	    }
    catch(e)
    {
	    alert(e);
	    return;
    }
} 





  
/*
 *  查看单个证书信息
 *  SealID
 * 			传入章的ID
*/
function TZ_ViewCert(SealID)
{
 
 var obj = document.getElementById(TZLastSealID);
	if(obj ==null)
	{
		alert("文档中没有签章");
		return;
		}
		

	var obj = document.getElementById(SealID);
	if(obj!=null)
	{
		obj.TZ_ViewCert();
	}
}

/*
		验证签章
		参数
					SealID 签章ID
*/
function TZ_Verify(SealID)
{
	var obj = document.getElementById(SealID);
//	alert("验证签章");
	if(obj!=null)
	{
		return obj.TZ_Verify();
	}
	return false;
}


/*
 * 批量验证
*/
 function TZ_BatVerify()
 {
	  
	 	var SealNames = TZTotalSealName.split(",");
	 	for(j=0;j<SealNames.length;j++)
	 	{
	 		var obj=document.getElementById(SealNames[j]);
	 		if(obj!=null)
	 		{
	 			SignControl.TZ_SetSignInfo(obj.TZ_GetSignInfo());
	 		}
	 	}

	 	SignControl.TZ_BatVerify("");  // 
		 /* */
 }
 
 /*
  * TZ_LockSeal
  * 参数
  * 	SealID
  * 			被锁住章ID，
  * 	bFlag
  * 			标志 (1 章锁定 0 解除锁定)
  * 说明
  * 	锁住印章 禁止章拖动
 */
 function TZ_LockSeal(SealID,bFlag)
 {
	 try
	 {
		  var Obj = document.getElementById(SealID);
		  if(Obj!=null)
			 {
			 	Obj.TZ_LockSeal(bFlag);
			 }
		  else
			 {
			  	alert("获得章对象错误");
			 }
	 }catch(e)
	 {
		 alert(e);
	 }
 }
 
 /*
    保护文档内容 bFlag 1:锁定  0:不锁定
 */
 function TZ_LockSignArea(SealID,bFlag)
 {
	   try
	 {
		  var Obj = document.getElementById(SealID);
		  if(Obj!=null)
			 {
			 	Obj.TZ_LockSignArea(bFlag);
			 	return 1;
			 }
		  else
			 {
			  	alert("获得章对象错误");
			  	return  0;
			 }
	 }catch(e)
	 {
		 alert("错误 "+e+"  SealID:"+SealID);
		 return  0;
	 }
	 
 }
 /*
 * 	 保存页面上所有章 (一般一个页面上签章总数不会太多)
*/
function SaveSealAll()
{
		 var obj = document.getElementById("WorkID");
           if(obj.value == "")
           {
           		obj.value = workID;
           }
           
         //  TZ_SaveSealToServer(obj.value);
	TZ_SaveSealAll(obj.value);	
}

 /*
  * TZ_SaveSealAll 保存页面全部签章
  * 	参数：
  * 		workid  传入的workID
  * 	作用：
  * 		保存其余章数据到服务器。并非全部。
 */
 function TZ_SaveSealAll(workid)
 {
	
	 if((null == workid) || (workid == ""))
    {
        alert("业务ID为空");
        return false;
    }
    TZSaveSignSealOther =  TZSpecialSealID;
    if(TZSaveSignSealOther == "")
    {
        alert("没有签章提交!");
        return false;
    }
    
    var names = TZSaveSignSealOther.split(",");
    var ErrorSave="";
    if(names.length == 0)
    {
        alert("没有签章");
        return false;
    }
    var str="";
    var bFlag = 0;

    for(i = 0 ; i < names.length;i++)
    {
       var bRtn = TZ_SaveSeal(names[i],workid);
       if(bRtn == "0")   // ZHANGJI
        {
            bFlag = true;
            str+=  names[i]+"\r\n";
        }
        else if(bRtn == -1 || bRtn ==  2)  // 签章删除了 或无权限保存他人签章 此时不算错误
        {
             bFlag = true;
        }
        else if(bRtn == 1)
        {
            bFlag = false;
            alert('保存失败，签章ID' + names[i]);
            if(ErrorSave=="")
            {
            	ErrorSave = names[i];
            }else
            {
            	ErrorSave=ErrorSave+','+names[i]+'\r\n';
            }

            continue;
        }
    }
    if(bFlag == true && str != "")
    alert('保存成功,签章ID:\r\n' + str);
    if(bFlag == true && str == "")
    alert('没有签章提交!' );
    if(bFlag == false)
    alert('签章ID:\r\n' + ErrorSave +'保存失败!');
 }
 

