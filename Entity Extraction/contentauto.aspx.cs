using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ektron.Cms.PageBuilder;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

using Ektron.Cms.Common;
using Ektron.Cms;
using Ektron.Cms.API;
using Ektron.Cms.Content;
using Ektron.Cms.Framework.Content;
using Ektron.Cms.Search;
using Ektron.Cms.Search.Expressions;
using Ektron.Cms.Search.Solr;

using ContentTypes;
using ContentTypes.Attraction;

public partial class Content : PageBuilder
{
    protected ContentType<Attraction> attractionItem;
    protected ContentTypeManager<Attraction> contentTypeManager;

    protected override void InitializeCulture()
    {
        base.InitializeCulture();

        switch (Ektron.Cms.CommonApi.Current.ContentLanguage)
        {
            case 1036:
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fr-FR");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("fr-FR");
                break;
            case 1033:
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                break;
            case 2057:
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-GB");
                break;
            case 1043:
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("nl-be");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("nl-be");
                break;
            case 1041:
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ja-JP");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ja-JP");
                break;
            case 14337:
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ar-AE");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ar-AE");
                break;
            default:
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-GB");
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-GB");
                break;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ContentManager contentManager = new ContentManager();
        ContentData cData = new ContentData();

        long id = 0;
        try
        {
            long.TryParse(Request["id"].ToString(), out id);
        }
        catch
        {
            id = 0;
        }

        cData = contentManager.GetItem(id, true);

        ExtractEntities contentEnt = new ExtractEntities();
        Extracted.Entities.RootObject entResults = contentEnt.init(cData.Html);
        List<Extracted.Entities.Entity> top2People = getRelevantContent(2, "Person", entResults);
        List<Extracted.Entities.Entity> top2Org = getRelevantContent(2, "Organization", entResults);
        QueryParams parm = new QueryParams();
        

        // Populate related content based on most relevant entity       
        IEnumerable<QueryResult> results = populateRelatedContent(id, parm, top2People[0].text, cData.Id, 5);
        morethreeList.DataSource = results;
        morethreeList.DataBind();
        morethreeList.Visible = true;
        if (results.Count() > 0)
        {
            morethree.Text = "<h4>More on&nbsp;" + top2People[0].text  + "</h4>";
            morethree.Visible = true;
        }

        // Populate related content based on most relevant entity       
        IEnumerable<QueryResult> results2 = populateRelatedContent(id, parm, top2People[1].text, cData.Id, 3);
        morefourList.DataSource = results2;
        morefourList.DataBind();
        morefourList.Visible = true;
        if (results2.Count() > 0)
        {
            morefour.Text = "<h4>More on&nbsp;" + top2People[1].text  + "</h4>";
            morefour.Visible = true;
        }

        // Populate related content based on second most relevant entity
        IEnumerable<QueryResult> results3 = populateRelatedContent(id, parm, top2Org[0].text, cData.Id, 3);
        moreoneList.DataSource = results3;
        moreoneList.DataBind();
        moreoneList.Visible = true;
        if (results3.Count() > 0)
        {
            moreone.Text = "<h4>More on&nbsp;" + top2Org[0].text + "</h4>";
            moreone.Visible = true;
        }

        // Populate related content based on second most relevant entity
        IEnumerable<QueryResult> results4 = populateRelatedContent(id, parm, top2Org[1].text, cData.Id, 3);
        moretwoList.DataSource = results4;
        moretwoList.DataBind();
        moretwoList.Visible = true;
        if (results4.Count() > 0)
        {
            moretwo.Text = "<h4>More on&nbsp;" + top2Org[1].text + "</h4>";
            moretwo.Visible = true;
        }

        List<Related> people = getRelatedContent(top2People);
        peopleList.DataSource = people;
        peopleList.DataBind();
        List<Related> organizations = getRelatedContent(top2Org);
        orgList.DataSource = organizations;
        orgList.DataBind();

        string bannerhtml = createBannerHTML(cData);
        bannerImage.Text = bannerhtml;
        phoneHeading.Text = cData.Title;

