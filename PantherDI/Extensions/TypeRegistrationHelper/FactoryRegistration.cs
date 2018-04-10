 
  
  

// Generated code. See FactoryRegistration.tt for code generation

using System;
using PantherDI.ContainerCreation.CatalogBuilderHelpers;
using PantherDI.Registry.Registration.Factory;

namespace PantherDI.Extensions.TypeRegistrationHelper
{
	/// <summary>
	/// Extends the ContainerBuilder by functions to register arbitrary functions as factory
	/// </summary>
	public static class FactoryRegistration
	{

		public static TypeCatalogBuilderHelper WithFactory<TIn1, T>(this TypeCatalogBuilderHelper src, Func<TIn1, T> @delegate, params object[] contracts)
		{
			return src.WithFactory(DelegateFactory.Create(@delegate, contracts));
		}


		public static TypeCatalogBuilderHelper WithFactory<TIn1, TIn2, T>(this TypeCatalogBuilderHelper src, Func<TIn1, TIn2, T> @delegate, params object[] contracts)
		{
			return src.WithFactory(DelegateFactory.Create(@delegate, contracts));
		}


		public static TypeCatalogBuilderHelper WithFactory<TIn1, TIn2, TIn3, T>(this TypeCatalogBuilderHelper src, Func<TIn1, TIn2, TIn3, T> @delegate, params object[] contracts)
		{
			return src.WithFactory(DelegateFactory.Create(@delegate, contracts));
		}


		public static TypeCatalogBuilderHelper WithFactory<TIn1, TIn2, TIn3, TIn4, T>(this TypeCatalogBuilderHelper src, Func<TIn1, TIn2, TIn3, TIn4, T> @delegate, params object[] contracts)
		{
			return src.WithFactory(DelegateFactory.Create(@delegate, contracts));
		}


		public static TypeCatalogBuilderHelper WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, T>(this TypeCatalogBuilderHelper src, Func<TIn1, TIn2, TIn3, TIn4, TIn5, T> @delegate, params object[] contracts)
		{
			return src.WithFactory(DelegateFactory.Create(@delegate, contracts));
		}


		public static TypeCatalogBuilderHelper WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, T>(this TypeCatalogBuilderHelper src, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, T> @delegate, params object[] contracts)
		{
			return src.WithFactory(DelegateFactory.Create(@delegate, contracts));
		}


		public static TypeCatalogBuilderHelper WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, T>(this TypeCatalogBuilderHelper src, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, T> @delegate, params object[] contracts)
		{
			return src.WithFactory(DelegateFactory.Create(@delegate, contracts));
		}


		public static TypeCatalogBuilderHelper WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, T>(this TypeCatalogBuilderHelper src, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, T> @delegate, params object[] contracts)
		{
			return src.WithFactory(DelegateFactory.Create(@delegate, contracts));
		}


		public static TypeCatalogBuilderHelper WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, T>(this TypeCatalogBuilderHelper src, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, T> @delegate, params object[] contracts)
		{
			return src.WithFactory(DelegateFactory.Create(@delegate, contracts));
		}


		public static TypeCatalogBuilderHelper WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, T>(this TypeCatalogBuilderHelper src, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, T> @delegate, params object[] contracts)
		{
			return src.WithFactory(DelegateFactory.Create(@delegate, contracts));
		}


		public static TypeCatalogBuilderHelper WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, T>(this TypeCatalogBuilderHelper src, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, T> @delegate, params object[] contracts)
		{
			return src.WithFactory(DelegateFactory.Create(@delegate, contracts));
		}


		public static TypeCatalogBuilderHelper WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, T>(this TypeCatalogBuilderHelper src, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, T> @delegate, params object[] contracts)
		{
			return src.WithFactory(DelegateFactory.Create(@delegate, contracts));
		}


		public static TypeCatalogBuilderHelper WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, T>(this TypeCatalogBuilderHelper src, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, T> @delegate, params object[] contracts)
		{
			return src.WithFactory(DelegateFactory.Create(@delegate, contracts));
		}


		public static TypeCatalogBuilderHelper WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, T>(this TypeCatalogBuilderHelper src, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, T> @delegate, params object[] contracts)
		{
			return src.WithFactory(DelegateFactory.Create(@delegate, contracts));
		}


		public static TypeCatalogBuilderHelper WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, T>(this TypeCatalogBuilderHelper src, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, T> @delegate, params object[] contracts)
		{
			return src.WithFactory(DelegateFactory.Create(@delegate, contracts));
		}


		public static TypeCatalogBuilderHelper WithFactory<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, TIn16, T>(this TypeCatalogBuilderHelper src, Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, TIn16, T> @delegate, params object[] contracts)
		{
			return src.WithFactory(DelegateFactory.Create(@delegate, contracts));
		}


		

	}
}