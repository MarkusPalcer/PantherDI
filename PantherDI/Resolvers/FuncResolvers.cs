 
  
  

// Generated code. See FuncResolvers.tt for code generation

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using PantherDI.Extensions;
using PantherDI.Registry.Registration.Dependency;
using PantherDI.Resolved.Providers;

namespace PantherDI.Resolvers
{

	public class Func1Resolver : GenericResolver 
	{
		public Func1Resolver() : base(typeof(Func<,>), typeof(InnerResolver<,>)) { }
		
		public class InnerResolver<TIn1,T> : IResolver 
		{
			public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
			{
				var types = new[] { typeof(TIn1) };
				
				foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()))
				{
					var func = FuncResolver.ProcessProvider<T>(types, provider);
					if (func == null) continue;

					Func<TIn1,T> Delegate(Dictionary<IDependency, object> objects)
					{
						return (p1) => 
							func(objects)(new object[] { p1 });
					}

					var result = DelegateProvider.WrapProvider<Func<TIn1,T>>(Delegate, provider);

					var dependenciesLeftUnresolved = provider.UnresolvedDependencies 
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn1).GetTypeInfo()));

					result.UnresolvedDependencies = new HashSet<IDependency>(dependenciesLeftUnresolved, new Dependency.EqualityComparer());

                    yield return result;
				}
			}
		}
	}

	public class Func2Resolver : GenericResolver 
	{
		public Func2Resolver() : base(typeof(Func<,,>), typeof(InnerResolver<,,>)) { }
		
		public class InnerResolver<TIn1, TIn2,T> : IResolver 
		{
			public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
			{
				var types = new[] { typeof(TIn1), typeof(TIn2) };
				
				foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()))
				{
					var func = FuncResolver.ProcessProvider<T>(types, provider);
					if (func == null) continue;

					Func<TIn1, TIn2,T> Delegate(Dictionary<IDependency, object> objects)
					{
						return (p1, p2) => 
							func(objects)(new object[] { p1, p2 });
					}

					var result = DelegateProvider.WrapProvider<Func<TIn1, TIn2,T>>(Delegate, provider);

					var dependenciesLeftUnresolved = provider.UnresolvedDependencies 
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn1).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn2).GetTypeInfo()));

					result.UnresolvedDependencies = new HashSet<IDependency>(dependenciesLeftUnresolved, new Dependency.EqualityComparer());

                    yield return result;
				}
			}
		}
	}

	public class Func3Resolver : GenericResolver 
	{
		public Func3Resolver() : base(typeof(Func<,,,>), typeof(InnerResolver<,,,>)) { }
		
		public class InnerResolver<TIn1, TIn2, TIn3,T> : IResolver 
		{
			public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
			{
				var types = new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3) };
				
				foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()))
				{
					var func = FuncResolver.ProcessProvider<T>(types, provider);
					if (func == null) continue;

					Func<TIn1, TIn2, TIn3,T> Delegate(Dictionary<IDependency, object> objects)
					{
						return (p1, p2, p3) => 
							func(objects)(new object[] { p1, p2, p3 });
					}

					var result = DelegateProvider.WrapProvider<Func<TIn1, TIn2, TIn3,T>>(Delegate, provider);

					var dependenciesLeftUnresolved = provider.UnresolvedDependencies 
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn1).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn2).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn3).GetTypeInfo()));

					result.UnresolvedDependencies = new HashSet<IDependency>(dependenciesLeftUnresolved, new Dependency.EqualityComparer());

                    yield return result;
				}
			}
		}
	}

	public class Func4Resolver : GenericResolver 
	{
		public Func4Resolver() : base(typeof(Func<,,,,>), typeof(InnerResolver<,,,,>)) { }
		
		public class InnerResolver<TIn1, TIn2, TIn3, TIn4,T> : IResolver 
		{
			public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
			{
				var types = new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4) };
				
				foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()))
				{
					var func = FuncResolver.ProcessProvider<T>(types, provider);
					if (func == null) continue;

					Func<TIn1, TIn2, TIn3, TIn4,T> Delegate(Dictionary<IDependency, object> objects)
					{
						return (p1, p2, p3, p4) => 
							func(objects)(new object[] { p1, p2, p3, p4 });
					}

					var result = DelegateProvider.WrapProvider<Func<TIn1, TIn2, TIn3, TIn4,T>>(Delegate, provider);

					var dependenciesLeftUnresolved = provider.UnresolvedDependencies 
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn1).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn2).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn3).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn4).GetTypeInfo()));

					result.UnresolvedDependencies = new HashSet<IDependency>(dependenciesLeftUnresolved, new Dependency.EqualityComparer());

                    yield return result;
				}
			}
		}
	}

	public class Func5Resolver : GenericResolver 
	{
		public Func5Resolver() : base(typeof(Func<,,,,,>), typeof(InnerResolver<,,,,,>)) { }
		
		public class InnerResolver<TIn1, TIn2, TIn3, TIn4, TIn5,T> : IResolver 
		{
			public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
			{
				var types = new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5) };
				
				foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()))
				{
					var func = FuncResolver.ProcessProvider<T>(types, provider);
					if (func == null) continue;

					Func<TIn1, TIn2, TIn3, TIn4, TIn5,T> Delegate(Dictionary<IDependency, object> objects)
					{
						return (p1, p2, p3, p4, p5) => 
							func(objects)(new object[] { p1, p2, p3, p4, p5 });
					}

					var result = DelegateProvider.WrapProvider<Func<TIn1, TIn2, TIn3, TIn4, TIn5,T>>(Delegate, provider);

					var dependenciesLeftUnresolved = provider.UnresolvedDependencies 
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn1).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn2).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn3).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn4).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn5).GetTypeInfo()));

					result.UnresolvedDependencies = new HashSet<IDependency>(dependenciesLeftUnresolved, new Dependency.EqualityComparer());

                    yield return result;
				}
			}
		}
	}

	public class Func6Resolver : GenericResolver 
	{
		public Func6Resolver() : base(typeof(Func<,,,,,,>), typeof(InnerResolver<,,,,,,>)) { }
		
		public class InnerResolver<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6,T> : IResolver 
		{
			public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
			{
				var types = new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6) };
				
				foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()))
				{
					var func = FuncResolver.ProcessProvider<T>(types, provider);
					if (func == null) continue;

					Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6,T> Delegate(Dictionary<IDependency, object> objects)
					{
						return (p1, p2, p3, p4, p5, p6) => 
							func(objects)(new object[] { p1, p2, p3, p4, p5, p6 });
					}

					var result = DelegateProvider.WrapProvider<Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6,T>>(Delegate, provider);

					var dependenciesLeftUnresolved = provider.UnresolvedDependencies 
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn1).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn2).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn3).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn4).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn5).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn6).GetTypeInfo()));

					result.UnresolvedDependencies = new HashSet<IDependency>(dependenciesLeftUnresolved, new Dependency.EqualityComparer());

                    yield return result;
				}
			}
		}
	}

	public class Func7Resolver : GenericResolver 
	{
		public Func7Resolver() : base(typeof(Func<,,,,,,,>), typeof(InnerResolver<,,,,,,,>)) { }
		
		public class InnerResolver<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7,T> : IResolver 
		{
			public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
			{
				var types = new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7) };
				
				foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()))
				{
					var func = FuncResolver.ProcessProvider<T>(types, provider);
					if (func == null) continue;

					Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7,T> Delegate(Dictionary<IDependency, object> objects)
					{
						return (p1, p2, p3, p4, p5, p6, p7) => 
							func(objects)(new object[] { p1, p2, p3, p4, p5, p6, p7 });
					}

					var result = DelegateProvider.WrapProvider<Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7,T>>(Delegate, provider);

					var dependenciesLeftUnresolved = provider.UnresolvedDependencies 
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn1).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn2).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn3).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn4).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn5).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn6).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn7).GetTypeInfo()));

					result.UnresolvedDependencies = new HashSet<IDependency>(dependenciesLeftUnresolved, new Dependency.EqualityComparer());

                    yield return result;
				}
			}
		}
	}

	public class Func8Resolver : GenericResolver 
	{
		public Func8Resolver() : base(typeof(Func<,,,,,,,,>), typeof(InnerResolver<,,,,,,,,>)) { }
		
		public class InnerResolver<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8,T> : IResolver 
		{
			public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
			{
				var types = new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7), typeof(TIn8) };
				
				foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()))
				{
					var func = FuncResolver.ProcessProvider<T>(types, provider);
					if (func == null) continue;

					Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8,T> Delegate(Dictionary<IDependency, object> objects)
					{
						return (p1, p2, p3, p4, p5, p6, p7, p8) => 
							func(objects)(new object[] { p1, p2, p3, p4, p5, p6, p7, p8 });
					}

					var result = DelegateProvider.WrapProvider<Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8,T>>(Delegate, provider);

					var dependenciesLeftUnresolved = provider.UnresolvedDependencies 
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn1).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn2).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn3).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn4).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn5).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn6).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn7).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn8).GetTypeInfo()));

					result.UnresolvedDependencies = new HashSet<IDependency>(dependenciesLeftUnresolved, new Dependency.EqualityComparer());

                    yield return result;
				}
			}
		}
	}

	public class Func9Resolver : GenericResolver 
	{
		public Func9Resolver() : base(typeof(Func<,,,,,,,,,>), typeof(InnerResolver<,,,,,,,,,>)) { }
		
		public class InnerResolver<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9,T> : IResolver 
		{
			public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
			{
				var types = new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7), typeof(TIn8), typeof(TIn9) };
				
				foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()))
				{
					var func = FuncResolver.ProcessProvider<T>(types, provider);
					if (func == null) continue;

					Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9,T> Delegate(Dictionary<IDependency, object> objects)
					{
						return (p1, p2, p3, p4, p5, p6, p7, p8, p9) => 
							func(objects)(new object[] { p1, p2, p3, p4, p5, p6, p7, p8, p9 });
					}

					var result = DelegateProvider.WrapProvider<Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9,T>>(Delegate, provider);

					var dependenciesLeftUnresolved = provider.UnresolvedDependencies 
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn1).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn2).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn3).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn4).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn5).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn6).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn7).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn8).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn9).GetTypeInfo()));

					result.UnresolvedDependencies = new HashSet<IDependency>(dependenciesLeftUnresolved, new Dependency.EqualityComparer());

                    yield return result;
				}
			}
		}
	}

	public class Func10Resolver : GenericResolver 
	{
		public Func10Resolver() : base(typeof(Func<,,,,,,,,,,>), typeof(InnerResolver<,,,,,,,,,,>)) { }
		
		public class InnerResolver<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10,T> : IResolver 
		{
			public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
			{
				var types = new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7), typeof(TIn8), typeof(TIn9), typeof(TIn10) };
				
				foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()))
				{
					var func = FuncResolver.ProcessProvider<T>(types, provider);
					if (func == null) continue;

					Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10,T> Delegate(Dictionary<IDependency, object> objects)
					{
						return (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10) => 
							func(objects)(new object[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10 });
					}

					var result = DelegateProvider.WrapProvider<Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10,T>>(Delegate, provider);

					var dependenciesLeftUnresolved = provider.UnresolvedDependencies 
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn1).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn2).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn3).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn4).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn5).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn6).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn7).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn8).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn9).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn10).GetTypeInfo()));

					result.UnresolvedDependencies = new HashSet<IDependency>(dependenciesLeftUnresolved, new Dependency.EqualityComparer());

                    yield return result;
				}
			}
		}
	}

	public class Func11Resolver : GenericResolver 
	{
		public Func11Resolver() : base(typeof(Func<,,,,,,,,,,,>), typeof(InnerResolver<,,,,,,,,,,,>)) { }
		
		public class InnerResolver<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11,T> : IResolver 
		{
			public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
			{
				var types = new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7), typeof(TIn8), typeof(TIn9), typeof(TIn10), typeof(TIn11) };
				
				foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()))
				{
					var func = FuncResolver.ProcessProvider<T>(types, provider);
					if (func == null) continue;

					Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11,T> Delegate(Dictionary<IDependency, object> objects)
					{
						return (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11) => 
							func(objects)(new object[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11 });
					}

					var result = DelegateProvider.WrapProvider<Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11,T>>(Delegate, provider);

					var dependenciesLeftUnresolved = provider.UnresolvedDependencies 
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn1).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn2).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn3).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn4).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn5).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn6).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn7).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn8).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn9).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn10).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn11).GetTypeInfo()));

					result.UnresolvedDependencies = new HashSet<IDependency>(dependenciesLeftUnresolved, new Dependency.EqualityComparer());

                    yield return result;
				}
			}
		}
	}

	public class Func12Resolver : GenericResolver 
	{
		public Func12Resolver() : base(typeof(Func<,,,,,,,,,,,,>), typeof(InnerResolver<,,,,,,,,,,,,>)) { }
		
		public class InnerResolver<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12,T> : IResolver 
		{
			public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
			{
				var types = new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7), typeof(TIn8), typeof(TIn9), typeof(TIn10), typeof(TIn11), typeof(TIn12) };
				
				foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()))
				{
					var func = FuncResolver.ProcessProvider<T>(types, provider);
					if (func == null) continue;

					Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12,T> Delegate(Dictionary<IDependency, object> objects)
					{
						return (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12) => 
							func(objects)(new object[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12 });
					}

					var result = DelegateProvider.WrapProvider<Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12,T>>(Delegate, provider);

					var dependenciesLeftUnresolved = provider.UnresolvedDependencies 
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn1).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn2).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn3).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn4).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn5).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn6).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn7).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn8).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn9).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn10).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn11).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn12).GetTypeInfo()));

					result.UnresolvedDependencies = new HashSet<IDependency>(dependenciesLeftUnresolved, new Dependency.EqualityComparer());

                    yield return result;
				}
			}
		}
	}

	public class Func13Resolver : GenericResolver 
	{
		public Func13Resolver() : base(typeof(Func<,,,,,,,,,,,,,>), typeof(InnerResolver<,,,,,,,,,,,,,>)) { }
		
		public class InnerResolver<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13,T> : IResolver 
		{
			public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
			{
				var types = new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7), typeof(TIn8), typeof(TIn9), typeof(TIn10), typeof(TIn11), typeof(TIn12), typeof(TIn13) };
				
				foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()))
				{
					var func = FuncResolver.ProcessProvider<T>(types, provider);
					if (func == null) continue;

					Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13,T> Delegate(Dictionary<IDependency, object> objects)
					{
						return (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13) => 
							func(objects)(new object[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13 });
					}

					var result = DelegateProvider.WrapProvider<Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13,T>>(Delegate, provider);

					var dependenciesLeftUnresolved = provider.UnresolvedDependencies 
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn1).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn2).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn3).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn4).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn5).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn6).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn7).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn8).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn9).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn10).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn11).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn12).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn13).GetTypeInfo()));

					result.UnresolvedDependencies = new HashSet<IDependency>(dependenciesLeftUnresolved, new Dependency.EqualityComparer());

                    yield return result;
				}
			}
		}
	}

	public class Func14Resolver : GenericResolver 
	{
		public Func14Resolver() : base(typeof(Func<,,,,,,,,,,,,,,>), typeof(InnerResolver<,,,,,,,,,,,,,,>)) { }
		
		public class InnerResolver<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14,T> : IResolver 
		{
			public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
			{
				var types = new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7), typeof(TIn8), typeof(TIn9), typeof(TIn10), typeof(TIn11), typeof(TIn12), typeof(TIn13), typeof(TIn14) };
				
				foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()))
				{
					var func = FuncResolver.ProcessProvider<T>(types, provider);
					if (func == null) continue;

					Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14,T> Delegate(Dictionary<IDependency, object> objects)
					{
						return (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14) => 
							func(objects)(new object[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14 });
					}

					var result = DelegateProvider.WrapProvider<Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14,T>>(Delegate, provider);

					var dependenciesLeftUnresolved = provider.UnresolvedDependencies 
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn1).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn2).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn3).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn4).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn5).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn6).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn7).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn8).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn9).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn10).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn11).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn12).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn13).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn14).GetTypeInfo()));

					result.UnresolvedDependencies = new HashSet<IDependency>(dependenciesLeftUnresolved, new Dependency.EqualityComparer());

                    yield return result;
				}
			}
		}
	}

	public class Func15Resolver : GenericResolver 
	{
		public Func15Resolver() : base(typeof(Func<,,,,,,,,,,,,,,,>), typeof(InnerResolver<,,,,,,,,,,,,,,,>)) { }
		
		public class InnerResolver<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15,T> : IResolver 
		{
			public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
			{
				var types = new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7), typeof(TIn8), typeof(TIn9), typeof(TIn10), typeof(TIn11), typeof(TIn12), typeof(TIn13), typeof(TIn14), typeof(TIn15) };
				
				foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()))
				{
					var func = FuncResolver.ProcessProvider<T>(types, provider);
					if (func == null) continue;

					Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15,T> Delegate(Dictionary<IDependency, object> objects)
					{
						return (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15) => 
							func(objects)(new object[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15 });
					}

					var result = DelegateProvider.WrapProvider<Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15,T>>(Delegate, provider);

					var dependenciesLeftUnresolved = provider.UnresolvedDependencies 
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn1).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn2).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn3).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn4).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn5).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn6).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn7).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn8).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn9).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn10).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn11).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn12).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn13).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn14).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn15).GetTypeInfo()));

					result.UnresolvedDependencies = new HashSet<IDependency>(dependenciesLeftUnresolved, new Dependency.EqualityComparer());

                    yield return result;
				}
			}
		}
	}

	public class Func16Resolver : GenericResolver 
	{
		public Func16Resolver() : base(typeof(Func<,,,,,,,,,,,,,,,,>), typeof(InnerResolver<,,,,,,,,,,,,,,,,>)) { }
		
		public class InnerResolver<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, TIn16,T> : IResolver 
		{
			public IEnumerable<IProvider> Resolve(Func<IDependency, IEnumerable<IProvider>> dependencyResolver, IDependency dependency)
			{
				var types = new[] { typeof(TIn1), typeof(TIn2), typeof(TIn3), typeof(TIn4), typeof(TIn5), typeof(TIn6), typeof(TIn7), typeof(TIn8), typeof(TIn9), typeof(TIn10), typeof(TIn11), typeof(TIn12), typeof(TIn13), typeof(TIn14), typeof(TIn15), typeof(TIn16) };
				
				foreach (var provider in dependencyResolver(dependency.ReplaceExpectedType<T>()))
				{
					var func = FuncResolver.ProcessProvider<T>(types, provider);
					if (func == null) continue;

					Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, TIn16,T> Delegate(Dictionary<IDependency, object> objects)
					{
						return (p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16) => 
							func(objects)(new object[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16 });
					}

					var result = DelegateProvider.WrapProvider<Func<TIn1, TIn2, TIn3, TIn4, TIn5, TIn6, TIn7, TIn8, TIn9, TIn10, TIn11, TIn12, TIn13, TIn14, TIn15, TIn16,T>>(Delegate, provider);

					var dependenciesLeftUnresolved = provider.UnresolvedDependencies 
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn1).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn2).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn3).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn4).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn5).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn6).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn7).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn8).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn9).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn10).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn11).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn12).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn13).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn14).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn15).GetTypeInfo()))
															 .Where(x => !x.ExpectedType.GetTypeInfo().IsAssignableFrom(typeof(TIn16).GetTypeInfo()));

					result.UnresolvedDependencies = new HashSet<IDependency>(dependenciesLeftUnresolved, new Dependency.EqualityComparer());

                    yield return result;
				}
			}
		}
	}

	public static class FuncResolvers 
	{
		public static Func<IResolver>[] All => new Func<IResolver>[] 
		{
			() => new Func0Resolver(),
			() => new Func1Resolver(),
			() => new Func2Resolver(),
			() => new Func3Resolver(),
			() => new Func4Resolver(),
			() => new Func5Resolver(),
			() => new Func6Resolver(),
			() => new Func7Resolver(),
			() => new Func8Resolver(),
			() => new Func9Resolver(),
			() => new Func10Resolver(),
			() => new Func11Resolver(),
			() => new Func12Resolver(),
			() => new Func13Resolver(),
			() => new Func14Resolver(),
			() => new Func15Resolver(),
			() => new Func16Resolver(),
		};
	}
}