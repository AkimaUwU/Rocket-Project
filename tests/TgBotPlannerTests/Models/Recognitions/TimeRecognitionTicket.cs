namespace TgBotPlannerTests.Models.Recognitions;

public abstract record TimeRecognitionTicket;

public sealed record PeriodicTimeRecognitionTicket(string Message) : TimeRecognitionTicket;

public sealed record SingleTimeRecognitionTicket(string Message) : TimeRecognitionTicket;

public sealed record UnknownRecognitionTicket : TimeRecognitionTicket;
