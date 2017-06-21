using One.Mock.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace One.Mock.Data
{
    public class ApplicationInitializer
    {

        private ApplicationContext _ctx;

        public ApplicationInitializer(ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        public async Task SeedAsync()
        {
            if (_ctx.Sites.Count() == 0)
            {
                var wenka = new Site()
                {
                    IsDefault = false,
                    Name = "测试问咖",
                    Url = "http://wenka.baidu.com",
                    Cookie = "BAIDUID=A2F037DE5E0E9A3BD2E500F819B12A18:FG=1; BIDUPSID=A2F037DE5E0E9A3BD2E500F819B12A18; PSTM=1484706230; BAIDUCUID=++; MCITY=-131%3A; BDUSS=mI4UWJVVENOaE03SmVuZE9CVXhhYk5uWkxIdFVjLXJ2M2tDdDJsVXd-ZU9UfmRZSUFBQUFBJCQAAAAAAAAAAAEAAACGv~YCY2hlbnJlbnNvbmcAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAI7Cz1iOws9Ya; SIGNIN_UC=70a2711cf1d3d9b1a82d2f87d633bd8a02400927066; BDORZ=B490B5EBF6F3CD402E515D22BCDA1598; PSINO=2; H_PS_PSSID=1459_21120_17001_22160; Hm_lvt_c40c737a91f5830c41317c23461cc655=1490014345,1490014511; Hm_lpvt_c40c737a91f5830c41317c23461cc655=1490099046"
                };
                _ctx.Sites.Add(wenka);
                _ctx.Sites.Add(new Site() { IsDefault = false, Name = "测试百度派", Url = "http://p.baidu.com" });

                var zm = new Site()
                {
                    IsDefault = true,
                    Name = "争鸣",
                    Url = "http://zhengming.baidu.com",
                    Cookie = "BAIDUID=A2F037DE5E0E9A3BD2E500F819B12A18:FG=1; BIDUPSID=A2F037DE5E0E9A3BD2E500F819B12A18; PSTM=1484706230; BAIDUCUID=++; MCITY=-131%3A; __cfduid=d5922d0fcaf3e9d2affd6ce18bf5ec7ba1490177397; BDUSS=BZQUo3STJhMGwyWVlWU2U4NmhGT1N1RVFuQjA3MHZWNk5TS1dHMDFTMUtLdnRZSVFBQUFBJCQAAAAAAAAAAAEAAACGv~YCY2hlbnJlbnNvbmcAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAEqd01hKndNYeT; SIGNIN_UC=70a2711cf1d3d9b1a82d2f87d633bd8a02403450655; PSINO=1; H_PS_PSSID=1459_21120_17001_22160; BDORZ=B490B5EBF6F3CD402E515D22BCDA1598; Hm_lvt_343114ed3f28fa493162c18db2e1371b=1490339976; Hm_lpvt_343114ed3f28fa493162c18db2e1371b=1490339986"
                };

      
                _ctx.Sites.Add(zm);
                _ctx.SitePaths.Add(NewMethod(zm));
                _ctx.SitePaths.Add(NewMethod1(zm));
                await _ctx.SaveChangesAsync();
            }
        }

        private static SitePath NewMethod(Site zm)
        {
            var sitepath = new SitePath();
            sitepath.Json = "{data:{crs:\"陈仁松测试\",cgi:{get:{appID:\"15\",debug:\"\"},post:[],request_param:{appID:\"15\",debug:\"\"}},user:{status:0,need_set_cookie:0,uid:49725318,uname:\"chenrensong\",displayname:\"chenrensong\",ltime:1490263370,utime:1490263370,atime:1490263370,acount:0,risk_rank:0,risk_code:0,gdata:null,pdata:\"\",encoding:\"gbk\",isIncomplete:0},errno:10203,data:{cms:false},errmsg:\"权限错误\"},conf:[]}";
            sitepath.Method = "get";
            sitepath.Query = "appID=15&debug";
            sitepath.Path = "/cornucopia/user/appreport";
            sitepath.DLL = "data.dll";
            sitepath.RequestEnabled = true;
            List<Expression> e = new List<Expression>();
            e.Add(new Expression() { Key = "data.crs", Value = "陈仁松测试17", IFKey = "data.cgi.get.appID", IFValue = "17" });
            e.Add(new Expression() { Key = "data.crs", Value = "陈仁松测试16", IFKey = "data.cgi.get.appID", IFValue = "16" });
            e.Add(new Expression() { Key = "data.crs", Value = "陈仁松测试15", IFKey = "data.cgi.get.appID", IFValue = "15" });
            var expression = Newtonsoft.Json.JsonConvert.SerializeObject(e);
            sitepath.Expression = expression;
            sitepath.Sites = zm;
            return sitepath;
        }

        private static SitePath NewMethod1(Site zm)
        {
            var sitepath = new SitePath();
            sitepath.Json = "{\"errno\":0,\"data\":{\"sum\":18,\"list\":[{\"qid\":186,\"createTime\":\"2017-03-24 16:27:23\",\"title\":\"\u53c8\u4e00\u660e\u661f\u5bb6\u4ea7\u906d\u5a07\u59bb\u638f\u7a7a\uff0c\u8001\u5c11\u914d\u4e3a\u4f55\u4e0d\u9760\u8c31\",\"status\":21,\"uid\":125498771,\"type\":\"\u6765\u81ea \u53d1\u5e03\u6587\u7ae0 \u7f16\u8f91\u5668\"},{\"qid\":185,\"createTime\":\"2017-03-24 16:26:17\",\"title\":\"\u5e1d\u90fd\u56db\u73af\u8fb9\u8d2d\u623f\u7684\u9996\u4ed8\u771f\u76f8\uff0c\u4e0d\u5403\u4e0d\u559d\u9700\u8981\u6512\u591a\u5c11\u5e74\",\"status\":21,\"uid\":125498771,\"type\":\"\u6765\u81ea \u53d1\u5e03\u6587\u7ae0 \u7f16\u8f91\u5668\"},{\"qid\":184,\"createTime\":\"2017-03-24 16:25:10\",\"title\":\"\",\"status\":2,\"uid\":125498771,\"type\":\"\u6765\u81ea \u672a\u77e5 \u7f16\u8f91\u5668\"},{\"qid\":183,\"createTime\":\"2017-03-24 16:24:21\",\"title\":\"\u5973\u4eba\u5a5a\u540e\u4f1a\u6539\u53d8\uff0c\u90fd\u662f\u7537\u4eba\u60f9\u7684\u7978\",\"status\":21,\"uid\":125498771,\"type\":\"\u6765\u81ea \u53d1\u5e03\u6587\u7ae0 \u7f16\u8f91\u5668\"},{\"qid\":182,\"createTime\":\"2017-03-24 16:22:33\",\"title\":\"\u65b0\u6b22\uff1f\u65e7\u7231\uff1f\u76f4\u5230\u6d41\u6cea\u7684\u90a3\u4e00\u523b\uff0c\u6211\u624d\u53d1\u73b0\u8fd8\u6709\u7b2c\u4e09\u79cd\u9009\u62e9\u2026\u2026\",\"status\":22,\"uid\":125498771,\"type\":\"\u6765\u81ea \u53d1\u5e03\u6587\u7ae0 \u7f16\u8f91\u5668\"},{\"qid\":16,\"createTime\":\"2017-03-24 14:19:37\",\"title\":\"\u5982\u4f55\u770b\u5230\u8fbe\u5185\u548c\u706b\u661f\u65f6\u4ee3\u4e24\u5bb6\u673a\u6784\",\"status\":21,\"uid\":1297457125,\"type\":\"\u6765\u81ea \u53d1\u5e03\u6587\u7ae0 \u7f16\u8f91\u5668\"},{\"qid\":14,\"createTime\":\"2017-03-23 18:44:15\",\"title\":\"\u51cf\u80a5\u7ecf\u9a8c\u5206\u4eab \u975e\u5e38\u5b9e\u7528\",\"status\":21,\"uid\":125498771,\"type\":\"\u6765\u81ea \u53d1\u5e03\u6587\u7ae0 \u7f16\u8f91\u5668\"},{\"qid\":12,\"createTime\":\"2017-03-22 16:09:50\",\"title\":\"\u8fd9\u4e2a\u662f\u6d4b\u8bd5\u6570\u636e-1\",\"status\":21,\"uid\":1297457125,\"type\":\"\u6765\u81ea \u53d1\u5e03\u6587\u7ae0 \u7f16\u8f91\u5668\"},{\"qid\":11,\"createTime\":\"2017-03-21 19:27:53\",\"title\":\"\u8fd9\u662f\u6d4b\u8bd5test\",\"status\":11,\"uid\":1297457125,\"type\":\"\u6765\u81ea \u53d1\u5e03\u6587\u7ae0 \u7f16\u8f91\u5668\"},{\"qid\":10,\"createTime\":\"2017-03-21 18:02:22\",\"title\":\"\u201c9-11\u201d\u6050\u6016\u88ad\u51fb\u9047\u96be\u8005\u5bb6\u5c5e\u96c6\u4f53\u8d77\u8bc9\u6c99\u7279\u653f\u5e9c\",\"status\":21,\"uid\":125498771,\"type\":\"\u6765\u81ea \u53d1\u5e03\u6587\u7ae0 \u7f16\u8f91\u5668\"}]},\"errmsg\":\"\u64cd\u4f5c\u6210\u529f\"}";
            sitepath.Method = "post";
            sitepath.Query = "";
            sitepath.Path = "/cornucopia/ajax/getadmincontents";
            sitepath.DLL = "data1.dll";
            sitepath.RequestEnabled = false;
            sitepath.Expression = "";
            sitepath.Sites = zm;
            return sitepath;
        }
    }
}
