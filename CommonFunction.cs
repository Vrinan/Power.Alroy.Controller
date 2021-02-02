using Power.Controls.PMS;
using Power.Global;
using Power.Service.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Power.Alroy.Controller
{
    public class CommonFunction : BaseControl
    {
        #region 虚拟登录
        /// <summary>
        /// 虚拟登录
        /// </summary>
        internal void VirtualLogin()
        {
            DataTable dtTemp = Power.Systems.StdSystem.UserDO.FindAllByTable("Code='admin'", "Code", "Code,PassWord", 0, 1);
            string pass = dtTemp.Rows[0]["PassWord"] == DBNull.Value ? "" : dtTemp.Rows[0]["PassWord"].ToString();
            Power.Controls.Action.ILoginAction loginAct = new Power.Controls.Action.LoginAction();
            loginAct.Login("admin", pass, "zh-CN");
        }
        #endregion

        #region 获取webconfig配置
        /// <summary>
        /// 获取webconfig配置
        /// </summary>
        /// <param name="key">需要获取的内容的key</param>
        /// <param name="value">获取的内容</param>
        /// string BIMGISSmsPassword = string.Empty;
        /// GetAppSetting("BIMGISSmsPassword", ref BIMGISSmsPassword);

        public void GetAppSetting(string key, ref string value, bool isEnd = false)
        {
            object obj = ConfigurationSettings.AppSettings[key];
            if (obj == null)
            {
                if (isEnd == true)
                    return;
                else
                {
                    value = "未配置" + key + ",请立即联系管理员！";
                }
            }
            value = obj.ToString();
        }
        #endregion

        #region  MD5字符串加密
        /// <summary>
        /// MD5字符串加密
        /// </summary>
        /// <param name="txt"></param>
        /// <returns>加密后字符串</returns>
        public static string GenerateMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        #endregion

        #region 设定/获取Cache
        public void setCache()
        {
            //设置
            PowerGlobal.Cache.Set("BIMGISSmsAccessToken", Convert.ToString("result"));
            PowerGlobal.Cache.ExpireAt("BIMGISSmsAccessToken", DateTime.Now.AddSeconds(86000));

            //获取token
            var accessToken = PowerGlobal.Cache.Get<String>("BIMGISSmsAccessToken");
            if (string.IsNullOrEmpty(accessToken))
            {
                //accessToken = GetAccessToken();
            }
        }
        #endregion

        #region 通用记录日志方法
        /// <summary>
        /// 通用记录日志方法
        /// </summary>
        /// <param name="code">方法编号 </param>
        /// <param name="name">方法名称</param>
        /// <param name="isSuccess">是否执行成功，1-成功，0-失败</param>
        /// <param name="total">总数量</param>
        /// <param name="successNum">成功数量/更新数量</param>
        /// <param name="failNum">失败数量</param>
        /// <param name="skipNum">跳过数量/异常数量</param>
        /// <param name="url">地址</param>
        /// <param name="memo">备注</param>
        /// <param name="dt">执行日期</param>
        /// <returns></returns>
        public string LogInterface(string code,string name,int isSuccess,int total,int successNum,int failNum,int skipNum,string url,string memo,DateTime dt)
        {
            Power.Business.IBaseBusiness bbs = Power.Business.BusinessFactory.CreateBusiness("Alroy_InterfaceLog");
            bbs.SetItem("Code", code);
            bbs.SetItem("Name", name);
            bbs.SetItem("IsSuccess", isSuccess);
            bbs.SetItem("Total", total);
            bbs.SetItem("SuccessNum", successNum);
            bbs.SetItem("FailNum", failNum);
            bbs.SetItem("SkipNum", skipNum);
            bbs.SetItem("Url", url);
            bbs.SetItem("Memo", memo);
            bbs.SetItem("ExecDate", dt);
            bbs.Save(System.ComponentModel.DataObjectMethodType.Insert);
            return "1";
        }
        #endregion

        #region 人员签章（1、单人；2、所有人员）

        #region 生成人员签章
        [ActionAttribute]
        public string InHumPic(string id, string humname)
        {
            Power.Business.IBusinessList userList = Power.Business.BusinessFactory.CreateBusinessOperate("HumanSign").FindAll("HumanId='" + id + "'", "", "Id,HumanId", 0, 0, Business.SearchFlag.IgnoreRight);
            if (userList.Count == 0)
            {
                Power.Business.IBaseBusiness tmpBusi = Power.Business.BusinessFactory.CreateBusiness("HumanSign");
                byte[] Picture = Power.Global.PowerGlobal.Image.CreateSignPicture(humname.ToString(), null, 0, 0);
                string HumanId = id;
                int Actived = 1;
                int Sequ = 1;
                string UpdDate = DateTime.Now.ToLocalTime().ToString();
                tmpBusi.SetItem("Id", Guid.NewGuid());
                tmpBusi.SetItem("HumanId", HumanId);
                tmpBusi.SetItem("Actived", Actived);
                tmpBusi.SetItem("Picture", Picture);
                tmpBusi.SetItem("Sequ", Sequ);
                tmpBusi.SetItem("UpdDate", UpdDate);
                tmpBusi.Save(System.ComponentModel.DataObjectMethodType.Insert);
                return "1";
            }
            else
            {
                return "0";
            }
        }
        #endregion

        #region 批量生成人员签章
        [ActionAttribute]
        public string BatchInHumPic()
        {
            int icount = 0;
            Power.Business.IBusinessList humanlist = Power.Business.BusinessFactory.CreateBusinessOperate("Human").FindAll("", "", "", 0, 0);
            foreach (Power.Business.IBaseBusiness item in humanlist)
            {
                Power.Business.IBusinessList userList = Power.Business.BusinessFactory.CreateBusinessOperate("HumanSign").FindAll("HumanId='" + item["id"] + "'", "", "Id,HumanId", 0, 0, Business.SearchFlag.IgnoreRight);
                if (userList.Count == 0)
                {
                    Power.Business.IBaseBusiness tmpBusi = Power.Business.BusinessFactory.CreateBusiness("HumanSign");
                    byte[] Picture = Power.Global.PowerGlobal.Image.CreateSignPicture(Convert.ToString(item["Name"]), null, 0, 0);
                    string HumanId = Convert.ToString(item["id"]);
                    int Actived = 1;
                    int Sequ = 1;
                    string UpdDate = DateTime.Now.ToLocalTime().ToString();
                    tmpBusi.SetItem("Id", Guid.NewGuid());
                    tmpBusi.SetItem("HumanId", HumanId);
                    tmpBusi.SetItem("Actived", Actived);
                    tmpBusi.SetItem("Picture", Picture);
                    tmpBusi.SetItem("Sequ", Sequ);
                    tmpBusi.SetItem("UpdDate", UpdDate);
                    tmpBusi.Save(System.ComponentModel.DataObjectMethodType.Insert);
                    icount++;
                }
            }
            return Convert.ToString(icount);
        }
        #endregion

        #endregion

        #region 删除流程(1、开发平台关键词对应表名到应用平台数据库；2、删除流程)
        /// <summary>
        /// 定时同步开发平台关键词对应表名到应用平台数据库
        /// </summary>
        /// <returns></returns>
        [ActionAttribute(Authorize = false)]
        public string UpdateKeyWordTableName()
        {
            //先虚拟登录
            VirtualLogin();

            //执行
            string sql = "";
            sql += " delete from Alroy_KeyWordTableName;";
            sql += " insert into Alroy_KeyWordTableName select a.EntityID,a.Description,a.KeyWord,b.TableName from PowerPlat.dbo.pp_Entity a inner join PowerPlat.dbo.pp_EntityTable b on a.EntityId = b.EntityID";
            DBHelper.ExecSQL(sql);

            //Log it
            LogInterface("UpdateKeyWordTableName", "开发平台关键词对应表名到应用平台数据库", 1, 0, 0, 0, 0, "", "", DateTime.Now);
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 删除流程
        /// </summary>
        /// <returns></returns>
        [ActionAttribute(Authorize = false)]
        public string DeleteWorkFlow(string FormId, string KeyWord, string KeyValue, string HumanId, string HumanName, string Title, string DeviceIP, string DelReason)
        {
            //根据keyword获取表明
            string sql = " select TableName from Alroy_KeyWordTableName where keyword='" + KeyWord + "' ";
            DataTable dt = DBHelper.QuerySQL(sql);
            if (dt.Rows.Count <= 0)
            {
                return "通过关键词未找到表名";
            }
            else
            {
                string tbName = Convert.ToString(dt.Rows[0]["TableName"]);
                //删除流程
                sql = " exec dbo.P_DeleteWorkFlow_Sam '" + KeyValue + "','" + tbName + "'";
                DBHelper.ExecSQL(sql);
                //Log
                Power.Business.IBaseBusiness bbs = Power.Business.BusinessFactory.CreateBusiness("Alroy_DeleteWorkFlowLog");
                bbs.SetItem("Id", Guid.NewGuid());
                bbs.SetItem("Title", Title);
                bbs.SetItem("DeleteReason", DelReason);
                bbs.SetItem("FormId", FormId);
                bbs.SetItem("KeyWord", KeyWord);
                bbs.SetItem("KeyValue", KeyValue);
                bbs.SetItem("HumanId", HumanId);
                bbs.SetItem("HumanName", HumanName);
                bbs.SetItem("ExecTime", DateTime.Now);
                bbs.SetItem("IPRecord", DeviceIP);
                bbs.SetItem("MacAdress", 0);
                bbs.Save(System.ComponentModel.DataObjectMethodType.Insert);
                return "success";
            }
        }
        #endregion

        #region 发送邮件
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="fromEmailAdress">发送人邮箱地址</param>
        /// <param name="fromEmailTitle">发送人</param>
        /// <param name="toEmailAdress">接收人邮箱地址</param>
        /// <param name="messageSubject">主题</param>
        /// <param name="messageBody">内容</param>
        public void SendMails(String fromEmailAdress, String fromEmailTitle, String toEmailAdress, String messageSubject, string messageBody)
        {
            MailHelper mail = new MailHelper();
            MailAddress messageFrom = new MailAddress(fromEmailAdress, fromEmailTitle);
            mail.SendMail(messageFrom, toEmailAdress, messageSubject, messageBody);
        }
        #endregion
    }
}
