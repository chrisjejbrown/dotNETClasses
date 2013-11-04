using System;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.IO;
using System.Text;
using Ektron.Cms;
using Ektron.Cms.Extensibility;
using Ektron.Cms.Common;
using Ektron.Cms.ObjectStrategies;
using Ektron.Cms.Extensibility;
using Ektron.Cms.Extensibility.Content;
using Ektron.Cms.API;
using Ektron.Cms.Content;
using Ektron.Cms.Framework.Content;

namespace Cms.MapURLs
{
    public class MapURLs : ContentStrategy
    {
        public override void OnBeforeAddContent(ContentData contentData, CmsEventArgs eventArgs)
        {
            if (contentData.FolderId == 163 || contentData.FolderId == 164)
            {
                SmartForm.URLmap.root URLMaps = getURLMaps();

                if (URLMaps.URL_Mappings.Length > 0 && contentData.Html != "")
                {
                    string newHTML = substituteURLs(contentData.Html, URLMaps);
                    contentData.Html = newHTML;

                }
            }
        }
        public override void OnBeforeUpdateContent(ContentData contentData, CmsEventArgs eventArgs)
        {
            if (contentData.FolderId == 163 || contentData.FolderId == 164)
            {
                SmartForm.URLmap.root URLMaps = getURLMaps();

                if (URLMaps.URL_Mappings.Length > 0 && contentData.Html != "")
                {
                    string newHTML = substituteURLs(contentData.Html, URLMaps);
                    contentData.Html = newHTML;
                }
            }
        }
        public string substituteURLs(string HTML, SmartForm.URLmap.root URLMaps)
        {
            // This loop resets all previous replacements so the next loop can re-do them all
            foreach (SmartForm.URLmap.rootURL_Mappings urlmap in URLMaps.URL_Mappings)
            {
                string searchTerm = urlmap.Term;
                string replaceWith = "<a class=\"NFLAutoLink\" target=\"_blank\" href=\"" + urlmap.MappedURL + "\">" + searchTerm + "</a>";
                HTML = HTML.Replace(replaceWith, searchTerm);
            }
            foreach (SmartForm.URLmap.rootURL_Mappings urlmap in URLMaps.URL_Mappings)
            {
                string searchTerm = urlmap.Term;
                string replaceWith = "<a class=\"NFLAutoLink\" target=\"_blank\" href=\"" + urlmap.MappedURL + "\">" + searchTerm + "</a>";
                HTML = HTML.Replace(searchTerm, replaceWith);
            }

            return HTML;
        }
        public SmartForm.URLmap.root getURLMaps()
        {
            ContentManager cm = new ContentManager();
            ContentData cd = new ContentData();
            cd = cm.GetItem(528);
            string xml = cd.Html;
            SmartForm.URLmap.root urlmap = (SmartForm.URLmap.root)Ektron.Cms.EkXml.Deserialize(typeof(SmartForm.URLmap.root), xml);
            return urlmap;
        }
    }
}