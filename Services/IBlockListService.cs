using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Web.Common.PublishedModels;
using Vertica.Umbraco.Headless.Core.Rendering;

public interface IBlockListService {
    object RenderQuoteBlock(BlockListItem<QuoteBlock> quoteBlock, IContentElementBuilder contentElementBuilder);
    object RenderArticlesBlock(BlockListItem<ArticlesBlock> articlesBlock, IContentElementBuilder contentElementBuilder);
}