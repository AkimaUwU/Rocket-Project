using TgBotPlannerTests.Models.Recognitions;

namespace TgBotPlannerTests.Models;

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
