namespace LinkNest.Application.Abstraction.Excecption
{
    public record ValidationError(string propertyName, string errorMessage);

}
