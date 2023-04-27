using ApiTarefas;
using Microsoft.EntityFrameworkCore;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(op => op.UseInMemoryDatabase("TarefasDB"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/tarefas", async (AppDbContext db) => await db.Tarefas.ToListAsync());

app.MapGet("/tarefas/{id}", async (int id, AppDbContext db) =>
    await db.Tarefas.FindAsync(id) is Tarefa tarefa ? Results.Ok(tarefa) : Results.NotFound());

app.MapGet("/tarefas/concluida", async (AppDbContext db) =>
                                 await db.Tarefas.Where(x => x.Concluida).ToListAsync());

app.MapPost("/tarefas", async (Tarefa tarefa, AppDbContext db) =>
{
    db.Tarefas.Add(tarefa);
    await db.SaveChangesAsync();

    return Results.Created($"/tarefas/{tarefa.Id}", tarefa);
});

app.MapPut("/tarefas/{id}", async (int id, Tarefa tarefa, AppDbContext db) =>
{
    var tf = await db.Tarefas.FindAsync(id);

    if (tf is null)
        return Results.NotFound();

    tf.Nome = tarefa.Nome;
    tf.Concluida = tarefa.Concluida;
    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/tarefas/{id}", async (int id, AppDbContext db) =>
{
    if (await db.Tarefas.FindAsync(id) is Tarefa tarefa)
    {
        db.Tarefas.Remove(tarefa);
        await db.SaveChangesAsync();
        Results.Ok(tarefa);
    }

    Results.NotFound();
});

app.Run();