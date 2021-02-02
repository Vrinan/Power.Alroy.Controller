using Power.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Power.Alroy.Control;

public partial class AlroyLogin : System.Web.UI.Page
{
    public string errmsg = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string opt = Request.Params["Opt"];
            if(string.IsNullOrEmpty(opt))
            {
                errmsg = "参数异常，登录失败";
                return;
            }
            string rdtUrl = string.Empty;
            rdtUrl = "/WebCenter/Open/00000000-0000-0000-0000-00000000000a";
            switch (opt)
            {
                //免密登录，UserCode或者UserName二选一登录系统
                case "BackDoor":
                    string userCode = Request.Params["UserCode"];
                    string userName = Request.Params["UserName"];
                    if (String.IsNullOrEmpty(userCode) && String.IsNullOrEmpty(userName))
                    {
                        errmsg = "免密登录失败，参数错误!";
                        return;
                    }
                    if (!String.IsNullOrEmpty(userCode))
                    {
                        Power.Alroy.Control.AlroyLogin stLogin = new Power.Alroy.Control.AlroyLogin();
                        ViewResultModel rlt = stLogin.SignleSignOnBackDoorWithUserCode(userCode);
                        if (rlt.success)
                        {
                            //设置打开项目管理
                            Power.Business.Common.Helper.setCookie("firstmenuids", "cccccccc-0000-0000-0000-000000000000", 1);
                            Power.Business.Common.Helper.setCookie("firstmenuname", "个人中心", 1);
                            Power.Business.Common.Helper.setCookie("menuids", "cccccccc-0000-0000-0000-000000000000", 1);
                            HttpContext.Current.Response.Redirect(rdtUrl, false);
                        }
                        else
                        {
                            errmsg = rlt.message;
                            return;
                        }
                    }
                    else
                    {
                        Power.Alroy.Control.AlroyLogin stLogin = new Power.Alroy.Control.AlroyLogin();
                        ViewResultModel rlt = stLogin.SignleSignOnBackDoorWithUserName(userName);
                        if (rlt.success)
                        {
                            //设置打开项目管理
                            Power.Business.Common.Helper.setCookie("firstmenuids", "cccccccc-0000-0000-0000-000000000000", 1);
                            Power.Business.Common.Helper.setCookie("firstmenuname", "个人中心", 1);
                            Power.Business.Common.Helper.setCookie("menuids", "cccccccc-0000-0000-0000-000000000000", 1);
                            HttpContext.Current.Response.Redirect(rdtUrl, false);
                        }
                        else
                        {
                            errmsg = rlt.message;
                            return;
                        }
                    }
                    break;
                //流程信息，msgId，自动查找用户编号登录
                case "WorkFlow":
                    string msgId = Request.Params["MsgId"];
                    if(string.IsNullOrEmpty(msgId))
                    {
                        errmsg = "MsgId不能为空";
                        return;
                    }
                    else
                    {
                        //Login
                        Power.Alroy.Control.AlroyLogin stLoginM = new Power.Alroy.Control.AlroyLogin();
                        ViewResultModel rltm = stLoginM.SignleSignWithMsgId(msgId);
                        if (rltm.success)
                        {
                            if (String.IsNullOrEmpty(msgId))
                            {
                                Power.Business.Common.Helper.setCookie("menuids", "aaaaaaaa-0000-0000-0000-000000000000", 1);
                            }
                            else
                            {
                                Power.Business.Common.Helper.setCookie("loginredirect", "/Message/Show/" + msgId, 1, false);
                                Power.Business.Common.Helper.setCookie("redirecttype", "open", 1, false);
                            }
                            //设置打开项目管理
                            Power.Business.Common.Helper.setCookie("firstmenuids", "aaaaaaaa-0000-0000-0000-000000000000", 1);
                            Power.Business.Common.Helper.setCookie("firstmenuname", "项目管理", 1);
                            HttpContext.Current.Response.Redirect(rdtUrl, false);
                        }
                        else
                        {
                            errmsg = rltm.message;
                        }
                    }
                    break;
                //窗体或表单，ViewType为打开类型，有Single-窗体，有Single+Web表单
                case "Form":
                    string viewType = Request.Params["ViewType"];
                    string singleId = Request.Params["FormId"];
                    string webId = Request.Params["KeyValue"];
                    if (String.IsNullOrEmpty(viewType))
                    {
                        errmsg = "免密登录失败，参数错误!";
                        return;
                    }
                    switch (viewType)
                    {
                        case "Single":
                            if (!String.IsNullOrEmpty(singleId) && String.IsNullOrEmpty(webId))
                            {
                                Power.Alroy.Control.AlroyLogin stLogin = new Power.Alroy.Control.AlroyLogin();
                                ViewResultModel rlt = stLogin.SignleSignOnBackDoorWithUserCode("Admin");
                                if (rlt.success)
                                {
                                    //设置打开项目管理
                                    Power.Business.Common.Helper.setCookie("firstmenuids", "cccccccc-0000-0000-0000-000000000000", 1);
                                    Power.Business.Common.Helper.setCookie("firstmenuname", "个人中心", 1);
                                    Power.Business.Common.Helper.setCookie("menuids", "cccccccc-0000-0000-0000-000000000000", 1);
                                    rdtUrl = "/Form/EditForm/" + singleId + "/";
                                    HttpContext.Current.Response.Redirect(rdtUrl, false);
                                }
                                else
                                {
                                    errmsg = rlt.message;
                                    return;
                                }
                            }
                            else
                            {
                                errmsg = "singleId错误";
                                return;
                            }
                            break;
                        case "Web":
                            if (!String.IsNullOrEmpty(singleId) && !String.IsNullOrEmpty(webId))
                            {
                                Power.Alroy.Control.AlroyLogin stLogin = new Power.Alroy.Control.AlroyLogin();
                                ViewResultModel rlt = stLogin.SignleSignOnBackDoorWithUserCode("Admin");
                                if (rlt.success)
                                {
                                    //设置打开项目管理
                                    Power.Business.Common.Helper.setCookie("firstmenuids", "cccccccc-0000-0000-0000-000000000000", 1);
                                    Power.Business.Common.Helper.setCookie("firstmenuname", "个人中心", 1);
                                    Power.Business.Common.Helper.setCookie("menuids", "cccccccc-0000-0000-0000-000000000000", 1);
                                    rdtUrl = "/Form/ValidForm/" + singleId + "/edit/" + webId + "/";
                                    HttpContext.Current.Response.Redirect(rdtUrl, false);
                                }
                                else
                                {
                                    errmsg = rlt.message;
                                    return;
                                }
                            }
                            else
                            {
                                errmsg = "WebFormId错误";
                                return;
                            }
                            break;
                        default:
                            errmsg = "参数错误，登陆失败";
                            return;
                            break;
                    }
                    break;
                default:
                    errmsg = "参数错误，登陆失败";
                    break;
            }
        }
    }
}