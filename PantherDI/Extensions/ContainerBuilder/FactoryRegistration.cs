 
  
  

// Generated code. See FactoryRegistration.tt for code generation

using System;
using PantherDI.Registry.Registration.Factory;

namespace PantherDI.Extensions.ContainerBuilder
{
	/// <summary>
	/// Extends the ContainerBuilder by functions to register arbitrary functions as factory
	/// </summary>
	public static class FactoryRegistration
	{
	

		/// <summary>
		/// Adds the given factory function to the container builder
		/// </summary>
		public static ContainerCreation.ContainerBuilder WithFactory<TIn1, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, T> @delegate, params object[] contracts)
		{
			return builder.WithFactory<T>(DelegateFactory.Create(@delegate, contracts));
		}

		/// <summary>
		/// Registers the given factory function to the container builder
		/// </summary>
		/// <returns>
		/// An means to configure the factory registration with a fluent interface
		/// </returns>
		public static FactoryRegistrationHelper RegisterFactory<TIn1, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, T> @delegate, params object[] contracts)
		{
			return builder.Register<T>(DelegateFactory.Create(@delegate, contracts));
		}

	

		/// <summary>
		/// Adds the given factory function to the container builder
		/// </summary>
		public static ContainerCreation.ContainerBuilder WithFactory<TIn1, TIn2, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, T> @delegate, params object[] contracts)
		{
			return builder.WithFactory<T>(DelegateFactory.Create(@delegate, contracts));
		}

		/// <summary>
		/// Registers the given factory function to the container builder
		/// </summary>
		/// <returns>
		/// An means to configure the factory registration with a fluent interface
		/// </returns>
		public static FactoryRegistrationHelper RegisterFactory<TIn1, TIn2, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, T> @delegate, params object[] contracts)
		{
			return builder.Register<T>(DelegateFactory.Create(@delegate, contracts));
		}

	

		/// <summary>
		/// Adds the given factory function to the container builder
		/// </summary>
		public static ContainerCreation.ContainerBuilder WithFactory<TIn1, TIn2, TIn3, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, T> @delegate, params object[] contracts)
		{
			return builder.WithFactory<T>(DelegateFactory.Create(@delegate, contracts));
		}

		/// <summary>
		/// Registers the given factory function to the container builder
		/// </summary>
		/// <returns>
		/// An means to configure the factory registration with a fluent interface
		/// </returns>
		public static FactoryRegistrationHelper RegisterFactory<TIn1, TIn2, TIn3, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, T> @delegate, params object[] contracts)
		{
			return builder.Register<T>(DelegateFactory.Create(@delegate, contracts));
		}

	

		/// <summary>
		/// Adds the given factory function to the container builder
		/// </summary>
		public static ContainerCreation.ContainerBuilder WithFactory<TIn1, TIn2, TIn3, TIn4, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, T> @delegate, params object[] contracts)
		{
			return builder.WithFactory<T>(DelegateFactory.Create(@delegate, contracts));
		}

		/// <summary>
		/// Registers the given factory function to the container builder
		/// </summary>
		/// <returns>
		/// An means to configure the factory registration with a fluent interface
		/// </returns>
		public static FactoryRegistrationHelper RegisterFactory<TIn1, TIn2, TIn3, TIn4, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, T> @delegate, params object[] contracts)
		{
			return builder.Register<T>(DelegateFactory.Create(@delegate, contracts));
		}

	

		/// <summary>
		/// Adds the given factory function to the container builder
		/// </summary>
		public static ContainerCreation.ContainerBuilder WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, T> @delegate, params object[] contracts)
		{
			return builder.WithFactory<T>(DelegateFactory.Create(@delegate, contracts));
		}

		/// <summary>
		/// Registers the given factory function to the container builder
		/// </summary>
		/// <returns>
		/// An means to configure the factory registration with a fluent interface
		/// </returns>
		public static FactoryRegistrationHelper RegisterFactory<TIn1, TIn2, TIn3, TIn4, TIn5, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, T> @delegate, params object[] contracts)
		{
			return builder.Register<T>(DelegateFactory.Create(@delegate, contracts));
		}

	

		/// <summary>
		/// Adds the given factory function to the container builder
		/// </summary>
		public static ContainerCreation.ContainerBuilder WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, T> @delegate, params object[] contracts)
		{
			return builder.WithFactory<T>(DelegateFactory.Create(@delegate, contracts));
		}

		/// <summary>
		/// Registers the given factory function to the container builder
		/// </summary>
		/// <returns>
		/// An means to configure the factory registration with a fluent interface
		/// </returns>
		public static FactoryRegistrationHelper RegisterFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, T> @delegate, params object[] contracts)
		{
			return builder.Register<T>(DelegateFactory.Create(@delegate, contracts));
		}

	

		/// <summary>
		/// Adds the given factory function to the container builder
		/// </summary>
		public static ContainerCreation.ContainerBuilder WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, T> @delegate, params object[] contracts)
		{
			return builder.WithFactory<T>(DelegateFactory.Create(@delegate, contracts));
		}

		/// <summary>
		/// Registers the given factory function to the container builder
		/// </summary>
		/// <returns>
		/// An means to configure the factory registration with a fluent interface
		/// </returns>
		public static FactoryRegistrationHelper RegisterFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, T> @delegate, params object[] contracts)
		{
			return builder.Register<T>(DelegateFactory.Create(@delegate, contracts));
		}

	

		/// <summary>
		/// Adds the given factory function to the container builder
		/// </summary>
		public static ContainerCreation.ContainerBuilder WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, T> @delegate, params object[] contracts)
		{
			return builder.WithFactory<T>(DelegateFactory.Create(@delegate, contracts));
		}

		/// <summary>
		/// Registers the given factory function to the container builder
		/// </summary>
		/// <returns>
		/// An means to configure the factory registration with a fluent interface
		/// </returns>
		public static FactoryRegistrationHelper RegisterFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, T> @delegate, params object[] contracts)
		{
			return builder.Register<T>(DelegateFactory.Create(@delegate, contracts));
		}

	

		/// <summary>
		/// Adds the given factory function to the container builder
		/// </summary>
		public static ContainerCreation.ContainerBuilder WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, T> @delegate, params object[] contracts)
		{
			return builder.WithFactory<T>(DelegateFactory.Create(@delegate, contracts));
		}

		/// <summary>
		/// Registers the given factory function to the container builder
		/// </summary>
		/// <returns>
		/// An means to configure the factory registration with a fluent interface
		/// </returns>
		public static FactoryRegistrationHelper RegisterFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, T> @delegate, params object[] contracts)
		{
			return builder.Register<T>(DelegateFactory.Create(@delegate, contracts));
		}

	

		/// <summary>
		/// Adds the given factory function to the container builder
		/// </summary>
		public static ContainerCreation.ContainerBuilder WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, T> @delegate, params object[] contracts)
		{
			return builder.WithFactory<T>(DelegateFactory.Create(@delegate, contracts));
		}

		/// <summary>
		/// Registers the given factory function to the container builder
		/// </summary>
		/// <returns>
		/// An means to configure the factory registration with a fluent interface
		/// </returns>
		public static FactoryRegistrationHelper RegisterFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, T> @delegate, params object[] contracts)
		{
			return builder.Register<T>(DelegateFactory.Create(@delegate, contracts));
		}

	

		/// <summary>
		/// Adds the given factory function to the container builder
		/// </summary>
		public static ContainerCreation.ContainerBuilder WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, T> @delegate, params object[] contracts)
		{
			return builder.WithFactory<T>(DelegateFactory.Create(@delegate, contracts));
		}

		/// <summary>
		/// Registers the given factory function to the container builder
		/// </summary>
		/// <returns>
		/// An means to configure the factory registration with a fluent interface
		/// </returns>
		public static FactoryRegistrationHelper RegisterFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, T> @delegate, params object[] contracts)
		{
			return builder.Register<T>(DelegateFactory.Create(@delegate, contracts));
		}

	

		/// <summary>
		/// Adds the given factory function to the container builder
		/// </summary>
		public static ContainerCreation.ContainerBuilder WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, T> @delegate, params object[] contracts)
		{
			return builder.WithFactory<T>(DelegateFactory.Create(@delegate, contracts));
		}

		/// <summary>
		/// Registers the given factory function to the container builder
		/// </summary>
		/// <returns>
		/// An means to configure the factory registration with a fluent interface
		/// </returns>
		public static FactoryRegistrationHelper RegisterFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, T> @delegate, params object[] contracts)
		{
			return builder.Register<T>(DelegateFactory.Create(@delegate, contracts));
		}

	

		/// <summary>
		/// Adds the given factory function to the container builder
		/// </summary>
		public static ContainerCreation.ContainerBuilder WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, T> @delegate, params object[] contracts)
		{
			return builder.WithFactory<T>(DelegateFactory.Create(@delegate, contracts));
		}

		/// <summary>
		/// Registers the given factory function to the container builder
		/// </summary>
		/// <returns>
		/// An means to configure the factory registration with a fluent interface
		/// </returns>
		public static FactoryRegistrationHelper RegisterFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, T> @delegate, params object[] contracts)
		{
			return builder.Register<T>(DelegateFactory.Create(@delegate, contracts));
		}

	

		/// <summary>
		/// Adds the given factory function to the container builder
		/// </summary>
		public static ContainerCreation.ContainerBuilder WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, T> @delegate, params object[] contracts)
		{
			return builder.WithFactory<T>(DelegateFactory.Create(@delegate, contracts));
		}

		/// <summary>
		/// Registers the given factory function to the container builder
		/// </summary>
		/// <returns>
		/// An means to configure the factory registration with a fluent interface
		/// </returns>
		public static FactoryRegistrationHelper RegisterFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, T> @delegate, params object[] contracts)
		{
			return builder.Register<T>(DelegateFactory.Create(@delegate, contracts));
		}

	

		/// <summary>
		/// Adds the given factory function to the container builder
		/// </summary>
		public static ContainerCreation.ContainerBuilder WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, T> @delegate, params object[] contracts)
		{
			return builder.WithFactory<T>(DelegateFactory.Create(@delegate, contracts));
		}

		/// <summary>
		/// Registers the given factory function to the container builder
		/// </summary>
		/// <returns>
		/// An means to configure the factory registration with a fluent interface
		/// </returns>
		public static FactoryRegistrationHelper RegisterFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, T> @delegate, params object[] contracts)
		{
			return builder.Register<T>(DelegateFactory.Create(@delegate, contracts));
		}

	

		/// <summary>
		/// Adds the given factory function to the container builder
		/// </summary>
		public static ContainerCreation.ContainerBuilder WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, TIn16, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, TIn16, T> @delegate, params object[] contracts)
		{
			return builder.WithFactory<T>(DelegateFactory.Create(@delegate, contracts));
		}

		/// <summary>
		/// Registers the given factory function to the container builder
		/// </summary>
		/// <returns>
		/// An means to configure the factory registration with a fluent interface
		/// </returns>
		public static FactoryRegistrationHelper RegisterFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, TIn16, T>(this ContainerCreation.ContainerBuilder builder, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, TIn16, T> @delegate, params object[] contracts)
		{
			return builder.Register<T>(DelegateFactory.Create(@delegate, contracts));
		}



		/// <summary>
		/// Adds the given factory function to the container builder
		/// </summary>
		public static ContainerCreation.ContainerBuilder WithFactory<T>(this ContainerCreation.ContainerBuilder builder, Func<T> @delegate, params object[] contracts)
		{
			return builder.WithFactory<T>(DelegateFactory.Create(@delegate, contracts));
		}

		/// <summary>
		/// Registers the given factory function to the container builder
		/// </summary>
		/// <returns>
		/// An means to configure the factory registration with a fluent interface
		/// </returns>
		public static FactoryRegistrationHelper RegisterFactory<T>(this ContainerCreation.ContainerBuilder builder, Func<T> @delegate, params object[] contracts)
		{
			return builder.Register<T>(DelegateFactory.Create(@delegate, contracts));
		}
	}
}