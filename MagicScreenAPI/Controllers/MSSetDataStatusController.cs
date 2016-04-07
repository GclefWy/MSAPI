using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace MagicScreenAPI.Controllers
{
    public class MSSetDataStatusController : Controller
    {
        // GET: MSSetDataStatus
        public JsonpResult DefaultSet(string partyid,string status)
        {

            try
            {
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
                    string sqlquery = string.Format(@"select id,partyid from partybasicinfo where partyid = '{0}'",partyid);
                    DataSet resultquery = SimpleDataHelper.Query(SimpleDataHelper.MSConnectionString, sqlquery);

                    //Update UserSign
                    string sqlupdateUserSign = string.Format(@"update UserSign set status = {0} where sceneid = '{1}'", status ,resultquery.Tables[0].Rows[0][0]);
                    SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sqlupdateUserSign);

                    //Update PartyWallImage
                    string sqlupdatePartyWallImage = string.Format(@"update PartyWallImage set status = {0} where sceneid = '{1}'", status, resultquery.Tables[0].Rows[0][0]);
                    SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sqlupdatePartyWallImage);

                    //Update PartyWallText
                    string sqlupdatePartyWallText = string.Format(@"update PartyWallText set status = {0} where sceneid = '{1}'", status, resultquery.Tables[0].Rows[0][0]);
                    SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sqlupdatePartyWallText);

                    //Update ParticipantTable
                    string sqlupdateParticipantTable = string.Format(@"update ParticipantTable set status = {0} where sceneid = '{1}'", status, resultquery.Tables[0].Rows[0][0]);
                    SimpleDataHelper.Excsql(SimpleDataHelper.MSConnectionString, sqlupdateParticipantTable);


                    return this.Jsonp(new
                    {
                        ErrorCode = 0x00,
                        ErrMsg = "更新成功"
                    });
                }

            }
            catch(Exception ex)
            {
                return this.Jsonp(new { ErrorCode = 0x01, ErrMsg = ex.Message });
            }
        }
    }
}