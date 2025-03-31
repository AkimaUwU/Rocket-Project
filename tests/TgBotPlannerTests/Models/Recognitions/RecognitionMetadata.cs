using System.Collections;

namespace TgBotPlannerTests.Models.Recognitions;

public record RecognitionMetadata(TimeRecognition Recognition);

public record RecognitionMetadataCollection : IEnumerable<RecognitionMetadata>
{
    private readonly List<RecognitionMetadata> _metadata;

    public RecognitionMetadataCollection(List<RecognitionMetadata> metadata) =>
        _metadata = metadata;

    public int Count => _metadata.Count;

    public IEnumerator<RecognitionMetadata> GetEnumerator() => _metadata.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
