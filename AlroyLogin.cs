using NewLife.Log;
using Newtonsoft.Json;
using Power.Controls.PMS;
using Power.Global;
using Power.Service.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
                                   

namespace Power.Alroy.Controller
{
    public class AlroyLogin : BaseControl
    {
        #region 免密登录-userCode
        /// <summary>
        /// 免密登陆(后门)
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <returns></returns>
        public ViewResultModel SignleSignOnBackDoorWithUserCode(string userCode)
        {
            ViewResultModel result = ViewResultModel.Create(true, "", "");
            APIControl api = new APIControl();
            result = api.Login(userCode, "zh-CN");
            return result;
        }
        #endregion

        #region 免密登录-userName
        /// <summary>
        /// 免密登陆(后门)
        /// </summary>
        /// <param name="userCode">用户名称</param>
        /// <returns></returns>
        public ViewResultModel SignleSignOnBackDoorWithUserName(string userName)
        {
            ViewResultModel result = ViewResultModel.Create(true, "", "");

            //根据用户名获取编号
            string sql = "select Code from PB_User where name='" + userName + "'";
            DataTable dt = DBHelper.QuerySQL(sql);
            if (dt.Rows.Count <= 0)
            {
                result.success = false;
                result.message = "根据用户名" + userName + "未找到对应编号，请核对";
                return result;
            }
            else
            {
                string userCode = Convert.ToString(dt.Rows[0]["Code"]);
                APIControl api = new APIControl();
                result = api.Login(userCode, "zh-CN");
                return result;
            }
        }
        #endregion

        #region 单点登录，流程审批-MsgId
        /// <summary>
        /// 单点登录，流程审批
        /// </summary>
        /// <param name="userCode">MsgId</param>
        /// <returns></returns>
        public ViewResultModel SignleSignWithMsgId(string MsgId)
        {
            ViewResultModel result = ViewResultModel.Create(true, "", "");

            //根据用户名获取编号
            string sql = "select Code from PB_User where HumanId =(select ToHumanId from PB_Messages where Id='" + MsgId + "')";
            DataTable dt = DBHelper.QuerySQL(sql);
            if (dt.Rows.Count <= 0)
            {
                result.success = false;
                result.message = "根据消息Id" + MsgId + "未找到对应用户编号，请核对";
                return result;
            }
            else
            {
                string userCode = Convert.ToString(dt.Rows[0]["Code"]);
                APIControl api = new APIControl();
                result = api.Login(userCode, "zh-CN");
                return result;
            }
        }
        #endregion
    }
}
