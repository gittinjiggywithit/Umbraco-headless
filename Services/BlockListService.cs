using System.Dynamic;
using System.Reflection;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.PublishedModels;
using Vertica.Umbraco.Headless.Core.Rendering;

public class BlockListService : IBlockListService
{
    private readonly IUmbracoContextAccessor _umbracoContextAcessor;

    public BlockListService(IUmbracoContextAccessor umbracoContextAcessor)
    {
        _umbracoContextAcessor = umbracoContextAcessor;
    }
    public object RenderArticlesBlock(BlockListItem<ArticlesBlock> articlesBlock, IContentElementBuilder contentElementBuilder)
    {
        var responses = new List<object>();

        var umbracoContext = _umbracoContextAcessor.GetRequiredUmbracoContext();

        var handled = contentElementBuilder.ContentElementWithSettingsFor(articlesBlock.Content, articlesBlock.Settings);

        foreach(var selectedArticle in articlesBlock.Content.Articles){

            var articleAsPublishedContent = umbracoContext.Content.GetById(selectedArticle.Id);
            if(articleAsPublishedContent is Article article){
                responses.Add(new {
                    Title = article.Title,
                    Author = article.Author,
                    Published = article.Published
                });
            }
        }
        
        var response = new ExpandoObject() as IDictionary<string, object>;

        //Todo: Extract to extension method
        Type myType = handled.GetType();
        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

        //Add properties to response object
        foreach (PropertyInfo prop in props)
        {
            if(prop.Name == "Content"){
                continue;
            }
            var content = new ExpandoObject() as IDictionary<string, object>;
            object propValue = prop.GetValue(handled, null); 

            response.Add(prop.Name, propValue);
        }
        response.Add("selectedArticles", responses);
        return response;
    }

    public object RenderQuoteBlock(BlockListItem<QuoteBlock> quoteBlock, IContentElementBuilder contentElementBuilder)
    {
        //Start by using the default handling and then add any quote block specific properties
        var handled = contentElementBuilder.ContentElementWithSettingsFor(quoteBlock.Content, quoteBlock.Settings);
        
        var response = new ExpandoObject() as IDictionary<string, object>;
        
        //Use reflecion to get properties of object at runtime - could be made as extension method for reusability
        Type myType = handled.GetType();
        IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

        //Add properties to response object
        foreach (PropertyInfo prop in props)
        {
            var content = new ExpandoObject() as IDictionary<string, object>;
            object propValue = prop.GetValue(handled, null); 

            response.Add(prop.Name, propValue);
        }

        //Add custom property to response object
        var customPropertyOnQuoteBlock = "Extended quoteblock with custom property";
        response.Add(nameof(customPropertyOnQuoteBlock), customPropertyOnQuoteBlock);

        return response;
    }
}