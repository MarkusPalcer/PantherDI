using PantherDI.Exceptions;

namespace PantherDI.ContainerCreation
{
    public class MultiGenerationConfiguration
    {
        public bool AllowMultipleGenerations { get; set; }

        public int MaximumGenerationCount { get; set; }

        public MultiGenerationConfiguration CreateChildConfiguration()
        {
            if (MaximumGenerationCount == 0)
            {
                throw new MaximumNumberOfGenerationsExceededException();
            }

            return new MultiGenerationConfiguration
            {
                AllowMultipleGenerations = MaximumGenerationCount != 0,
                MaximumGenerationCount = MaximumGenerationCount > 0 ? MaximumGenerationCount - 1 : -1
            };
        }
    }
}