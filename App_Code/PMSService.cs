using System.Collections.Generic;
using System.Web.Services;
using Newtonsoft.Json;
using System.Collections;
using Power.Service;
using Power.Systems.StdSystem;
using Power.Business;
using System;
using System.Data;
using XCode.DataAccessLayer;
using Power.Global;
using Power.Controls.PMS;
using System.Text;
using System.Linq;
using Power.Service.Common;

/// <summary>
/// Summary description for GetInvoiceFromNC
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class PMSService : System.Web.Services.WebService
{
    public PMSService()
    {
    }

    #region 流程数量WorkFlowNum(string userCode)
    [WebMethod(Description = "流程数量")]
    public string WorkFlowNum(string userCode)
    {
        WorkFlowCountData rtnData = new WorkFlowCountData();
        rtnData.code = "200";
        rtnData.msg = "";

        if (string.IsNullOrEmpty(userCode))
        {
            rtnData.code = "0";
            rtnData.msg = "传入的用户编号错误！";
            return JsonConvert.SerializeObject(rtnData);
        }

        UserBO userBO = UserBO.FindByKey(new string[] { "Code" }, new string[] { userCode }, SearchFlag.IgnoreRight);
        if (userBO == null)
        {
            rtnData.code = "0";
            rtnData.msg = "PMS系统中不存在编号为[" + userCode + "]的用户！";
            return JsonConvert.SerializeObject(rtnData);
        }

        //获取流程数量
        var workInfo = WorkFlowHelper.GetMyWorkInfos(userBO.HumanId.ToString(), 0, 0, "");

        Hashtable hs = new Hashtable();
        hs.Add("num", workInfo.Total);

        rtnData.data = hs;

        return JsonConvert.SerializeObject(rtnData);
    }
    #endregion

    #region 待办信息WorkFlowInfo(string userCode, string currentPage, string showCount)
    [WebMethod(Description = "待办信息")]
    public string WorkFlowInfo(string userCode, string currentPage, string showCount)
    {
        WorkFlowInfoData rtnData = new WorkFlowInfoData();
        rtnData.code = "200";
        rtnData.msg = "";

        if (string.IsNullOrEmpty(userCode))
        {
            rtnData.code = "0";
            rtnData.msg = "传入的用户编号错误！";
            return JsonConvert.SerializeObject(rtnData);
        }

        UserBO userBO = UserBO.FindByKey(new string[] { "Code" }, new string[] { userCode }, SearchFlag.IgnoreRight);
        if (userBO == null)
        {
            rtnData.code = "0";
            rtnData.msg = "PMS系统中不存在编号为[" + userCode + "]的用户！";
            return JsonConvert.SerializeObject(rtnData);
        }

        //判断页码curPage、每页显示数量shCount是否合法
        int curPage = 0;
        int shCount = 0;
        try
        {
            curPage = Convert.ToInt32(currentPage);
            shCount = Convert.ToInt32(showCount);
        }
        catch
        {
            rtnData.code = "0";
            rtnData.msg = "页码/每页显示数量 格式错误";
            return JsonConvert.SerializeObject(rtnData);
        }

        //总条数totalResult
        var workInfo = WorkFlowHelper.GetMyWorkInfos(userBO.HumanId.ToString(), curPage - 1, shCount, "");
        int totalResult = workInfo.Total;
        //无待审批流程
        if (totalResult == 0)
        {
            rtnData.code = "200";
            rtnData.msg = "无待审批流程";
            return JsonConvert.SerializeObject(rtnData);
        }
        //总页数totalPage
        int totalPage = 0;
        totalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(totalResult) / shCount));

        //流程信息
        List<Hashtable> hsList = new List<Hashtable>();
        foreach (var item in workInfo.Records)
        {
            Hashtable hs = new Hashtable();
            var itemInfo = JsonConvert.DeserializeObject<Hashtable>(JsonConvert.SerializeObject(item));
            hs.Add("URL", "http://127.0.0.1:8080/StandardPage/StandardLogin.aspx?Opt=WorkFlowStandard&MsgId=" + Convert.ToString(itemInfo["Id"]));
            hs.Add("TM", Convert.ToDateTime(itemInfo["wfDate"]).ToString("yyyy-MM-dd HH:mm:ss"));//格式化
            hs.Add("TITLE", Convert.ToString(itemInfo["Title"]));
            hsList.Add(hs);
        }

        //返回值定义
        Hashtable hsRtn = new Hashtable();
        hsRtn.Add("currentPage", curPage);
        hsRtn.Add("showCount", shCount);
        hsRtn.Add("totalResult", totalResult);
        hsRtn.Add("totalPage", totalPage);
        hsRtn.Add("listData", hsList);

        rtnData.data = hsRtn;
        return JsonConvert.SerializeObject(rtnData);
    }
    #endregion

    #region 重置密码ResetPwd(string userCode)
    [WebMethod(Description = "重置密码")]
    public string ResetPwd(string userCode)
    {
        WorkFlowCountData rtnData = new WorkFlowCountData();
        rtnData.code = "200";
        rtnData.msg = "";

        if (string.IsNullOrEmpty(userCode))
        {
            rtnData.code = "0";
            rtnData.msg = "传入的用户编号错误！";
            return JsonConvert.SerializeObject(rtnData);
        }

        UserBO userBO = UserBO.FindByKey(new string[] { "Code" }, new string[] { userCode }, SearchFlag.IgnoreRight);
        if (userBO == null)
        {
            rtnData.code = "0";
            rtnData.msg = "PMIS系统中不存在编号为[" + userCode + "]的用户！";
            return JsonConvert.SerializeObject(rtnData);
        }

        //重置
        string sql = "";
        sql += " update pb_user set password='' where Code='" + userCode + "'";
        int rt = DBHelper.ExecSQL(sql);
        rtnData.code = Convert.ToString(rt);
        if (rt == 1)
        {
            rtnData.msg = "'"+userCode+"'密码已重置为空";
        }
        else
        {
            rtnData.msg = "数据库连接失败";
        }

        return JsonConvert.SerializeObject(rtnData);
    }
    #endregion

    #region 返回值类
    private class WorkFlowCountData
    {
        public string code { get; set; }
        public string msg { get; set; }
        public Hashtable data { get; set; }//待办数量
    }

    private class WorkFlowInfoData
    {
        public string code { get; set; }
        public string msg { get; set; }
        public Hashtable data { get; set; }
    }

    private class ResetPwdInfo
    {
        public string code { get; set; }
        public string msg { get; set; }
        public Hashtable data { get; set; }
    }
    #endregion
}
