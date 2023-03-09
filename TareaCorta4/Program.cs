using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniValidation;
using System.Data;
using TareaCorta4API.DataAccess.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Semana3PvContext>(options =>
{
    options.UseSqlServer("name=ConnectionStrings:DefaultConnection");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");
//Login
app.MapPost("/Login", async (Usuario usu, Semana3PvContext context, string id, string contraseña) =>
{
    try
    {
        if (!MiniValidator.TryValidate(usu, out var errors))
        {
            return Results.BadRequest(new { codigo = -2, mensaje = StatusCodes.Status404NotFound, errores = errors });
        }

        if (await context.Usuarios.AnyAsync<Usuario>(S => S.Nombre == id && S.Contraseña == contraseña))
        {
            await context.SaveChangesAsync();
            return Results.Ok(
                new
                {
                    codigo = 0,
                    usu = usu,
                    mensaje = StatusCodes.Status200OK,
                });
        }
    }
    catch (Exception)
    {

        throw;
    }
    return Results.NotFound();


});

//Usuarios
app.MapPost("/RegistrarUsuario", async ([FromBody] Usuario usu, Semana3PvContext context) =>
{
    try
    {
        
        if (!MiniValidator.TryValidate(usu, out var errors))
        {
            return Results.BadRequest(new { codigo = -2, mensaje = StatusCodes.Status404NotFound, errores = errors });
        }
        await context.Usuarios.AddAsync(usu);
        await context.SaveChangesAsync();
        return Results.Created($"/Servidor/{usu.Identificacion}",
        new
        {
            codigo = 0,
            mensaje = StatusCodes.Status201Created,
            usu = usu,
            //nombreEstado = cliente.EstadoNavigation!.Nombre
        });
    }
    catch (Exception exc)
    {
        return Results.Json(new
        {
            codigo = -1,
            mensaje = StatusCodes.Status409Conflict
        },
        statusCode: StatusCodes.Status500InternalServerError);

    }

});

app.MapGet("MostrarUsuarios", async (Semana3PvContext context) =>
{
    return Results.Ok(await context.Usuarios.ToListAsync());
});

app.MapGet("ConsultarUsuario/{id}", async (int id, Semana3PvContext context) =>
{
    if (await context.Usuarios.AnyAsync<Usuario>(x => x.Identificacion == id))
    {
        return Results.Ok(await context.Usuarios.FirstAsync<Usuario>(x => x.Identificacion == id));
    }
    return Results.NotFound();
});
app.MapPut("/ActualizarUsuario", async (int id, Usuario usu, Semana3PvContext context) =>
{
    var todo = await context.Usuarios.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Nombre = usu.Nombre;
    todo.Apellido = usu.Apellido;
   

    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/EliminarUsuario", async (int id, Semana3PvContext context) =>
{
    
    try
    {

        var usuarioAEliminar =context.Usuarios.FirstOrDefault(u => u.Identificacion == id);     
        var RelacionContacto = context.Contactos.Any(p => p.Identificacion == id);
        if (!RelacionContacto)
        {
            context.Usuarios.Remove(usuarioAEliminar);
            context.SaveChanges();

            return Results.Ok();
        }
    }
    catch (Exception)
    {

        return Results.NotFound();
    }
    return Results.NotFound();
});

//Contactos
app.MapPost("/Contacto", async ([FromBody] Contacto Cont, Semana3PvContext context) =>
{
    try
    {
        if (!MiniValidator.TryValidate(Cont, out var errors))
        {
            return Results.BadRequest(new { codigo = -2, mensaje = StatusCodes.Status404NotFound, errores = errors });
        }
        await context.Contactos.AddAsync(Cont);
        await context.SaveChangesAsync();
        return Results.Created($"/Servidor/{Cont.Identificacion}",
        new
        {
            codigo = 0,
            mensaje = StatusCodes.Status201Created,
            serv = Cont,
            //nombreEstado = cliente.EstadoNavigation!.Nombre
        });
    }
    catch (Exception exc)
    {
        return Results.Json(new
        {
            codigo = -1,
            mensaje = StatusCodes.Status409Conflict
        },
        statusCode: StatusCodes.Status409Conflict);

    }

});


app.MapPut("/Contacto/{Identificacion}", async (int id, Contacto Cont, Semana3PvContext context) =>
{
    try
    {
        var todo = await context.Contactos.FindAsync(id);

        if (todo is null) return Results.NotFound();

        todo.Identificacion = Cont.Identificacion;
        todo.Nombre = Cont.Nombre;
        todo.Apellidos = Cont.Apellidos;
        todo.Telefono = Cont.Telefono;
        todo.Correos = Cont.Correos;
        todo.Facebook = Cont.Facebook;
        todo.Instagram = Cont.Instagram;
        todo.Twitter = Cont.Twitter;


        await context.SaveChangesAsync();

        return Results.Ok();
    }
    catch (Exception exc)
    {
        return Results.Json(new { codigo = -1, mensaje = exc.Message },
        statusCode: StatusCodes.Status500InternalServerError);
    }
});

app.MapDelete("/Contacto/{Identificacion}", async (int Identificacion, Semana3PvContext context) =>
{
    if (await context.Contactos.FindAsync(Identificacion) is Contacto todo)
    {
        context.Contactos.Remove(todo);
        await context.SaveChangesAsync();
        return Results.Ok();
    }

    return Results.NotFound();
});


app.MapGet("MostrarTodoslosContactos", async (Semana3PvContext context) =>
{
    return Results.Ok(await context.Contactos.ToListAsync());
});

app.MapGet("ConsultarContacto/{id}", async (int id, Semana3PvContext context) =>
{
    if (await context.Contactos.AnyAsync<Contacto>(x => x.Identificacion == id))
    {
        return Results.Ok(await context.Contactos.FirstAsync<Contacto>(x => x.Identificacion == id));
    }
    return Results.NotFound();
});


//Cambiar Contraseña
app.MapPut("/CambiarContraseña", async (int codigo, string Contraseña, Semana3PvContext context) =>
{
    try
    {
        if (await context.Usuarios.AnyAsync(x => x.Identificacion == codigo))
        {
            var user = await context.Usuarios.FindAsync(codigo);
            user.Contraseña = Convert.ToString(Contraseña != "" ? Contraseña : false);
            context.Usuarios.Update(user);
            await context.SaveChangesAsync();
            return Results.Ok();
        }
        else if (await context.Usuarios.AnyAsync(x => x.Identificacion == codigo))
        {
            var user = await context.Usuarios.FindAsync(codigo);
            user.Contraseña = Convert.ToString(Contraseña != "" ? Contraseña : false);
            context.Usuarios.Update(user);
            await context.SaveChangesAsync();
            return Results.NoContent();
        }
        return Results.NotFound("No existe la identificacion del parámetro ingresado.");
    }
    catch (Exception exc)
    {
        return Results.Json(new { codigo = -1, mensaje = exc.Message },
        statusCode: StatusCodes.Status500InternalServerError);
    }
});

//Cambiar Estado
app.MapPut("/CambiarEstadoUsuario", async (int codigo, short tipoAlerta, Semana3PvContext context) =>
{
    try
    {
        if (await context.Usuarios.AnyAsync(x => x.Identificacion == codigo))
        {
            var user = await context.Usuarios.FindAsync(codigo);
            user.Estado = Convert.ToInt32(tipoAlerta == 1 ? true : false);
            context.Usuarios.Update(user);
            await context.SaveChangesAsync();
            return Results.Ok();
        }
        else if (await context.Usuarios.AnyAsync(x => x.Identificacion == codigo))
        {
            var user = await context.Usuarios.FindAsync(codigo);
            user.Estado = Convert.ToInt32(tipoAlerta == 1 ? true : false);
            context.Usuarios.Update(user);
            await context.SaveChangesAsync();
            return Results.Ok();
        }
        return Results.NotFound("No existe la identificacion del parámetro ingresado.");
    }
    catch (Exception exc)
    {
        return Results.Json(new { codigo = -1, mensaje = exc.Message },
        statusCode: StatusCodes.Status500InternalServerError);
    }
});
app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}