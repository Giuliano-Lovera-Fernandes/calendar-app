
using Dima.Api.Data;
using Dima.Api.EndPoints;
using Dima.Api.Handlers;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Categories;
using Dima.Core.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.CustomSchemaIds(n => n.FullName);
    x.EnableAnnotations();
});

builder.Services
    .AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddIdentityCookies();
builder.Services.AddAuthorization();

var cnnStr = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(
    x => { 
            x.UseSqlServer(cnnStr);
        });

builder.Services.AddTransient<ICategoryHandler, CategoryHandler>();
builder.Services.AddTransient<ITransactionHandler, TransactionHandler>();



var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//app.MapPost(
//    "/v1/categories",
//    (Request request, Handler handler)
//        => handler.Handle(request))
//    .WithName("Categories: Create")
//    .WithSummary("Cria uma nova categoria")
//    .Produces<Response>();

//app.MapPost(
//    "/v1/categories",
//    async (CreateCategoryRequest request, 
//        ICategoryHandler handler)
//        => await handler.CreateAsync(request))
//    .WithName("Categories: Create")
//    .WithSummary("Cria uma nova categoria")
//    .Produces<Response<Category>>();

//app.MapPut(
//    "/v1/categories/{id}",
//    async (long id,
//        UpdateCategoryRequest request,
//        ICategoryHandler handler)
//        =>  {
//            request.Id = id;
//            return await handler.UpdateAsync(request);
//           })
//    .WithName("Categories: Update")
//    .WithSummary("Atualiza uma nova categoria")
//    .Produces<Response<Category?>>();

//app.MapDelete(
//    "/v1/categories/{id}",
//    async (long id,        
//        ICategoryHandler handler)
//        => {
//            var request = new DeleteCategoryRequest { Id = id, UserId = "test@giuliano.io" };
//            return await handler.DeleteAsync(request);
//           })
//    .WithName("Categories: Delete")
//    .WithSummary("Exclui uma nova categoria")
//    .Produces<Response<Category?>>();

//app.MapGet(
//    "/v1/categories",
//    async (ICategoryHandler handler)
//        => {
//            var request = new GetAllCategoriesRequest { UserId = "test@giuliano.io" };
//            return await handler.GetAllAsync(request);
//        })
//    .WithName("Categories: Get All")
//    .WithSummary("Retorna todas as categoria")
//    .Produces<PagedResponse<List<Category?>>>();

//app.MapGet(
//    "/v1/categories/{id}",
//    async (long id,
//        ICategoryHandler handler)
//        => {
//            var request = new GetCategoryByIdRequest { Id = id, UserId = "test@giuliano.io" };
//            return await handler.GetByIdAsync(request);
//        })
//    .WithName("Categories: Get by Id")
//    .WithSummary("Retorna uma categoria")
//    .Produces<Response<Category?>>();
app.MapGet("/", () => new { message = "Ok" });
app.MapEndPoints();
app.Run();

public class Request
{
    
    public string Title { get; set; } = string.Empty;

    //public DateTime CreateAt { get; set; } = DateTime.Now;    

    //public int Type { get; set; } 

    //public decimal Amount { get; set; }

    //public long CategoryId { get; set; }    

    public string Description { get; set; } = string.Empty;
}

//public class Response 
//{
//    public long Id { get; set; }
//    public string Title { get; set; } = string.Empty;
//    //public string Description { get; set; } = string.Empty;
//}

//public class Handler(AppDbContext context)
//{    
//    public Response Handle(Request request)
//    {
//        var category = new Category
//        { 
//            Title = request.Title,
//            Description = request.Description
//        };

//        context.Categories.Add(category);
//        context.SaveChanges();

//        return new Response
//        {
//            Id = category.Id,
//            Title = category.Title
//        };
//    }
//}



