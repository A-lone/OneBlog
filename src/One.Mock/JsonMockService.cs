using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace One.Mock
{
    public class JsonMockService
    {
        public object jsonToObject(string jsonstr, Type objectType)
        //传递两个参数，一个是json字符串，一个是要创建的对象的类型    
        {
            string[] jsons = jsonstr.Split(new char[] { ',' });//将json字符串分解成 “属性：值”数组         
            for (int i = 0; i < jsons.Length; i++)
            {
                jsons[i] = jsons[i].Replace("\"", "");
            }//去掉json字符串的双引号        
            object obj = System.Activator.CreateInstance(objectType); //tob_id_4294使用反射动态创建对象          
            PropertyInfo[] pis = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);//获得对象的所有public属性         
            if (pis != null)//如果获得了属性    
                foreach (PropertyInfo pi in pis)//针对每一个属性进行循环         
                {
                    for (int i = 0; i < jsons.Length; i++)//检查json字符串中的所有“属性：值”类表       
                    {
                        if (jsons[i].Split(new char[] { ':' })[0] == pi.Name)//如果对象的属性名称恰好和json中的属性名相同                
                        {
                            Type proertyType = pi.PropertyType; //获得对象属性的类型                
                            pi.SetValue(obj, Convert.ChangeType(jsons[i].Split(new char[] { ':' })[1], proertyType), null);
                            //将json字符串中的字符串类型的“值”转换为对象属性的类型，并赋值给对象属性           
                        }
                    }
                }
            return obj;
        }



        public static Object SetPropValue(Object source, String name, object value)
        {
            PropertyInfo result = null;
            object obj = source;
            object lasted = null;
            Dictionary<object, PropertyInfo> dict = new Dictionary<object, PropertyInfo>();
            foreach (String part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }
                dict.Add(obj, info);
                obj = info.GetValue(obj, null);
            }
            //Type proertyType = result.PropertyType; //获得对象属性的类型        

            for (int i = dict.Count - 1; i >= 0; i--)
            {
                var item = dict.ElementAt(i);
                item.Value.SetValue(item.Key, value, null);
                value = item.Key;
                obj = value;
            }

            return obj;
        }


        public static Object GetPropValue(Object obj, String name)
        {
            foreach (String part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }

            return obj;
        }

        public static T GetPropValue<T>(Object obj, String name)
        {
            Object retval = GetPropValue(obj, name);
            if (retval == null) { return default(T); }

            // throws InvalidCastException if types are incompatible
            return (T)retval;
        }

        internal static void Update(object obj, string fieldname, object value)
        {
            obj.GetType().GetField(fieldname).SetValue(obj, value);
            //Type info = typeof(T);
            //PropertyInfo[] propertys = info.GetProperties();

            //foreach (PropertyInfo pi in propertys)
            //{
            //    info.GetProperty(pi.Name).SetValue(target, pi.GetValue(source, null), null);
            //}
        }

        public static CookieContainer AddCookieToContainer(string cookie, CookieContainer cc, string domain)
        {
            try
            {
                string[] tempCookies = cookie.Split(';');
                string tempCookie = null;
                int Equallength = 0;//  =的位置 
                string cookieKey = null;
                string cookieValue = null;
                //qg.gome.com.cn  cookie 
                for (int i = 0; i < tempCookies.Length; i++)
                {
                    if (!string.IsNullOrEmpty(tempCookies[i]))
                    {
                        tempCookie = tempCookies[i];

                        Equallength = tempCookie.IndexOf("=");

                        if (Equallength != -1)       //有可能cookie 无=，就直接一个cookiename；比如:a=3;ck;abc=; 
                        {

                            cookieKey = tempCookie.Substring(0, Equallength).Trim();
                            //cookie=

                            if (Equallength == tempCookie.Length - 1)    //这种是等号后面无值，如：abc=; 
                            {
                                cookieValue = "";
                            }
                            else
                            {
                                cookieValue = tempCookie.Substring(Equallength + 1, tempCookie.Length - Equallength - 1).Trim();
                            }
                        }

                        else
                        {
                            cookieKey = tempCookie.Trim();
                            cookieValue = "";
                        }

                        cc.Add(new Uri("http://" + domain), new Cookie(cookieKey, cookieValue, "", domain));

                    }

                }

                return cc;
            }
            catch (Exception ex)
            {
                return cc;
            }
        }

    }

}
