namespace PantherDI.ContainerCreation.ContainerBuilderHelpers
{
    public class MultipleGenerationContainerBuilderHelper : IContainerBuilderHelper
    {
        MultiGenerationConfiguration _configuration = new MultiGenerationConfiguration
        {
            AllowMultipleGenerations = true,
            MaximumGenerationCount = -1
        };

        #region Implementation of IContainerBuilderHelper

        public void ApplyTo(ContainerBuilder containerBuilder)
        {
            containerBuilder.MultiGenerationConfiguration.AllowMultipleGenerations = _configuration.AllowMultipleGenerations;
            containerBuilder.MultiGenerationConfiguration.MaximumGenerationCount = _configuration.MaximumGenerationCount;
        }

        #endregion

        public MultipleGenerationContainerBuilderHelper WithMaxiumNumberOfChildGenerations(int value)
        {
            _configuration.MaximumGenerationCount = value;
            return this;
        }
    }
}