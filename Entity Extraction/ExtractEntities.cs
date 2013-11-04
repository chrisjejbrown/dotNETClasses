using System;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Linq;
using System.Runtime.Serialization.Json;


/// <summary>
/// Summary description for ExtractEntities
/// </summary>
public class ExtractEntities
{
    public Extracted.Entities.RootObject init(string HTML)
	{
        string callURL = createURL(HTML);
        string entityJSON = getEntities(callURL);
        Extracted.Entities.RootObject extEnt = Deserialize<Extracted.Entities.RootObject>(entityJSON);
        return extEnt;   
	}
    public string createURL(string text)
    {
        string url = "http://access.alchemyapi.com/calls/html/HTMLGetRankedNamedEntities?apikey=ae8100d6a9c813472822424342ac58247ad6af20";
        url += "&outputMode=json";
        url += "&html=" + HttpContext.Current.Server.UrlEncode(text);        
        return url;
    }
    public string getEntities(string url)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";
        try
        {
            WebResponse response = request.GetResponse();
            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }
        catch (WebException ex)
        {
            WebResponse errorResponse = ex.Response;
            using (Stream responseStream = errorResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                String errorText = reader.ReadToEnd();
                // log errorText
            }
            throw;
        }
    }
    
    public static string Serialize<T>(T obj)
    {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        MemoryStream ms = new MemoryStream();
        serializer.WriteObject(ms, obj);
        string retVal = Encoding.UTF8.GetString(ms.ToArray());
        return retVal;
    }

    public static T Deserialize<T>(string json)
    {
        T obj = Activator.CreateInstance<T>();
        MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
        obj = (T)serializer.ReadObject(ms);
        ms.Close();
        return obj;
    }
}