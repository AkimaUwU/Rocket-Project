using CSharpFunctionalExtensions;
using System.ComponentModel.Design;
using RocketPlaner.domain.Abstractions;

public class RocketTaskDestination : DomainEntity
{
	public string ChatId{ get; init; }

	private RocketTaskDestination() : base(Guid.Empty)  => ChatId = string.Empty;	

	private RocketTaskDestination(Guid id, string chatId) : base(id) => ChatId = chatId;		        	    

	public static Result<RocketTaskDestination> Create(string chatId)
	{
		if (string.IsNullOrWhiteSpace(chatId))
			return Result.Failure<RocketTaskDestination>("ID чата не было указано");
		
		return new RocketTaskDestination(Guid.NewGuid(), chatId);
	}
}