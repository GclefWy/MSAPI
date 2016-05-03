using System;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Runtime.Serialization.Json;
using System.Data;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MagicScreenAPI.Controllers
{
    public class MSBasicController : Controller
    {


        // GET: MSBasic
        // 用户表接口
        //api/useroperation?operationtype=2&username=Dyy&usermobile=18610261609&userpwd=abcde operationtype 1为查询 2为插入
        public JsonpResult UserOperation(string operationtype, string username, string usermobile, string userpwd)
        {
            Common.AddResponseHeader();
            try
            {
                //if(operationtype == "1")
                switch (operationtype)
                {
                    case "1"://查询
                        if (username == "")
                        {
                            string sql1 = "select UserID,UserName,UserMobile,UserPWD from [UserTable] with(nolock) where usermobile = '" + usermobile + "'";
                            DataSet result1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sql1);
                            var UserInfo = DataTableToListModel<UserInfo>.ConvertToModel(result1.Tables[0]);



                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                UserInfo
                            });
                        }
                        else
                        {
                            string sql1 = "select UserID,UserName,UserMobile,UserPWD from [UserTable] with(nolock) where username = '" + username + "'";
                            DataSet result1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sql1);
                            var UserInfo = DataTableToListModel<UserInfo>.ConvertToModel(result1.Tables[0]);

                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                UserInfo
                            });
                        }


                    case "2"://插入
                        if (username == "" || usermobile == "" || userpwd == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "插入字段不能为空"
                            });
                        }
                        else
                        {
                            string sql2 = @"insert into UserTable with(rowlock) (userid,username,usermobile,userpwd)
                                            select newid(),'" + username + "','" + usermobile + "','" + userpwd + "'";
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql2);

                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "插入成功"
                            });
                        }

                    case "3"://更新
                        if (username == "" || userpwd == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "更新缺少必要条件"
                            });
                        }
                        else
                        {
                            string sql3 = @"update UserTable
                                            set userpwd= '" + userpwd + @"',updatetime = getdate() where username = '" + username + "'" ;

                            

                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql3);

                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "更新成功"
                            });
                        }


                    case "4"://验证

                        string sql4 = "select UserID,UserName,UserMobile,UserPWD from [UserTable] with(nolock) where usermobile = '" + usermobile + "' and UserPWD ='" + userpwd + "'";
                        DataSet result4 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sql4);
                        var VerifyInfo = DataTableToListModel<UserInfo>.ConvertToModel(result4.Tables[0]);



                        return this.Jsonp(new
                        {
                            ErrorCode = 0x00,
                            VerifyInfo
                        });


                    default:
                        return this.Jsonp(new
                        {
                            ErrorCode = 0x02,
                            ErrMsg = "非法的操作类型代码"
                        });



                }

            }
            catch (Exception ex)
            {
                return this.Jsonp(new { ErrorCode = 0x01, ErrMsg = ex.Message });
            }
        } 

        // 用户活动关系表
        //api/relationoperation?operationtype=2&userid=A6B507EC-2F89-4744-9538-3A055C895C83&partyid=A6B507EC-2F89-4744-9538-3A055C895C83 插入
        //api/relationoperation?operationtype=3&userid=A6B507EC-2F89-4744-9538-3A055C895C83&partyid=A6B507EC-2F89-4744-9538-3A055C895C83&state=0 更新
        //api/relationoperation?operationtype=1&userid=A6B507EC-2F89-4744-9538-3A055C895C83 查询
        public JsonpResult RelationOperation(string operationtype, string userid, string partyid, string state)
        {
            Common.AddResponseHeader();
            try
            {
                //if(operationtype == "1")
                switch (operationtype)
                {
                    case "1"://查询

                        string sql1 = "select UserID,PartyID,State from [UserPartyRelation] with(nolock) where userid = '" + userid + "' order by createtime desc";
                        DataSet result1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sql1);
                        var UserPartyRelation = DataTableToListModel<UserPartyRelation>.ConvertToModel(result1.Tables[0]);

                        return this.Jsonp(new
                        {
                            ErrorCode = 0x00,
                            UserPartyRelation
                        });



                    case "2"://插入
                        if (userid == "" || partyid == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "插入字段不能为空"
                            });
                        }
                        else
                        {
                            string sql2 = @"insert into UserPartyRelation with(rowlock) (userid,partyid)
                                            select '" + userid + "','" + partyid + "'";
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql2);

                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "插入成功"
                            });
                        }

                    case "3"://更新
                        if (userid == "" || partyid == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "更新缺少必要条件"
                            });
                        }
                        else
                        {
                            string sql3 = @"update UserPartyRelation
                                            set state= " + state + @",updatetime = getdate() where userid = '" + userid + "'" + " and partyid = '" + partyid + "'";
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql3);

                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "更新成功"
                            });
                        }

                    default:
                        return this.Jsonp(new
                        {
                            ErrorCode = 0x02,
                            ErrMsg = "非法的操作类型代码"
                        });



                }

            }
            catch (Exception ex)
            {
                return this.Jsonp(new { ErrorCode = 0x01, ErrMsg = ex.Message });
            }

        }

        //活动基础信息表
        //api/partybaseoperation?operationtype=1&partyid=65036AAE-3F5D-49DF-AA2D-E48206279A7E,5787EE02-5734-4122-868F-DCF7075A0C10,D2BBAFA8-E641-46CF-A351-A76BB07C7CD9&partyschema=测试的主题&channel=测试的通道&serviceaccount=测试的服务账号&partytime=2015-12-23&partyaddress=上海市宝山区&partyheadcount=200&partylimitcount=300&partylinkman=测试联系人&mobile=123456789&partyinvitecode=65036AAE-3F5D-49DF-AA2D-E48206279A7E&partystate=1 查询
        //api/partybaseoperation?operationtype=2&partyid=65036AAE-3F5D-49DF-AA2D-E48206279A7E&partyschema=测试的主题&channel=测试的通道&serviceaccount=测试的服务账号&partytime=2015-12-23&partyaddress=上海市宝山区&partyheadcount=200&partylimitcount=300&partylinkman=测试联系人&mobile=123456789&partyinvitecode=65036AAE-3F5D-49DF-AA2D-E48206279A7E&partystate=1 插入
        //api/partybaseoperation?operationtype=3&partyid=65036AAE-3F5D-49DF-AA2D-E48206279A7E&partyschema=测试的主题&channel=测试的通道&serviceaccount=测试的服务账号&partytime=2015-12-23&partyaddress=上海市宝山区&partyheadcount=200&partylimitcount=300&partylinkman=测试联系人&mobile=123456789&partyinvitecode=65036AAE-3F5D-49DF-AA2D-E48206279A7E&partystate=0 更新
        public JsonpResult PartyBaseOperation(string operationtype, string partyid, string partyschema, string channel,
            string serviceaccount, string partytime, string partyaddress, string partyheadcount, string partylimitcount,
            string partylinkman, string mobile, string partyinvitecode, string partystate, string partywxid)
        {
            Common.AddResponseHeader();
            try
            {
                //if(operationtype == "1")

                switch (operationtype)
                {
                    case "1"://查询

                        string[] splitpartyid = partyid.Split(new Char[] { ',' });
                        string querypartyid = "";

                        for (int i = 0; i < splitpartyid.Length; i++)
                        {
                            querypartyid += "'" + splitpartyid[i] + "',";
                        }

                        querypartyid = querypartyid.Substring(0, querypartyid.Length - 1);

                        string sql1 = @"select [PartyID],[PartySchema],[Channel],[ServiceAccount],convert(varchar(10),[PartyTime],120) as PartyTime,[PartyAddress]
                                              ,[PartyHeadCount],[PartyLimitCount],[PartyLinkman],[Mobile],[PartyInviteCode],[PartyState],[PartyWXID],[PartyEWCode]
      ,[PartyControlCode]
                                              from [PartyBasicInfo] with(nolock) where PartyID in (" + querypartyid + ") order by createtime desc";
                        DataSet result1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sql1);
                        var PartyBaseInfo = DataTableToListModel<PartyBaseInfo>.ConvertToModel(result1.Tables[0]);

                        return this.Jsonp(new
                        {
                            ErrorCode = 0x00,
                            PartyBaseInfo
                        });



                    case "2"://插入
                        if (partyid == "" || partyschema == ""  || partyaddress == ""
                            || partyheadcount == "" || partylimitcount == "" || partylinkman == "" || mobile == "" || partystate == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "插入字段不能为空"
                            });
                        }
                        else
                        {


                            string signwall = "";
                            string startwall = "";
                            string textwall = "";
                            string picwall = "";
                            string danwall = "";
                            string weddingwall = "";

                            string querysql = @"select 1 as id,moduleid,templetid,templeturl from  [TempletIndex] where TempletID='3DC866D7-B7A3-4172-A009-253DF2FBF2ED'
  --启动墙
  union
   select 2 as id ,moduleid,templetid,templeturl from  [TempletIndex] where TempletID='756193C6-7D03-42AD-95AC-45B8F90DAB48'
   --文字墙
   union
   select 3 as id ,moduleid,templetid,templeturl from  [TempletIndex] where TempletID='B656120E-77EE-47A8-9F64-D9C621D86762'
   --照片墙
   union
   select 4 as id ,moduleid,templetid,templeturl from  [TempletIndex] where TempletID='1B559D8C-F1BE-40E8-9C6E-09D453FEC289'

   --弹幕
   union
   select 5 as id ,moduleid,templetid,templeturl from  [TempletIndex] where TempletID='028779DA-E9A9-4E6A-AD00-A37A2C69AB19'

   --婚纱照
   union
   select 6 as id ,moduleid,templetid,templeturl from  [TempletIndex] where TempletID='1BDBABBF-EF34-4154-9095-FFB2146F2143'";

                            DataSet queryresult = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, querysql);



                            signwall = "{\"templeteid\":\"" + queryresult.Tables[0].Rows[0][2].ToString() + "\"," + "\"url\":\"" + queryresult.Tables[0].Rows[0][3].ToString() + "\"}";
                            startwall = "{\"templeteid\":\"" + queryresult.Tables[0].Rows[1][2].ToString() + "\"," + "\"url\":\"" + queryresult.Tables[0].Rows[1][3].ToString() + "\"}";
                            textwall = "{\"templeteid\":\"" + queryresult.Tables[0].Rows[2][2].ToString() + "\"," + "\"url\":\"" + queryresult.Tables[0].Rows[2][3].ToString() + "\"}";
                            picwall = "{\"templeteid\":\"" + queryresult.Tables[0].Rows[3][2].ToString() + "\"," + "\"url\":\"" + queryresult.Tables[0].Rows[3][3].ToString() + "\"}";
                            danwall = "{\"templeteid\":\"" + queryresult.Tables[0].Rows[4][2].ToString() + "\"," + "\"url\":\"" + queryresult.Tables[0].Rows[4][3].ToString() + "\"}";
                            weddingwall = "{\"templeteid\":\"" + queryresult.Tables[0].Rows[5][2].ToString() + "\"," + "\"url\":\"" + queryresult.Tables[0].Rows[5][3].ToString() + "\"}";


                            TempletState ts1 = (TempletState)JsonConvert.DeserializeObject(signwall, typeof(TempletState));
                            TempletState ts2 = (TempletState)JsonConvert.DeserializeObject(startwall, typeof(TempletState));
                            TempletState ts3 = (TempletState)JsonConvert.DeserializeObject(textwall, typeof(TempletState));
                            TempletState ts4 = (TempletState)JsonConvert.DeserializeObject(picwall, typeof(TempletState));
                            TempletState ts5 = (TempletState)JsonConvert.DeserializeObject(danwall, typeof(TempletState));
                            TempletState ts6 = (TempletState)JsonConvert.DeserializeObject(weddingwall, typeof(TempletState));

                            string psignwall = JsonConvert.SerializeObject(ts1);
                            string pstartwall = JsonConvert.SerializeObject(ts2);
                            string ptextwall = JsonConvert.SerializeObject(ts3);
                            string ppicwall = JsonConvert.SerializeObject(ts4);
                            string pdanwall = JsonConvert.SerializeObject(ts5);
                            string pweddingwall = JsonConvert.SerializeObject(ts6);


                            //判断partytime是不是空 如果是空则改成今天
                            if (string.IsNullOrEmpty(partytime)) { partytime = DateTime.Today.ToString(); }

                            string sql2 = @"INSERT INTO [PartyBasicInfo] with(rowlock)
                                            ([PartyID],[PartySchema],[PartyTime],[PartyAddress],[PartyHeadCount],[PartyLimitCount]
                                             ,[PartyLinkman],[Mobile],[PartyInviteCode],[PartyState],[CreateTime])
                                            VALUES ('" + partyid + "','" + partyschema + "','" + partytime + "','"
                                            + partyaddress +partyheadcount+ "','20','"  + "','" + partylinkman + "','" + mobile + "','"
                                            + partyinvitecode + "','" + partystate + "'," + @"getdate()); 
                                            insert into PartyStateInfo with(rowlock) (PartyID,[PartySignWall]
           ,[PartyStartWall]
           ,[PartyTextWall]
           ,[PartyPicWall]
           ,[PartyDanWall]
           ,[PartyWeddingWall]) values ('" + partyid + @"','" + psignwall + @"','" + pstartwall + @"','" + ptextwall + @"','" + ppicwall + @"','" + pdanwall + @"','" + pweddingwall + @"');
                                             insert into ControlTable with(rowlock) (PartyID) values ('" + partyid + @"')";
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql2);

                            DataSet wxresult = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, @"select ID from PartyBasicInfo where partyid = '" + partyid + "'");



                            string wxid = Common.ParamEncrypt(Convert.ToInt32(wxresult.Tables[0].Rows[0][0].ToString()));

                            string sqlupdate = @"update PartyBasicInfo set PartyWXID = '" + wxid + "' where partyid = '" + partyid + "'";

                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sqlupdate);

                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "插入成功"
                            });
                        }

                    case "3"://更新
                        if (partyid == "" || partyschema == "" || channel == "" || partytime == "" || partyaddress == ""
                            || partyheadcount == "" || partylimitcount == "" || partylinkman == "" || mobile == "" || partystate == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "更新缺少必要条件"
                            });
                        }
                        else
                        {
                            string sql3 = @"update [PartyBasicInfo]
                                            set [PartyID] = '" + partyid + @"'
                                            ,[PartySchema] = '" + partyschema + @"'
                                            ,[Channel] = '" + channel + @"'
                                            
                                            ,[PartyTime] = '" + partytime + @"'
                                            ,[PartyAddress] = '" + partyaddress + @"'
                                            ,[PartyHeadCount] = " + partyheadcount + @"
                                            ,[PartyLimitCount] = " + partylimitcount + @"
                                            ,[PartyLinkman] = '" + partylinkman + @"'
                                            ,[Mobile] = '" + mobile + @"'
                                            ,[PartyInviteCode] = '" + partyinvitecode + @"'
                                            ,[PartyState] = " + partystate + @"
                                            
                                            ,[UpdateTime] = getdate()" +
                                            @"where partyid = '" + partyid + "'";
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql3);

                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "更新成功"
                            });
                        }

                    case "4"://只更新活动状态
                        if (partyid == ""  || partystate == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "更新缺少必要条件"
                            });
                        }
                        else
                        {
                            string sql3 = @"update [PartyBasicInfo]
                                            set 
                                            [PartyState] = " + partystate + @"
                                            
                                            ,[UpdateTime] = getdate()" +
                                            @"where partyid = '" + partyid + "'";
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql3);

                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "更新成功"
                            });
                        }

                    default:
                        return this.Jsonp(new
                        {
                            ErrorCode = 0x02,
                            ErrMsg = "非法的操作类型代码"
                        });



                }

            }
            catch (Exception ex)
            {
                return this.Jsonp(new { ErrorCode = 0x01, ErrMsg = ex.Message });
            }


        }

        //与会者表
        //api/participantoperation?operationtype=2&partyid=65036AAE-3F5D-49DF-AA2D-E48206279A7E&participantid=65036AAE-3F5D-49DF-AA2D-E48206279A7E&participantname=测试的通道&participantpic=测试的服务账号&participantstate=1&sceneid=10 插入
        public JsonpResult ParticipantOperation(string operationtype, string partyid, string participantid, string participantname,
             string participantpic, string participantstate, string sceneid)
        {

            Common.AddResponseHeader();
            try
            {
                //if(operationtype == "1")
                switch (operationtype)
                {
                    case "1"://查询

                        string[] splitpartyid = partyid.Split(new Char[] { ',' });
                        string querypartyid = "";

                        for (int i = 0; i < splitpartyid.Length; i++)
                        {
                            querypartyid += "'" + splitpartyid[i] + "',";
                        }

                        querypartyid = querypartyid.Substring(0, querypartyid.Length - 1);

                        //                        string sql1 = @"select [PartyID]
                        //      ,[ParticipantID]
                        //      ,[ParticipantName]
                        //      ,[ParticipantPIC]
                        //      ,[ParticipantState]
                        //,[SceneID]
                        //                                              from [ParticipantTable] with(nolock) where PartyID in (" + querypartyid + ")";


                        string sql1 = @";with t1 as(
select [PartyID]
      ,[ParticipantID]
      ,[ParticipantName]
      ,[ParticipantPIC]
      ,[ParticipantState]
,[SceneID],
ROW_NUMBER() over (partition by partyid,ParticipantID order by ParticipantID desc) cc
from [ParticipantTable] with(nolock)  where PartyID in (" + querypartyid + @") and status =1
)
select * from t1 where cc=1  ";

                        DataSet result1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sql1);
                        var Participant = DataTableToListModel<Participant>.ConvertToModel(result1.Tables[0]);

                        return this.Jsonp(new
                        {
                            ErrorCode = 0x00,
                            Participant
                        });

                    case "4"://查询总数

                        string[] splitpartyid1 = partyid.Split(new Char[] { ',' });
                        string querypartyid1 = "";

                        for (int i = 0; i < splitpartyid1.Length; i++)
                        {
                            querypartyid1 += "'" + splitpartyid1[i] + "',";
                        }

                        querypartyid = querypartyid1.Substring(0, querypartyid1.Length - 1);

                        string sqlc = @"select count(distinct ParticipantID) as CountTotal
                                              from [ParticipantTable] with(nolock) where PartyID in (" + querypartyid + ")";
                        DataSet resultc = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sqlc);
                        string TotalCount = resultc.Tables[0].Rows[0][0].ToString();

                        return this.Jsonp(new
                        {
                            ErrorCode = 0x00,
                            TotalCount
                        });



                    case "2"://插入 目前并未使用该接口
                        if (partyid == "" || participantid == "" || participantname == "" || participantpic == "" || participantstate == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "插入字段不能为空"
                            });
                        }
                        else
                        {
                            string sql2 = @"INSERT INTO [ParticipantTable] with(rowlock)
                                            ([PartyID]
      ,[ParticipantID]
      ,[ParticipantName]
      ,[ParticipantPIC]
      ,[ParticipantState],[SceneID],[CreateTime])
                                            VALUES ('" + partyid + "','" + participantid + "','" + participantname + "','" + participantpic + "','" + participantstate + "','" + sceneid + "',"
                                             + @"getdate()); ";
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql2);



                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "插入成功"
                            });
                        }

                    case "3"://更新
                        if (partyid == "" || participantid == "" || participantstate == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "更新缺少必要条件"
                            });
                        }
                        else
                        {
                            string sql3 = @"update [ParticipantTable]
                                            set 
                                            [ParticipantState] = '" + participantstate + @"'

                                            ,[UpdateTime] = getdate() " +
                                            @"where status =1 and partyid = '" + partyid + "' and participantid ='" + participantid + "'";
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql3);

                            string sqlqureysceneid = string.Format(@"select id from PartyBasicInfo where partyid = '{0}'",partyid);

                            DataSet resultsceneid = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sqlqureysceneid);
                            string sid = resultsceneid.Tables[0].Rows[0][0].ToString();

                            string sql3plus = string.Format(@"update [UserSign]
                                            set 
                                            [ParticipantState] = '{2}'
                                             where Sceneid = '{0}' and openid ='{1}'", sid,participantid,participantstate);
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql3plus);

                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "更新成功"
                            });
                        }

                    default:
                        return this.Jsonp(new
                        {
                            ErrorCode = 0x02,
                            ErrMsg = "非法的操作类型代码"
                        });



                }

            }
            catch (Exception ex)
            {
                return this.Jsonp(new { ErrorCode = 0x01, ErrMsg = ex.Message });
            }
        }

        //活动状态信息
        //1为查询 3为更新
        public JsonpResult PartyStateOperation(string operationtype, string partyid, string partysigncount, string partysignwall,
            string partystartwall, string partytextwall, string partypicwall, string partydanwall, string partyweddingwall)
        {
            Common.AddResponseHeader();
            try
            {
                //if(operationtype == "1")
                switch (operationtype)
                {
                    case "1"://查询

                        string[] splitpartyid = partyid.Split(new Char[] { ',' });
                        string querypartyid = "";

                        for (int i = 0; i < splitpartyid.Length; i++)
                        {
                            querypartyid += "'" + splitpartyid[i] + "',";
                        }

                        querypartyid = querypartyid.Substring(0, querypartyid.Length - 1);

                        string sql1 = @"select [PartyID]
      ,[PartySignCount]
      ,[PartySignWall]
      ,[PartyStartWall]
      ,[PartyTextWall]
      ,[PartyPicWall]
      ,[PartyDanWall]
,[PartyWeddingWall]
                                              from [PartyStateInfo] with(nolock) where PartyID in (" + querypartyid + ")";
                        DataSet result1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sql1);
                        var PartyStateInfo = DataTableToListModel<PartyStateInfo>.ConvertToModel(result1.Tables[0]);

                        return this.Jsonp(new
                        {
                            ErrorCode = 0x00,
                            PartyStateInfo
                        });



                    //              case "2"://插入
                    //                  if (partyid == "" || partysigncount == "" || partysignwall == "" || partystartwall == "" || partytextwall == "" || partypicwall == "" || partytextwall == "")
                    //                  {
                    //                      return Json(new
                    //                      {
                    //                          ErrorCode = 0x02,
                    //                          ErrMsg = "插入字段不能为空"
                    //                      });
                    //                  }
                    //                  else
                    //                  {
                    //                      string sql2 = @"INSERT INTO [PartyStateInfo] with(rowlock)
                    //                                      ([PartyID]
                    //,[ParticipantID]
                    //,[ParticipantName]
                    //,[ParticipantPIC]
                    //,[ParticipantState],[CreateTime])
                    //                                      VALUES ('" + partyid + "','" + participantid + "','" + participantname + "','" + participantpic + "','" + participantstate + "',"
                    //                                       + @"getdate()); ";
                    //                      SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql2);



                    //                      return Json(new
                    //                      {
                    //                          ErrorCode = 0x00,
                    //                          ErrMsg = "插入成功"
                    //                      });
                    //                  }

                    case "3"://更新
                        if (partyid == "" || partysigncount == "" || partysignwall == "" || partystartwall == "" || partytextwall == "" || partypicwall == "" || partydanwall == "" || partyweddingwall == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "更新缺少必要条件"
                            });
                        }
                        else
                        {
                            string sql3 = @"update [PartyStateInfo]
                                            set [PartyID] = '" + partyid + @"'
                                            ,[PartySignCount] = " + partysigncount + @"
                                            ,[PartySignWall] = '" + partysignwall + @"'
                                            ,[PartyStartWall] = '" + partystartwall + @"'
                                            ,[PartyTextWall] = '" + partytextwall + @"'
,[PartyPicWall] = '" + partypicwall + @"'
,[PartyDanWall] = '" + partydanwall + @"'
,[PartyWeddingWall] = '" + partyweddingwall + @"'
                                            ,[UpdateTime] = getdate()" +
                                            @"where partyid = '" + partyid + "'";
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql3);

                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "更新成功"
                            });
                        }

                    case "4"://更新2

                        if (partyid == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "更新缺少必要条件"
                            });
                        }
                        else
                        {
                            TempletState ts = (TempletState)JsonConvert.DeserializeObject(partysignwall, typeof(TempletState));

                            string querysql1 = @"select TempletUrl from TempletIndex where TempletID = '" + ts.templeteid.ToString() + "'";

                            DataSet queryresult1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, querysql1);

                            ts.url = queryresult1.Tables[0].Rows[0][0].ToString();

                            string ZHpartysignwall = JsonConvert.SerializeObject(ts);

                            string sql3 = @"update [PartyStateInfo]
                                            set [PartyID] = '" + partyid + @"'
                                            
                                            ,[PartySignWall] = '" + ZHpartysignwall + @"'
                                           
                                            ,[UpdateTime] = getdate()" +
                                            @"where partyid = '" + partyid + "'";
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql3);

                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "更新成功"
                            });
                        }


                    case "5"://更新3
                        if (partyid == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "更新缺少必要条件"
                            });
                        }
                        else
                        {
                            TempletState ts = (TempletState)JsonConvert.DeserializeObject(partystartwall, typeof(TempletState));

                            string querysql1 = @"select TempletUrl from TempletIndex where TempletID = '" + ts.templeteid.ToString() + "'";

                            DataSet queryresult1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, querysql1);

                            ts.url = queryresult1.Tables[0].Rows[0][0].ToString();

                            string ZHpartystartwall = JsonConvert.SerializeObject(ts);

                            string sql3 = @"update [PartyStateInfo]
                                            set [PartyID] = '" + partyid + @"'
                                            
                                              ,[PartyStartWall] = '" + ZHpartystartwall + @"'
                                           
                                            ,[UpdateTime] = getdate()" +
                                            @"where partyid = '" + partyid + "'";
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql3);

                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "更新成功"
                            });
                        }

                    case "6"://更新3
                        if (partyid == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "更新缺少必要条件"
                            });
                        }
                        else
                        {

                            //string templetid = partytextwall;

                            //var ser = new DataContractJsonSerializer(typeof(TempletState));

                            //var ms = new MemoryStream(Encoding.UTF8.GetBytes(partytextwall));

                            TempletState ts = (TempletState)JsonConvert.DeserializeObject(partytextwall, typeof(TempletState));

                            string querysql1 = @"select TempletUrl from TempletIndex where TempletID = '" + ts.templeteid.ToString() + "'";

                            DataSet queryresult1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, querysql1);

                            ts.url = queryresult1.Tables[0].Rows[0][0].ToString();

                            string ZHpartytextwall = JsonConvert.SerializeObject(ts);

                            string sql3 = @"update [PartyStateInfo]
                                            set [PartyID] = '" + partyid + @"'
                                            
                                              ,[PartyTextWall] = '" + ZHpartytextwall + @"'
                                           
                                            ,[UpdateTime] = getdate()" +
                                            @"where partyid = '" + partyid + "'";
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql3);

                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "更新成功"
                            });
                        }

                    case "7"://更新3
                        if (partyid == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "更新缺少必要条件"
                            });
                        }
                        else
                        {

                            TempletState ts = (TempletState)JsonConvert.DeserializeObject(partypicwall, typeof(TempletState));

                            string querysql1 = @"select TempletUrl from TempletIndex where TempletID = '" + ts.templeteid.ToString() + "'";

                            DataSet queryresult1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, querysql1);

                            ts.url = queryresult1.Tables[0].Rows[0][0].ToString();

                            string ZHpartypicwall = JsonConvert.SerializeObject(ts);

                            string sql3 = @"update [PartyStateInfo]
                                            set [PartyID] = '" + partyid + @"'
                                            
                                              ,[PartyPicWall] = '" + ZHpartypicwall + @"'
                                           
                                            ,[UpdateTime] = getdate()" +
                                            @"where partyid = '" + partyid + "'";
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql3);

                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "更新成功"
                            });
                        }

                    case "8"://更新3
                        if (partyid == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "更新缺少必要条件"
                            });
                        }
                        else
                        {

                            TempletState ts = (TempletState)JsonConvert.DeserializeObject(partydanwall, typeof(TempletState));

                            string querysql1 = @"select TempletUrl from TempletIndex where TempletID = '" + ts.templeteid.ToString() + "'";

                            DataSet queryresult1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, querysql1);

                            ts.url = queryresult1.Tables[0].Rows[0][0].ToString();

                            string ZHpartydanwall = JsonConvert.SerializeObject(ts);

                            string sql3 = @"update [PartyStateInfo]
                                            set [PartyID] = '" + partyid + @"'
                                            
                                              ,[PartyDanWall] = '" + ZHpartydanwall + @"'
                                           
                                            ,[UpdateTime] = getdate()" +
                                            @"where partyid = '" + partyid + "'";
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql3);

                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "更新成功"
                            });
                        }

                    case "9"://更新3
                        if (partyid == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "更新缺少必要条件"
                            });
                        }
                        else
                        {
                            TempletState ts = (TempletState)JsonConvert.DeserializeObject(partyweddingwall, typeof(TempletState));

                            string querysql1 = @"select TempletUrl from TempletIndex where TempletID = '" + ts.templeteid.ToString() + "'";

                            DataSet queryresult1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, querysql1);

                            ts.url = queryresult1.Tables[0].Rows[0][0].ToString();

                            string ZHpartyweddingwall = JsonConvert.SerializeObject(ts);


                            string sql3 = @"update [PartyStateInfo]
                                            set [PartyID] = '" + partyid + @"'
                                            
                                              ,[PartyWeddingWall] = '" + ZHpartyweddingwall + @"'
                                           
                                            ,[UpdateTime] = getdate()" +
                                            @"where partyid = '" + partyid + "'";
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql3);

                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "更新成功"
                            });
                        }

                    default:
                        return this.Jsonp(new
                        {
                            ErrorCode = 0x02,
                            ErrMsg = "非法的操作类型代码"
                        });



                }

            }
            catch (Exception ex)
            {
                return this.Jsonp(new { ErrorCode = 0x01, ErrMsg = ex.Message });
            }
        }

        //场控信息
        //1为查询 3为更新
        public JsonpResult ControlOperation(string operationtype, string partyid, string startstate, string screenstate, string danmakustate, string lotterystate, string state)
        {
            Common.AddResponseHeader();
            try
            {
                //if(operationtype == "1")
                switch (operationtype)
                {
                    case "1"://查询

                        string[] splitpartyid = partyid.Split(new Char[] { ',' });
                        string querypartyid = "";

                        for (int i = 0; i < splitpartyid.Length; i++)
                        {
                            querypartyid += "'" + splitpartyid[i] + "',";
                        }

                        querypartyid = querypartyid.Substring(0, querypartyid.Length - 1);

                        string sql1 = @"select [PartyID]
      ,[StartState]
      ,[ScreenState]
      ,[DanmakuState]
      ,[LotteryState]
      ,[State]
                                              from [ControlTable] with(nolock) where PartyID in (" + querypartyid + ")";
                        DataSet result1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sql1);
                        var Control = DataTableToListModel<Control>.ConvertToModel(result1.Tables[0]);

                        return this.Jsonp(new
                        {
                            ErrorCode = 0x00,
                            Control
                        });


                    case "3"://更新
                        if (partyid == "" || startstate == "" || screenstate == "" || danmakustate == "" || lotterystate == "" || state == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "更新缺少必要条件"
                            });
                        }
                        else
                        {
                            string sql3 = @"update [ControlTable]
                                            set [PartyID] = '" + partyid + @"'
                                            ,[StartState] = " + startstate + @"
                                            ,[ScreenState] = " + screenstate + @"
                                            ,[DanmakuState] = " + danmakustate + @"
                                            ,[LotteryState] = " + lotterystate + @"
,[State] = " + state + @"

                                            ,[UpdateTime] = getdate()" +
                                            @"where partyid = '" + partyid + "'";
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql3);

                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "更新成功"
                            });
                        }

                    default:
                        return this.Jsonp(new
                        {
                            ErrorCode = 0x02,
                            ErrMsg = "非法的操作类型代码"
                        });



                }

            }
            catch (Exception ex)
            {
                return this.Jsonp(new { ErrorCode = 0x01, ErrMsg = ex.Message });
            }

        }

        //模块信息
        public JsonpResult ModuleOperation(string operationtype, string moduleid, string modulename, string moduleurl1, string moduleurl2, string moduleurl3, string state)
        {
            Common.AddResponseHeader();
            try
            {
                //if(operationtype == "1")
                switch (operationtype)
                {
                    case "1"://查询

                        //string[] splitpartyid = moduleid.Split(new Char[] { ',' });
                        //string querypartyid = "";

                        //for (int i = 0; i < splitpartyid.Length; i++)
                        //{
                        //    querypartyid += "'" + splitpartyid[i] + "',";
                        //}

                        //querypartyid = querypartyid.Substring(0, querypartyid.Length - 1);

                        string sql1 = @"select [ModuleID]
      ,[ModuleName]
      ,[ModulePic]
      ,[ModuleUrl1]
      ,[ModuleUrl2]
      ,[ModuleUrl3]
      ,[State]
                                              from [ModuleIndex] with(nolock) ";
                        DataSet result1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sql1);
                        var Module = DataTableToListModel<Module>.ConvertToModel(result1.Tables[0]);




                        return this.Jsonp(new
                        {
                            ErrorCode = 0x00,
                            Module
                        });




                    default:
                        return this.Jsonp(new
                        {
                            ErrorCode = 0x02,
                            ErrMsg = "非法的操作类型代码"
                        });



                }

            }
            catch (Exception ex)
            {
                return this.Jsonp(new { ErrorCode = 0x01, ErrMsg = ex.Message });
            }

        }

        //模版信息
        public JsonpResult TempletOperation(string operationtype, string moduleid, string templetid, string templetname, string templetpic, string templeturl, string templetpra, string state)
        {
            Common.AddResponseHeader();
            try
            {
                //if(operationtype == "1")
                switch (operationtype)
                {
                    case "1"://查询

                        string[] splitpartyid = moduleid.Split(new Char[] { ',' });
                        string querypartyid = "";

                        for (int i = 0; i < splitpartyid.Length; i++)
                        {
                            querypartyid += "'" + splitpartyid[i] + "',";
                        }

                        querypartyid = querypartyid.Substring(0, querypartyid.Length - 1);

                        string sql1 = @"select [ModuleID]
      ,[TempletID]
      ,[TempletName]
      ,[TempletPic]
,[TempletUrl]
      ,[TempletPra]
      ,[CreateTime]
      ,[UpdateTime]
      ,[State],[PreviewUrl],[BackGroudFlag]
                                              from [TempletIndex] with(nolock) where ModuleID in (" + querypartyid + ")";
                        DataSet result1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sql1);
                        var Templet = DataTableToListModel<Templet>.ConvertToModel(result1.Tables[0]);

                        return this.Jsonp(new
                        {
                            ErrorCode = 0x00,
                            Templet
                        });




                    default:
                        return this.Jsonp(new
                        {
                            ErrorCode = 0x02,
                            ErrMsg = "非法的操作类型代码"
                        });



                }

            }
            catch (Exception ex)
            {
                return this.Jsonp(new { ErrorCode = 0x01, ErrMsg = ex.Message });
            }

        }

        //文字墙接口
        public JsonpResult TextWallOperation(string operationtype, string id)
        {

            Common.AddResponseHeader();
            try
            {
                switch (operationtype)
                {
                    case "1"://查询

                        string[] splitpartyid = id.Split(new Char[] { ',' });
                        string querypartyid = "";

                        for (int i = 0; i < splitpartyid.Length; i++)
                        {
                            querypartyid += "'" + splitpartyid[i] + "',";
                        }

                        querypartyid = querypartyid.Substring(0, querypartyid.Length - 1);

                        string sql1 = @"select  b.[OpenID]
      ,b.[NickName]
      ,b.[HeadUrl]
      ,b.[SceneID]
      ,convert(varchar(30),a.[Rowtime],121) as rowtime,a.sendmessage,
a.id from PartyWallText a,
UserSign b 
where a.status =1 and b.status = 1 and b.ParticipantState = 1 and a.openid=b.openid and a.SceneID=b.SceneID
 and a.sceneid='" + id + "' order by rowtime desc";
                        DataSet result1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sql1);

                        string sqlupdate = @"update PartyWallText set DisplayCount = DisplayCount+1 where sceneid ='"+id+"'";

                        SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString,sqlupdate);

                        var Templet = DataTableToListModel<TextWall>.ConvertToModel(result1.Tables[0]);

                        return this.Jsonp(new
                        {
                            ErrorCode = 0x00,
                            Templet
                        });

                    case "2"://新增

                        string[] splitpartyid1 = id.Split(new Char[] { ',' });
                        string querypartyid1 = "";

                        for (int i = 0; i < splitpartyid1.Length; i++)
                        {
                            querypartyid1 += "'" + splitpartyid1[i] + "',";
                        }

                        querypartyid1 = querypartyid1.Substring(0, querypartyid1.Length - 1);

                        string sql2 = @"select top 12 b.[OpenID]
      ,b.[NickName]
      ,b.[HeadUrl]
      ,b.[SceneID]
      ,convert(varchar(30),a.[Rowtime],121) as rowtime,a.sendmessage,
a.id from PartyWallText a,
UserSign b 
where a.status =1 and b.status = 1 and b.ParticipantState = 1 and a.openid=b.openid and a.SceneID=b.SceneID
 and a.sceneid='" + id + "' order by displaycount asc,rowtime desc";
                        DataSet result2 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sql2);

                        string tid = "";

                        for (int j = 0; j < result2.Tables[0].Rows.Count; j++)
                        {
                            tid += "'" + result2.Tables[0].Rows[j][6].ToString() + "',";

                        }

                        string sqlupdateAdd = @"update PartyWallText set DisplayCount = DisplayCount+1 where id in (" + tid.Substring(0, tid.Length - 1) + ")";

                        SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sqlupdateAdd);


                        var Templet1 = DataTableToListModel<TextWall>.ConvertToModel(result2.Tables[0]);

                        return this.Jsonp(new
                        {
                            ErrorCode = 0x00,
                            Templet1
                        });



                    default:
                        return this.Jsonp(new
                        {
                            ErrorCode = 0x02,
                            ErrMsg = "非法的操作类型代码"
                        });



                }
            }
            catch (Exception ex)
            {
                return this.Jsonp(new { ErrorCode = 0x01, ErrMsg = ex.Message });
            }

        }

        //照片墙接口
        public JsonpResult PicWallOperation(string operationtype, string id)
        {

            Common.AddResponseHeader();

            

            try
            {
                switch (operationtype)
                {
                    case "1"://查询

                        string[] splitpartyid = id.Split(new Char[] { ',' });
                        string querypartyid = "";

                        for (int i = 0; i < splitpartyid.Length; i++)
                        {
                            querypartyid += "'" + splitpartyid[i] + "',";
                        }

                        querypartyid = querypartyid.Substring(0, querypartyid.Length - 1);

                        string sql1 = @"select  b.[OpenID]
      ,b.[NickName]
      ,b.[HeadUrl]
      ,b.[SceneID]
      ,convert(varchar(30),a.[Rowtime],121) as rowtime,a.picType,
      a.Url,a.Alt,
a.id from PartyWallImage a,
UserSign b 
where a.status =1 and b.status = 1 and b.ParticipantState = 1 and a.openid=b.openid and a.SceneID=b.SceneID and Url <> 'http://7xqyc8.com1.z0.glb.clouddn.com/FncNeS8C0jK7bl_Y6aYn8-u2h-C2'
 and a.sceneid='" + id + "' order by rowtime desc";
                        DataSet result1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sql1);

                        string sqlupdate = @"update PartyWallImage set DisplayCount = DisplayCount+1 where sceneid ='" + id + "'";

                        SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sqlupdate);

                        var Templet = DataTableToListModel<PicWall>.ConvertToModel(result1.Tables[0]);

                        return this.Jsonp(new
                        {
                            ErrorCode = 0x00,
                            Templet
                        });

                    case "2"://新增

                        string[] splitpartyid1 = id.Split(new Char[] { ',' });
                        string querypartyid1 = "";

                        for (int i = 0; i < splitpartyid1.Length; i++)
                        {
                            querypartyid1 += "'" + splitpartyid1[i] + "',";
                        }

                        querypartyid1 = querypartyid1.Substring(0, querypartyid1.Length - 1);

                        string sql2 = @"select top 16 b.[OpenID]
      ,b.[NickName]
      ,b.[HeadUrl]
      ,b.[SceneID]
      ,convert(varchar(30),a.[Rowtime],121) as rowtime,a.picType,
      a.Url,a.Alt,
a.id from PartyWallImage a,
UserSign b 
where a.status =1 and b.status = 1 and b.ParticipantState = 1 and a.openid=b.openid and a.SceneID=b.SceneID and Url <> 'http://7xqyc8.com1.z0.glb.clouddn.com/FncNeS8C0jK7bl_Y6aYn8-u2h-C2'
 and a.sceneid='" + id + "' order by displaycount asc,rowtime desc";
                        DataSet result2 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sql2);

                        string tid = "";

                        for (int j = 0; j < result2.Tables[0].Rows.Count; j++)
                        {
                            tid +=  result2.Tables[0].Rows[j][8].ToString() + " ,";

                        }

                        string sqlupdateAdd = @"update PartyWallImage set DisplayCount = DisplayCount+1 where id in (" + tid.Substring(0, tid.Length - 1) + ")";

                        SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sqlupdateAdd);


                        var Templet1 = DataTableToListModel<PicWall>.ConvertToModel(result2.Tables[0]);

                        return this.Jsonp(new
                        {
                            ErrorCode = 0x00,
                            Templet1
                        });



                    default:
                        return this.Jsonp(new
                        {
                            ErrorCode = 0x02,
                            ErrMsg = "非法的操作类型代码"
                        });



                }
            }
            catch (Exception ex)
            {
                return this.Jsonp(new { ErrorCode = 0x01, ErrMsg = ex.Message });
            }

        }


        //签到墙
        public JsonpResult SignWallOperation(string operationtype, string id)
        {

            Common.AddResponseHeader();
            try
            {
                switch (operationtype)
                {
                    case "1"://查询

                        string[] splitpartyid = id.Split(new Char[] { ',' });
                        string querypartyid = "";

                        for (int i = 0; i < splitpartyid.Length; i++)
                        {
                            querypartyid += "'" + splitpartyid[i] + "',";
                        }

                        querypartyid = querypartyid.Substring(0, querypartyid.Length - 1);



                        string sql1 = @"select  [OpenID]
      ,[NickName]
      ,[HeadUrl]
      ,[SceneID],
convert(varchar(30),[Rowtime],121) as rowtime from dbo.UserSign where status=1 and SceneID = '" + id + "' order by Rowtime desc";

                        DataSet result1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sql1);

                        string oid = "";

                        for (int j = 0; j < result1.Tables[0].Rows.Count; j++)
                        {
                            oid += "'" + result1.Tables[0].Rows[j][0].ToString() + "',";

                        }



                        string sql3 = @"update UserSign with(rowlock)
      set state=2 where SceneID = '" + id + "' and openid in (" + oid.Substring(0, oid.Length - 1) + ")";


                        SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql3);


                        var Signwall = DataTableToListModel<SignWall>.ConvertToModel(result1.Tables[0]);

                        return this.Jsonp(new
                        {
                            ErrorCode = 0x00,
                            Signwall
                        });

                    case "2"://新增

                        string[] splitpartyid1 = id.Split(new Char[] { ',' });
                        string querypartyid1 = "";

                        for (int i = 0; i < splitpartyid1.Length; i++)
                        {
                            querypartyid1 += "'" + splitpartyid1[i] + "',";
                        }

                        querypartyid1 = querypartyid1.Substring(0, querypartyid1.Length - 1);

                        string sql2 = @"select top 1 [OpenID]
      ,[NickName]
      ,[HeadUrl]
      ,[SceneID],
convert(varchar(30),[Rowtime],121) as rowtime from dbo.UserSign where status=1 and state =1 and SceneID = '" + id + "' order by Rowtime desc";
                        DataSet result2 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sql2);

                        string sql4 = @"update UserSign with(rowlock)
      set state=2 where SceneID = '" + id + "' and OpenID='" + result2.Tables[0].Rows[0][0].ToString() + "'";


                        SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql4);

                        var Signwall1 = DataTableToListModel<SignWall>.ConvertToModel(result2.Tables[0]);

                        return this.Jsonp(new
                        {
                            ErrorCode = 0x00,
                            Signwall1
                        });

                    case "3"://更新



                        string sql5 = @"update UserSign with(rowlock)
      set state=1 where SceneID = '" + id + "'";


                        SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql5);



                        return this.Jsonp(new
                        {
                            ErrorCode = 0x00,
                            ErrMsg = "更新成功"
                        });


                    default:
                        return this.Jsonp(new
                        {
                            ErrorCode = 0x02,
                            ErrMsg = "非法的操作类型代码"
                        });



                }
            }
            catch (Exception ex)
            {
                return this.Jsonp(new { ErrorCode = 0x01, ErrMsg = ex.Message });
            }

        }

        //婚纱照存储
        public JsonpResult WeddingPicOperation(string operationtype, string id, string partyid, string picurl, string orderflag, string picstate)
        {
            Common.AddResponseHeader();
            try
            {
                switch (operationtype)
                {
                    case "1"://查询

                        string[] splitpartyid = partyid.Split(new Char[] { ',' });
                        string querypartyid = "";

                        for (int i = 0; i < splitpartyid.Length; i++)
                        {
                            querypartyid += "'" + splitpartyid[i] + "',";
                        }

                        querypartyid = querypartyid.Substring(0, querypartyid.Length - 1);

                        string sql1 = @"select [ID],[PartyID]
      ,[PicUrl]
      ,[OrderFlag]
      ,[PicState]
                                              from [WeddingPic] with(nolock) where PartyID in (" + querypartyid + ")";
                        DataSet result1 = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sql1);
                        var Weddingpic = DataTableToListModel<WeddingPic>.ConvertToModel(result1.Tables[0]);

                        return this.Jsonp(new
                        {
                            ErrorCode = 0x00,
                            Weddingpic
                        });



                    case "2"://插入
                        if (partyid == "" || picurl == "" || orderflag == "" || picstate == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "插入字段不能为空"
                            });
                        }
                        else
                        {
                            string sql2 = @"INSERT INTO [WeddingPic] with(rowlock)
                                            ([PartyID]
      ,[PicUrl]
      ,[OrderFlag]
      ,[PicState]
      ,[CreateTime])
                                            VALUES ('" + partyid + "','" + picurl + "','" + orderflag + "'," + picstate + ","
                                             + @"getdate()); ";
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql2);



                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "插入成功"
                            });
                        }

                    case "3"://更新
                        if (partyid == "" || picurl == "" || orderflag == "" || picstate == "")
                        {
                            return this.Jsonp(new
                            {
                                ErrorCode = 0x02,
                                ErrMsg = "更新缺少必要条件"
                            });
                        }
                        else
                        {
                            string sql3 = @"update [WeddingPic]
                                            set [PartyID] = '" + partyid + @"'
                                            ,[PicUrl] = '" + picurl + @"'
                                            ,[OrderFlag] = '" + orderflag + @"'
                                            ,[PicState] = '" + picstate + @"'
                                            
                                            ,[UpdateTime] = getdate()" +
                                            @"where id = '" + id + "'";
                            SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sql3);

                            return this.Jsonp(new
                            {
                                ErrorCode = 0x00,
                                ErrMsg = "更新成功"
                            });
                        }

                    default:
                        return this.Jsonp(new
                        {
                            ErrorCode = 0x02,
                            ErrMsg = "非法的操作类型代码"
                        });
                }
            }
            catch (Exception ex)
            {
                return this.Jsonp(new { ErrorCode = 0x01, ErrMsg = ex.Message });
            }
        }


        public static string Reverse(string text)
        {
            char[] cArray = text.ToCharArray();
            StringBuilder reverse = new StringBuilder();
            for (int i = cArray.Length - 1; i > -1; i--)
            {
                reverse.Append(cArray[i]);

            }
            return reverse.ToString();
        }





    }




    #region DataModelClass

    //用户信息
    class UserInfo
    {
        public string userid { get; set; }

        public string username { get; set; }

        public string usermobile { get; set; }

        public string userpwd { get; set; }

    }

    //用户活动关系

    class UserPartyRelation

    {
        public string userid { get; set; }

        public string partyid { get; set; }

        public int state { get; set; }

    }

    //活动基础信息
    class PartyBaseInfo
    {
        public string partyid { get; set; }

        public string partyschema { get; set; }

        public string channel { get; set; }

        public string serviceaccount { get; set; }

        public string partytime { get; set; }

        public string partyaddress { get; set; }

        public int partyheadcount { get; set; }

        public int partylimitcount { get; set; }

        public string partylinkman { get; set; }

        public string mobile { get; set; }

        public string partyinvitecode { get; set; }

        public int partystate { get; set; }

        public string partywxid { get; set; }

        public string partyewcode { get; set; }

        public string partycontrolcode { get; set; }



    }

    //与会者信息
    class Participant
    {
        public string partyid { get; set; }

        public string participantid { get; set; }

        public string participantname { get; set; }

        public string participantpic { get; set; }

        public int participantstate { get; set; }

        public string sceneid { get; set; }


    }

    //活动状态信息
    class PartyStateInfo
    {
        public string partyid { get; set; }

        public int partysigncount { get; set; }

        public string partysignwall { get; set; }
        public string partystartwall { get; set; }
        public string partytextwall { get; set; }
        public string partypicwall { get; set; }

        public string partydanwall { get; set; }

        public string partyweddingwall { get; set; }

    }

    //场控信息
    class Control
    {
        public string partyid { get; set; }
        public int startstate { get; set; }
        public int screenstate { get; set; }

        public int danmakustate { get; set; }
        public int lotterystate { get; set; }
        public int state { get; set; }
    }

    //模块信息
    class Module
    {
        public string moduleid { get; set; }
        public string modulename { get; set; }
        public string modulepic { get; set; }
        public string moduleurl1 { get; set; }

        public string moduleurl2 { get; set; }

        public string moduleurl3 { get; set; }

        public int state { get; set; }


    }

    //模板信息
    class Templet
    {
        public string moduleid { get; set; }

        public string templetid { get; set; }

        public string templetname { get; set; }
        public string templetpic { get; set; }

        public string templeturl { get; set; }
        public string templetpra { get; set; }

        public int state { get; set; }

        public string previewurl { get; set; }

        public int backgroudflag { get; set; }


    }

    class TextWall
    {


        public string openid { get; set; }
        public string nickname { get; set; }

        public string headurl { get; set; }

        public string sceneid { get; set; }

        public string rowtime { get; set; }

        public string sendmessage { get; set; }

        public string id { get; set; }



    }

    class SignWall
    {


        public string openid { get; set; }
        public string nickname { get; set; }

        public string headurl { get; set; }

        public string sceneid { get; set; }

        public string rowtime { get; set; }





    }

    class WeddingPic
    {

        public Int64 id { get; set; }
        public string partyid { get; set; }
        public string picurl { get; set; }
        public int orderflag { get; set; }
        public int picstate { get; set; }

    }

    class TempletState
    {

        public string templeteimg { get; set; }

        public string templeteid { get; set; }

        public string url { get; set; }

    }

    class PicWall
    {
        public string openid { get; set; }

        public string nickname { get; set; }

        public string headurl { get; set; }

        public string sceneid { get; set; }

        public string rowtime { get; set; }

        public int pictype { get; set; }

        public string url { get; set; }

        public string alt { get; set; }


        public Int64 id { get; set; }

    }

    #endregion

}