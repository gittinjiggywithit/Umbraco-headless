using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;
using Vertica.Umbraco.Headless.Core.Models;
using Vertica.Umbraco.Headless.Core.Rendering;

public class BlockListHandler : IPropertyRenderer
{
    private readonly IBlockListService _blockListService;

    public BlockListHandler(IBlockListService blockListService)
    {
        _blockListService = blockListService;
    }

    //Declare the property type this handler should be used for - custom handlers will take precedence
    public string PropertyEditorAlias => Constants.PropertyEditors.Aliases.BlockList;

    public Type TypeFor(IPublishedPropertyType propertyType) => typeof(ContentElementWithSettings[]);

    public virtual object? ValueFor(object umbracoValue, IPublishedProperty property, IContentElementBuilder contentElementBuilder)
        => umbracoValue is IEnumerable<BlockListItem> items
            ? items.Select(i => RenderBlock(i, contentElementBuilder)).ToArray()
            : null;
        
    //Add any non-default rendering here
    private object RenderBlock(BlockListItem block, IContentElementBuilder contentElementBuilder){

        if(block is BlockListItem<QuoteBlock> quoteBlock){
            
            return _blockListService.RenderQuoteBlock(quoteBlock, contentElementBuilder);
        }

        if(block is BlockListItem<ArticlesBlock> articlesBlock){
            return _blockListService.RenderArticlesBlock(articlesBlock, contentElementBuilder);
        }

        return contentElementBuilder.ContentElementWithSettingsFor(block.Content, block.Settings);
    }
}