        string merchandiseSearch = top2People[0].text + " " + top2People[1].text + " " + top2Org[0].text + " " + top2Org[1].text;
        try
        {
            List<ContentType<Attraction>> relatedMerchandise = populateMerchandise(merchandiseSearch, 4);
            if (relatedMerchandise.Count > 0)
            {
                merchandiseList.DataSource = relatedMerchandise;
                merchandiseList.DataBind();
                merchandiseHeader.Visible = true;
                merchandiseHeader.Text = "<h3 style=\"margin-top: 1em;\">RELATED MERCHANDISE</h3>";
            }
        }
        catch
        {
        }

    }
    public List<Related> getRelatedContent(List<Extracted.Entities.Entity> entityList)
    {
        List<Related> returnList = new List<Related>();
        foreach (Extracted.Entities.Entity related in entityList)
        {
            if (related.disambiguated != null)
            {
                Related newOne = new Related();
                newOne.name = related.text;
                if (related.disambiguated.dbpedia != null)
                {
                    newOne.websiteName = "External Website Link";
                    newOne.websiteLink = related.disambiguated.dbpedia;
                    newOne.websiteClass = "icon-share";
                    newOne.websiteShow = "Display: block;";
                }
                else
                {
                    newOne.websiteShow = "Display: none;";
                }
                newOne.searchLink = "/templates/Search.aspx?q=" + related.text;
                
                if (related.disambiguated.website != null)
                {
                    newOne.homepageLink = related.disambiguated.website;
                    newOne.homepageClass = "icon-home";
                    newOne.homepageName = "Homepage";
                    newOne.homepageShow = "Display: block;";
                }
                else
                {
                    newOne.homepageShow = "Display: none;";
                }
                returnList.Add(newOne);
            }
        }
        return returnList;
    }
    public List<Extracted.Entities.Entity> getRelevantContent(int amount, string entType, Extracted.Entities.RootObject results)
    {
        List<Extracted.Entities.Entity> qResult = new List<Extracted.Entities.Entity>();
        qResult = results.entities.Where(x => x.type == entType).ToList();
        return qResult;
    }
    public string createBannerHTML(ContentData cData)
    {

        string carouselImage = "";
        string carouselCaption = "";
        string carouselQuote = "";

        // Using old API to get metadata (need to update to framework API)
        Ektron.Cms.API.Metadata mapi = new Ektron.Cms.API.Metadata();
        Ektron.Cms.CustomAttributeList metadataList = new Ektron.Cms.CustomAttributeList();
        metadataList = mapi.GetContentMetadataList(cData.Id);

        if (metadataList["Landing Page Image"].Value != null)
        {
            carouselImage = metadataList["Landing Page Image"].Value.ToString();
        }
        if (metadataList["Carousel caption"].Value != null)
        {
            carouselCaption = metadataList["Carousel caption"].Value.ToString();
        }

        carouselQuote = cData.Teaser.ToString();
        string bannerHtml = "<div class='item active'>";
        bannerHtml += "<img src='" + carouselImage + "' alt='" + cData.Title + "' />";
        bannerHtml += "<div class='container'>";
        bannerHtml += "<div class='opaqueBg'>";
        bannerHtml += "<div class='carousel-caption hero-unit'>";
        bannerHtml += "	<span class='title'>" + cData.Title + "</span>&nbsp;";
        bannerHtml += "	<span class='lead'>" + carouselCaption + "</span>";
        bannerHtml += "	<div class='quote hidden-tablet hidden-phone'>" + carouselQuote + "</div><br/>";
        bannerHtml += "</div>";
        bannerHtml += "</div>";
        bannerHtml += "</div>";
        bannerHtml += "</div>";
        return bannerHtml;
    }

    private List<ContentType<Attraction>> populateMerchandise(string title, int paging)
    {
        //  IntegerPropertyExpression sfid = new IntegerPropertyExpression(smartFormID);
        //  id = current page id to exclude from query results
        //  parameters = current page query string
        //  title = title of current page
        //  contentBlockId = id of content block on current page to exclude from query results

        IEnumerable<QueryResult> results = new List<QueryResult>();
        KeywordSearchCriteria criteria = new KeywordSearchCriteria();
        criteria.QueryText = title;
        criteria.ImplicitAnd = false;
        criteria.OrderBy = new List<OrderData>()
                {
                    new OrderData(SearchContentProperty.Rank, OrderDirection.Descending)
                };

        criteria.ExpressionTree =
               QueryProperties.Id > 0 &
               QueryProperties.SFID.EqualTo(9);
        criteria.PagingInfo = new PagingInfo(paging);
        criteria.PagingInfo.CurrentPage = 1;
        criteria.ReturnProperties = new HashSet<PropertyExpression>()
                {
                    SearchContentProperty.Id,
                    SearchContentProperty.Title,
                    SearchContentProperty.QuickLink,
                    SearchContentProperty.Description
                };

        ISearchManager manager = ObjectFactory.GetSearchManager();

        SearchResponseData response = manager.Search(criteria);
        List<ContentType<Attraction>> merchandiseList = new List<ContentType<Attraction>>();
        if (response.Results.Count > 0)
        {
            results = GetResults(response);
            foreach (Content.QueryResult merchandise in results)
            {
                ContentTypeManager<Attraction> contentTypeManagerAttraction = new ContentTypeManager<Attraction>();
                ContentType<Attraction> cd = contentTypeManagerAttraction.GetItem(merchandise.Id);
                merchandiseList.Add(cd);
            }
        }
        return merchandiseList;
    }
    private IEnumerable<QueryResult> populateRelatedContent(long id, QueryParams parameters, string title, long contentBlockId, int paging)
    {
        //  IntegerPropertyExpression sfid = new IntegerPropertyExpression(smartFormID);
        //  id = current page id to exclude from query results
        //  parameters = current page query string
        //  title = title of current page
        //  contentBlockId = id of content block on current page to exclude from query results

        IEnumerable<QueryResult> results = new List<QueryResult>();
        KeywordSearchCriteria criteria = new KeywordSearchCriteria();
        criteria.QueryText = title;
        criteria.OrderBy = new List<OrderData>()
                {
                    new OrderData(SearchContentProperty.Rank, OrderDirection.Descending)
                };

        criteria.ExpressionTree =
               QueryProperties.Id > 0 &
               QueryProperties.Id != contentBlockId &
               QueryProperties.Id != id &
               QueryProperties.SFID.NotEqualTo(10) &
               QueryProperties.SFID.NotEqualTo(9);
        criteria.PagingInfo = new PagingInfo(paging);
        criteria.PagingInfo.CurrentPage = 1;
        criteria.ReturnProperties = new HashSet<PropertyExpression>()
                {
                    SearchContentProperty.Id,
                    SearchContentProperty.Title,
                    SearchContentProperty.QuickLink,
                    SearchContentProperty.Description
                };

        ISearchManager manager = ObjectFactory.GetSearchManager();

        SearchResponseData response = manager.Search(criteria);

        if (response.Results.Count > 0)
        {
            results = GetResults(response);
        }
        return results;
    }
    public override void Error(string message)
    {
        throw new NotImplementedException();
    }
    public override void Notify(string message)
    {
        throw new NotImplementedException();
    }
    private class QueryProperties
    {
        public static IntegerPropertyExpression Id { get { return SearchContentProperty.Id; } }

        public static IntegerPropertyExpression Language { get { return SearchContentProperty.Language; } }

        public static StringPropertyExpression Title { get { return SearchContentProperty.Title; } }

        public static StringPropertyExpression Summary { get { return SearchContentProperty.HighlightedSummary; } }

        public static StringPropertyExpression QuickLink { get { return SearchContentProperty.QuickLink; } }

        public static DatePropertyExpression DateModified { get { return SearchContentProperty.DateModified; } }

        public static IntegerPropertyExpression Size { get { return SearchContentProperty.Size; } }

        public static StringPropertyExpression Author
        {
            get
            {
                return SearchSolrProperty.CreateExactStringProperty(SearchContentProperty.Author);
            }
        }

        public static StringPropertyExpression Taxonomy
        {
            get
            {
                return SearchSolrProperty.CreateExactStringProperty(SearchContentProperty.TaxonomyCategory);
            }
        }

        public static StringPropertyExpression FolderIdPath { get { return SearchContentProperty.FolderIdPath; } }
        public static StringPropertyExpression FolderPath { get { return SearchContentProperty.FolderPath; } }
        public static IntegerPropertyExpression SFID { get { return SearchContentProperty.XmlConfigId; } }
    }
    public class QueryParams
    {
        public QueryParams()
        {
            this.LastModifiedRefiners = new List<string>();
            this.AuthorRefiners = new List<string>();
            this.TaxonomyRefiners = new List<string>();
            this.Language = 1033;
            this.Page = 0;
        }

        public QueryParams(NameValueCollection parameters)
        {
            this.Query = HttpUtility.UrlDecode(parameters["q"] ?? parameters["searchtext"] ?? string.Empty);

            int page;
            if (int.TryParse(parameters["pageid"], out page))
            {
                this.Page = page;
            }
            else
            {
                this.Page = 0;
            }

            int lang;
            if (int.TryParse(parameters["langid"], out lang))
            {
                this.Language = lang;
            }
            else
            {
                this.Language = 1033;
            }

            string[] lastModifiedRefiners = (parameters["modified"] ?? string.Empty).Split(
                new string[] { "," },
                StringSplitOptions.RemoveEmptyEntries);

            this.LastModifiedRefiners = new List<string>(from r in lastModifiedRefiners select HttpUtility.UrlDecode(r));

            string[] authorRefiners = (parameters["authors"] ?? string.Empty).Split(
                new string[] { "," },
                StringSplitOptions.RemoveEmptyEntries);

            this.AuthorRefiners = new List<string>(from r in authorRefiners select HttpUtility.UrlDecode(r));

            string[] taxonomyRefiners = (parameters["tax"] ?? string.Empty).Split(
                new string[] { "," },
                StringSplitOptions.RemoveEmptyEntries);

            this.TaxonomyRefiners = new List<string>(from r in taxonomyRefiners select HttpUtility.UrlDecode(r));

        }

        public string Query { get; set; }

        public int Page { get; set; }

        public int Language { get; set; }

        public List<string> LastModifiedRefiners { get; private set; }

        public List<string> AuthorRefiners { get; private set; }

        public List<string> TaxonomyRefiners { get; private set; }

        public override string ToString()
        {
            return string.Format(
                "q={0}&page={1}&modified={2}&authors={3}&tax={4}&langtype={5}",
                HttpUtility.UrlEncode(this.Query ?? string.Empty),
                this.Page,
                string.Join(",", from r in this.LastModifiedRefiners select HttpUtility.UrlEncode(r)),
                string.Join(",", from r in this.AuthorRefiners select HttpUtility.UrlEncode(r)),
                string.Join(",", from r in this.TaxonomyRefiners select HttpUtility.UrlEncode(r)),
                this.Language);
        }
    }
    private class QueryResult
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string QuickLink { get; set; }

        public string Description { get; set; }

    }
    private IEnumerable<QueryResult> GetResults(SearchResponseData response)
    {
        return from r in response.Results
               select new QueryResult()
               {
                   Id = r[SearchContentProperty.Id],
                   Title = r[SearchContentProperty.Title],
                   QuickLink = r[SearchContentProperty.QuickLink],
                   Description = r[SearchContentProperty.Description]
               };
    }
    public class ContentList
    {
        public string title { get; set; }
        public long cid { get; set; }
        public string contentHTML { get; set; }
    }
    public class Related
    {
        public string name { get; set; }
        public string websiteName { get; set; }
        public string websiteLink { get; set; }
        public string websiteShow { get; set; }
        public string searchLink { get; set; }
        public string websiteClass { get; set; }
        public string homepageLink { get; set; }
        public string homepageName { get; set; }
        public string homepageClass { get; set; }
        public string homepageShow { get; set; }
    }
}