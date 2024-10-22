

using System.Net.Http.Headers;
using System.Xml.Schema;
using CSharpFunctionalExtensions;
using RocketPlaner.domain.abstraction;

namespace RocketPlaner.domain.models.RocketTasks.ValueObjects;

public class RocketTaskType : DomainValueObject
{
    

    public string ValueType {get; private set;}

    private RocketTaskType()
    {
        ValueType=string.Empty;

    }
    private RocketTaskType(string type)
    {
        ValueType=type;
    }
    public static Result<RocketTaskType> Create(string type)
    {
        if (string.IsNullOrWhiteSpace(type)) 
        {

            return Result.Failure<RocketTaskType>("Тип задачи не указан");
        }
        if(rocketTaskTypesMassiv.Any(x => x.ValueType == type) == false) return Result.Failure<RocketTaskType>("Некорректный тип задачи");
        return new RocketTaskType(type);


    }
    public override IEnumerable<object> GetEqualityComponents()
    {

        yield return ValueType;
    }

    private static RocketTaskType[] rocketTaskTypesMassiv = [new("Повторяющииеся"), new("Одноразовое")];

    public static RocketTaskType Defoult => new RocketTaskType();

}
