
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using ToDo.Application.Interfaces.TdoItem;
    using ToDo.Application.Interfaces.User;
    using ToDo.Application.Services.AuthServices;
    using ToDo.Application.Services.TodoServices;
    using ToDo.Domin.Entities;
    using ToDo.Infrastructure.Persistance;
    using ToDo.Infrastructure.Persistance.Repository;



    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();


    builder.Services.AddScoped<IAccountGenericService<ApplicationUser>, AccountGenericService<ApplicationUser>>();
    builder.Services.AddScoped<IAccountGenericRepository<ApplicationUser>, AccountGenericRepository<ApplicationUser>>();
    builder.Services.AddScoped<ITodoGenericRepository<TodoItem>,TodoGenericRepository<TodoItem>>();
    builder.Services.AddScoped<ITodoGenericService<TodoItem>,TodoGenericService<TodoItem>>();
    builder.Services.AddScoped<ITodoGenericRepository<TodoItemComment>, TodoGenericRepository<TodoItemComment>>();
    builder.Services.AddScoped<ITodoGenericService<TodoItemComment>, TodoGenericService<TodoItemComment>>();

    // Configure Database Context
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



    // Configure Identity (for your custom ApplicationUser)
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    // Configure Identity Options
    builder.Services.Configure<IdentityOptions>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
    });

    // Configure Cookie Authentication
    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.Name = "ToDoAppCookie";
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout";
        options.AccessDeniedPath = "/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(120);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
    });
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=TodoItems}/{id?}");



    app.Run();
