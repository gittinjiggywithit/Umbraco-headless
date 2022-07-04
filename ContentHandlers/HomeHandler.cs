using Newtonsoft.Json;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.PublishedModels;
using Vertica.Umbraco.Headless.Core.Rendering;

public class HomeContentModelBuilder : IContentModelBuilder
{
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;

    public HomeContentModelBuilder(IUmbracoContextAccessor umbracoContextAccessor)
    {
        _umbracoContextAccessor = umbracoContextAccessor;
    }

    //Declare the model type this content builder should be used for
    public string ContentTypeAlias() => Home.ModelTypeAlias;

    //Declare the model type to be returned
    public Type ModelType() => typeof(HomeContentModel);

    //Build the content model
    public object? BuildContentModel(IPublishedElement content, IContentElementBuilder contentElementBuilder){
        //Create content model from strongly typed Home object
        if(content is Home home){            
            return new HomeContentModel {
                Title = home.Title ?? string.Empty,
                Blocks = HandleBlocksWithDefaultBlockHandler(contentElementBuilder, content),
                IsPreview = _umbracoContextAccessor.GetRequiredUmbracoContext().InPreviewMode, 
                DogImage = GetDogImage().Result
            };

        }
        return null;
    }

    //Reuse logic for handling blocks
    private object HandleBlocksWithDefaultBlockHandler(IContentElementBuilder contentElementBuilder, IPublishedElement content) 
    => contentElementBuilder.PropertyValueFor(content, content.GetProperty(nameof(Home.Blocks)));

    //Fetch an image of a random dog from 3rd party
    private async Task<DogImageDto> GetDogImage(){
        
        using var client = new HttpClient();

        var result = await client.GetStringAsync("https://dog.ceo/api/breeds/image/random");
        var dogImage = JsonConvert.DeserializeObject<DogImageDto>(result);
        
        return dogImage;
    }
}

public class HomeContentModel
{
    public string Title { get; set; } = string.Empty;
    public object? Blocks { get; set; }
    public bool IsPreview { get; set; }
    public DogImageDto? DogImage {get; set;}
}

//Dto object for dezerialing response from dog.ceo api
public class DogImageDto {
    public string Message { get; set; } = string.Empty;
}