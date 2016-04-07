using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace MagicScreenAPI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            // 默认情况下对 Entity Framework 使用 LocalDB
            //Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    "Default", // 路由名称
            //    "{controller}/{action}/{id}", // 带有参数的 URL
            //    new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 参数默认值
            //);

            MvcHelper helper = MvcHelper.Create("MagicScreenAPI.Controllers");
            //用户信息接口路径
            helper.MapRoute("api/useroperation/", "MSBasic", "UserOperation");
            //用户活动关系接口路径
            helper.MapRoute("api/relationoperation/", "MSBasic", "RelationOperation");
            //活动接口路径
            helper.MapRoute("api/partybaseoperation/", "MSBasic", "PartyBaseOperation");
            //与会者接口路径
            helper.MapRoute("api/participantoperation/", "MSBasic", "ParticipantOperation");
            //活动状态接口路径
            helper.MapRoute("api/partystateoperation/", "MSBasic", "PartyStateOperation");
            //场控接口路径
            helper.MapRoute("api/controloperation/", "MSBasic", "ControlOperation");
            //模块接口路径
            helper.MapRoute("api/moduleoperation/", "MSBasic", "ModuleOperation");
            //效果接口路径
            helper.MapRoute("api/templetoperation/", "MSBasic", "TempletOperation");
            //文字墙接口路径
            helper.MapRoute("api/textwalloperation/", "MSBasic", "TextWallOperation");
            //照片墙接口路径
            helper.MapRoute("api/picwalloperation/", "MSBasic", "PicWallOperation");

            //签到墙接口路径
            helper.MapRoute("api/signwalloperation/", "MSBasic", "SignWallOperation");

            //婚纱照墙接口路径
            helper.MapRoute("api/weddingpicoperation/", "MSBasic", "WeddingPicOperation");

            //数据初始化
            helper.MapRoute("api/defaultset/", "MSSetDataStatus", "DefaultSet");

        }


    }
}
