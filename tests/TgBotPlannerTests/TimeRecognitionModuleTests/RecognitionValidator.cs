using TgBotPlannerTests.TimeRecognitionModuleTests.Recognitions;

namespace TgBotPlannerTests.TimeRecognitionModuleTests;

public static class RecognitionValidator
{
    public static bool IsRecognized(TimeRecognition recognition) =>
        recognition switch
        {
            UnrecognizedTime => false,
            _ => true,
        };

    public static bool CanProcessTicket(TimeRecognitionTicket ticket) =>
        ticket switch
        {
            UnknownRecognitionTicket => false,
            _ => true,
        };
}
