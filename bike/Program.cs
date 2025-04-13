using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

List<Motorbike> repo = [];

app.MapGet("/", () => repo);
app.MapPost("/", (Motorbike dto) => 
{
    var validationResults = new List<ValidationResult>();
    if (!Validator.TryValidateObject(dto, new ValidationContext(dto), validationResults, true))
        { return Results.BadRequest(validationResults); }
    dto.id = Guid.NewGuid();
    repo.Add(dto);
    return Results.Created($"/{dto.Numphone}",dto);
});
app.MapPut("/", ([FromQuery]Guid id, UpdateMotorbikeDTO dto) => 
{
    Motorbike buffer = repo.Find(b  => b.id == id);
    if (buffer == null)
    {
        return Results.NotFound();
    }

    buffer.Bikenum = dto.bikenum;
    buffer.Mark = dto.mark;
    buffer.Lastname = dto.lastname;
    buffer.Numphone = dto.numphone;

    var validationResults = new List<ValidationResult>();
    if (!Validator.TryValidateObject(buffer, new ValidationContext(buffer), validationResults, true))
    { return Results.BadRequest(validationResults); }
    repo.Remove(buffer);
    repo.Add(buffer);
    return Results.Json(buffer);
});
app.MapDelete("/", ([FromQuery] Guid id) => 
{
    Motorbike buffer = repo.Find(b => b.id == id);
    repo.Remove(buffer);
});
app.Run();


class Motorbike
{
    public Guid id  { get; set; }
    [Required(ErrorMessage = "Motorcycle number is required")]
    public string Bikenum { get; set; }
    public string Mark { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Patronymic { get; set; }
    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^(?:\+?(\d{1}))?[-. (]?\d{3}[-. )]?\d{3}[-. ]?\d{4}$", ErrorMessage = "Incorrect phone number format")]
    public string Numphone { get; set; }
};

record class UpdateMotorbikeDTO(string bikenum, string mark, string lastname, string numphone);
